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
using Polimi.DEIB.VahidJalili.MuSERA.Warehouse;
using System;
using System.Collections.Generic;

namespace Polimi.DEIB.VahidJalili.MuSERA.Functions.Plots
{
    internal class PlotData : Plots
    {
        internal PlotData(
            ChartPlotter chartPlotter,
            HorizontalAxisTitle horizontalAxisTitle,
            VerticalAxisTitle verticalAxisTitle,
            ExtendedDi3 di3,
            int hAxisBinWidth)
            : base(
            chartPlotter,
            horizontalAxisTitle,
            verticalAxisTitle,
            di3,
            hAxisBinWidth)
        { }

        internal void Update(PlotType plotType)
        {
            this.plotType = plotType;
            Update();
        }
        internal void Update(string selectedChr)
        {
            this.selectedChr = selectedChr;
            // TODO : only if plot type is to display the selected interval then update plot.
        }
        internal void Update(int hAxisBinWidth)
        {
            this.hAxisBinWidth = hAxisBinWidth;
            if (plotType != PlotType.Overview) // and any other type that does not need hAxisBinWidth
                Update();
        }
        internal void Update(SortedDictionary<double, double> ERToFeatureDistances, PlotType plotOption)
        {
            this.ERToFeatureDistances = ERToFeatureDistances;
            plotType = plotOption;
            Update();
        }
        internal void Update(AnalysisResult<Interval<int, MChIPSeqPeak>, MChIPSeqPeak>.ProcessedER selectedER, UInt32 selectedERSampleID, string selectedChr, int dichotomies, bool includeGenes, bool includeGeneralFeatures)
        {
            this.selectedER = selectedER;
            this.selectedChr = selectedChr;
            this.dichotomies = dichotomies;
            this.includeGenes = includeGenes;
            this.selectedERSampleID = selectedERSampleID;
            this.includeGeneralFeatures = includeGeneralFeatures;
            plotType = PlotType.GenomeBrowser;
            Update();
        }
        internal void Update(Session<Interval<int, MChIPSeqPeak>, MChIPSeqPeak> selectedSession)
        {
            selectedER = null;
            this.selectedSession = selectedSession;
        }
        internal void Update(AnalysisResult<Interval<int, MChIPSeqPeak>, MChIPSeqPeak> selectedAnalysisResult)
        {
            selectedER = null;
            this.selectedAnalysisResult = selectedAnalysisResult;
            Update();
        }
        private void Update()
        {
            if (selectedAnalysisResult == null) return;
            switch (plotType)
            {
                case PlotType.Overview:
                    PlotOverview();
                    break;

                case PlotType.Classification_1st:
                case PlotType.Classification_2nd_2in1:
                case PlotType.Classification_2nd_4in1:
                case PlotType.Classification_3rd:
                case PlotType.Classification_4th:
                    PlotClassifications(plotType);
                    break;

                case PlotType.GenomeBrowser:
                    GenomeBrowser();
                    break;

                case PlotType.ERToFeature:
                    PlotERToFeatureDistance();
                    break;

                case PlotType.NND:
                    PlotNND();
                    break;

                case PlotType.Xsqrd:
                    PlotXsqrd();
                    break;

                case PlotType.SamplePValueDistribution:
                    PlotSamplePValueDistribution();
                    break;

                case PlotType.ChrwideStats:
                    PlotChrWideStats();
                    break;
            }
        }
    }
}
