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

using Polimi.DEIB.VahidJalili.GIFP;
using Polimi.DEIB.VahidJalili.MuSERA.Models;
using Polimi.DEIB.VahidJalili.MuSERA.ViewModels;
using Polimi.DEIB.VahidJalili.MuSERA.Warehouse;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;


namespace Polimi.DEIB.VahidJalili.MuSERA.Views
{
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Interactive Mode BackColor
        /// </summary>
        private SolidColorBrush IMBC { set; get; }
        /// <summary>
        /// Batch Mode BackColor
        /// </summary>
        private SolidColorBrush BMBC { set; get; }
        public MainWindow()
        {
            Samples<Interval<int, MChIPSeqPeak>, MChIPSeqPeak>.Data = new Dictionary<uint, ParsedChIPseqPeaks<int, Interval<int, MChIPSeqPeak>, MChIPSeqPeak>>();
            RefSeqGenes<Interval<int, MRefSeqGenes>, MRefSeqGenes>.Data = new Dictionary<uint, ParsedRefSeqGenes<int, Interval<int, MRefSeqGenes>, MRefSeqGenes>>();
            GeneralFeatures<Interval<int, MGeneralFeatures>, MGeneralFeatures>.Data = new Dictionary<uint, ParsedGeneralFeatures<int, Interval<int, MGeneralFeatures>, MGeneralFeatures>>();
            Sessions<Interval<int, MChIPSeqPeak>, MChIPSeqPeak>.Data = new Dictionary<string, Session<Interval<int, MChIPSeqPeak>, MChIPSeqPeak>>();

            InitializeComponent();
            var applicationViewModel = new ApplicationViewModel(Console_RTB, Chart, HorizontalAxisTitle, VerticalAxisTitle);
            DataContext = applicationViewModel;

            /// Yes, this is NOT the best way. It would be better if the Document property of RTB would be 
            /// binded to a property of ViewModel. But such a binding is very tricky process that I decided 
            /// to go this way rather implementing a "Bindable RichTextBox".
            applicationViewModel.sessionsViewModel.selectedSessionDetails_RTB = SessionDetail_RTB;
            applicationViewModel.sessionsViewModel.selectedSampleAnalysisOverview_RTB = SelectedSampleAnalysisOverview_RTB;

            Back_BT.Visibility = Visibility.Hidden;

            Main_TabControl.SelectedIndex = 0;

            IMBC = new SolidColorBrush(Color.FromArgb(255, 20, 194, 60));
            BMBC = new SolidColorBrush(Color.FromArgb(255, 200, 0, 255));
            LinearGradientBrush C_BC = new LinearGradientBrush(); // Cast Back Color
            C_BC.StartPoint = new Point(0, 0);
            C_BC.EndPoint = new Point(0, 1);
            C_BC.GradientStops.Add(new GradientStop(Color.FromArgb(0, 0, 0, 0), 0.0));
            C_BC.GradientStops.Add(new GradientStop(Color.FromArgb(0, 0, 0, 0), 0.84));
            C_BC.GradientStops.Add(new GradientStop(IMBC.Color, 0.85));
            C_BC.GradientStops.Add(new GradientStop(IMBC.Color, 1.0));

            Select_Interactive_Mode.Background = C_BC;
            Select_Interactive_Mode.Foreground = IMBC;

            ResultsTab.SelectedIndex = 0;
        }


        private void CachedSamplesDG_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (CachedSamplesDG != null && CachedSamplesDG.SelectedIndex != -1)
            {
                var selectedSampleHashKey = ((CachedDataSummary)CachedSamplesDG.Items.GetItemAt(CachedSamplesDG.SelectedIndex)).fileHashKey;
                var selectedSample = Samples<Interval<int, MChIPSeqPeak>, MChIPSeqPeak>.Data[selectedSampleHashKey];
                FlowDocument f = new FlowDocument();
                Paragraph p = new Paragraph();
                p.Inlines.Add("Selected sample is described as following:\n");
                p.Inlines.Add("Filename:\t" + selectedSample.fileName + "\n");
                p.Inlines.Add("Absolute path:\t" + selectedSample.filePath + "\n");
                p.Inlines.Add("Sample Genome:\t" + selectedSample.genome + "   (" + selectedSample.assembly + ")\n");
                p.Inlines.Add("Peaks count:\t" + selectedSample.intervalsCount.ToString("N0") + "\n");
                p.Inlines.Add("Lowest p-value:\t" + selectedSample.pValueMin.metadata.value.ToString() + "\n");
                p.Inlines.Add("Highest p-value:\t" + selectedSample.pValueMax.metadata.value.ToString() + "\n");
                p.Inlines.Add("Mean p-value:\t" + selectedSample.pValueMean.ToString());
                f.Blocks.Add(p);
                CachedDataOverviewRTB.Document = f;

                sampleDetailsGrid.RowDefinitions[0].Height = new GridLength(181, GridUnitType.Star);
                sampleDetailsGrid.RowDefinitions[1].Height = new GridLength(585, GridUnitType.Star);
                sampleDetailsGrid.RowDefinitions[2].Height = new GridLength(198, GridUnitType.Star);

                Parsing_Message_DG.Columns.Clear();
                Parsing_Message_DG.ItemsSource = selectedSample.messages;
                DataGridTextColumn ChromosomeColumn_title = new DataGridTextColumn();
                ChromosomeColumn_title.Header = "\tMessage";
                ChromosomeColumn_title.Binding = new Binding(".");
                ChromosomeColumn_title.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
                Parsing_Message_DG.Columns.Add(ChromosomeColumn_title);

                chromosome_Details_DG.ItemsSource = selectedSample.chrStatistics;
                InteractiveModeDetailsTab.SelectedIndex = 1;
                Back_BT.Visibility = Visibility.Visible;
            }
        }
        private void CachedFeaturesDG_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (CachedFeaturesDG != null && CachedFeaturesDG.SelectedIndex != -1)
            {
                var selectedFeature = (CachedFeaturesSummary)CachedFeaturesDG.Items.GetItemAt(CachedFeaturesDG.SelectedIndex);
                FlowDocument f = new FlowDocument();
                Paragraph p = new Paragraph();
                DataGridTextColumn ChromosomeColumn_title = new DataGridTextColumn();
                ChromosomeColumn_title.Header = "\tMessage";
                ChromosomeColumn_title.Binding = new Binding(".");
                ChromosomeColumn_title.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
                Parsing_Message_DG.Columns.Add(ChromosomeColumn_title);

                sampleDetailsGrid.RowDefinitions[0].Height = new GridLength(181, GridUnitType.Star);
                sampleDetailsGrid.RowDefinitions[1].Height = new GridLength(3, GridUnitType.Star);
                sampleDetailsGrid.RowDefinitions[2].Height = new GridLength(780, GridUnitType.Star);

                switch (selectedFeature.dataType)
                {
                    case DataType.GeneralFeatures:
                        var selectedGeneralFeatures = GeneralFeatures<Interval<int, MGeneralFeatures>, MGeneralFeatures>.Data[selectedFeature.fileHashKey];
                        p.Inlines.Add("Selected sample is described as following:\n");
                        p.Inlines.Add("Filename:\t" + selectedGeneralFeatures.fileName + "\n");
                        p.Inlines.Add("Absolute path:\t" + selectedGeneralFeatures.filePath + "\n");
                        p.Inlines.Add("Sample Genome:\t" + selectedGeneralFeatures.genome + "   (" + selectedGeneralFeatures.assembly + ")\n");
                        p.Inlines.Add("Data type:\tGeneral features\n");
                        p.Inlines.Add("Total features count:\t" + selectedGeneralFeatures.intervalsCount.ToString("N0") + "\n");
                        p.Inlines.Add("Following features are determined:\n");
                        foreach (var feature in selectedGeneralFeatures.determinedFeatures)
                            p.Inlines.Add("\tFeature:" + feature.title + "\tCount:" + feature.count.ToString("N0") + "\n");
                        f.Blocks.Add(p);
                        CachedDataOverviewRTB.Document = f;
                        Parsing_Message_DG.ItemsSource = selectedGeneralFeatures.messages;
                        chromosome_Details_DG.ItemsSource = null;
                        break;

                    case DataType.RefSeqGenes:
                        var selectedGene = RefSeqGenes<Interval<int, MRefSeqGenes>, MRefSeqGenes>.Data[selectedFeature.fileHashKey];
                        p.Inlines.Add("Selected sample is described as following:\n");
                        p.Inlines.Add("Filename:\t" + selectedGene.fileName + "\n");
                        p.Inlines.Add("Absolute path:\t" + selectedGene.filePath + "\n");
                        p.Inlines.Add("Sample Genome:\t" + selectedGene.genome + "   (" + selectedGene.assembly + ")\n");
                        p.Inlines.Add("Data type:\tGenes\n");
                        p.Inlines.Add("Total features count:\t" + selectedGene.intervalsCount.ToString("N0") + "\n");
                        f.Blocks.Add(p);
                        CachedDataOverviewRTB.Document = f;
                        Parsing_Message_DG.ItemsSource = selectedGene.messages;
                        chromosome_Details_DG.ItemsSource = null;
                        break;
                }


                InteractiveModeDetailsTab.SelectedIndex = 1;
                Back_BT.Visibility = Visibility.Visible;
            }
        }


        private void Back_BT_MouseEnter(object sender, MouseEventArgs e)
        {
            Back_BT.Foreground = new SolidColorBrush(Colors.White);
            Back_BT.Background = new SolidColorBrush(Color.FromArgb(255, 50, 155, 255));
        }
        private void Back_BT_MouseLeave(object sender, MouseEventArgs e)
        {
            Back_BT.Foreground = new SolidColorBrush(Color.FromArgb(255, 50, 155, 255));
            Back_BT.Background = new SolidColorBrush(Colors.White);
        }
        private void Back_BT_MouseDown(object sender, MouseButtonEventArgs e)
        {
            InteractiveModeDetailsTab.SelectedIndex = 0;
            Back_BT.Visibility = Visibility.Hidden;
        }


        private void Select_Interactive_Mode_MouseEnter(object sender, MouseEventArgs e)
        {
            if (Main_TabControl.SelectedIndex != 0)
            {
                LinearGradientBrush C_BC = new LinearGradientBrush(); // Cast Back Color
                C_BC.StartPoint = new Point(0, 0);
                C_BC.EndPoint = new Point(0, 1);
                C_BC.GradientStops.Add(new GradientStop(Color.FromArgb(0, 0, 0, 0), 0.0));
                C_BC.GradientStops.Add(new GradientStop(Color.FromArgb(0, 0, 0, 0), 0.84));
                C_BC.GradientStops.Add(new GradientStop(IMBC.Color, 0.85));
                C_BC.GradientStops.Add(new GradientStop(IMBC.Color, 1.0));

                Select_Interactive_Mode.Background = C_BC;
                Select_Interactive_Mode.Foreground = IMBC;
            }
        }
        private void Select_Interactive_Mode_MouseLeave(object sender, MouseEventArgs e)
        {
            if (Main_TabControl.SelectedIndex != 0)
            {
                Select_Interactive_Mode.Foreground = new SolidColorBrush(Color.FromArgb(255, 50, 155, 255));
                Select_Interactive_Mode.Background = new SolidColorBrush(Colors.Transparent);
            }
        }
        private void Select_Interactive_Mode_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Select_Batch_Mode.Foreground = new SolidColorBrush(Color.FromArgb(255, 50, 155, 255));
            Select_Batch_Mode.Background = new SolidColorBrush(Colors.Transparent);

            Main_TabControl.SelectedIndex = 0;
        }

        private void Select_Batch_Mode_MouseEnter(object sender, MouseEventArgs e)
        {
            if (Main_TabControl.SelectedIndex != 1)
            {
                LinearGradientBrush C_BC = new LinearGradientBrush(); // Cast Back Color
                C_BC.StartPoint = new Point(0, 0);
                C_BC.EndPoint = new Point(0, 1);
                C_BC.GradientStops.Add(new GradientStop(Color.FromArgb(0, 0, 0, 0), 0.0));
                C_BC.GradientStops.Add(new GradientStop(Color.FromArgb(0, 0, 0, 0), 0.84));
                C_BC.GradientStops.Add(new GradientStop(BMBC.Color, 0.85));
                C_BC.GradientStops.Add(new GradientStop(BMBC.Color, 1.0));

                Select_Batch_Mode.Background = C_BC;
                Select_Batch_Mode.Foreground = BMBC;
            }
        }
        private void Select_Batch_Mode_MouseLeave(object sender, MouseEventArgs e)
        {
            if (Main_TabControl.SelectedIndex != 1)
            {
                Select_Batch_Mode.Foreground = new SolidColorBrush(Color.FromArgb(255, 50, 155, 255));
                Select_Batch_Mode.Background = new SolidColorBrush(Colors.Transparent);
            }
        }
        private void Select_Batch_Mode_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Select_Interactive_Mode.Foreground = new SolidColorBrush(Color.FromArgb(255, 50, 155, 255));
            Select_Interactive_Mode.Background = new SolidColorBrush(Colors.Transparent);

            Main_TabControl.SelectedIndex = 1;
        }


        private void Priority_L1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ProcessPrioritySlider.Value = 0;
        }
        private void Priority_L2_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ProcessPrioritySlider.Value = 1;
        }
        private void Priority_L3_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ProcessPrioritySlider.Value = 2;
        }
        private void Priority_L4_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ProcessPrioritySlider.Value = 3;
        }
        private void Priority_L5_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ProcessPrioritySlider.Value = 4;
        }
        private void ProcessPrioritySlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            FlowDocument flowDocument = new FlowDocument();
            Paragraph paragraph = new Paragraph();
            Priority_L1.FontWeight = FontWeights.Normal;
            Priority_L2.FontWeight = FontWeights.Normal;
            Priority_L3.FontWeight = FontWeights.Normal;
            Priority_L4.FontWeight = FontWeights.Normal;
            Priority_L5.FontWeight = FontWeights.Normal;

            switch ((int)(ProcessPrioritySlider.Value))
            {
                case 0:
                    Priority_L1.FontWeight = FontWeights.ExtraBold;
                    paragraph.Inlines.Add("Program will be most responsive, while batch running process will be slow");
                    break;

                case 1:
                    Priority_L2.FontWeight = FontWeights.ExtraBold;
                    paragraph.Inlines.Add("Program will be relatively responsive, while batch running process will be a bit slow");
                    break;

                case 2:
                    Priority_L3.FontWeight = FontWeights.ExtraBold;
                    paragraph.Inlines.Add("The response time of program, and the speed of batch running process will be in balance");
                    break;

                case 3:
                    Priority_L4.FontWeight = FontWeights.ExtraBold;
                    paragraph.Inlines.Add("Batch running process will be relatively fast, while program will be a bit laggy");
                    break;

                case 4:
                    Priority_L5.FontWeight = FontWeights.ExtraBold;
                    paragraph.Inlines.Add("Batch running process will be at highest speed, while the program becomes laggiest");
                    break;
            }

            flowDocument.Blocks.Add(paragraph);
            PriorityExplanationRTB.Document = flowDocument;
        }


        private void CVS_1st_TypeA_Filter(object sender, FilterEventArgs e)
        {// Collection View Source First Classification - Type A - Filter
            var t = e.Item as PValueDistribution;
            if (t != null) e.Accepted = t.type == ERClassificationType.Stringent ? true : false;
        }
        private void CVS_1st_TypeB_Filter(object sender, FilterEventArgs e)
        {// Collection View Source First Classification - Type A - Filter
            var t = e.Item as PValueDistribution;
            if (t != null) e.Accepted = t.type == ERClassificationType.Weak ? true : false;
        }
        private void CVS_2nd_TypeA_Filter(object sender, FilterEventArgs e)
        {// Collection View Source Second Classification - Type A - Filter
            var t = e.Item as PValueDistribution;
            if (t != null)
                e.Accepted =
                    (t.type == ERClassificationType.StringentConfirmed || t.type == ERClassificationType.WeakConfirmed) ? true : false;
        }
        private void CVS_2nd_TypeB_Filter(object sender, FilterEventArgs e)
        {// Collection View Source Second Classification - Type B - Filter
            var t = e.Item as PValueDistribution;
            if (t != null)
                e.Accepted =
                    (t.type == ERClassificationType.StringentDiscarded || t.type == ERClassificationType.WeakDiscarded) ? true : false;
        }
        private void CVS_3rd_TypeA_Filter(object sender, FilterEventArgs e)
        {// Collection View Source Third Classification - Type A - Filter
            var t = e.Item as PValueDistribution;
            if (t != null) e.Accepted = t.type == ERClassificationType.StringentConfirmed ? true : false;
        }
        private void CVS_3rd_TypeB_Filter(object sender, FilterEventArgs e)
        {// Collection View Source Third Classification - Type B - Filter
            var t = e.Item as PValueDistribution;
            if (t != null) e.Accepted = t.type == ERClassificationType.WeakConfirmed ? true : false;
        }
        private void CVS_4th_TypeA_Filter(object sender, FilterEventArgs e)
        {// Collection View Source Fourth Classification - Type A - Filter
            var t = e.Item as PValueDistribution;
            if (t != null) e.Accepted = t.type == ERClassificationType.TruePositive ? true : false;
        }
        private void CVS_4th_TypeB_Filter(object sender, FilterEventArgs e)
        {// Collection View Source Fourth Classification - Type B - Filter
            var t = e.Item as PValueDistribution;
            if (t != null) e.Accepted = t.type == ERClassificationType.FalsePositive ? true : false;
        }
    }
}