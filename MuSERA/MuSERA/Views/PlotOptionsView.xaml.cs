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

using Polimi.DEIB.VahidJalili.MuSERA.Models;
using Polimi.DEIB.VahidJalili.MuSERA.ViewModels;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Polimi.DEIB.VahidJalili.MuSERA.Views
{
    /// <summary>
    /// Interaction logic for PlotOptionsView.xaml
    /// </summary>
    public partial class PlotOptionsView : Window
    {
        internal PlotOptionsView(PlotOptions defaultOptions, uint totalERsCount)
        {
            InitializeComponent();

            var plotOptionsViewModel = new PlotOptionsViewModel(defaultOptions, totalERsCount);
            DataContext = plotOptionsViewModel;

            _dummyOptions = new PlotOptions(defaultOptions.plotHeight, defaultOptions.plotWidth, defaultOptions.totalERsCount);

            ShowLegend.IsChecked = defaultOptions.showLegend;
            ShowOverviewLegend.IsChecked = defaultOptions.showOverviewLegend;
            StrokeThickness_TB.Text = defaultOptions.strokeThickness.ToString();
            LegendLineThickness_TB.Text = defaultOptions.legendLineThickness.ToString();

            PDataLabelFontSize_TB.Text = defaultOptions.dataLabelFontSize.ToString();
            ADataLabelFontSize_TB.Text = defaultOptions.actualDataLabelFontSize.ToString();

            PDataLabelDistance_TB.Text = defaultOptions.dataLabelDistance.ToString();
            ADataLabelDistance_TB.Text = defaultOptions.actualDataLabelDistance.ToString();

            PInterBarGap_TB.Text = defaultOptions.interBarGap.ToString();
            AInterBarGap_TB.Text = defaultOptions.actualInterBarGap.ToString();

            PBarWidth_TB.Text = defaultOptions.barWidth.ToString();
            ABarWidth_TB.Text = defaultOptions.actualBarWidth.ToString();
        }

        internal PlotOptions Options
        {
            get
            {
                var options = new PlotOptions();
                options.showLegend = ShowLegend.IsChecked == true ? true : false;
                options.showOverviewLegend = ShowOverviewLegend.IsChecked == true ? true : false;
                options.strokeThickness = Convert.ToDouble(StrokeThickness_TB.Text);
                options.legendLineThickness = Convert.ToDouble(LegendLineThickness_TB.Text);

                options.dataLabelFontSize = Convert.ToDouble(PDataLabelFontSize_TB.Text);
                options.actualDataLabelFontSize = Convert.ToDouble(ADataLabelFontSize_TB.Text);

                options.dataLabelDistance = Convert.ToDouble(PDataLabelDistance_TB.Text);
                options.actualDataLabelDistance = Convert.ToDouble(ADataLabelDistance_TB.Text);

                options.interBarGap = Convert.ToDouble(PInterBarGap_TB.Text);
                options.actualInterBarGap = Convert.ToDouble(AInterBarGap_TB.Text);

                options.barWidth = Convert.ToDouble(PBarWidth_TB.Text);
                options.actualBarWidth = Convert.ToDouble(ABarWidth_TB.Text);
                return options;
            }
        }
        private PlotOptions _dummyOptions { set; get; }

        private void OK_BT_MouseEnter(object sender, MouseEventArgs e)
        {
            OK_BT.Foreground = new SolidColorBrush(Colors.White);
            OK_BT.Background = new SolidColorBrush(Color.FromArgb(255, 50, 155, 255));
        }
        private void OK_BT_MouseLeave(object sender, MouseEventArgs e)
        {
            OK_BT.Foreground = new SolidColorBrush(Color.FromArgb(255, 50, 155, 255));
            OK_BT.Background = new SolidColorBrush(Color.FromArgb(20, 255, 255, 255));
        }
        private void OK_BT_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void Cancel_BT_MouseEnter(object sender, MouseEventArgs e)
        {
            Cancel_BT.Foreground = new SolidColorBrush(Colors.White);
            Cancel_BT.Background = new SolidColorBrush(Color.FromArgb(255, 50, 155, 255));
        }
        private void Cancel_BT_MouseLeave(object sender, MouseEventArgs e)
        {
            Cancel_BT.Foreground = new SolidColorBrush(Color.FromArgb(255, 50, 155, 255));
            Cancel_BT.Background = new SolidColorBrush(Color.FromArgb(20, 255, 255, 255));
        }
        private void Cancel_BT_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DialogResult = false;
            Close();
        }


        private void TextBox_KeyDown_EventHandler(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.NumPad0:
                case Key.NumPad1:
                case Key.NumPad2:
                case Key.NumPad3:
                case Key.NumPad4:
                case Key.NumPad5:
                case Key.NumPad6:
                case Key.NumPad7:
                case Key.NumPad8:
                case Key.NumPad9:
                case Key.D0:
                case Key.D1:
                case Key.D2:
                case Key.D3:
                case Key.D4:
                case Key.D5:
                case Key.D6:
                case Key.D7:
                case Key.D8:
                case Key.D9:
                case Key.E:
                case Key.Subtract:
                case Key.OemMinus:
                    e.Handled = false;
                    break;

                case Key.Decimal:
                case Key.OemPeriod:
                    if (((TextBox)sender).Text.Contains('.'))
                        e.Handled = true;
                    else
                        e.Handled = false;
                    break;

                case Key.Enter:
                    break;

                case Key.Tab:
                case Key.System:
                    e.Handled = false;
                    break;

                default:
                    e.Handled = true;
                    break;
            }
        }
    }
}
