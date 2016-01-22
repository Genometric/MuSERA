/** Copyright © 2013-2015 Vahid Jalili
 * 
 * This file is part of MuSERA project.
 * MuSERA is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation,
 * either version 3 of the License, or (at your option) any later version.
 * MuSERA is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A 
 * PARTICULAR PURPOSE. See the GNU General Public License for more details.
 * You should have received a copy of the GNU General Public License along with Foobar. If not, see http://www.gnu.org/licenses/.
 **/

using Polimi.DEIB.VahidJalili.GIFP;
using Polimi.DEIB.VahidJalili.MuSERA.Analyzer;
using Polimi.DEIB.VahidJalili.MuSERA.Models;
using Polimi.DEIB.VahidJalili.MuSERA.ViewModels;
using Polimi.DEIB.VahidJalili.MuSERA.Warehouse;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Threading;

namespace Polimi.DEIB.VahidJalili.MuSERA.Commands
{
    internal class AnalyzeBase
    {
        public AnalyzeBase()
        {
            _analysisBGW = new BackgroundWorker();
            _analysisBGW.WorkerReportsProgress = true;
            _analysisBGW.DoWork += _analysisBGW_DoWork;
            _analysisBGW.ProgressChanged += _analysisBGW_ProgressChanged;
            _analysisBGW.RunWorkerCompleted += _analysisBGW_RunWorkerCompleted;
            _timer = new DispatcherTimer();
            _timer.Tick += _timer_Tick;
            _timer.Interval = new TimeSpan(0, 0, 0, 0, 1);
            _analyzer = new Analyzer<Interval<int, MChIPSeqPeak>, MChIPSeqPeak>();
            _analyzer.StatusChanged += _analyzerStatusChanged;
        }

        protected string analysisET { set { _analysisET = value; } get { return _analysisET; } }
        private string _analysisET;

        private string _currentSessionTitle { set; get; }
        private Session<Interval<int, MChIPSeqPeak>, MChIPSeqPeak> _currentSession { set; get; }
        private DateTime _startTime { set; get; }
        private TimeSpan _tempDuration { set; get; }
        private DispatcherTimer _timer { set; get; }
        internal AnalysisOptions analysisOptions { set; get; }
        private BackgroundWorker _analysisBGW { set; get; }
        private Analyzer<Interval<int, MChIPSeqPeak>, MChIPSeqPeak> _analyzer { set; get; }

        public void Execute()
        {
            _currentSessionTitle = Sessions<Interval<int, MChIPSeqPeak>, MChIPSeqPeak>.GetSessionTitle();
            _currentSession = new Session<Interval<int, MChIPSeqPeak>, MChIPSeqPeak>()
            {
                elapsedTime = "",
                isCompleted = false,
                status = "Initializing",
                startTime = DateTime.Now,
                options = analysisOptions,
                endTime = default(DateTime),
                samples = _analyzer.GetSamples(),
                index = Sessions<Interval<int, MChIPSeqPeak>, MChIPSeqPeak>.Data.Count
            };

            ApplicationViewModel.Default.uiProperties.interactiveGUIState = InteractiveGUIState.RunningAnalysis;
            ApplicationViewModel.Default.sessionsViewModel.sessions.Add(_currentSessionTitle, _currentSession);
            ApplicationViewModel.Default.sessionsViewModel.UpdateSessionsSummary();

            _startTime = DateTime.Now;
            _timer.Start();
            _analysisBGW.RunWorkerAsync();
        }
        public void AddSample(ParsedChIPseqPeaks<int, Interval<int, MChIPSeqPeak>, MChIPSeqPeak> sample)
        {
            _analyzer.AddSample(sample);
        }

        private void _analysisBGW_DoWork(object sender, DoWorkEventArgs e)
        {
            try { _analyzer.Run(analysisOptions); }
            catch (OutOfMemoryException oom)
            {
                MessageBox.Show("Out-of-memory !!\n" +
                    "MuSERA does not have enough free memory space to execute the analysis. The program exits now.",
                    "OutOfMemory", MessageBoxButton.OK, MessageBoxImage.Stop);
            }
        }
        private void _analysisBGW_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            _timer.Stop();
            ApplicationViewModel.Default.uiProperties.interactiveGUIState = InteractiveGUIState.AnalysisFinished;
            _currentSession.analysisResults = _analyzer.GetResults();
            _currentSession.mergedReplicates = _analyzer.GetMergedReplicates();
            _currentSession.elapsedTime = DateTime.Now.Subtract(_startTime).ToString();
            _currentSession.status = "Completed.";
            _currentSession.isCompleted = true;
            _analyzer = new Analyzer<Interval<int, MChIPSeqPeak>, MChIPSeqPeak>();
            _analyzer.StatusChanged += _analyzerStatusChanged;
            _currentSession.CalculateSimilarities();
        }
        private void _analysisBGW_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            _currentSession.status = ((Progress)e.UserState).ToString();
        }
        private void _analyzerStatusChanged(object sender, AnalyzerEventArgs e)
        {
            _analysisBGW.ReportProgress((int)Math.Floor(e.Value.percentage), e.Value);
        }
        private void _timer_Tick(object sender, EventArgs e)
        {
            _tempDuration = DateTime.Now.Subtract(_startTime).Duration();
            ApplicationViewModel.Default.uiProperties.analysisET = string.Format("{0} : {1} : {2} : {3}",
                (_tempDuration.Hours < 10 ? "0" + _tempDuration.Hours.ToString() : _tempDuration.Hours.ToString()),
                (_tempDuration.Minutes < 10 ? "0" + _tempDuration.Minutes.ToString() : _tempDuration.Minutes.ToString()),
                (_tempDuration.Seconds < 10 ? "0" + _tempDuration.Seconds.ToString() : _tempDuration.Seconds.ToString()),
                Math.Round(_tempDuration.Milliseconds % 100.0));
        }
    }
}
