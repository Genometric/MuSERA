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

using Microsoft.Research.DynamicDataDisplay;
using Polimi.DEIB.VahidJalili.MuSERA.Functions.Plots;
using Polimi.DEIB.VahidJalili.MuSERA.Models;
using Polimi.DEIB.VahidJalili.MuSERA.Warehouse;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Controls;

namespace Polimi.DEIB.VahidJalili.MuSERA.ViewModels
{
    internal class SessionsViewModel : INotifyPropertyChanged
    {
        public SessionsViewModel(ChartPlotter chartPlotter, HorizontalAxisTitle horizontalAxisTitle, VerticalAxisTitle verticalAxisTitle)
        {
            /// This is default value and has to be
            /// updated if the defaul option is changed in View.
            _hAxisBinWidth = 10;

            di3 = new ExtendedDi3();
            _selectedSampleIndex = -1;
            _selectedSessionID = null;
            _selectedSampleID = null;
            _genomeBrowserDictomies = 8;
            _genomeBrowserIncludeGenes = true;
            _genomeBrowserIncludeGeneralFeatures = true;
            plotData = new PlotData(chartPlotter, horizontalAxisTitle, verticalAxisTitle, di3, _hAxisBinWidth);
            sessionsSummary = new ObservableCollection<SessionSummary>();
        }

        /// <summary>
        /// Sets and gets all the available sessions.
        /// </summary>
        public Dictionary<string, Session<Interval<int, MChIPSeqPeak>, MChIPSeqPeak>> sessions
        {
            set { Sessions<Interval<int, MChIPSeqPeak>, MChIPSeqPeak>.Data = value; }
            get { return Sessions<Interval<int, MChIPSeqPeak>, MChIPSeqPeak>.Data; }
        }

        internal PlotType plotType
        {
            set
            {
                _plotType = value;
                NotifyPropertyChanged("plotType");
            }
            get { return _plotType; }
        }
        private PlotType _plotType;


        /// <summary>
        /// Sets and gets the selected session. This is the session chosen by single-click on session label.
        /// </summary>
        public SessionSummary previewSession
        {
            set
            {
                _previewSession = value;
                NotifyPropertyChanged("previewSession");
                selectedSessionDetails_RTB.Document = sessions[previewSession.id].ToFlowDocument();
                sessions[previewSession.id].PropertyChanged += selectedSession_PropertyChanged;
            }
            get { return _previewSession; }
        }
        private SessionSummary _previewSession;
        private void selectedSession_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "status")
                selectedSessionDetails_RTB.Document = sessions[previewSession.id].ToFlowDocument();
        }


        public ObservableCollection<SessionSummary> sessionsSummary
        {
            private set
            {
                _sessionsSummary = value;
                NotifyPropertyChanged("sessionsSummary");
            }
            get { return _sessionsSummary; }
        }
        private ObservableCollection<SessionSummary> _sessionsSummary;


        /// <summary>
        /// Sets and gets the selected session. This is the session chosen by double-click on session label.
        /// </summary>
        public Session<Interval<int, MChIPSeqPeak>, MChIPSeqPeak> selectedSession
        {
            set
            {
                _selectedSession = value;
                NotifyPropertyChanged("selectedSession");
            }
            get { return _selectedSession; }
        }
        private Session<Interval<int, MChIPSeqPeak>, MChIPSeqPeak> _selectedSession;


        /// <summary>
        /// Gets the analysis results of the selected sample in the chosen session.
        /// </summary>
        public AnalysisResult<Interval<int, MChIPSeqPeak>, MChIPSeqPeak> selectedAnalysisResult
        {
            set
            {
                _selectedAnalysisResult = value;
                NotifyPropertyChanged("selectedAnalysisResult");
            }
            get { return _selectedAnalysisResult; }
        }
        private AnalysisResult<Interval<int, MChIPSeqPeak>, MChIPSeqPeak> _selectedAnalysisResult;


        /// <summary>
        /// Sets and get the selected session ID.
        /// </summary>
        public string selectedSessionID
        {
            set
            {
                _selectedSessionID = value;
                NotifyPropertyChanged("selectedSessionID");
            }
            get { return _selectedSessionID; }
        }
        private string _selectedSessionID;


        /// <summary>
        /// Sets and gets the ID of the sample selected in samples ComboBox of selected session.
        /// </summary>
        public uint selectedSampleID_old
        {
            set
            {
                _selectedSampleID_old = value;
                NotifyPropertyChanged("selectedSampleID");
            }
            get { return _selectedSampleID_old; }
        }
        private uint _selectedSampleID_old;


        // TODO : IMPORTANT => why the ID is string ? should not it be UInt32 ?
        public string selectedSampleID
        {
            set
            {
                _selectedSampleID = value;
                NotifyPropertyChanged("selectedSampleID");
            }
            get { return _selectedSampleID; }
        }
        private string _selectedSampleID;


        /// <summary>
        /// Sets and gets the index of the sample selected in samples ComboBox of selected session.
        /// </summary>
        public int selectedSampleIndex
        {
            set
            {
                _selectedSampleIndex = value;
                NotifyPropertyChanged("selectedSampleIndex");
            }
            get { return _selectedSampleIndex; }
        }
        private int _selectedSampleIndex;


        public List<string> sessionChrs
        {
            set
            {
                _sessionChrs = value;
                NotifyPropertyChanged("sessionChrs");
            }
            get { return _sessionChrs; }
        }
        private List<string> _sessionChrs;


        public string selectedChr
        {
            set
            {
                _selectedChr = value;
                NotifyPropertyChanged("selectedChr");
            }
            get { return _selectedChr; }
        }
        private string _selectedChr;


        public int hAxisBinWidth
        {
            set
            {
                _hAxisBinWidth = value;
                NotifyPropertyChanged("hAxisBinWidth");
            }
            get { return _hAxisBinWidth; }
        }
        private int _hAxisBinWidth;


        public int genomeBrowserDictomies
        {
            set
            {
                _genomeBrowserDictomies = value;
                NotifyPropertyChanged("genomeBrowserDictomies");
            }
            get { return _genomeBrowserDictomies; }
        }
        private int _genomeBrowserDictomies;


        public bool genomeBrowserIncludeGenes
        {
            set
            {
                _genomeBrowserIncludeGenes = value;
                NotifyPropertyChanged("genomeBrowserIncludeGenes");
            }
            get { return _genomeBrowserIncludeGenes; }
        }
        private bool _genomeBrowserIncludeGenes;


        public bool genomeBrowserIncludeGeneralFeatures
        {
            set
            {
                _genomeBrowserIncludeGeneralFeatures = value;
                NotifyPropertyChanged("genomeBrowserIncludeGeneralFeatures");
            }
            get { return _genomeBrowserIncludeGeneralFeatures; }
        }
        private bool _genomeBrowserIncludeGeneralFeatures;


        /// <summary>
        /// Sets and get the selected ERs.
        /// <para>To be more comprehensive, selected is of ProcessedER type.
        /// This will allow to represent all classifications (i.e., Stringent/Weak, 
        /// Confirmed/Discarded, and TP/FP) in one type and specify the classification
        /// through "classification" property.</para>
        /// </summary>
        public AnalysisResult<Interval<int, MChIPSeqPeak>, MChIPSeqPeak>.ProcessedER selectedER
        {
            set
            {
                _selectedER = value;
                NotifyPropertyChanged("selectedER");
            }
            get { return _selectedER; }
        }
        private AnalysisResult<Interval<int, MChIPSeqPeak>, MChIPSeqPeak>.ProcessedER _selectedER;


        /// <summary>
        /// Integrated Genome Browser Data.
        /// <para>Is an extension to Di3 which has
        /// SC, SD, WC, and WD sets indexed together 
        /// with selected features. This enables the 
        /// integrated genome browser to visualize
        /// the an ER with all features/ERs surrounding it.</para>
        /// </summary>
        public ExtendedDi3 di3
        {
            set
            {
                _di3 = value;
                NotifyPropertyChanged("di3");
            }
            get { return _di3; }
        }
        private ExtendedDi3 _di3;

        public RichTextBox selectedSessionDetails_RTB { set; get; }
        public RichTextBox selectedSampleAnalysisOverview_RTB { set; get; }
        public DataGrid SetsInDetails { set; get; }
        internal PlotData plotData { set; get; }

        public void UpdateSessionsSummary()
        {
            foreach (var session in sessions)
                if (!sessionsSummary.Any(value => value.id == session.Key))
                {
                    /// Note: by default "session summary" has id==label. User can change label to match his needs,
                    /// but id will remain intact so that it can be used to connect the summary to its relavant session details.
                    sessionsSummary.Add(new SessionSummary(session.Key, session.Value.index, session.Key));
                }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                if (name == "selectedSampleID")
                    selectedSampleAnalysisOverview_RTB.Document = selectedSession.Overview(Convert.ToUInt32(selectedSampleID));

                // TODO: check => Maybe NOT needed : if (name == "selectedChr") plotData.Update(selectedChr);
                if (name == "selectedER") { } // plot the ER genome browser
                if (name == "plotType") plotData.Update(plotType);
                if (name == "hAxisBinWidth") plotData.Update(hAxisBinWidth);
                if (name == "selectedAnalysisResult")
                {
                    _selectedER = null;
                    plotData.Update(selectedAnalysisResult);
                }
                if (name == "selectedER" && selectedER != null)
                {
                    ApplicationViewModel.Default.plotRadioButtons.GenomeBrowser = true;
                    plotData.Update(selectedER, Convert.ToUInt32(selectedSampleID), selectedChr, genomeBrowserDictomies, genomeBrowserIncludeGenes, genomeBrowserIncludeGeneralFeatures);
                }
                if (name == "selectedSession") // Then update Di3.
                {
                    _selectedER = null;

                    di3.Update(
                        selectedSession,
                        (from f in ApplicationViewModel.Default.cachedFeaturesSummary
                         where f.selected == true
                         select f).ToList<CachedFeaturesSummary>());
                    plotData.Update(selectedSession);
                }

                handler(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
