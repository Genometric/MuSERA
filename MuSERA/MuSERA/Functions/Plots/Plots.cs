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
using Polimi.DEIB.VahidJalili.MuSERA.Models;
using Polimi.DEIB.VahidJalili.MuSERA.ViewModels;
using Polimi.DEIB.VahidJalili.MuSERA.Warehouse;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Polimi.DEIB.VahidJalili.MuSERA.Functions.Plots
{
    internal class Plots
    {
        public Plots(
            ChartPlotter chartPlotter,
            HorizontalAxisTitle horizontalAxisTitle,
            VerticalAxisTitle verticalAxisTitle,
            ExtendedDi3 di3,
            int hAxisBinWidth)
        {
            _di3 = di3;
            _selectedSessionSamples = _selectedSessionSamples;
            _plotOverview = new PlotOverview(chartPlotter, horizontalAxisTitle, verticalAxisTitle);
            _plotClassifications = new PlotClassifications(chartPlotter, horizontalAxisTitle, verticalAxisTitle);
            _genomeBrowser = new GenomeBrowser(chartPlotter, horizontalAxisTitle, verticalAxisTitle, di3);
            _plotERToFeature = new PlotERToFeatureDistance(chartPlotter, horizontalAxisTitle, verticalAxisTitle);
            _plotNND = new PlotNND(chartPlotter, horizontalAxisTitle, verticalAxisTitle, di3);
            _plotXsqrd = new PlotXsqrd(chartPlotter, horizontalAxisTitle, verticalAxisTitle);
            _plotSamplesPValueDistribution = new PlotSamplesPValueDistribution(chartPlotter, horizontalAxisTitle, verticalAxisTitle);
            _plotChrWideStats = new Functions.Plots.PlotChrWideStats(chartPlotter, horizontalAxisTitle, verticalAxisTitle);
            plotOptions = new PlotOptions(chartPlotter.ActualHeight, chartPlotter.ActualWidth, 0);
            this.hAxisBinWidth = hAxisBinWidth;
        }

        public PlotOptions plotOptions { set; get; }
        protected int hAxisBinWidth { set; get; }
        protected string selectedChr { set; get; }
        protected PlotType plotType { set; get; }
        protected int dichotomies { set; get; }
        protected bool includeGenes { set; get; }
        protected bool includeGeneralFeatures { set; get; }
        protected uint selectedERSampleID { set; get; }
        protected AnalysisResult<Interval<int, MChIPSeqPeak>, MChIPSeqPeak>.ProcessedER selectedER { set; get; }
        protected Session<Interval<int, MChIPSeqPeak>, MChIPSeqPeak> selectedSession { set; get; }
        protected AnalysisResult<Interval<int, MChIPSeqPeak>, MChIPSeqPeak> selectedAnalysisResult { set; get; }
        protected SortedDictionary<double, double> ERToFeatureDistances { set; get; }
        private PlotOverview _plotOverview { set; get; }
        private PlotClassifications _plotClassifications { set; get; }
        private PlotNND _plotNND { set; get; }
        private PlotXsqrd _plotXsqrd { set; get; }
        private PlotSamplesPValueDistribution _plotSamplesPValueDistribution { set; get; }
        private GenomeBrowser _genomeBrowser { set; get; }
        private PlotERToFeatureDistance _plotERToFeature { set; get; }
        private PlotChrWideStats _plotChrWideStats { set; get; }
        private Dictionary<uint, string> _selectedSessionSamples { set; get; }
        private ExtendedDi3 _di3 { set; get; }



        protected void PlotOverview()
        {
            _plotOverview.Plot(selectedAnalysisResult, plotOptions);
        }
        protected void PlotClassifications(PlotType plotType)
        {
            PValueDistributions data = null;
            switch (plotType)
            {
                case PlotType.Classification_1st:
                    data = selectedAnalysisResult.CCD(ERClassificationCategory.First, hAxisBinWidth);
                    break;

                case PlotType.Classification_2nd_2in1:
                case PlotType.Classification_2nd_4in1:
                    data = selectedAnalysisResult.CCD(ERClassificationCategory.Second, hAxisBinWidth);
                    break;

                case PlotType.Classification_3rd:
                    data = selectedAnalysisResult.CCD(ERClassificationCategory.Third, hAxisBinWidth);
                    break;

                case PlotType.Classification_4th:
                    data = selectedAnalysisResult.CCD(ERClassificationCategory.Fourth, hAxisBinWidth);
                    break;
            }

            _plotClassifications.Plot(data, plotType, plotOptions);
        }
        protected void GenomeBrowser()
        {
            _genomeBrowser.Plot(selectedER, selectedERSampleID, selectedChr, dichotomies, includeGenes, includeGeneralFeatures, plotOptions);
        }
        protected void PlotERToFeatureDistance()
        {
            // TODO : update this part such that the required information is provided from outside and avoid call to "ApplicationViewModel"
            _plotERToFeature.Plot(ApplicationViewModel.Default.sessionsViewModel.di3.GetER2FDD(hAxisBinWidth), plotOptions);
        }
        protected void PlotNND()
        {
            _plotNND.Plot(plotOptions, hAxisBinWidth);
        }
        protected void PlotXsqrd()
        {
            _plotXsqrd.Plot(selectedAnalysisResult.GetXSqrdDistributions(hAxisBinWidth), plotOptions);
        }
        protected void PlotSamplePValueDistribution()
        {
            var data = new Dictionary<uint, PValueDistributions>();
            var labels = new Dictionary<uint, string>();
            foreach (var result in selectedSession.analysisResults)
            {
                labels.Add(result.Key, ApplicationViewModel.Default.cachedDataSummary.First(x => x.fileHashKey == result.Key).label);
                data.Add(result.Key, result.Value.GetInputPValueDistribution(hAxisBinWidth));
            }

            var sampleIDs = selectedSession.analysisResults.Keys.ToList();
            _plotSamplesPValueDistribution.Plot(data, labels, plotOptions);
        }
        protected void PlotChrWideStats()
        {
            _plotChrWideStats.Plot(selectedAnalysisResult.chrwideStats, plotOptions);
        }
    }
}
