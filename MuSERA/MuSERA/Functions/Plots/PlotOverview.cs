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
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Polimi.DEIB.VahidJalili.MuSERA.Functions.Plots
{
    internal class PlotOverview
    {
        public PlotOverview(ChartPlotter chartPlotter, HorizontalAxisTitle horizontalAxisTitle, VerticalAxisTitle verticalAxisTitle)
        {
            _chartPlotter = chartPlotter;
            _horizontalAxisTitle = horizontalAxisTitle;
            _verticalAxisTitle = verticalAxisTitle;
            _colorDLC = 0.1;


            _bars = new List<RectangleHighlight>();
            for (int i = 0; i < 13; i++)
                _bars.Add(new RectangleHighlight());

            _txts = new List<ViewportUIContainer>();
            for (int i = 0; i < 8; i++)
                _txts.Add(new ViewportUIContainer());
        }

        private ChartPlotter _chartPlotter { set; get; }
        private HorizontalAxisTitle _horizontalAxisTitle { set; get; }
        private VerticalAxisTitle _verticalAxisTitle { set; get; }
        private AnalysisResult<Interval<int, MChIPSeqPeak>, MChIPSeqPeak> _data { set; get; }
        private uint _totalERsCount { set; get; }
        private double _height { set; get; }
        private double _width { set; get; }
        private PlotOptions _plotOptions { set; get; }
        private List<RectangleHighlight> _bars { set; get; }
        private List<ViewportUIContainer> _txts { set; get; }
        private List<LegendInfo> _legendInfo { set; get; }

        /// <summary>
        /// Sets and gets the coefficient used to Darken/Lighten colors (if required).
        /// </summary>
        private double _colorDLC { set; get; }

        public void Plot(AnalysisResult<Interval<int, MChIPSeqPeak>, MChIPSeqPeak> data, PlotOptions plotOptions)
        {
            _data = data;
            _totalERsCount = _data.total____s + _data.total____w;
            _plotOptions = plotOptions;
            _plotOptions.totalERsCount = _data.total____s + _data.total____w;
            _plotOptions.plotHeight = _chartPlotter.ActualHeight;
            _plotOptions.plotWidth = _chartPlotter.ActualWidth;
            if (plotOptions.actualDataLabelFontSize <= 1) plotOptions.actualDataLabelFontSize = 1;

            while (_chartPlotter.Children.Count > 12)
                _chartPlotter.Children.RemoveAt(_chartPlotter.Children.Count - 1);

            DrawPlot();
            GenerateLegend();
            UpdateChartPlotter();
            UpdateAxes();
        }

        private void DrawPlot()
        {
            double drawingStart = _plotOptions.actualBarWidth;
            double shiftTXT = _plotOptions.actualBarWidth / 2;

            _bars[0].Bounds = new Rect(drawingStart, 0, _plotOptions.actualBarWidth, _totalERsCount);
            _bars[0].Fill = _plotOptions.ColorClassificationsLine[ERClassificationType.Input];
            _bars[0].Stroke = new SolidColorBrush(Colors.Black);
            _bars[0].StrokeThickness = _plotOptions.strokeThickness;
            _txts[0].Position = new Point(drawingStart + shiftTXT, _totalERsCount + _plotOptions.actualDataLabelDistance);
            _txts[0].Content = new Label
            {
                Content = "Total ERs\n" + _totalERsCount.ToString(),
                FontSize = _plotOptions.actualDataLabelFontSize,
                FontWeight = FontWeights.Normal,
                Background = new SolidColorBrush(Colors.White)
            };


            drawingStart = (_plotOptions.actualBarWidth * 1) + (_plotOptions.actualBarWidth + _plotOptions.actualInterBarGap);

            _bars[1].Bounds = new Rect(drawingStart, 0, _plotOptions.actualBarWidth, _data.total____s);
            _bars[1].Fill = _plotOptions.ColorClassificationsLine[ERClassificationType.Stringent];
            _bars[1].Stroke = new SolidColorBrush(Colors.Black);
            _bars[1].StrokeThickness = _plotOptions.strokeThickness;
            _txts[1].Position = new Point(drawingStart + shiftTXT, _data.total____s + _plotOptions.actualDataLabelDistance);
            _txts[1].Content = new Label
            {
                Content = "Stringent ERs\n" + _data.total____s.ToString() + "\t( " + Math.Round(_data.total____s * 100.0 / _totalERsCount, 2).ToString() + " % )",
                FontSize = _plotOptions.actualDataLabelFontSize,
                FontWeight = FontWeights.Normal,
                Background = new SolidColorBrush(Colors.White)
            };


            drawingStart = (_plotOptions.actualBarWidth * 2) + (_plotOptions.actualBarWidth + (_plotOptions.actualInterBarGap * 2));

            _bars[2].Bounds = new Rect(drawingStart, 0, _plotOptions.actualBarWidth, _data.total____w);
            _bars[2].Fill = _plotOptions.ColorClassificationsLine[ERClassificationType.Weak];
            _bars[2].Stroke = new SolidColorBrush(Colors.Black);
            _bars[2].StrokeThickness = _plotOptions.strokeThickness;
            _txts[2].Position = new Point(drawingStart + shiftTXT, _data.total____w + _plotOptions.actualDataLabelDistance);
            _txts[2].Content = new Label
            {
                Content = "Weak ERs\n" + _data.total____w.ToString() + "\t( " + Math.Round(_data.total____w * 100.0 / _totalERsCount, 2).ToString() + " % )",
                FontSize = _plotOptions.actualDataLabelFontSize,
                FontWeight = FontWeights.Normal,
                Background = new SolidColorBrush(Colors.White)
            };


            drawingStart = (_plotOptions.actualBarWidth * 3) + (_plotOptions.actualBarWidth + (_plotOptions.actualInterBarGap * 3));

            _bars[3].Bounds = new Rect(drawingStart, 0, _plotOptions.actualBarWidth, _data.total___sc);
            _bars[3].Fill = _plotOptions.ColorClassificationsLine[ERClassificationType.StringentConfirmed];
            _bars[3].Stroke = new SolidColorBrush(Colors.Black);
            _bars[3].StrokeThickness = _plotOptions.strokeThickness;

            _bars[4].Bounds = new Rect(drawingStart, _data.total___sc, _plotOptions.actualBarWidth, _data.total___wc);
            _bars[4].Fill = _plotOptions.ColorClassificationsLine[ERClassificationType.WeakConfirmed];
            _bars[4].Stroke = new SolidColorBrush(Colors.Black);
            _bars[4].StrokeThickness = _plotOptions.strokeThickness;

            _txts[3].Position = new Point(drawingStart + shiftTXT, _data.total___sc + _data.total___wc + _plotOptions.actualDataLabelDistance);
            _txts[3].Content = new Label
            {
                Content = "Confirmed ERs\n" + (_data.total___sc + _data.total___wc).ToString() + "\t( " + Math.Round((_data.total___sc + _data.total___wc) * 100.0 / _totalERsCount, 2).ToString() + " % )",
                FontSize = _plotOptions.actualDataLabelFontSize,
                FontWeight = FontWeights.Normal,
                Background = new SolidColorBrush(Colors.White)
            };


            drawingStart = (_plotOptions.actualBarWidth * 4) + (_plotOptions.actualBarWidth + (_plotOptions.actualInterBarGap * 4));

            _bars[5].Bounds = new Rect(drawingStart, 0, _plotOptions.actualBarWidth, _data.total__sdc);
            _bars[5].Fill = _plotOptions.ColorClassificationsLine[ERClassificationType.StringentDiscarded];
            _bars[5].Stroke = new SolidColorBrush(Colors.Black);
            _bars[5].StrokeThickness = _plotOptions.strokeThickness;

            _bars[6].Bounds = new Rect(drawingStart, _data.total__sdc, _plotOptions.actualBarWidth, _data.total__sdt);
            _bars[6].Fill = _plotOptions.ColorSDT;
            _bars[6].Stroke = new SolidColorBrush(Colors.Black);
            _bars[6].StrokeThickness = _plotOptions.strokeThickness;

            _bars[7].Bounds = new Rect(drawingStart, _data.total__sdc + _data.total__sdt, _plotOptions.actualBarWidth, _data.total__wdc);
            _bars[7].Fill = _plotOptions.ColorClassificationsLine[ERClassificationType.WeakDiscarded];
            _bars[7].Stroke = new SolidColorBrush(Colors.Black);
            _bars[7].StrokeThickness = _plotOptions.strokeThickness;

            _bars[8].Bounds = new Rect(drawingStart, _data.total__sdc + _data.total__sdt + _data.total__wdc, _plotOptions.actualBarWidth, _data.total__wdt);
            _bars[8].Fill = _plotOptions.ColorWDT;
            _bars[8].Stroke = new SolidColorBrush(Colors.Black);
            _bars[8].StrokeThickness = _plotOptions.strokeThickness;

            _txts[4].Position = new Point(drawingStart + shiftTXT, (_data.total__sdc + _data.total__sdt + _data.total__wdc + _data.total__wdt) + _plotOptions.actualDataLabelDistance);
            _txts[4].Content = new Label
            {
                Content = "Discarded ERs\n" + (_data.total__sdc + _data.total__sdt + _data.total__wdc + _data.total__wdt).ToString() + "\t( " + Math.Round((_data.total__sdc + _data.total__sdt + _data.total__wdc + _data.total__wdt) * 100.0 / _totalERsCount, 2).ToString() + " % )",
                FontSize = _plotOptions.actualDataLabelFontSize,
                FontWeight = FontWeights.Normal,
                Background = new SolidColorBrush(Colors.White)
            };


            drawingStart = (_plotOptions.actualBarWidth * 5) + (_plotOptions.actualBarWidth + (_plotOptions.actualInterBarGap * 5));

            _bars[9].Bounds = new Rect(drawingStart, 0, _plotOptions.actualBarWidth, _data.total___so);
            _bars[9].Fill = _plotOptions.ColorSO;
            _bars[9].Stroke = new SolidColorBrush(Colors.Black);
            _bars[9].StrokeThickness = _plotOptions.strokeThickness;

            _bars[10].Bounds = new Rect(drawingStart, _data.total___so, _plotOptions.actualBarWidth, _data.total___wo);
            _bars[10].Fill = _plotOptions.ColorWO;
            _bars[10].Stroke = new SolidColorBrush(Colors.Black);
            _bars[10].StrokeThickness = _plotOptions.strokeThickness;

            _txts[5].Position = new Point(drawingStart + shiftTXT, _data.total____o + _plotOptions.actualDataLabelDistance);
            _txts[5].Content = new Label
            {
                Content = "Output set\n" + _data.total____o.ToString() + "\t( " + Math.Round(_data.total____o * 100.0 / _totalERsCount, 2).ToString() + " % )",
                FontSize = _plotOptions.actualDataLabelFontSize,
                FontWeight = FontWeights.Normal,
                Background = new SolidColorBrush(Colors.White)
            };


            drawingStart = (_plotOptions.actualBarWidth * 6) + (_plotOptions.actualBarWidth + (_plotOptions.actualInterBarGap * 6));

            _bars[11].Bounds = new Rect(drawingStart, 0, _plotOptions.actualBarWidth, _data.total___TP);
            _bars[11].Fill = _plotOptions.ColorClassificationsLine[ERClassificationType.TruePositive];
            _bars[11].Stroke = new SolidColorBrush(Colors.Black);
            _bars[11].StrokeThickness = _plotOptions.strokeThickness;

            _txts[6].Position = new Point(drawingStart + shiftTXT, _data.total___TP + _plotOptions.actualDataLabelDistance);
            _txts[6].Content = new Label
            {
                Content = "mtc\n" + _data.total___TP.ToString() + "\t( " + Math.Round(_data.total___TP * 100.0 / _data.total____o, 2).ToString() + " % )",
                FontSize = _plotOptions.actualDataLabelFontSize,
                FontWeight = FontWeights.Normal,
                Background = new SolidColorBrush(Colors.White)
            };


            drawingStart = (_plotOptions.actualBarWidth * 7) + (_plotOptions.actualBarWidth + (_plotOptions.actualInterBarGap * 7));

            _bars[12].Bounds = new Rect(drawingStart, 0, _plotOptions.actualBarWidth, _data.total___FP);
            _bars[12].Fill = _plotOptions.ColorClassificationsLine[ERClassificationType.FalsePositive];
            _bars[12].Stroke = new SolidColorBrush(Colors.Black);
            _bars[12].StrokeThickness = _plotOptions.strokeThickness;

            _txts[7].Position = new Point(drawingStart + shiftTXT, _data.total___FP + _plotOptions.actualDataLabelDistance);
            _txts[7].Content = new Label
            {
                Content = "mtd\n" + _data.total___FP.ToString() + "\t( " + Math.Round(_data.total___FP * 100.0 / _data.total____o, 2).ToString() + " % )",
                FontSize = _plotOptions.actualDataLabelFontSize,
                FontWeight = FontWeights.Normal,
                Background = new SolidColorBrush(Colors.White)
            };
        }
        private void GenerateLegend()
        {
            _legendInfo = new List<LegendInfo>();

            _legendInfo.Add(new LegendInfo()
            {
                brush = Brushes.Transparent,
                lineDescription = "Overview\t"
            });

            _legendInfo.Add(new LegendInfo()
            {
                brush = _plotOptions.ColorClassificationsLine[ERClassificationType.Input],
                lineDescription = "Total ERs\t"
            });

            _legendInfo.Add(new LegendInfo()
            {
                brush = _plotOptions.ColorClassificationsLine[ERClassificationType.Stringent],
                lineDescription = "Stringent ERs\t"
            });

            _legendInfo.Add(new LegendInfo()
            {
                brush = _plotOptions.ColorClassificationsLine[ERClassificationType.Weak],
                lineDescription = "Weak ERs\t"
            });

            _legendInfo.Add(new LegendInfo()
            {
                brush = _plotOptions.ColorClassificationsLine[ERClassificationType.StringentConfirmed],
                lineDescription = "Stringent Confirmed\t"
            });

            _legendInfo.Add(new LegendInfo()
            {
                brush = _plotOptions.ColorClassificationsLine[ERClassificationType.WeakConfirmed],
                lineDescription = "Weak Confirmed\t"
            });

            _legendInfo.Add(new LegendInfo()
            {
                brush = _plotOptions.ColorClassificationsLine[ERClassificationType.StringentDiscarded],
                lineDescription = "Stringent Discarded (Reason : |R_ji,*| < C)\t"
            });

            _legendInfo.Add(new LegendInfo()
            {
                brush = _plotOptions.ColorSDT,
                lineDescription = "Stringent Discarded (Reason : TEST)\t"
            });

            _legendInfo.Add(new LegendInfo()
            {
                brush = _plotOptions.ColorClassificationsLine[ERClassificationType.WeakDiscarded],
                lineDescription = "Weak Discarded (Reason : |R_ji,*| < C)\t"
            });

            _legendInfo.Add(new LegendInfo()
            {
                brush =_plotOptions.ColorWDT,
                lineDescription = "Weak Discarded (Reason : TEST)\t"
            });

            _legendInfo.Add(new LegendInfo()
            {
                brush =_plotOptions.ColorSO,
                lineDescription = "Stringent Confirmed in Output set\t"
            });

            _legendInfo.Add(new LegendInfo()
            {
                brush =_plotOptions.ColorWO,
                lineDescription = "Weak Confirmed in Output set\t"
            });

            _legendInfo.Add(new LegendInfo()
            {
                brush = _plotOptions.ColorClassificationsLine[ERClassificationType.TruePositive],
                lineDescription = "Multiple-Testing Confirmed (mtc)\t"
            });

            _legendInfo.Add(new LegendInfo()
            {
                brush = _plotOptions.ColorClassificationsLine[ERClassificationType.FalsePositive],
                lineDescription = "Multiple-Testing Discarded (mtd)\t"
            });
        }
        private void UpdateChartPlotter()
        {
            foreach (var bar in _bars)
                _chartPlotter.Children.Add(bar);

            foreach (var txt in _txts)
                _chartPlotter.Children.Add(txt);

            List<Point> dummyPoints = GetDummyPoints();

            var dummyPointsDataSource = new EnumerableDataSource<Point>(dummyPoints);
            dummyPointsDataSource.SetXMapping(x => x.X);
            dummyPointsDataSource.SetYMapping(y => y.Y);

            List<Point> nullPoints = new List<Point>();
            var nullDataSource = new EnumerableDataSource<Point>(nullPoints);
            nullDataSource.SetXMapping(x => x.X);
            nullDataSource.SetYMapping(y => y.Y);

            foreach (var info in _legendInfo)
            {
                if (info.lineDescription == "Overview\t")
                {
                    _chartPlotter.AddLineGraph(
                        dummyPointsDataSource,
                        new Pen(info.brush, 0),
                        new CirclePointMarker { Size = 0, Fill = info.brush },
                        new PenDescription(info.lineDescription));
                }
                else if (_plotOptions.showOverviewLegend == true)
                {
                    _chartPlotter.AddLineGraph(
                        nullDataSource,
                        new Pen(info.brush, 4),
                        new CirclePointMarker { Size = 4, Fill = info.brush },
                        new PenDescription(info.lineDescription));
                }
            }

            if (_plotOptions.showOverviewLegend == true)
            {
                _height = Math.Ceiling(_chartPlotter.ActualHeight);
                _width = Math.Ceiling(_chartPlotter.ActualWidth);
            }

            _chartPlotter.Viewport.Visible = GetViewPortArea();
        }

        private double GetFontSize()
        {
            int fontSizeCoefficient = 14;
            return (_chartPlotter.ActualHeight * ((fontSizeCoefficient * 100) / 480)) / 100;
        }
        private Rect GetViewPortArea()
        {
            if (_height != 0 && _width != 0)
            {
                if (_width >= 900 && _width <= 1000 && _height >= 340 && _height <= 400)
                    return new Rect(0, -((_totalERsCount * 6) / 100), (_plotOptions.actualBarWidth * 13) + (_plotOptions.actualBarWidth + (_plotOptions.actualInterBarGap * 7)), _totalERsCount + ((_totalERsCount * 40) / 100));
                return new Rect(0, -((_totalERsCount * 6) / 100), (_plotOptions.actualBarWidth * 9) + (_plotOptions.actualBarWidth + (_plotOptions.actualInterBarGap * 7)), _totalERsCount + ((_totalERsCount * 60) / 100));
            }
            else
                return new Rect(0, -((_totalERsCount * 6) / 100), (_plotOptions.actualBarWidth * 9) + (_plotOptions.actualBarWidth + (_plotOptions.actualInterBarGap * 7)), _totalERsCount + ((_totalERsCount * 60) / 100));
        }
        private List<Point> GetDummyPoints()
        {
            List<Point> rtv = new List<Point>();
            rtv.Add(new Point() { X = 0, Y = -((_totalERsCount * 5) / 100) });
            rtv.Add(new Point() { X = (_plotOptions.actualBarWidth * 9) + (_plotOptions.actualBarWidth + (_plotOptions.actualInterBarGap * 7)), Y = _totalERsCount + ((_totalERsCount * 30) / 100) });
            return rtv;
        }
        private void UpdateAxes()
        {
            _horizontalAxisTitle.Content = "Categories";
            _verticalAxisTitle.Content = "ERs count\n";
            ((VerticalAxis)_chartPlotter.VerticalAxis).AxisControl.TickSize = 0;
            ((VerticalAxis)_chartPlotter.VerticalAxis).ShowMinorTicks = false;
            ((VerticalAxis)_chartPlotter.VerticalAxis).ShowMayorLabels = true;
            ((HorizontalAxis)_chartPlotter.HorizontalAxis).ShowMayorLabels = false;
            ((HorizontalAxis)_chartPlotter.HorizontalAxis).ShowMinorTicks = false;
            ((HorizontalAxis)_chartPlotter.HorizontalAxis).Visibility = Visibility.Collapsed;
            ((VerticalAxis)_chartPlotter.VerticalAxis).LabelProvider = new ExponentialLabelProvider();
        }
    }
}
