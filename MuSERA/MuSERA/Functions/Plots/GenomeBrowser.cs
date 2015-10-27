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
using Microsoft.Research.DynamicDataDisplay.Charts;
using Microsoft.Research.DynamicDataDisplay.DataSources;
using Microsoft.Research.DynamicDataDisplay.PointMarkers;
using Polimi.DEIB.VahidJalili.DI3;
using Polimi.DEIB.VahidJalili.DI3.AuxiliaryComponents;
using Polimi.DEIB.VahidJalili.MuSERA.Functions.Plots.Extensions;
using Polimi.DEIB.VahidJalili.MuSERA.Models;
using Polimi.DEIB.VahidJalili.MuSERA.ViewModels;
using Polimi.DEIB.VahidJalili.MuSERA.Warehouse;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace Polimi.DEIB.VahidJalili.MuSERA.Functions.Plots
{
    internal class GenomeBrowser
    {
        public GenomeBrowser(
            ChartPlotter chartPlotter,
            HorizontalAxisTitle horizontalAxisTitle,
            VerticalAxisTitle verticalAxisTitle,
            ExtendedDi3 genomeBrowserData)
        {
            _chartPlotter = chartPlotter;
            _horizontalAxisTitle = horizontalAxisTitle;
            _verticalAxisTitle = verticalAxisTitle;
            _genomeBrowserData = genomeBrowserData;
            _sampleLabelToAxisTick = new Dictionary<uint, Tuple<int, string>>();
            _YAxis = new NonOverlappingYAxis();
            _selectedERAndNeighbors = new HashSet<uint>();
        }

        private ChartPlotter _chartPlotter { set; get; }
        private PlotOptions _plotOptions { set; get; }
        private HorizontalAxisTitle _horizontalAxisTitle { set; get; }
        private VerticalAxisTitle _verticalAxisTitle { set; get; }
        private ExtendedDi3 _genomeBrowserData { set; get; }
        private AnalysisResult<Interval<int, MChIPSeqPeak>, MChIPSeqPeak>.ProcessedER _selectedER { set; get; }
        private Dictionary<uint, Tuple<int, string>> _sampleLabelToAxisTick { set; get; }
        private uint _selectedERSampleID { set; get; }
        private string _selectedChr { set; get; }
        private bool _includeGenes { set; get; }
        private bool _includeGeneralFeatures { set; get; }
        private Neighbors _neighbors { set; get; }
        private int _lineThickness { set; get; }
        private NonOverlappingYAxis _YAxis { set; get; }
        private HashSet<uint> _selectedERAndNeighbors { set; get; }

        public void Plot(
            AnalysisResult<Interval<int, MChIPSeqPeak>, MChIPSeqPeak>.ProcessedER selectedER,
            uint selectedERSampleID,
            string selectedChr,
            int dichotomies,
            bool includeGenes,
            bool includeGeneralFeatures,
            PlotOptions plotOptions)
        {
            _lineThickness = 4;
            _selectedER = selectedER;
            _selectedERSampleID = selectedERSampleID;
            _selectedChr = selectedChr;
            _includeGenes = includeGenes;
            _includeGeneralFeatures = includeGeneralFeatures;
            _plotOptions = plotOptions;
            _selectedERAndNeighbors.Clear();

            while (_chartPlotter.Children.Count > 12)
                _chartPlotter.Children.RemoveAt(_chartPlotter.Children.Count - 1);

            if (_selectedER == null) return;

            try
            {
                _YAxis.Reset();
                _neighbors = _genomeBrowserData.di3[selectedChr].FindNeighbors(_selectedER.er.left, dichotomies, includeGenes, includeGeneralFeatures);
                if (_plotOptions.showLegend) PlotLegand();
                UpdateSampleLabelToAxisTickAssociation();
                PlotNeighborERs();
                PlotNeighborGenes();
                PlotNeighborGeneralFeatures();
                PlotSelectedER();
                UpdateAxes();
                _chartPlotter.LegendVisible = _plotOptions.showLegend;
            }
            catch(Exception exception)
            {
                if(exception.GetType() == typeof(KeyNotFoundException) || 
                    exception.GetType() == typeof(IndexOutOfRangeException))
                {
                    MessageBox.Show("MuSERA Genome browser is busy and has not prepared the necessary data for your selected ER yet. \n" +
                        "Please try again later; it is recommended to wait until all intervals of the selected session are processed.",
                        "MuSERA is busy ...", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

        private void PlotLegand()
        {
            List<Point> nullPoints = new List<Point>();
            var nullPointsDS = new EnumerableDataSource<Point>(nullPoints);
            nullPointsDS.SetXMapping(x => x.X);
            nullPointsDS.SetYMapping(y => y.Y);

            _chartPlotter.AddLineGraph(nullPointsDS,
                new Pen() { Brush = _plotOptions.ColorClassificationsLine[ERClassificationType.StringentConfirmed], Thickness = _lineThickness },
                new PenDescription("Stringent Confirmed"));

            _chartPlotter.AddLineGraph(nullPointsDS,
                new Pen() { Brush = _plotOptions.ColorClassificationsLine[ERClassificationType.StringentDiscarded], Thickness = _lineThickness },
                new PenDescription("Stringent Discarded"));

            _chartPlotter.AddLineGraph(nullPointsDS,
                new Pen() { Brush = _plotOptions.ColorClassificationsLine[ERClassificationType.WeakConfirmed], Thickness = _lineThickness },
                new PenDescription("Weak Confirmed"));

            _chartPlotter.AddLineGraph(nullPointsDS,
                new Pen() { Brush = _plotOptions.ColorClassificationsLine[ERClassificationType.WeakDiscarded], Thickness = _lineThickness },
                new PenDescription("Weak Discarded"));

            if (_includeGenes)
                _chartPlotter.AddLineGraph(nullPointsDS,
                    new Pen() { Brush = _plotOptions.ColorGenesLine, Thickness = _lineThickness / 2.0 },
                    new PenDescription("Refseq genes"));

            if (_includeGeneralFeatures)
            {
                var dGF = new HashSet<byte>(); // determined general features

                foreach (var generalFeature in _neighbors.GeneralFeatures)
                {

                }
                /*if (((Data_Source)Application.Current.Properties["Data Source"]).features_Data != null)
            {
                var detFea = ((Data_Source)Application.Current.Properties["Data Source"]).features_Data.determined_Features;
                for (int i = 0; i < detFea.Count; i++)
                {
                    _chartPlotter.AddLineGraph(nullPointsDS,
                    new Pen() { Brush = FPC[i], Thickness = lineThickness / 2.0 },
                    new PenDescription(detFea[i][0]));
                }
            }*/
            }
        }
        private void PlotSelectedER()
        {
            int Y = _sampleLabelToAxisTick[_selectedERSampleID].Item1;
            int
                minY = Y,
                maxY = Y,
                minX = _selectedER.er.left,
                maxX = _selectedER.er.right;

            List<PointER> selectedERPoints = new List<PointER>();
            selectedERPoints.Add(new PointER()
            {
                x = _selectedER.er.left,
                y = Y,
                left = _selectedER.er.left,
                right = _selectedER.er.right,
                name = _selectedER.er.metadata.name,
                pValue = _selectedER.er.metadata.value
            });
            selectedERPoints.Add(new PointER()
            {
                x = _selectedER.er.right,
                y = Y,
                left = _selectedER.er.left,
                right = _selectedER.er.right,
                name = _selectedER.er.metadata.name,
                pValue = _selectedER.er.metadata.value
            });

            var selectedERPointsDS = new EnumerableDataSource<PointER>(selectedERPoints);
            selectedERPointsDS.SetXMapping(x => x.x);
            selectedERPointsDS.SetYMapping(y => y.y);
            selectedERPointsDS.AddMapping(CircleElementPointMarker.ToolTipTextProperty,
                    t => string.Format("Start : {0}\nStop : {1}\nName : {2}\np-value : {3}", t.left.ToString(), t.right.ToString(), t.name, t.pValue.ToString()));

            var penDescription = new PenDescription("   " + _selectedER.er.metadata.name + "   ");
            penDescription.LegendItem.Visibility = (_plotOptions.showLegend == true ? Visibility.Visible : Visibility.Collapsed);

            _chartPlotter.AddLineGraph(
                selectedERPointsDS,
                new Pen(new SolidColorBrush(Color.FromArgb(255, 50, 200, 255)), 10),
                new CircleElementPointMarker { Size = 12, Fill = new SolidColorBrush(Color.FromArgb(255, 50, 200, 255)), Brush = new SolidColorBrush(Color.FromArgb(255, 50, 200, 255)) },
                penDescription);

            _selectedERAndNeighbors.Add(_selectedER.er.hashKey);
            if (_selectedER.supportingERs != null)
            {
                foreach (var supportingER in _selectedER.supportingERs)
                {
                    _selectedERAndNeighbors.Add(supportingER.er.hashKey);
                    Y = _sampleLabelToAxisTick[supportingER.sampleIndex].Item1;

                    minY = Math.Min(minY, Y);
                    maxY = Math.Max(maxY, Y);

                    minX = Math.Min(minX, supportingER.er.left);
                    maxX = Math.Max(maxX, supportingER.er.right);

                    List<PointER> supERsPoints = new List<PointER>();
                    supERsPoints.Add(new PointER()
                    {
                        x = supportingER.er.left,
                        y = Y,
                        left = supportingER.er.left,
                        right = supportingER.er.right,
                        name = supportingER.er.metadata.name,
                        pValue = supportingER.er.metadata.value
                    });
                    supERsPoints.Add(new PointER()
                    {
                        x = supportingER.er.right,
                        y = Y,
                        left = supportingER.er.left,
                        right = supportingER.er.right,
                        name = supportingER.er.metadata.name,
                        pValue = supportingER.er.metadata.value
                    });

                    var supERsDS = new EnumerableDataSource<PointER>(supERsPoints);
                    supERsDS.SetXMapping(x => x.x);
                    supERsDS.SetYMapping(y => y.y);
                    supERsDS.AddMapping(CircleElementPointMarker.ToolTipTextProperty,
                    t => string.Format("Start : {0}\nStop : {1}\nName : {2}\np-value : {3}", t.left.ToString(), t.right.ToString(), t.name, t.pValue.ToString()));

                    var supERPenDescription = new PenDescription("   " + supportingER.er.metadata.name + "   ");
                    supERPenDescription.LegendItem.Visibility = (_plotOptions.showLegend == true ? Visibility.Visible : Visibility.Collapsed);

                    _chartPlotter.AddLineGraph(
                        supERsDS,
                        new Pen(Brushes.MediumPurple, 10),
                        new CircleElementPointMarker { Size = 12, Fill = Brushes.MediumPurple, Brush = Brushes.MediumPurple },
                        supERPenDescription);
                }
            }

            int hExtension = (int)Math.Ceiling(((maxY - minY) * 60.0) / 100.0);
            int wExtension = ((maxX - minX) * 10) / 100;
            _chartPlotter.Viewport.Visible =
                new Rect(minX - wExtension, minY - hExtension, (maxX - minX) + (2 * wExtension), maxY + (2 * hExtension));
        }
        private void PlotNeighborERs()
        {
            List<Tuple<uint, Interval<int, MChIPSeqPeak>, ERClassificationType[]>> ERs = _neighbors.ERs;
            foreach (var tuple in ERs)
            {
                if (_selectedERAndNeighbors.Contains(tuple.Item2.hashKey)) continue;
                List<PointER> points = new List<PointER>();
                points.Add(new PointER()
                {
                    x = tuple.Item2.left,
                    y = _sampleLabelToAxisTick[tuple.Item1].Item1,
                    left = tuple.Item2.left,
                    right = tuple.Item2.right,
                    name = tuple.Item2.metadata.name,
                    pValue = tuple.Item2.metadata.value
                });
                points.Add(new PointER()
                {
                    x = tuple.Item2.right,
                    y = _sampleLabelToAxisTick[tuple.Item1].Item1,
                    left = tuple.Item2.left,
                    right = tuple.Item2.right,
                    name = tuple.Item2.metadata.name,
                    pValue = tuple.Item2.metadata.value
                });

                var pointsDS = new EnumerableDataSource<PointER>(points);
                pointsDS.SetXMapping(x => x.x);
                pointsDS.SetYMapping(y => y.y);
                pointsDS.AddMapping(CircleElementPointMarker.ToolTipTextProperty,
                    t => string.Format("Start : {0}\nStop : {1}\nName : {2}\np-value : {3}", t.left.ToString(), t.right.ToString(), t.name, t.pValue.ToString()));

                Pen pen = new Pen();
                pen.Brush = _plotOptions.ColorClassificationsLine[tuple.Item3[0]];
                pen.Thickness = _lineThickness;

                PenDescription penDescription = new PenDescription();
                penDescription.LegendItem.Visibility = Visibility.Collapsed;
                _chartPlotter.AddLineGraph(
                    pointsDS,
                    pen,
                    new CircleElementPointMarker()
                    {
                        Size = 8,
                        Brush = _plotOptions.ColorClassificationsMarker[tuple.Item3[0]],
                        Fill = _plotOptions.ColorClassificationsMarker[tuple.Item3[0]]
                    },
                    penDescription);
            }
        }
        private void PlotNeighborGenes()
        {
            double yCoordinate = 0;
            List<Tuple<uint, Interval<int, MRefSeqGenes>>> genes = _neighbors.RefSeqGenes;
            foreach (var gene in genes)
            {
                var points = new List<PointGene>();
                yCoordinate = _YAxis.Coordinate(gene.Item1, gene.Item2.left, gene.Item2.right, _sampleLabelToAxisTick[gene.Item1].Item1);

                points.Add(new PointGene()
                {
                    x = gene.Item2.left,
                    y = yCoordinate,
                    left = gene.Item2.left,
                    right = gene.Item2.right,
                    officialGeneSymbol = gene.Item2.metadata.officialGeneSymbol,
                    refSeqID = gene.Item2.metadata.refSeqID
                });
                points.Add(new PointGene()
                {
                    x = gene.Item2.right,
                    y = yCoordinate,
                    left = gene.Item2.left,
                    right = gene.Item2.right,
                    officialGeneSymbol = gene.Item2.metadata.officialGeneSymbol,
                    refSeqID = gene.Item2.metadata.refSeqID
                });

                var pointsDS = new EnumerableDataSource<PointGene>(points);
                pointsDS.SetXMapping(x => x.x);
                pointsDS.SetYMapping(y => y.y);
                pointsDS.AddMapping(CircleElementPointMarker.ToolTipTextProperty,
                    t => string.Format("Start : {0}\nStop : {1}\nRefSeqID : {2}\nGene Symbol : {3}", t.left.ToString(), t.right.ToString(), t.refSeqID.ToString(), t.officialGeneSymbol.ToString()));

                Pen pen = new Pen();
                pen.Brush = _plotOptions.ColorGenesLine;
                pen.Thickness = _lineThickness / 2.0;

                PenDescription penDescription = new PenDescription();
                penDescription.LegendItem.Visibility = Visibility.Collapsed;

                _chartPlotter.AddLineGraph(
                    pointsDS,
                    pen,
                    new CircleElementPointMarker()
                    {
                        Size = 8,
                        Brush = _plotOptions.ColorGenesMarker,
                        Fill = _plotOptions.ColorGenesMarker
                    },
                    penDescription);
            }
        }
        private void PlotNeighborGeneralFeatures()
        {
            List<Tuple<uint, Interval<int, MGeneralFeatures>>> features = _neighbors.GeneralFeatures;
            foreach (var feature in features)
            {
                List<Point> points = new List<Point>();

                points.Add(new Point() { X = feature.Item2.left, Y = _sampleLabelToAxisTick[feature.Item1].Item1 });
                points.Add(new Point() { X = feature.Item2.right, Y = _sampleLabelToAxisTick[feature.Item1].Item1 });

                var pointsDS = new EnumerableDataSource<Point>(points);
                pointsDS.SetXMapping(x => x.X);
                pointsDS.SetYMapping(y => y.Y);

                Pen pen = new Pen();
                pen.Brush = new SolidColorBrush(Color.FromArgb(255, 255, 53, 0)); // TODO: this color shall be changed with respect to feature type.
                pen.Thickness = _lineThickness / 4.0;

                PenDescription penDescription = new PenDescription();
                penDescription.LegendItem.Visibility = Visibility.Collapsed;

                _chartPlotter.AddLineGraph(
                    pointsDS,
                    pen,
                    new TrianglePointMarker()
                    {
                        Size = 8,
                        Pen = pen,
                        Fill = new SolidColorBrush(Colors.Transparent)
                    },
                    penDescription);
            }
        }


        // TODO: a function to get stored colors that are open to public and user can change is needed.

        private void UpdateSampleLabelToAxisTickAssociation()
        {
            /// TODO: This dictionary does not need to be cleared and 
            /// re-populated if neighter _di3 nor labels 
            /// nor other rtv (e.g., include genes or general features)
            /// is changed. Try updating the calls so that I know whether 
            /// the index or the labels are changed so re-population is 
            /// required, otherwise skip all this procedure.
            _sampleLabelToAxisTick.Clear();

            int eCounter = 1, fCounter = -1;
            foreach (var indexedSample in _genomeBrowserData.di3[_selectedChr].indexedSamples)
            {
                switch (indexedSample.Value.intervalType)
                {
                    case IntervalType.ER:
                        foreach (var sample in ApplicationViewModel.Default.cachedDataSummary)
                            if (sample.fileHashKey == indexedSample.Key)
                            {
                                if (sample.fileHashKey == _selectedERSampleID)
                                    _sampleLabelToAxisTick.Add(sample.fileHashKey, new Tuple<int, string>(0, sample.label));
                                else
                                    _sampleLabelToAxisTick.Add(sample.fileHashKey, new Tuple<int, string>(eCounter++, sample.label));
                                break;
                            }
                        break;

                    case IntervalType.Gene:
                    case IntervalType.GeneralFeature:
                        if ((indexedSample.Value.intervalType == IntervalType.Gene && _includeGenes) ||
                            (indexedSample.Value.intervalType == IntervalType.GeneralFeature && _includeGeneralFeatures))
                            foreach (var feature in ApplicationViewModel.Default.cachedFeaturesSummary)
                                if (feature.fileHashKey == indexedSample.Key)
                                {
                                    _sampleLabelToAxisTick.Add(feature.fileHashKey, new Tuple<int, string>(fCounter--, feature.label));
                                    break;
                                }
                        break;
                }
            }
        }
        private void UpdateAxes()
        {
            _verticalAxisTitle.Content = "Samples";
            _horizontalAxisTitle.Content = "Genome position";
            ((VerticalAxis)_chartPlotter.VerticalAxis).AxisControl.TickSize = 0;
            ((VerticalAxis)_chartPlotter.VerticalAxis).ShowMinorTicks = false;
            ((VerticalAxis)_chartPlotter.VerticalAxis).ShowMayorLabels = true;
            ((HorizontalAxis)_chartPlotter.HorizontalAxis).ShowMayorLabels = true;
            ((HorizontalAxis)_chartPlotter.HorizontalAxis).ShowMinorTicks = true;
            ((HorizontalAxis)_chartPlotter.HorizontalAxis).Visibility = Visibility.Visible;
            ((HorizontalAxis)_chartPlotter.HorizontalAxis).LabelProvider = new ExponentialLabelProvider();
            ((VerticalAxis)_chartPlotter.VerticalAxis).LabelProvider.CustomFormatter = (tickinfo) =>
            {
                foreach (var item in _sampleLabelToAxisTick)
                    if (item.Value.Item1 == tickinfo.Tick)
                        return item.Value.Item2;
                return "";
            };
        }

        /*
        internal class LabelProvider : NumericLabelProviderBase
        {
            public override UIElement[] CreateLabels(ITicksInfo<double> ticksInfo)
            {
                var ticks = ticksInfo.Ticks;
                Init(ticks);
                
                UIElement[] res = new UIElement[ticks.Length];
                LabelTickInfo<double> tickInfo = new LabelTickInfo<double> { Info = ticksInfo.Info };
                for (int i = 0; i < res.Length; i++)
                {
                    tickInfo.Tick = ticks[i];
                    tickInfo.Index = i;
                    string labelText = "";

                    if (Convert.ToInt32(tickInfo.Tick) == 1)
                    {
                        labelText = "High";
                    }
                    else if (Convert.ToInt32(tickInfo.Tick) == 0)
                    {
                        labelText = "Medium";
                    }
                    else if (Convert.ToInt32(tickInfo.Tick) == -1)
                    {
                        labelText = "Low";
                    }
                    else
                    {
                        labelText = "";
                    }

                    TextBlock label = (TextBlock)GetResourceFromPool();
                    if (label == null)
                    {
                        label = new TextBlock();
                    }

                    label.Text = labelText;
                    label.ToolTip = ticks[i].ToString();

                    res[i] = label;

                    ApplyCustomView(tickInfo, label);
                }
                return null;//res;
            }
        }*/
    }
}
