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
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace Polimi.DEIB.VahidJalili.MuSERA.Functions.Plots
{
    internal class PlotClassifications
    {
        public PlotClassifications(ChartPlotter chartPlotter, HorizontalAxisTitle horizontalAxisTitle, VerticalAxisTitle verticalAxisTitle)
        {
            _chartPlotter = chartPlotter;
            _horizontalAxisTitle = horizontalAxisTitle;
            _verticalAxisTitle = verticalAxisTitle;
        }

        private ChartPlotter _chartPlotter { set; get; }

        /// <summary>
        /// Gets the selected analysis results.
        /// </summary>
        private PValueDistributions _data { set; get; }
        private PlotOptions _plotOptions { set; get; }
        private HorizontalAxisTitle _horizontalAxisTitle { set; get; }
        private VerticalAxisTitle _verticalAxisTitle { set; get; }

        public void Plot(PValueDistributions data, PlotType plotType, PlotOptions plotOptions)
        {
            _data = data;
            _plotOptions = plotOptions;

            while (_chartPlotter.Children.Count > 12)
                _chartPlotter.Children.RemoveAt(_chartPlotter.Children.Count - 1);

            switch (plotType)
            {
                case PlotType.Classification_1st:
                    Plot_1st_Classification();
                    break;

                case PlotType.Classification_2nd_2in1:
                    Plot_2nd_Classification_2in1();
                    break;

                case PlotType.Classification_2nd_4in1:
                    Plot_2nd_Classification_4in1();
                    break;

                case PlotType.Classification_3rd:
                    Plot_3rd_Classification();
                    break;

                case PlotType.Classification_4th:
                    Plot_4th_Classification();
                    break;
            }

            UpdateAxes();
            _chartPlotter.FitToView();
        }
        private void Plot_1st_Classification()
        {
            var sERs = new SortedSet<Point>(new PointComparer());
            var wERs = new SortedSet<Point>(new PointComparer());

            foreach (var entry in _data)
                switch (entry.type)
                {
                    case ERClassificationType.Stringent:
                        sERs.Add(new Point(x: entry.logpValue, y: entry.frequency));
                        break;

                    case ERClassificationType.Weak:
                        wERs.Add(new Point(x: entry.logpValue, y: entry.frequency));
                        break;
                }

            if (sERs.Count > 0)
                PlotLineGraph(sERs,
                    _plotOptions.ColorClassificationsLine[ERClassificationType.Stringent],
                    _plotOptions.ColorClassificationsMarker[ERClassificationType.Stringent],
                    "Stringent ERs");

            if (wERs.Count > 0)
                PlotLineGraph(wERs,
                    _plotOptions.ColorClassificationsLine[ERClassificationType.Weak],
                    _plotOptions.ColorClassificationsMarker[ERClassificationType.Weak],
                    "Weak ERs");
        }
        private void Plot_2nd_Classification_2in1()
        {
            var cERs = new SortedSet<Point>(new PointComparer());
            var dERs = new SortedSet<Point>(new PointComparer());

            foreach (var entry in _data)
                switch (entry.type)
                {
                    case ERClassificationType.StringentConfirmed:
                    case ERClassificationType.WeakConfirmed:
                        cERs.Add(new Point(x: entry.logpValue, y: entry.frequency));
                        break;

                    case ERClassificationType.StringentDiscarded:
                    case ERClassificationType.WeakDiscarded:
                        dERs.Add(new Point(x: entry.logpValue, y: entry.frequency));
                        break;
                }

            if (cERs.Count > 0)
                PlotLineGraph(cERs,
                    _plotOptions.ColorClassificationsLine[ERClassificationType.Confirmed],
                    _plotOptions.ColorClassificationsMarker[ERClassificationType.Confirmed],
                    "Confirmed ERs");

            if (dERs.Count > 0)
                PlotLineGraph(dERs,
                    _plotOptions.ColorClassificationsLine[ERClassificationType.Discarded],
                    _plotOptions.ColorClassificationsMarker[ERClassificationType.Discarded],
                    "Discarded ERs");
        }
        private void Plot_2nd_Classification_4in1()
        {
            var scERs = new SortedSet<Point>(new PointComparer());
            var wcERs = new SortedSet<Point>(new PointComparer());
            var sdERs = new SortedSet<Point>(new PointComparer());
            var wdERs = new SortedSet<Point>(new PointComparer());

            foreach (var entry in _data)
                switch (entry.type)
                {
                    case ERClassificationType.StringentConfirmed:
                        scERs.Add(new Point(x: entry.logpValue, y: entry.frequency));
                        break;

                    case ERClassificationType.WeakConfirmed:
                        wcERs.Add(new Point(x: entry.logpValue, y: entry.frequency));
                        break;

                    case ERClassificationType.StringentDiscarded:
                        sdERs.Add(new Point(x: entry.logpValue, y: entry.frequency));
                        break;

                    case ERClassificationType.WeakDiscarded:
                        wdERs.Add(new Point(x: entry.logpValue, y: entry.frequency));
                        break;
                }

            if (scERs.Count > 0)
                PlotLineGraph(scERs,
                    _plotOptions.ColorClassificationsLine[ERClassificationType.StringentConfirmed],
                    _plotOptions.ColorClassificationsMarker[ERClassificationType.StringentConfirmed],
                    "Stringent-Confirmed ERs");

            if (wcERs.Count > 0)
                PlotLineGraph(wcERs,
                    _plotOptions.ColorClassificationsLine[ERClassificationType.WeakConfirmed],
                    _plotOptions.ColorClassificationsMarker[ERClassificationType.WeakConfirmed],
                    "Weak-Confirmed ERs");

            if (sdERs.Count > 0)
                PlotLineGraph(sdERs,
                    _plotOptions.ColorClassificationsLine[ERClassificationType.StringentDiscarded],
                    _plotOptions.ColorClassificationsMarker[ERClassificationType.StringentDiscarded],
                    "Stringent-Discarded ERs");

            if (wdERs.Count > 0)
                PlotLineGraph(wdERs,
                    _plotOptions.ColorClassificationsLine[ERClassificationType.WeakDiscarded],
                    _plotOptions.ColorClassificationsMarker[ERClassificationType.WeakDiscarded],
                    "Weak-Discarded ERs");
        }
        private void Plot_3rd_Classification()
        {
            var scERs = new SortedSet<Point>(new PointComparer());
            var wcERs = new SortedSet<Point>(new PointComparer());

            foreach (var entry in _data)
                switch (entry.type)
                {
                    case ERClassificationType.StringentConfirmed:
                        scERs.Add(new Point(x: entry.logpValue, y: entry.frequency));
                        break;

                    case ERClassificationType.WeakConfirmed:
                        wcERs.Add(new Point(x: entry.logpValue, y: entry.frequency));
                        break;
                }

            if (scERs.Count > 0)
                PlotLineGraph(scERs,
                    _plotOptions.ColorClassificationsLine[ERClassificationType.StringentConfirmed],
                    _plotOptions.ColorClassificationsMarker[ERClassificationType.StringentConfirmed],
                    "Stringent-Confirmed ERs");

            if (wcERs.Count > 0)
                PlotLineGraph(wcERs,
                    _plotOptions.ColorClassificationsLine[ERClassificationType.WeakConfirmed],
                    _plotOptions.ColorClassificationsMarker[ERClassificationType.WeakConfirmed],
                    "Weak-Confirmed ERs");
        }
        private void Plot_4th_Classification()
        {
            var tpERs = new SortedSet<Point>(new PointComparer());
            var fpERs = new SortedSet<Point>(new PointComparer());

            foreach (var entry in _data)
                switch (entry.type)
                {
                    case ERClassificationType.TruePositive:
                        tpERs.Add(new Point(x: entry.logpValue, y: entry.frequency));
                        break;

                    case ERClassificationType.FalsePositive:
                        fpERs.Add(new Point(x: entry.logpValue, y: entry.frequency));
                        break;
                }

            if (tpERs.Count > 0)
                PlotLineGraph(tpERs,
                    _plotOptions.ColorClassificationsLine[ERClassificationType.TruePositive],
                    _plotOptions.ColorClassificationsMarker[ERClassificationType.TruePositive],
                    "Multiple-Testing Confirmed ERs");

            if (fpERs.Count > 0)
                PlotLineGraph(fpERs,
                    _plotOptions.ColorClassificationsLine[ERClassificationType.FalsePositive],
                    _plotOptions.ColorClassificationsMarker[ERClassificationType.FalsePositive],
                    "Multiple-Testing Discarded ERs");
        }

        private void PlotLineGraph(SortedSet<Point> points, Brush lineColor, Brush markerColor, string lineDescription)
        {
            var pointsDataSource = new EnumerableDataSource<Point>(points);
            pointsDataSource.SetXMapping(x => x.X);
            pointsDataSource.SetYMapping(y => y.Y);

            var penDescription = new PenDescription(lineDescription);
            penDescription.LegendItem.Visibility = (_plotOptions.showLegend == true ? Visibility.Visible : Visibility.Collapsed);

            _chartPlotter.AddLineGraph(
                pointsDataSource,
                new Pen(lineColor, 0.5),
                new CirclePointMarker { Size = 4, Fill = markerColor },
                penDescription);
        }

        private void UpdateAxes()
        {
            _horizontalAxisTitle.Content = "\n-10 Log10(p-value)";
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
