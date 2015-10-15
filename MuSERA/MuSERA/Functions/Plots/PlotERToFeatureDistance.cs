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
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace Polimi.DEIB.VahidJalili.MuSERA.Functions.Plots
{
    internal class PlotERToFeatureDistance
    {
        public PlotERToFeatureDistance(
            ChartPlotter chartPlotter,
            HorizontalAxisTitle horizontalAxisTitle,
            VerticalAxisTitle verticalAxisTitle)
        {
            _chartPlotter = chartPlotter;
            _horizontalAxisTitle = horizontalAxisTitle;
            _verticalAxisTitle = verticalAxisTitle;
        }

        private ChartPlotter _chartPlotter { set; get; }
        private HorizontalAxisTitle _horizontalAxisTitle { set; get; }
        private VerticalAxisTitle _verticalAxisTitle { set; get; }
        private SortedDictionary<double, double> _data { set; get; }
        private PlotOptions _plotOptions { set; get; }

        public void Plot(SortedDictionary<double, double> data, PlotOptions plotOptions)
        {
            _data = data;
            _plotOptions = plotOptions;

            while (_chartPlotter.Children.Count > 12)
                _chartPlotter.Children.RemoveAt(_chartPlotter.Children.Count - 1);

            if (_data.Count > 0) PlotLineGraph(_data, Brushes.OrangeRed, Brushes.OrangeRed, "ER-to-Feature distance (bp)");
            UpdateAxes();
            _chartPlotter.FitToView();
        }
        private void PlotLineGraph(SortedDictionary<double, double> points, Brush lineColor, Brush markerColor, string lineDescription)
        {
            var pointsDataSource = new EnumerableDataSource<KeyValuePair<double, double>>(points);
            pointsDataSource.SetXMapping(x => x.Key);
            pointsDataSource.SetYMapping(y => y.Value);

            var penDescription = new PenDescription(lineDescription);
            penDescription.LegendItem.Visibility = (_plotOptions.showLegend == true ? Visibility.Visible : Visibility.Collapsed);

            _chartPlotter.AddLineGraph(
                pointsDataSource,
                new Pen(lineColor, 0),
                new CirclePointMarker { Size = 5, Fill = markerColor },
                penDescription);
        }
        private void UpdateAxes()
        {
            _horizontalAxisTitle.Content = "\nDistance to closest feature (bp)\n[-]Up-stream , [+]Down-stream";
            _verticalAxisTitle.Content = "Count\n";
            ((VerticalAxis)_chartPlotter.VerticalAxis).AxisControl.TickSize = 1;
            ((VerticalAxis)_chartPlotter.VerticalAxis).ShowMinorTicks = true;
            ((VerticalAxis)_chartPlotter.VerticalAxis).ShowMayorLabels = true;
            ((HorizontalAxis)_chartPlotter.HorizontalAxis).ShowMayorLabels = true;
            ((HorizontalAxis)_chartPlotter.HorizontalAxis).ShowMinorTicks = true;
            ((HorizontalAxis)_chartPlotter.HorizontalAxis).Visibility = Visibility.Visible;
            ((HorizontalAxis)_chartPlotter.HorizontalAxis).LabelProvider = new ExponentialLabelProvider();
            ((VerticalAxis)_chartPlotter.VerticalAxis).LabelProvider = new ExponentialLabelProvider();
        }
    }
}
