/** Copyright © 2013-2015 Vahid Jalili
 * 
 * This file is part of MuSERA project.
 * MuSERA is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation,
 * either version 3 of the License, or (at your option) any later version.
 * MuSERA is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A 
 * PARTICULAR PURPOSE. See the GNU General Public License for more details.
 * You should have received a copy of the GNU General Public License along with Foobar. If not, see http://www.gnu.org/licenses/.
 **//** Copyright © 2013-2015 Vahid Jalili
 * 
 * This file is part of MuSERA project.
 * MuSERA is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation,
 * either version 3 of the License, or (at your option) any later version.
 * MuSERA is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A 
 * PARTICULAR PURPOSE. See the GNU General Public License for more details.
 * You should have received a copy of the GNU General Public License along with Foobar. If not, see http://www.gnu.org/licenses/.
 **/

using Polimi.DEIB.VahidJalili.MuSERA.Exporter;
using Polimi.DEIB.VahidJalili.MuSERA.Models;
using Polimi.DEIB.VahidJalili.MuSERA.Warehouse;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows;

namespace Polimi.DEIB.VahidJalili.MuSERA.ViewModels
{
    internal class SaveViewModel : INotifyPropertyChanged
    {
        public SaveViewModel(ObservableCollection<SessionSummary> sessionsSummary)
        {
            sessionsSelections = new ObservableCollection<SaveSessionDataGridElement>();
            foreach (var session in sessionsSummary)
                sessionsSelections.Add(new SaveSessionDataGridElement(
                    session.id,
                    session.label,
                    session.label,
                    true));

            addBEDHeader = true;
            export_Overview = true;
            export_R_j__o_BED = true;
            export_R_j__o_XML = true;
            export_R_j__s_BED = true;
            export_R_j__w_BED = true;
            export_R_j__b_BED = true;
            export_R_j__c_BED = true;
            export_R_j__c_XML = true;
            export_R_j__d_BED = true;
            export_R_j__d_XML = true;

            _saveBGW = new BackgroundWorker();
            _saveBGW.DoWork += _saveBGW_DoWork;
            _saveBGW.RunWorkerCompleted += _saveBGW_RunWorkerCompleted;

            fileProgress = 0;
            sampleProgress = 0;
            buttonsEnable = true;
            maxFileProgressBarValue = 100;
            maxSampleProgressBarValue = 100;
            saveButtonLabel = "Save      ";
            cancelButtonLable = "Cancel     ";
            progressBarsVisibility = Visibility.Hidden;
        }

        public ObservableCollection<SaveSessionDataGridElement> sessionsSelections
        {
            set
            {
                _sessionsSelections = value;
                NotifyPropertyChanged("sessionsSelections");
            }
            get { return _sessionsSelections; }
        }
        private ObservableCollection<SaveSessionDataGridElement> _sessionsSelections;

        private BackgroundWorker _saveBGW { set; get; }

        public int fileProgress
        {
            set
            {
                _fileProgress = value;
                NotifyPropertyChanged("fileProgress");
            }
            get { return _fileProgress; }
        }
        private int _fileProgress;

        public int sampleProgress
        {
            set
            {
                _sampleProgress = value;
                NotifyPropertyChanged("sampleProgress");
            }
            get { return _sampleProgress; }
        }
        private int _sampleProgress;

        public bool saveCompleted
        {
            set
            {
                _saveCompleted = value;
                NotifyPropertyChanged("saveCompleted");
            }
            get { return _saveCompleted; }
        }
        private bool _saveCompleted;

        public int maxFileProgressBarValue
        {
            set
            {
                _maxFileProgressBarValue = value;
                NotifyPropertyChanged("maxFileProgressBarValue");
            }
            get { return _maxFileProgressBarValue; }
        }
        private int _maxFileProgressBarValue;

        public int maxSampleProgressBarValue
        {
            set
            {
                _maxSampleProgressBarValue = value;
                NotifyPropertyChanged("maxSampleProgressBarValue");
            }
            get { return _maxSampleProgressBarValue; }
        }
        private int _maxSampleProgressBarValue;

        public string savePath
        {
            set
            {
                _savePath = value;
                NotifyPropertyChanged("savePath");
            }
            get { return _savePath; }
        }
        private string _savePath;

        public bool export_R_j__o_BED
        {
            set
            {
                _export_R_j__o_BED = value;
                NotifyPropertyChanged("export_R_j__o_BED");
            }
            get
            { return _export_R_j__o_BED; }
        }
        private bool _export_R_j__o_BED;

        public bool export_R_j__o_XML
        {
            set
            {
                _export_R_j__o_XML = value;
                NotifyPropertyChanged("export_R_j__o_XML");
            }
            get
            { return _export_R_j__o_XML; }
        }
        private bool _export_R_j__o_XML;

        public bool export_R_j__s_BED
        {
            set
            {
                _export_R_j__s_BED = value;
                NotifyPropertyChanged("export_R_j__s_BED");
            }
            get
            { return _export_R_j__s_BED; }
        }
        private bool _export_R_j__s_BED;

        public bool export_R_j__w_BED
        {
            set
            {
                _export_R_j__w_BED = value;
                NotifyPropertyChanged("export_R_j__w_BED");
            }
            get
            { return _export_R_j__w_BED; }
        }
        private bool _export_R_j__w_BED;

        public bool export_R_j__b_BED
        {
            set
            {
                _export_R_j__b_BED = value;
                NotifyPropertyChanged("export_R_j__b_BED");
            }
            get
            { return _export_R_j__b_BED; }
        }
        private bool _export_R_j__b_BED;

        public bool export_R_j__c_BED
        {
            set
            {
                _export_R_j__c_BED = value;
                NotifyPropertyChanged("export_R_j__c_BED");
            }
            get
            { return _export_R_j__c_BED; }
        }
        private bool _export_R_j__c_BED;

        public bool export_R_j__c_XML
        {
            set
            {
                _export_R_j__c_XML = value;
                NotifyPropertyChanged("export_R_j__c_XML");
            }
            get
            { return _export_R_j__c_XML; }
        }
        private bool _export_R_j__c_XML;

        public bool export_R_j__d_BED
        {
            set
            {
                _export_R_j__d_BED = value;
                NotifyPropertyChanged("export_R_j__d_BED");
            }
            get
            { return _export_R_j__d_BED; }
        }
        private bool _export_R_j__d_BED;

        public bool export_R_j__d_XML
        {
            set
            {
                _export_R_j__d_XML = value;
                NotifyPropertyChanged("export_R_j__d_XML");
            }
            get
            { return _export_R_j__d_XML; }
        }
        private bool _export_R_j__d_XML;

        public bool export_Overview
        {
            set
            {
                _export_Overview = value;
                NotifyPropertyChanged("export_Overview");
            }
            get
            { return _export_Overview; }
        }
        private bool _export_Overview;

        public bool addBEDHeader
        {
            set
            {
                _addBEDHeader = value;
                NotifyPropertyChanged("addBEDHeader");
            }
            get { return _addBEDHeader; }
        }
        private bool _addBEDHeader;

        public Visibility progressBarsVisibility
        {
            set
            {
                _progressBarsVisibility = value;
                NotifyPropertyChanged("progressBarsVisibility");
            }
            get { return _progressBarsVisibility; }
        }
        private Visibility _progressBarsVisibility;

        public bool buttonsEnable
        {
            set
            {
                _buttonsEnable = value;
                NotifyPropertyChanged("buttonsEnable");
            }
            get { return _buttonsEnable; }
        }
        private bool _buttonsEnable;

        public string saveButtonLabel
        {
            set
            {
                _saveButtonLabel = value;
                NotifyPropertyChanged("saveButtonLabel");
            }
            get { return _saveButtonLabel; }
        }
        private string _saveButtonLabel;

        public string cancelButtonLable
        {
            set
            {
                _cancelButtonLable = value;
                NotifyPropertyChanged("cancelButtonLable");
            }
            get { return _cancelButtonLable; }
        }
        private string _cancelButtonLable;

        public void SaveSelectedSessions()
        {
            maxFileProgressBarValue = 0;
            if (export_R_j__o_BED) maxFileProgressBarValue++;
            if (export_R_j__o_XML) maxFileProgressBarValue++;
            if (export_R_j__s_BED) maxFileProgressBarValue++;
            if (export_R_j__w_BED) maxFileProgressBarValue++;
            if (export_R_j__b_BED) maxFileProgressBarValue++;
            if (export_R_j__c_BED) maxFileProgressBarValue++;
            if (export_R_j__c_XML) maxFileProgressBarValue++;
            if (export_R_j__d_BED) maxFileProgressBarValue++;
            if (export_R_j__d_XML) maxFileProgressBarValue++;

            maxSampleProgressBarValue = 0;
            foreach (var session in sessionsSelections)
                if (session.isChecked)
                    maxSampleProgressBarValue += Sessions<Interval<int, MChIPSeqPeak>, MChIPSeqPeak>.Data[session.id].samples.Count;

            progressBarsVisibility = Visibility.Visible;
            buttonsEnable = false;
            saveButtonLabel = "Saving ...      ";
            cancelButtonLable = "please wait ...      ";

            _saveBGW.RunWorkerAsync();
        }
        private void _saveBGW_DoWork(object sender, DoWorkEventArgs e)
        {
            fileProgress = 0;
            sampleProgress = 0;

            foreach (var session in sessionsSelections)
            {
                if (session.isChecked)
                {
                    var exporter = new Exporter<Interval<int, MChIPSeqPeak>, MChIPSeqPeak>(Sessions<Interval<int, MChIPSeqPeak>, MChIPSeqPeak>.Data[session.id]);
                    exporter.FileProgressChanged += exporter_FileProgressChanged;
                    exporter.SampleProgressChanged += exporter_SampleProgressChanged;

                    var options = new ExportOptions(
                        sessionPath: savePath + Path.DirectorySeparatorChar + session.folder + Path.DirectorySeparatorChar,
                        includeBEDHeader: true,
                        Export_R_j__o_BED: export_R_j__o_BED,
                        Export_R_j__o_XML: export_R_j__o_XML,
                        Export_R_j__s_BED: export_R_j__s_BED,
                        Export_R_j__w_BED: export_R_j__w_BED,
                        Export_R_j__b_BED: export_R_j__b_BED,
                        Export_R_j__c_BED: export_R_j__c_BED,
                        Export_R_j__c_XML: export_R_j__c_XML,
                        Export_R_j__d_BED: export_R_j__d_BED,
                        Export_R_j__d_XML: export_R_j__d_XML,
                        Export_Chromosomewide_stats: false);

                    exporter.Export(options);
                }
            }
        }
        private void exporter_FileProgressChanged(object sender, ExporterEventArgs e)
        {
            fileProgress = e.status;
        }
        private void exporter_SampleProgressChanged(object sender, ExporterEventArgs e)
        {
            sampleProgress = e.status;
        }
        private void _saveBGW_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            progressBarsVisibility = Visibility.Hidden;
            buttonsEnable = true;
            saveButtonLabel = "Save      ";
            cancelButtonLable = "Cancel     ";
            saveCompleted = true;
        }


        protected void NotifyPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
