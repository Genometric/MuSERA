/** Copyright © 2013-2015 Vahid Jalili
 * 
 * This file is part of MuSERA project.
 * MuSERA is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation,
 * either version 3 of the License, or (at your option) any later version.
 * MuSERA is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A 
 * PARTICULAR PURPOSE. See the GNU General Public License for more details.
 * You should have received a copy of the GNU General Public License along with Foobar. If not, see http://www.gnu.org/licenses/.
 **/

using Microsoft.Research.DynamicDataDisplay;
using Polimi.DEIB.VahidJalili.GIFP;
using Polimi.DEIB.VahidJalili.MuSERA.Analyzer;
using Polimi.DEIB.VahidJalili.MuSERA.Exporter;
using Polimi.DEIB.VahidJalili.MuSERA.Functions.Plots;
using Polimi.DEIB.VahidJalili.MuSERA.Models;
using Polimi.DEIB.VahidJalili.MuSERA.Warehouse;
using Polimi.DEIB.VahidJalili.MuSERA.Warehouse.Classes;
using System;
using System.Collections.Generic;
using System.IO;

namespace Polimi.DEIB.VahidJalili.MuSERA.Functions.BatchMode
{
    internal class ExecuteAtJob : IDisposable
    {
        public string Status
        {
            set
            {
                if (_status != value)
                {
                    _status = value;
                    OnStatusValueChaned(value);
                }
            }
            get { return _status; }
        }
        private string _status;
        public event EventHandler<ValueEventArgs> StatusChanged;
        private void OnStatusValueChaned(string value)
        {
            if (StatusChanged != null)
                StatusChanged(this, new ValueEventArgs(value));
        }


        public void Dispose()
        {
            Dispose(true);
            GC.Collect();
            GC.SuppressFinalize(this);
            GC.WaitForPendingFinalizers();
        }
        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed && disposing)
            { // TODO : Empty and release collection if needed.
                disposed = true;
            }
        }


        private ChartPlotter _chartPlotter { set; get; }
        private VerticalAxisTitle _vAxisTitle { set; get; }
        private HorizontalAxisTitle _hAxisTitle { set; get; }
        private Header _chartPlotterHeader { set; get; }
        private BatchOptions _batchOptions { set; get; }
        private PlotData _plotData { set; get; }

        private Dictionary<uint, ParsedChIPseqPeaks<int, Interval<int, MChIPSeqPeak>, MChIPSeqPeak>> _samples { set; get; }
        private Session<Interval<int, MChIPSeqPeak>, MChIPSeqPeak> _currentSession { set; get; }

        public ExecuteAtJob(PlotOptions plotOptions)
        {           
            _chartPlotter = new ChartPlotter();
            _vAxisTitle = new VerticalAxisTitle();
            _hAxisTitle = new HorizontalAxisTitle();
            _chartPlotterHeader = new Header();

            _plotData = new PlotData(
                chartPlotter: _chartPlotter,
                horizontalAxisTitle: _hAxisTitle,
                verticalAxisTitle: _vAxisTitle,
                di3: null,
                hAxisBinWidth: 1);

            _plotData.plotOptions = plotOptions;
        }
        public void Run(BatchOptions batchOptions)
        {
            _batchOptions = batchOptions;
            _chartPlotter.Width = _batchOptions.plotOptions.plotWidth;
            _chartPlotter.Height = _batchOptions.plotOptions.plotHeight;
            _chartPlotter.FontSize = _batchOptions.plotOptions.fontSize;
            _vAxisTitle.FontSize = _batchOptions.plotOptions.axisFontSize;
            _hAxisTitle.FontSize = _batchOptions.plotOptions.axisFontSize;
            _chartPlotterHeader.FontSize = _batchOptions.plotOptions.headerFontSize;

            _chartPlotter.Children.Add(_vAxisTitle);
            _chartPlotter.Children.Add(_hAxisTitle);
            _chartPlotter.Children.Add(_chartPlotterHeader);

            int counter = 0;
            int errorCounter = 0;
            foreach (var session in _batchOptions.sessions)
            {
                Status = "Processing Session  " + (++counter).ToString() + " \\ " + _batchOptions.sessions.Count.ToString() + "\t:\t" + session.title;

                Status = "\t\tLoading samples";
                try { LoadSamples(session); }
                catch (Exception e)
                {
                    Status = string.Format("Error in loading sample(s) ! {0}", e.Message);
                    Status = "Session is skipped !";
                    errorCounter++;
                    continue;
                }
                Status = "\t\tLoad completed ; " + session.samples.Count.ToString() + " samples are loaded";

                if (_samples.Count < 2)
                {
                    Status = "\t\tInsufficient samples; session skipped";
                    continue;
                }

                Status = "\t\tAnalysis started";
                try { RunAnalysis(session); }
                catch (Exception e)
                {
                    Status = string.Format("Error in analyzing samples ! {0}", e.Message);
                    Status = "Session is skipped !";
                    errorCounter++;
                    continue;
                }
                Status = "\t\tAnalysis completed";


                Status = "\t\tSaving analysis results";
                try { ExportResults(session, counter); }
                catch(Exception e)
                {
                    Status = string.Format("Error in saving results ! {0}", e.Message);
                    Status = "Session is skipped !";
                    errorCounter++;
                    continue;
                }
                Status = "\t\tSave completed";


                Status = "\t\tPlotting started";
                try { PlotOverview(session); }
                catch(Exception e)
                {
                    Status = string.Format("Error in plotting ! {0}", e.Message);
                    Status = "Session is skipped !";
                    errorCounter++;
                    continue;
                }
                Status = "\t\tPlotting completed";


                Status = "\t\tExporting Log";
                try { ExportLog(session, counter); }
                catch (Exception e)
                {
                    Status = string.Format("Error in exporting log ! {0}", e.Message);
                    Status = "Session is skipped !";
                    errorCounter++;
                    continue;
                }
                Status = "\t\tLog added";
            }

            if (errorCounter == 0)
                Status = "at-Job process completed successfully.";
            else
                Status = string.Format("at-Job process completed with {0} error{1}", errorCounter, (errorCounter == 1 ? "" : "s"));
        }
        private void LoadSamples(AtJobSession session)
        {
            var parsedSampled = new HashSet<string>();
            _samples = new Dictionary<uint, ParsedChIPseqPeaks<int, Interval<int, MChIPSeqPeak>, MChIPSeqPeak>>();

            foreach (string sample in session.samples)
            {
                if (!File.Exists(sample))
                {
                    Status = "\t\t!< WARNING >! : the file " + sample + " does not exist !";
                    continue;
                }
                if (parsedSampled.Contains(sample))
                {
                    Status = "\t\t!< WARNING >! : the file " + sample + " is already loaded for this session ! (the first one will be considered)";
                    continue;
                }

                var bedParser = new BEDParser<Interval<int, MChIPSeqPeak>, MChIPSeqPeak>(
                    source: sample,
                    species: Genomes.HomoSapiens,
                    assembly: Assemblies.hg19,
                    readOnlyValidChrs: false,
                    startOffset: session.parserParameters.startOffset,
                    chrColumn: session.parserParameters.chrColumn,
                    leftEndColumn: session.parserParameters.leftColumn,
                    rightEndColumn: session.parserParameters.rightColumn,
                    nameColumn: session.parserParameters.nameColumn,
                    valueColumn: session.parserParameters.pValueColumn,
                    summitColumn: session.parserParameters.summitColumn,
                    strandColumn: session.parserParameters.strandColumn,
                    defaultValue: session.parserParameters.defaultpValue,
                    pValueFormat: session.parserParameters.pValueConversion,
                    dropPeakIfInvalidValue: session.parserParameters.dropIfNopValue);

                var data = bedParser.Parse();
                parsedSampled.Add(sample);
                _samples.Add(data.fileHashKey, data);
            }
        }
        private void RunAnalysis(AtJobSession session)
        {
            var startTime = DateTime.Now;
            var analyzer = new Analyzer<Interval<int, MChIPSeqPeak>, MChIPSeqPeak>();
            foreach (var sample in _samples)
                analyzer.AddSample(sample.Value);
            analyzer.StatusChanged += AnalyzerStatusChanged;

            _currentSession = new Session<Interval<int, MChIPSeqPeak>, MChIPSeqPeak>()
            {
                elapsedTime = "",
                isCompleted = false,
                status = "Initializing",
                startTime = DateTime.Now,
                options = session.analysisOptions,
                endTime = default(DateTime),
                samples = analyzer.GetSamples(),
                index = 0
            };

            analyzer.Run(session.analysisOptions);
            _currentSession.analysisResults = analyzer.GetResults();
            _currentSession.mergedReplicates = analyzer.GetMergedReplicates();
            _currentSession.elapsedTime = DateTime.Now.Subtract(startTime).ToString();
            _currentSession.status = "Completed.";
            _currentSession.isCompleted = true;
        }
        private void ExportResults(AtJobSession session, int counter)
        {
            var exporter = new Exporter<Interval<int, MChIPSeqPeak>, MChIPSeqPeak>(_currentSession);

            if (session.title != null) exporter.samplePath = session.outputPath + session.title + Path.DirectorySeparatorChar;
            else exporter.samplePath = session.outputPath + "Session_" + counter.ToString();

            var options = new ExportOptions(
                        sessionPath: exporter.samplePath,
                        includeBEDHeader: true,
                        Export_R_j__o_BED: session.exportOptions.Export_R_j__o_BED,
                        Export_R_j__o_XML: session.exportOptions.Export_R_j__o_XML,
                        Export_R_j__s_BED: session.exportOptions.Export_R_j__s_BED,
                        Export_R_j__w_BED: session.exportOptions.Export_R_j__w_BED,
                        Export_R_j__b_BED: session.exportOptions.Export_R_j__b_BED,
                        Export_R_j__c_BED: session.exportOptions.Export_R_j__c_BED,
                        Export_R_j__c_XML: session.exportOptions.Export_R_j__c_XML,
                        Export_R_j__d_BED: session.exportOptions.Export_R_j__d_BED,
                        Export_R_j__d_XML: session.exportOptions.Export_R_j__d_XML,
                        Export_Chromosomewide_stats: false);

            exporter.Export(options);
        }
        private void PlotOverview(AtJobSession session)
        {
            foreach (var result in _currentSession.analysisResults)
            {
                _plotData.Update(result.Value);

                _chartPlotter.SaveScreenshot(
                    session.outputPath +
                    session.title + Path.DirectorySeparatorChar +
                    Path.GetFileNameWithoutExtension(result.Value.FileName) + "__overview.png");
            }
        }
        private void ExportLog(AtJobSession session, int counter)
        {
            int sampleIndex = 0;
            foreach (var analysisResult in _currentSession.analysisResults)
            {
                var supFiles = new List<string>();
                foreach (var sample in _currentSession.samples)
                    if (sample.Key != analysisResult.Key)
                        supFiles.Add(sample.Value);

                var exporter = new LogExporter(
                    IDsFile: _batchOptions.logFile,
                    statsFile: _batchOptions.logFile,
                    sessionTitle: session.title,
                    sessionIndex: counter.ToString(),
                    sampleFile: analysisResult.Value.FileName,
                    sampleIndex: (sampleIndex++).ToString(),
                    supSamples: supFiles,
                    analysisOptions: session.analysisOptions,
                    analysisResult: analysisResult.Value);

                exporter.Export();
            }
        }
        private void AnalyzerStatusChanged(object sender, Polimi.DEIB.VahidJalili.MuSERA.Analyzer.AnalyzerEventArgs e)
        {
            if (e.Value.percentage == 100)
                Status = "\t\t\t" + e.Value.ToString();
        }
    }
}
