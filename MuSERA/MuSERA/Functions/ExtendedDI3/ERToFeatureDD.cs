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
using Polimi.DEIB.VahidJalili.MuSERA.ViewModels;
using Polimi.DEIB.VahidJalili.MuSERA.Warehouse;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading;
using System.Windows.Forms;

namespace Polimi.DEIB.VahidJalili.MuSERA.Models.ExtendedDI3
{
    internal class ERToFeatureDD : INotifyPropertyChanged
    {
        public ERToFeatureDD(Dictionary<string, Di3> di3)
        {
            _di3 = di3;
            _updateFABGW = new BackgroundWorker();
            _updateFABGW.DoWork += _updateFABGW_DoWork;
            _updateFABGW.WorkerSupportsCancellation = true;
            _updateFABGW.RunWorkerCompleted += _updateFABGW_RunWorkerCompleted;
            _autoResetEvent = new AutoResetEvent(false);

            options = new NNOptions();
            options.distanceBinSize = 1;
            options.frequencyBinSize = 1;
            indexedChrs = new ObservableCollection<MSCBNChr>();
            indexedSamples = new ObservableCollection<MSCBNSamples>();
            indexedGeneralFeatures = new ObservableCollection<MSCBNGeneralFeatures>();

            ERClassifications = new ObservableCollection<MSCBNERClassifications>();
            foreach (var item in Enum.GetValues(typeof(ERClassificationType)))
                ERClassifications.Add(new MSCBNERClassifications(false, (ERClassificationType)item));
        }

        private Dictionary<string, Di3> _di3 { set; get; }

        /// <summary>
        /// Represents the default value for maximum log of p-value. 
        /// p-value lower than this value will be truncated. 
        /// </summary>
        private const double _defaultMaxLogValue = 3300.0;

        internal SortedDictionary<double, double> estimatedDD { set; get; }

        public NNOptions options
        {
            private set
            {
                _options = value;
                NotifyPropertyChanged("options");
            }
            get { return _options; }
        }
        private NNOptions _options;

        public ObservableCollection<MSCBNChr> indexedChrs
        {
            set
            {
                _indexedChrs = value;
                NotifyPropertyChanged("indexedChrs");
            }
            get { return _indexedChrs; }
        }
        private ObservableCollection<MSCBNChr> _indexedChrs;

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

        public bool useGenes
        {
            set
            {
                _useGenes = value;
                NotifyPropertyChanged("useGenes");
            }
            get { return _useGenes; }
        }
        private bool _useGenes;

        public bool useGeneralFeatures
        {
            set
            {
                _useGeneralFeatures = value;
                NotifyPropertyChanged("useGeneralFeatures");
            }
            get { return _useGeneralFeatures; }
        }
        private bool _useGeneralFeatures;

        public ObservableCollection<MSCBNSamples> indexedSamples
        {
            set
            {
                _indexedSamples = value;
                NotifyPropertyChanged("indexedSamples");
            }
            get { return _indexedSamples; }
        }
        private ObservableCollection<MSCBNSamples> _indexedSamples;

        public ObservableCollection<MSCBNGeneralFeatures> indexedGeneralFeatures
        {
            set
            {
                _indexedGeneralFeatures = value;
                NotifyPropertyChanged("indexedGeneralFeatures");
            }
            get { return _indexedGeneralFeatures; }
        }
        private ObservableCollection<MSCBNGeneralFeatures> _indexedGeneralFeatures;

        public ObservableCollection<MSCBNERClassifications> ERClassifications
        {
            set
            {
                _ERClassifications = value;
                NotifyPropertyChanged("ERClassifications");
            }
            get { return _ERClassifications; }
        }
        private ObservableCollection<MSCBNERClassifications> _ERClassifications;

        public bool updatedFlag
        { // TODO: whole this "Flag" idea is not that elegant, try a batter work around
            set
            {
                _updatedFlag = value;
                NotifyPropertyChanged("updatedFlag");

                /// TODO : Following is not the best possible solution, 
                /// find a better work arround.
                if (ApplicationViewModel.Default.sessionsViewModel.plotType == PlotType.ERToFeature)
                    ApplicationViewModel.Default.sessionsViewModel.plotType = PlotType.ERToFeature;
            }
            get { return _updatedFlag; }
        }
        private bool _updatedFlag;

        /// <summary>
        /// Sets and get a background worker to perform functional analysis.
        /// </summary>
        private BackgroundWorker _updateFABGW { set; get; }
        private AutoResetEvent _autoResetEvent { set; get; }

        public void CalculateERToFeatureDistances()
        {
            _autoResetEvent.Reset();
            while (_updateFABGW.IsBusy)
            {
                _updateFABGW.CancelAsync();
                _autoResetEvent.WaitOne();
                _autoResetEvent.Set();

                /// TODO: Try to avoid this using 
                /// dispatcher or background workers,
                /// and then remove reference to 
                /// Windows.Forms
                Application.DoEvents();
            }

            options.targetERClassificationTypes.Clear();
            foreach (var item in ERClassifications)
                if (item.isChecked)
                    options.targetERClassificationTypes.Add(item.classification);

            options.targetFeatures.Clear();
            foreach (var item in indexedGeneralFeatures)
                if (item.isChecked)
                    options.targetFeatures.Add(item.featureCode);

            options.targetSampleIDs.Clear();
            foreach (var item in indexedSamples)
                if (item.isChecked)
                    options.targetSampleIDs.Add(item.sampleID);

            if (_useGenes) options.targetAnnotationType = IntervalType.Gene;
            else if (_useGeneralFeatures) options.targetAnnotationType = IntervalType.GeneralFeature;
            else options.targetAnnotationType = IntervalType.ER;
            // If the value of the parameter is set to IntervalType.ER, then no functional analysis will be performed.

            _updateFABGW.RunWorkerAsync();
        }
        private void _updateFABGW_DoWork(object sender, DoWorkEventArgs e)
        {
            double stepCounter = 0;
            status = 0;
            foreach (var chr in indexedChrs)
            {
                status = ((++stepCounter) * 100) / indexedChrs.Count;
                if (_updateFABGW.CancellationPending) { e.Cancel = true; break; ; }
                if (chr.isChecked)
                {
                    var result = _di3[chr.chr].GetERToFeatureDistance(options);
                    foreach (var item in result)
                        if (estimatedDD.ContainsKey(item.Key))
                            estimatedDD[item.Key] += item.Value;
                        else
                            estimatedDD.Add(item.Key, item.Value);
                }
            }

            _autoResetEvent.Set();
            status = 100;
        }
        private void _updateFABGW_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            updatedFlag = !updatedFlag;
        }

        public SortedDictionary<double, double> GetER2FDD(int distanceBinWidth)
        {
            _autoResetEvent.Reset();
            while (_updateFABGW.IsBusy)
            {
                /// Don't cancel the background worker, 
                /// just wait for it to finish it's job.
                _autoResetEvent.WaitOne();
                _autoResetEvent.Set();
                Application.DoEvents();
            }

            var rtv = new SortedDictionary<double, double>();
            if (estimatedDD == null || estimatedDD.Count == 0) return rtv;

            double distance = 0;
            foreach (var item in estimatedDD)
            {
                distance = Math.Floor(item.Key / distanceBinWidth) * distanceBinWidth;
                if (Double.IsInfinity(distance)) distance = _defaultMaxLogValue;
                if (rtv.ContainsKey(distance))
                    rtv[distance] += item.Value;
                else
                    rtv.Add(distance, item.Value);
            }
            return rtv;
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
