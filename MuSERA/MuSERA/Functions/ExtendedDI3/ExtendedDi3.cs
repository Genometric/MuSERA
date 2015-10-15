/** Copyright © 2013-2015 Vahid Jalili
 * 
 * This file is part of MuSERA project.
 * MuSERA is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation,
 * either version 3 of the License, or (at your option) any later version.
 * MuSERA is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A 
 * PARTICULAR PURPOSE. See the GNU General Public License for more details.
 * You should have received a copy of the GNU General Public License along with Foobar. If not, see http://www.gnu.org/licenses/.
 **/

using Polimi.DEIB.VahidJalili.DI3;
using Polimi.DEIB.VahidJalili.DI3.AuxiliaryComponents;
using Polimi.DEIB.VahidJalili.MuSERA.Models.ExtendedDI3;
using Polimi.DEIB.VahidJalili.MuSERA.Warehouse;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Windows.Forms;

namespace Polimi.DEIB.VahidJalili.MuSERA.Models
{
    internal class ExtendedDi3 : Di3, INotifyPropertyChanged
    {
        public ExtendedDi3()
        {
            di3 = new Dictionary<string, Di3>();
            ER2FDD = new ERToFeatureDD(di3);
            NNDD = new NNDistribution(di3);

            _updateBGW = new BackgroundWorker();
            _updateBGW.DoWork += _updateBGW_DoWork;
            _updateBGW.WorkerSupportsCancellation = true;
            _updateBGW.RunWorkerCompleted += _updateBGW_RunWorkerCompleted;

            _autoResetEvent = new AutoResetEvent(false);

            _tIndexedSample = new Dictionary<uint, bool>();
            _tIndexedGeneralFeatures = new Dictionary<byte, bool>();
        }


        public ERToFeatureDD ER2FDD
        {
            set
            {
                _ER2FDD = value;
                NotifyPropertyChanged("ER2FDD");
            }
            get { return _ER2FDD; }
        }
        private ERToFeatureDD _ER2FDD;

        public NNDistribution NNDD
        {
            set
            {
                _NNDD = value;
                NotifyPropertyChanged("NNDD");
            }
            get { return _NNDD; }
        }
        private NNDistribution _NNDD;

        public double status
        {
            set
            {
                if (_status != value)
                {
                    _status = value;
                    NotifyPropertyChanged("status");
                }
            }
            get { return _status; }
        }
        private double _status;

        /// <summary>
        /// Represents the default value for maximum log of p-value. 
        /// p-value lower than this value will be truncated. 
        /// </summary>
        private const double _defaultMaxLogValue = 3300.0;

        private Session<Interval<int, MChIPSeqPeak>, MChIPSeqPeak> _selectedSession { set; get; }
        private List<CachedFeaturesSummary> _selectedFeatures { set; get; }
        private BackgroundWorker _updateBGW { set; get; }
        private AutoResetEvent _autoResetEvent { set; get; }

        public bool NNDDUpdatedFlag
        {
            set
            {
                _NNDDUpdatedFlag = value;
                NotifyPropertyChanged("NNDDUpdatedFlag");
            }
            get { return _NNDDUpdatedFlag; }
        }
        private bool _NNDDUpdatedFlag;

        public NNOptions NNDDOptions
        {
            private set
            {
                _NNDDOptions = value;
                NotifyPropertyChanged("NNDDOptions");
            }
            get { return _NNDDOptions; }
        }
        private NNOptions _NNDDOptions;

        public Dictionary<string, Di3> di3
        {
            private set
            {
                _di3 = value;
                NotifyPropertyChanged("di3");
            }
            get { return _di3; }
        }
        private Dictionary<string, Di3> _di3;

        private Dictionary<UInt32, bool> _tIndexedSample { set; get; }
        private Dictionary<byte, bool> _tIndexedGeneralFeatures { set; get; }

        public void Update(Session<Interval<int, MChIPSeqPeak>, MChIPSeqPeak> selectedSession, List<CachedFeaturesSummary> selectedFeatures)
        {
            status = 0;
            _selectedSession = selectedSession;
            _selectedFeatures = selectedFeatures;

            if (selectedFeatures.Count > 0 && selectedFeatures[0].dataType == DataType.RefSeqGenes)
            {
                ER2FDD.useGenes = true;
                ER2FDD.useGeneralFeatures = false;
            }
            else if (selectedFeatures.Count > 0 && selectedFeatures[0].dataType == DataType.GeneralFeatures)
            {
                ER2FDD.useGenes = false;
                ER2FDD.useGeneralFeatures = true;
            }

            ER2FDD.estimatedDD = new SortedDictionary<double, double>();
            ER2FDD.updatedFlag = !ER2FDD.updatedFlag;
            NNDD.estimatedDD = new Dictionary<ERClassificationType, SortedDictionary<double, double>>();
            NNDD.updatedFlag = !NNDD.updatedFlag;

            _autoResetEvent.Reset();
            while (_updateBGW.IsBusy)
            {
                _updateBGW.CancelAsync();
                _autoResetEvent.WaitOne();
                _autoResetEvent.Set();
                Application.DoEvents();
            }
            _updateBGW.RunWorkerAsync();
        }
        private void _updateBGW_DoWork(object sender, DoWorkEventArgs e)
        {
            double totalProcessSteps = (_selectedSession.analysisResults.Count * 3) + _selectedFeatures.Count + 1;
            double step = 0;
            status = step / totalProcessSteps;
            di3.Clear();

            foreach (var sample in _selectedSession.analysisResults)
            {
                if (_updateBGW.CancellationPending) { e.Cancel = true; break; ; }
                IndexERs(sample.Key, sample.Value.R_j__c, e);
                status = ((++step) / totalProcessSteps) * 100.0;

                if (_updateBGW.CancellationPending) { e.Cancel = true; break; }
                IndexERs(sample.Key, sample.Value.R_j__d, e);
                status = ((++step) / totalProcessSteps) * 100.0;

                if (_updateBGW.CancellationPending) { e.Cancel = true; break; }
                IndexERs(sample.Key, sample.Value.R_j__o, e);
                status = ((++step) / totalProcessSteps) * 100.0;
            }

            foreach (var features in _selectedFeatures)
            {
                if (_updateBGW.CancellationPending) { e.Cancel = true; break; }
                switch (features.dataType)
                {
                    case DataType.GeneralFeatures:
                        IndexGFs(features.fileHashKey, e);
                        break;
                    case DataType.RefSeqGenes:
                        IndexRGs(features.fileHashKey, e);
                        break;
                }
                status = ((++step) / totalProcessSteps) * 100.0;
            }

            _autoResetEvent.Set();
        }
        private void _updateBGW_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            UpdateObservableCollections();
            status = 100.0;
        }
        private void IndexERs(uint sampleID, Dictionary<string, Dictionary<UInt64, AnalysisResult<Interval<int, MChIPSeqPeak>, MChIPSeqPeak>.ProcessedER>> ERs, DoWorkEventArgs e)
        {// Index ERs
            foreach (var chr in ERs)
            {
                if (_updateBGW.CancellationPending) { e.Cancel = true; break; ; }
                if (!di3.ContainsKey(chr.Key)) di3.Add(chr.Key, new Di3());
                foreach (var er in chr.Value)
                    di3[chr.Key].Insert(er.Value.er.left, er.Value.er.right, er.Value.er, sampleID, er.Value.classification);
            }
        }
        private void IndexERs(uint sampleID, Dictionary<string, List<AnalysisResult<Interval<int, MChIPSeqPeak>, MChIPSeqPeak>.ProcessedER>> ERs, DoWorkEventArgs e)
        {// Index ERs
            foreach (var chr in ERs)
            {
                if (_updateBGW.CancellationPending) { e.Cancel = true; break; ; }
                if (!di3.ContainsKey(chr.Key)) di3.Add(chr.Key, new Di3());
                foreach (var er in chr.Value)
                    di3[chr.Key].Insert(er.er.left, er.er.right, er.er, sampleID, er.statisticalClassification); // TP and FP should be passed as classification
            }
        }
        private void IndexGFs(uint sampleID, DoWorkEventArgs e)
        {// Index General Features
            foreach (var chr in GeneralFeatures<Interval<int, MGeneralFeatures>, MGeneralFeatures>.Data[sampleID].intervals)
            {
                if (_updateBGW.CancellationPending) { e.Cancel = true; break; }
                if (!di3.ContainsKey(chr.Key)) continue;
                foreach (var strand in chr.Value)
                    foreach (var gf in strand.Value)
                        di3[chr.Key].Insert(gf.left, gf.right, gf, sampleID);
            }
        }
        private void IndexRGs(uint sampleID, DoWorkEventArgs e)
        {// Index Ref-seq Genes
            foreach (var chr in RefSeqGenes<Interval<int, MRefSeqGenes>, MRefSeqGenes>.Data[sampleID].intervals)
            {
                if (_updateBGW.CancellationPending) { e.Cancel = true; break; }
                if (!di3.ContainsKey(chr.Key)) continue;
                foreach (var strand in chr.Value)
                    foreach (var rg in strand.Value)
                        di3[chr.Key].Insert(rg.left, rg.right, rg, sampleID);
            }
        }
        private void UpdateObservableCollections()
        {
            ER2FDD.indexedChrs.Clear();
            ER2FDD.indexedSamples.Clear();
            NNDD.indexedChrs.Clear();
            NNDD.indexedSamples.Clear();
            _tIndexedSample.Clear();
            _tIndexedGeneralFeatures.Clear();

            foreach (var chr in di3)
            {
                ER2FDD.indexedChrs.Add(new MSCBNChr(true, chr.Key));
                NNDD.indexedChrs.Add(new MSCBNChr(true, chr.Key));
                foreach (var sample in chr.Value.indexedSamples)
                    if (sample.Value.intervalType == IntervalType.ER)
                        if (_tIndexedSample.ContainsKey(sample.Key) == false)
                            _tIndexedSample.Add(sample.Key, false);

                foreach (var gfs in chr.Value.indexedGeneralFeatures)
                    foreach (var gf in gfs.Value)
                        if (!_tIndexedGeneralFeatures.ContainsKey(gf.Key))
                            _tIndexedGeneralFeatures.Add(gf.Key, false);
            }

            foreach (var item in _tIndexedSample)
            {
                ER2FDD.indexedSamples.Add(new MSCBNSamples(true, item.Key));
                NNDD.indexedSamples.Add(new MSCBNSamples(true, item.Key));
            }

            var tDic = new HashSet<string>();
            foreach (var chr in di3)
                foreach (var gfs in chr.Value.indexedGeneralFeatures)
                    foreach (var gf in gfs.Value)
                        if (tDic.Contains(gfs.Key.ToString() + "|" + gf.Key.ToString()) == false)
                            ER2FDD.indexedGeneralFeatures.Add(new MSCBNGeneralFeatures(true, gfs.Key, gf.Key));
        }



        /// <summary>
        /// Returns ER-to-Feature distance distribution
        /// in the provide hAxisBinWidth.
        /// </summary>
        public SortedDictionary<double, double> GetER2FDD(int resolution)
        {
            return ER2FDD.GetER2FDD(resolution);
        }
        public Dictionary<ERClassificationType, SortedDictionary<double, double>> GetNNDD(int resolution)
        {
            return NNDD.GetNNDD(resolution);
        }



        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
