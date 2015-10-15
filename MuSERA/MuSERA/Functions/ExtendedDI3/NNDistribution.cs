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
    /// <summary>
    /// Nearest Neighbour Distance Distribution class provides
    /// means of calcuating inter-interval distances.
    /// </summary>
    internal class NNDistribution : INotifyPropertyChanged
    {
        public NNDistribution(Dictionary<string, Di3> di3)
        {
            _di3 = di3;
            _updateNNDDBGW = new BackgroundWorker();
            _updateNNDDBGW.DoWork += _updateNNDDBGW_DoWork;
            _updateNNDDBGW.WorkerSupportsCancellation = true;
            _updateNNDDBGW.RunWorkerCompleted += _updateNNDDBGW_RunWorkerCompleted;
            _autoResetEvent = new AutoResetEvent(false);

            _options = new NNOptions();
            _options.distanceBinSize = 1;
            _options.frequencyBinSize = 1;
            indexedChrs = new ObservableCollection<MSCBNChr>();
            indexedSamples = new ObservableCollection<MSCBNSamples>();

            ERClassifications = new ObservableCollection<MSCBNERClassifications>();
            foreach (var item in Enum.GetValues(typeof(ERClassificationType)))
                ERClassifications.Add(new MSCBNERClassifications(false, (ERClassificationType)item));
        }

        /// <summary>
        /// Represents the default value for maximum log of p-value. 
        /// p-value lower than this value will be truncated. 
        /// </summary>
        private const double _defaultMaxLogValue = 3300.0;

        Dictionary<string, Di3> _di3 { set; get; }

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
        private double maximumSteps { set; get; }

        internal Dictionary<ERClassificationType, SortedDictionary<double, double>> estimatedDD { set; get; }

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

        public bool treadERClassificationsSeparately
        {
            set
            {
                _treadERClassificationsSeparately = value;
                NotifyPropertyChanged("treadERClassificationsSeparately");
            }
            get { return _treadERClassificationsSeparately; }
        }
        private bool _treadERClassificationsSeparately;

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

        public bool updatedFlag
        { // TODO: whole this "Flag" idea is not that elegant, try a batter work around
            set
            {
                _updatedFlag = value;
                NotifyPropertyChanged("updatedFlag");

                /// TODO : Following is not the best possible solution, 
                /// find a better work arround.
                if (ApplicationViewModel.Default.sessionsViewModel.plotType == PlotType.NND)
                    ApplicationViewModel.Default.sessionsViewModel.plotType = PlotType.NND;
            }
            get { return _updatedFlag; }
        }
        private bool _updatedFlag;

        private BackgroundWorker _updateNNDDBGW { set; get; }
        private AutoResetEvent _autoResetEvent { set; get; } // TODO: Make sure background worker is correctly canceled.

        public void CalculateNNDD(uint selectedSampleID)
        {
            _autoResetEvent.Reset();
            while (_updateNNDDBGW.IsBusy)
            {
                _updateNNDDBGW.CancelAsync();
                _autoResetEvent.WaitOne();
                _autoResetEvent.Set();
                Application.DoEvents();
            }

            estimatedDD.Clear();
            options.targetSampleIDs.Clear();
            foreach (var item in indexedSamples)
                if (item.isChecked)
                    options.targetSampleIDs.Add(item.sampleID);

            _updateNNDDBGW.RunWorkerAsync(selectedSampleID);
        }
        private void _updateNNDDBGW_DoWork(object sender, DoWorkEventArgs e)
        {
            status = 0;
            double stepCounter = 0;
            maximumSteps = 0;
            foreach (var item in ERClassifications)
                if (item.isChecked)
                    maximumSteps++;
            try
            {
                if (!treadERClassificationsSeparately)
                {
                    options.targetERClassificationTypes.Clear();
                    foreach (var item in ERClassifications)
                    {
                        if (item.isChecked)
                        {
                            options.targetERClassificationTypes.Add(item.classification);
                            status = ((++stepCounter) * 100.0) / maximumSteps;
                        }
                        if (_updateNNDDBGW.CancellationPending) { e.Cancel = true; break; ; }
                    }

                    RunNNDD((uint)e.Argument, 0);
                }
                else
                {
                    foreach (var item in ERClassifications)
                        if (item.isChecked)
                        {
                            options.targetERClassificationTypes.Clear();
                            options.targetERClassificationTypes.Add(item.classification);
                            RunNNDD((uint)e.Argument, item.classification);
                            status = ((++stepCounter) * 100.0) / maximumSteps;
                            if (_updateNNDDBGW.CancellationPending) { e.Cancel = true; break; ; }
                        }
                }
            }
            catch (OutOfMemoryException)
            {
                System.Windows.MessageBox.Show("Out-of-memory !!\n" +
                    "MuSERA does not have enough free memory space to execute the analysis. The program exits now.",
                    "OutOfMemory", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Stop);
                System.Environment.Exit(0);
            }

            _autoResetEvent.Set();
            status = 100;
        }
        private void _updateNNDDBGW_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            updatedFlag = !updatedFlag;
        }
        private void RunNNDD(uint sampleID, ERClassificationType classification)
        {
            var selectedSampleIDs = new List<uint>();
            selectedSampleIDs.Add(sampleID);
            estimatedDD.Add(classification, new SortedDictionary<double, double>());
            foreach (var chr in indexedChrs)
            {
                if (chr.isChecked)
                {
                    var result = _di3[chr.chr].GetNNDD(options, selectedSampleIDs);
                    foreach (var entry in result)
                        if (estimatedDD[classification].ContainsKey(entry.Key))
                            estimatedDD[classification][entry.Key] += entry.Value;
                        else
                            estimatedDD[classification].Add(entry.Key, entry.Value);
                }
            }
        }

        public Dictionary<ERClassificationType, SortedDictionary<double, double>> GetNNDD(int distanceBinWidth)
        {
            _autoResetEvent.Reset();
            while (_updateNNDDBGW.IsBusy)
            {
                /// Don't cancel worker if it's busy,
                /// just wait till it's finished.
                _autoResetEvent.WaitOne();
                _autoResetEvent.Set();
                Application.DoEvents();
            }

            var rtv = new Dictionary<ERClassificationType, SortedDictionary<double, double>>();
            if (estimatedDD == null || estimatedDD.Count == 0) return rtv;

            foreach (var entry in estimatedDD)
            {
                double distance = 0;
                if (!rtv.ContainsKey(entry.Key)) rtv.Add(entry.Key, new SortedDictionary<double, double>());
                foreach (var item in entry.Value)
                {
                    distance = Math.Floor(item.Key / distanceBinWidth) * distanceBinWidth;
                    if (double.IsInfinity(distance)) distance = _defaultMaxLogValue;
                    if (rtv[entry.Key].ContainsKey(distance))
                        rtv[entry.Key][distance] += item.Value;
                    else
                        rtv[entry.Key].Add(distance, item.Value);
                }
            }
            return rtv;
        }
        public ObservableCollection<Tuple<string, double, double>> GetFlattendNNDD(int resolution)
        {
            _autoResetEvent.Reset();
            while (_updateNNDDBGW.IsBusy)
            {
                /// Don't cancel worker if it's busy,
                /// just wait till it's finished.
                _autoResetEvent.WaitOne();
                _autoResetEvent.Set();
                Application.DoEvents();
            }

            var rtv = new ObservableCollection<Tuple<string, double, double>>();
            var hirarchicalStructure = GetNNDD(resolution);
            if (treadERClassificationsSeparately)
                foreach (var entry in hirarchicalStructure)
                    foreach (var item in entry.Value)
                        rtv.Add(new Tuple<string, double, double>(entry.Key.ToString(), item.Key, item.Value));
            else
                foreach (var entry in hirarchicalStructure)
                    foreach (var item in entry.Value)
                        rtv.Add(new Tuple<string, double, double>("All", item.Key, item.Value));

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
