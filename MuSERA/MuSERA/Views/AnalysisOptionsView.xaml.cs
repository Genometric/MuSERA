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

using Polimi.DEIB.VahidJalili.MuSERA.ViewModels;
using Polimi.DEIB.VahidJalili.MuSERA.Warehouse;
using Polimi.DEIB.VahidJalili.MuSERA.XSquaredData;
using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Polimi.DEIB.VahidJalili.MuSERA.Views
{
    /// <summary>
    /// Interaction logic for AnalysisOptionsView.xaml
    /// </summary>
    public partial class AnalysisOptionsView : Window
    {
        string
            Default_Stringent_Threshold = "1.0E-08",
            Default__Weak__Threshold = "1.0E-04",

            /// if this Value is changed, make sure that Chi_sqrd_L is displaying correct Value
            Default___RTP__Threshold = "1.0E-08";

        public byte selectedSampleCount = 0;

        private bool Pressed_Button = false;

        //Microsoft.Office.Interop.Excel.Application Excel_app;
        //Microsoft.Office.Interop.Excel.WorksheetFunction Work_Sheet_Function;

        public AnalysisOptionsView()
        {
            InitializeComponent();
            DataContext = new AnalysisOptionsViewModel();
            //Excel_app = new Microsoft.Office.Interop.Excel.Application();
            //Work_Sheet_Function = Excel_app.WorksheetFunction;

            TauS_TB.Text = Default_Stringent_Threshold;
            TauW_TB.Text = Default__Weak__Threshold;
            Gamma_TB.Text = Default___RTP__Threshold;
        }

        public AnalysisOptions Options
        {
            get
            {
                var options = new AnalysisOptions();
                options.C = Convert.ToByte(C_TB.Text);
                options.alpha = float.Parse(alpha_TB.Text);
                options.tauS = Convert.ToDouble(TauS_TB.Text);
                options.tauW = Convert.ToDouble(TauW_TB.Text);
                options.gamma = Convert.ToDouble(Gamma_TB.Text);
                if (BH_RB.IsChecked == true) options.fDRProcedure = FDRProcedure.BenjaminiHochberg;
                options.replicateType = (BioRep_RB.IsChecked == true ? ReplicateType.Biological : ReplicateType.Technical);
                options.multipleIntersections = (MPI_O1.IsChecked == true ? MultipleIntersections.UseLowestPValue : MultipleIntersections.UseHighestPValue);

                return options;
            }
        }

        private void Run_Analysis_BT_MouseEnter(object sender, MouseEventArgs e)
        {
            Run_Analysis_BT.Foreground = new SolidColorBrush(Colors.White);
            Run_Analysis_BT.Background = new SolidColorBrush(Color.FromArgb(255, 50, 155, 255));
        }
        private void Run_Analysis_BT_MouseLeave(object sender, MouseEventArgs e)
        {
            Run_Analysis_BT.Foreground = new SolidColorBrush(Color.FromArgb(255, 50, 155, 255));
            Run_Analysis_BT.Background = new SolidColorBrush(Color.FromArgb(20, 255, 255, 255));
        }
        private void Run_Analysis_BT_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
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
            Pressed_Button = false;
            this.DialogResult = false;
            this.Close();
        }

        private void Tau_S_TB_KeyDown(object sender, KeyEventArgs e)
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
                case Key.OemPlus:
                case Key.Add:
                    e.Handled = false;
                    break;

                case Key.Decimal:
                case Key.OemPeriod:
                    bool Dot_Found = false;
                    for (int i = 0; i < TauS_TB.Text.Length; i++)
                    {
                        if (TauS_TB.Text[i] == '.')
                        {
                            Dot_Found = true;
                            i = TauS_TB.Text.Length;
                        }
                    }

                    e.Handled = Dot_Found;
                    break;

                case Key.Enter:
                    if (IsValidDouble(TauS_TB.Text))
                    {
                        double Parsed_Text = double.Parse(TauS_TB.Text);

                        if (Parsed_Text <= 1.0 && Parsed_Text >= 0.0)
                        {
                            TauS_TB.Text = Parsed_Text.ToString();
                        }
                        else
                        {
                            Parsed_Text = double.Parse(Default_Stringent_Threshold);
                            TauS_TB.Text = Parsed_Text.ToString();
                        }
                    }
                    else
                    {
                        TauS_TB.Text = double.Parse(Default_Stringent_Threshold).ToString();
                    }
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
        private void Tau_S_TB_LostFocus(object sender, RoutedEventArgs e)
        {
            if (IsValidDouble(TauS_TB.Text))
            {
                double Parsed_Text = double.Parse(TauS_TB.Text);

                if (Parsed_Text <= 1.0 && Parsed_Text >= 0.0)
                {
                    TauS_TB.Text = Parsed_Text.ToString();
                }
                else
                {
                    Parsed_Text = double.Parse(Default_Stringent_Threshold);
                    TauS_TB.Text = Parsed_Text.ToString();
                }
            }
            else
            {
                TauS_TB.Text = double.Parse(Default_Stringent_Threshold).ToString();
            }
        }

        private void Tau_W_TB_KeyDown(object sender, KeyEventArgs e)
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
                case Key.OemPlus:
                case Key.Add:
                    e.Handled = false;
                    break;

                case Key.Decimal:
                case Key.OemPeriod:
                    bool Dot_Found = false;
                    for (int i = 0; i < TauW_TB.Text.Length; i++)
                    {
                        if (TauW_TB.Text[i] == '.')
                        {
                            Dot_Found = true;
                            i = TauW_TB.Text.Length;
                        }
                    }

                    e.Handled = Dot_Found;
                    break;

                case Key.Enter:
                    if (IsValidDouble(TauW_TB.Text))
                    {
                        double Parsed_Text = double.Parse(TauW_TB.Text);

                        if (Parsed_Text <= 1.0 && Parsed_Text >= 0.0)
                        {
                            TauW_TB.Text = Parsed_Text.ToString();
                        }
                        else
                        {
                            Parsed_Text = double.Parse(Default__Weak__Threshold);
                            TauW_TB.Text = Parsed_Text.ToString();
                        }
                    }
                    else
                    {
                        TauW_TB.Text = double.Parse(Default__Weak__Threshold).ToString();
                    }
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
        private void Tau_W_TB_LostFocus(object sender, RoutedEventArgs e)
        {
            if (IsValidDouble(TauW_TB.Text))
            {
                double Parsed_Text = double.Parse(TauW_TB.Text);

                if (Parsed_Text <= 1.0 && Parsed_Text >= 0.0)
                {
                    TauW_TB.Text = Parsed_Text.ToString();
                }
                else
                {
                    Parsed_Text = double.Parse(Default__Weak__Threshold);
                    TauW_TB.Text = Parsed_Text.ToString();
                }
            }
            else
            {
                TauW_TB.Text = double.Parse(Default__Weak__Threshold).ToString();
            }
        }

        private void Gamma_TB_KeyDown(object sender, KeyEventArgs e)
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
                case Key.OemPlus:
                case Key.Add:
                    e.Handled = false;
                    break;

                case Key.Decimal:
                case Key.OemPeriod:
                    bool Dot_Found = false;
                    for (int i = 0; i < Gamma_TB.Text.Length; i++)
                    {
                        if (Gamma_TB.Text[i] == '.')
                        {
                            Dot_Found = true;
                            i = Gamma_TB.Text.Length;
                        }
                    }

                    e.Handled = Dot_Found;
                    break;

                case Key.Enter:
                    if (IsValidDouble(Gamma_TB.Text))
                    {
                        double Parsed_Text = double.Parse(Gamma_TB.Text);

                        if (Parsed_Text <= 1.0 && Parsed_Text >= 0.0 && Parsed_Text > 1E-20)
                        {
                            Gamma_TB.Text = Parsed_Text.ToString();
                            Chi_sqrd_L.Content = Math.Round(ChiSquaredCache.ChiSqrdINVRTP(Parsed_Text, (byte)(selectedSampleCount * 2)), 3).ToString();
                        }
                        else
                        {
                            Parsed_Text = double.Parse("1.0E-19");
                            Gamma_TB.Text = Parsed_Text.ToString();
                            Chi_sqrd_L.Content = Math.Round(ChiSquaredCache.ChiSqrdINVRTP(Parsed_Text, (byte)(selectedSampleCount * 2)), 3).ToString();
                        }
                    }
                    else
                    {
                        Gamma_TB.Text = double.Parse(Default___RTP__Threshold).ToString();
                        Chi_sqrd_L.Content = Math.Round(ChiSquaredCache.ChiSqrdINVRTP(double.Parse(Default___RTP__Threshold), (byte)(selectedSampleCount * 2)), 3).ToString();
                    }
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
        private void Gamma_TB_LostFocus(object sender, RoutedEventArgs e)
        {
            double Parsed_Text = 0.0;

            if (IsValidDouble(Gamma_TB.Text))
            {
                Parsed_Text = double.Parse(Gamma_TB.Text);

                if (Parsed_Text <= 1.0 && Parsed_Text >= 0.0 && Parsed_Text > 1E-20)
                {
                    Gamma_TB.Text = Parsed_Text.ToString();
                    Chi_sqrd_L.Content = Math.Round(ChiSquaredCache.ChiSqrdINVRTP(Parsed_Text, (byte)(selectedSampleCount * 2)), 3).ToString();
                }
                else
                {
                    Parsed_Text = double.Parse("1E-19");
                    Gamma_TB.Text = Parsed_Text.ToString();
                    Chi_sqrd_L.Content = Math.Round(ChiSquaredCache.ChiSqrdINVRTP(Parsed_Text, (byte)(selectedSampleCount * 2)), 3).ToString();
                }
            }
            else
            {
                Parsed_Text = double.Parse(Default___RTP__Threshold);
                Gamma_TB.Text = Parsed_Text.ToString();
                Chi_sqrd_L.Content = Math.Round(ChiSquaredCache.ChiSqrdINVRTP(Parsed_Text, (byte)(selectedSampleCount * 2)), 3).ToString();
            }
        }


        public double[] Result()
        {
            // index 0 :
            //          0 : Cancel button is pressed 
            //          1 : Run Analysis is pressed

            double[] Result_array = new double[11];

            if (Pressed_Button == true)
                Result_array[0] = 1;
            else
                Result_array[0] = 0;

            //------------------------------
            if (BioRep_RB.IsChecked == true)
                Result_array[1] = 1;
            else
                Result_array[1] = 2;


            Result_array[2] = Convert.ToDouble(TauS_TB.Text);
            Result_array[3] = Convert.ToDouble(TauW_TB.Text);
            Result_array[4] = Convert.ToDouble(Gamma_TB.Text);

            Result_array[5] = Convert.ToDouble(C_TB.Text);

            // ----------------------
            if (MPI_O1.IsChecked == true)
                Result_array[6] = 1;
            else
                Result_array[6] = 2;

            if (BH_RB.IsChecked == true)
                Result_array[7] = 4;
            else if (WF_Y_P_RB.IsChecked == true)
                Result_array[7] = 3;
            else if (Bonferroni_SD_RB.IsChecked == true)
                Result_array[7] = 2;
            else if (Bonferroni_RB.IsChecked == true)
                Result_array[7] = 1;

            Result_array[8] = float.Parse(alpha_TB.Text);

            return Result_array;
        }



        private bool IsValidDouble(string The_Number)
        {
            if (Regex.IsMatch(The_Number, @"^[0-9]+\.[0-9]*E-[0-9]+$") ||
                Regex.IsMatch(The_Number, @"^[0-9]*\.[0-9]+E-[0-9]+$") ||
                Regex.IsMatch(The_Number, @"^[0-9]*\.[0-9]*$") ||
                Regex.IsMatch(The_Number, @"^[0-9]+E-[0-9]+$") ||
                Regex.IsMatch(The_Number, @"^[0-9]+\.E-[0-9]+$") ||
                Regex.IsMatch(The_Number, @"^[0-9]*\.[0-9]+$") ||
                Regex.IsMatch(The_Number, @"^[0-9]+\.[0-9]*$") ||
                Regex.IsMatch(The_Number, @"^[0-9]+$"))
                return true;
            else
                return false;
        }



        private void TecRep_RB_Checked(object sender, RoutedEventArgs e)
        {
            C_TB.Text = Convert.ToString(selectedSampleCount);
            C_TB.IsReadOnly = true;
        }
        private void BioRep_RB_Checked(object sender, RoutedEventArgs e)
        {
            C_TB.IsReadOnly = false;
            C_TB.Text = Math.Ceiling(selectedSampleCount / 2.0).ToString();
        }

        private void C_TB_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
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
                case Key.Tab:
                    e.Handled = false;
                    break;

                default:
                    e.Handled = true;
                    break;
            }
        }

        private void alpha_TB_KeyDown(object sender, KeyEventArgs e)
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
                case Key.OemPlus:
                case Key.Add:
                case Key.Tab:
                case Key.System:
                    e.Handled = false;
                    break;

                case Key.Decimal:
                case Key.OemPeriod:
                    bool Dot_Found = false;
                    for (int i = 0; i < alpha_TB.Text.Length; i++)
                    {
                        if (alpha_TB.Text[i] == '.')
                        {
                            Dot_Found = true;
                            i = alpha_TB.Text.Length;
                        }
                    }

                    e.Handled = Dot_Found;
                    break;
            }
        }
        private void alpha_TB_LostFocus(object sender, RoutedEventArgs e)
        {
            if (IsValidDouble(alpha_TB.Text))
                alpha_TB.Text = (double.Parse(alpha_TB.Text)).ToString();
            else
                alpha_TB.Text = "0.005";
        }

        private void Vali_c_TB_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
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
                case Key.Tab:
                    e.Handled = false;
                    break;

                default:
                    e.Handled = true;
                    break;
            }
        }
        private void Disc_c_TB_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
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
                case Key.Tab:
                    e.Handled = false;
                    break;

                default:
                    e.Handled = true;
                    break;
            }
        }
    }
}
