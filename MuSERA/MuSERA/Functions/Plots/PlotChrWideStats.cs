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
using Polimi.DEIB.VahidJalili.MuSERA.Models;
using Polimi.DEIB.VahidJalili.MuSERA.Warehouse;
using Polimi.DEIB.VahidJalili.MuSERA.Warehouse.Classes;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace Polimi.DEIB.VahidJalili.MuSERA.Functions.Plots
{
    internal class PlotChrWideStats
    {
        public PlotChrWideStats(
            ChartPlotter chartPlotter,
            HorizontalAxisTitle horizontalAxisTitle,
            VerticalAxisTitle verticalAxisTitle)
        {
            _chartPlotter = chartPlotter;
            _horizontalAxisTitle = horizontalAxisTitle;
            _verticalAxisTitle = verticalAxisTitle;
            _chrToXMapping = new Dictionary<string, int>();
            _sortedChrs = new List<string>();
            _pointComparer = new PointComparer();
        }

        private ChartPlotter _chartPlotter { set; get; }
        private HorizontalAxisTitle _horizontalAxisTitle { set; get; }
        private VerticalAxisTitle _verticalAxisTitle { set; get; }
        private PlotOptions _plotOptions { set; get; }
        private Dictionary<string, int> _chrToXMapping { set; get; }
        private List<string> _sortedChrs { set; get; }
        private Dictionary<string, ChrWideStat> _data { set; get; }
        private PointComparer _pointComparer { set; get; }

        public void Plot(Dictionary<string, ChrWideStat> data, PlotOptions plotOptions)
        {
            _data = data;
            _plotOptions = plotOptions;
            while (_chartPlotter.Children.Count > 12)
                _chartPlotter.Children.RemoveAt(_chartPlotter.Children.Count - 1);

            _sortedChrs.Clear();
            foreach (var chr in data)
                _sortedChrs.Add(chr.Key);
            _sortedChrs.Sort(new ChrComparer());

            _chrToXMapping.Clear();
            for (int i = 0; i < _sortedChrs.Count; i++)
                _chrToXMapping.Add(_sortedChrs[i], i);

            Overall();
            Stringent();
            Weak();
            Confirmed();
            Discarded();
            Output();
            TP();
            FP();

            UpdateAxes();
            _chartPlotter.FitToView();
        }
        private void Overall()
        {
            var points = new SortedSet<Point>(_pointComparer);
            foreach (var chr in _data)
                points.Add(new Point(_chrToXMapping[chr.Key], chr.Value.R_j__t_c));
            PlotLineGraph(
                points,
                _plotOptions.ColorClassificationsLine[ERClassificationType.Input],
                _plotOptions.ColorClassificationsMarker[ERClassificationType.Input],
                "Overall ERs",
                2,
                DashStyles.Solid);
        }
        private void Stringent()
        {
            var points = new SortedSet<Point>(_pointComparer);
            foreach (var chr in _data)
                points.Add(new Point(_chrToXMapping[chr.Key], chr.Value.R_j__s_c));
            PlotLineGraph(
                points,
                _plotOptions.ColorClassificationsLine[ERClassificationType.Stringent],
                _plotOptions.ColorClassificationsMarker[ERClassificationType.Stringent],
                "Stringent ERs",
                1.5,
                DashStyles.Solid);
        }
        private void Weak()
        {
            var points = new SortedSet<Point>(_pointComparer);
            foreach (var chr in _data)
                points.Add(new Point(_chrToXMapping[chr.Key], chr.Value.R_j__w_c));
            PlotLineGraph(
                points,
                _plotOptions.ColorClassificationsLine[ERClassificationType.Weak],
                _plotOptions.ColorClassificationsMarker[ERClassificationType.Weak],
                "Weak ERs",
                1.5,
                DashStyles.Dash);
        }
        private void Confirmed()
        {
            var points = new SortedSet<Point>(_pointComparer);
            foreach (var chr in _data)
                points.Add(new Point(_chrToXMapping[chr.Key], chr.Value.R_j__c_c));
            PlotLineGraph(
                points,
                _plotOptions.ColorClassificationsLine[ERClassificationType.Confirmed],
                _plotOptions.ColorClassificationsMarker[ERClassificationType.Confirmed],
                "Confirmed ERs",
                2,
                DashStyles.Solid);
        }
        private void Discarded()
        {
            var points = new SortedSet<Point>(_pointComparer);
            foreach (var chr in _data)
                points.Add(new Point(_chrToXMapping[chr.Key], chr.Value.R_j__d_c));
            PlotLineGraph(
                points,
                _plotOptions.ColorClassificationsLine[ERClassificationType.Discarded],
                _plotOptions.ColorClassificationsMarker[ERClassificationType.Discarded],
                "Discarded ERs",
                1.5,
                DashStyles.Dot);
        }
        private void Output()
        {
            var points = new SortedSet<Point>(_pointComparer);
            foreach (var chr in _data)
                points.Add(new Point(_chrToXMapping[chr.Key], chr.Value.R_j__o_c));
            PlotLineGraph(
                points,
                _plotOptions.ColorClassificationsLine[ERClassificationType.Output],
                _plotOptions.ColorClassificationsMarker[ERClassificationType.Output],
                "Output ERs",
                2.5,
                DashStyles.Solid);
        }
        private void TP()
        {
            var points = new SortedSet<Point>(_pointComparer);
            foreach (var chr in _data)
                points.Add(new Point(_chrToXMapping[chr.Key], chr.Value.R_j_TP_c));
            PlotLineGraph(
                points,
                _plotOptions.ColorClassificationsLine[ERClassificationType.TruePositive],
                _plotOptions.ColorClassificationsMarker[ERClassificationType.TruePositive],
                "Multiple-Testing Confirmed ERs",
                1.5,
                DashStyles.Solid);
        }
        private void FP()
        {
            var points = new SortedSet<Point>(_pointComparer);
            foreach (var chr in _data)
                points.Add(new Point(_chrToXMapping[chr.Key], chr.Value.R_j_FP_c));
            PlotLineGraph(
                points,
                _plotOptions.ColorClassificationsLine[ERClassificationType.FalsePositive],
                _plotOptions.ColorClassificationsMarker[ERClassificationType.FalsePositive],
                "Multiple-Testing Discarded ERs",
                1.5,
                DashStyles.DashDotDot);
        }
        private void PlotLineGraph(SortedSet<Point> points, Brush lineColor, Brush markerColor, string lineDescription, double lineThickness, DashStyle dashStyle)
        {
            var pointsDataSource = new EnumerableDataSource<Point>(points);
            pointsDataSource.SetXMapping(x => x.X);
            pointsDataSource.SetYMapping(y => y.Y);

            var penDescription = new PenDescription(lineDescription);
            penDescription.LegendItem.Visibility = (_plotOptions.showLegend == true ? Visibility.Visible : Visibility.Collapsed);

            _chartPlotter.AddLineGraph(
                pointsDataSource,
                new Pen(lineColor, lineThickness),
                new CirclePointMarker { Size = 4, Fill = markerColor },
                penDescription);
        }
        private void UpdateAxes()
        {
            _horizontalAxisTitle.Content = "\bChromosome";
            _verticalAxisTitle.Content = "ERs count\n";
            ((VerticalAxis)_chartPlotter.VerticalAxis).AxisControl.TickSize = 1;
            ((VerticalAxis)_chartPlotter.VerticalAxis).ShowMinorTicks = true;
            ((VerticalAxis)_chartPlotter.VerticalAxis).ShowMayorLabels = true;
            ((HorizontalAxis)_chartPlotter.HorizontalAxis).ShowMayorLabels = true;
            ((HorizontalAxis)_chartPlotter.HorizontalAxis).ShowMinorTicks = true;
            ((HorizontalAxis)_chartPlotter.HorizontalAxis).Visibility = Visibility.Visible;
            ((VerticalAxis)_chartPlotter.VerticalAxis).LabelProvider = new ExponentialLabelProvider();
            ((HorizontalAxis)_chartPlotter.HorizontalAxis).LabelProvider.CustomFormatter = (tickinfo) =>
            {
                foreach (var mapping in _chrToXMapping)
                    if (mapping.Value == tickinfo.Tick)
                        return mapping.Key;
                return "";
            };
        }
    }
}
