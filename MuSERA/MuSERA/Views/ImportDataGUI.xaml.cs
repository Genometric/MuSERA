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
using Polimi.DEIB.VahidJalili.MuSERA.ImportData;
using Polimi.DEIB.VahidJalili.MuSERA.ViewModels;
using Polimi.DEIB.VahidJalili.MuSERA.Warehouse;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace Polimi.DEIB.VahidJalili.MuSERA.Views
{
    /// <summary>
    /// Interaction logic for ImportDataGUI.xaml
    /// </summary>
    public partial class ImportDataGUI : Window
    {
        #region -_-     private members declaration        -_-

        /// <summary>
        ///  Offset. Is the Canvas.GetLeft(Field_1_Title_L). 
        ///  Anytime the position of this member is changed,
        ///  the Value of this parameter should be updated. 
        /// </summary>
        private byte _o = 0;

        /// <summary>
        /// Is the _width of Field_*_Title_L. 
        /// Anytime the _width is changed, this parameter
        /// needs to be updated.
        /// </summary>
        private byte _w = 78;

        /// <summary>
        /// Is the distance between Field_*_Title_L.
        /// Anytime the distance is updated, this parameter
        /// needs to be updated
        /// </summary>
        private byte _d = 0;

        /// <summary>
        /// Shift Button Width. This variable should be updated anytime 
        /// the shift button'flowDocument _width is changed.
        /// </summary>
        private byte _sbw = 39;

        /// <summary>
        /// Presents file schema. Will be used as default file schema
        /// to initialize corresponding labels. Also the contents
        /// will be used to assign column numbers to each of the 
        /// essential fields.
        /// </summary>
        private string[] _fileSchema = new string[] { "chr", "start", "stop", "name", "p-value", "pass", "pass", "pass", "pass", "pass", "pass", "pass" };

        private Dictionary<Genomes, GenomeAssemblies.GenomeInfo> _genomeInfo { set; get; }

        private class FileSample
        {
            #region -_-     Public Members     -_-
            public string column_0 { set; get; }
            public string column_1 { set; get; }
            public string column_2 { set; get; }
            public string column_3 { set; get; }
            public string column_4 { set; get; }
            public string column_5 { set; get; }
            public string column_6 { set; get; }
            public string column_7 { set; get; }
            public string column_8 { set; get; }
            public string column_9 { set; get; }
            public string column_10 { set; get; }
            public string column_11 { set; get; }
            #endregion
        }
        private ObservableCollection<FileSample> _fileSamplePortion { set; get; }

        /// <summary>
        /// Sets and gets back color used for UI elements at MouseEnter.
        /// </summary>
        private SolidColorBrush _MouseEnter_BC { set; get; }
        /// <summary>
        /// Sets and gets back color used for UI elements at MouseLeave.
        /// </summary>
        private SolidColorBrush _MouseLeave_BC { set; get; }

        private BackgroundWorker parser_BGW = new BackgroundWorker();

        #endregion

        private Microsoft.Win32.OpenFileDialog inputFileChooser = new Microsoft.Win32.OpenFileDialog();

        public ImportDataGUI()
        {
            InitializeComponent();

            _fileSamplePortion = new ObservableCollection<FileSample>();
            _MouseEnter_BC = new SolidColorBrush(Color.FromArgb(255, 0, 50, 120));
            _MouseLeave_BC = new SolidColorBrush(Colors.Transparent);

            _genomeInfo = GenomeAssemblies.AllGenomeAssemblies();
            Genome_CB.Items.Clear();
            foreach (var genome in _genomeInfo)
                Genome_CB.Items.Add(genome.Value.genomeTitle);
            Genome_CB.SelectedIndex = 0;

            UpdateFieldTitlesWithFileSchema();

            ShiftLeft_L.Visibility = Visibility.Hidden;
            ShiftRight_L.Visibility = Visibility.Hidden;

            ShiftLeft_L.Background = _MouseEnter_BC;
            ShiftRight_L.Background = _MouseEnter_BC;

            Canvas.SetLeft(ShiftLeft_L, 0);
            Canvas.SetLeft(ShiftRight_L, 39);

            UpdateFileSample_DG();

            parser_BGW.DoWork += parser_BGW_DoWork;
            parser_BGW.RunWorkerCompleted += parser_BGW_RunWorkerCompleted;
            parser_BGW.WorkerSupportsCancellation = true;

            Process_Current_Sample_L.Visibility = Visibility.Hidden;
            Process_Overall_L.Visibility = Visibility.Hidden;
            CurrentSample_PB.Visibility = Visibility.Hidden;
            OverallProcess_PB.Visibility = Visibility.Hidden;
        }

        private void ImportDataGUI_Closing(object sender, CancelEventArgs e)
        {
            if (parser_BGW.IsBusy == true)
            {
                MessageBoxResult r = MessageBox.Show(
                    "Parser is busy caching files, do you want to cancel the process ?",
                    "Parser busy",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);

                if (r == MessageBoxResult.OK)
                {
                    parser_BGW.CancelAsync();
                    this.Close();
                }
                else if (r == MessageBoxResult.No)
                {
                    e.Cancel = true;
                }
            }
        }


        private void UpdateFieldTitlesWithFileSchema()
        {
            UpdateFieldTitleAndIndex(Field_1_Title_L, Field_1_Index_L, _fileSchema[0]);
            UpdateFieldTitleAndIndex(Field_2_Title_L, Field_2_Index_L, _fileSchema[1]);
            UpdateFieldTitleAndIndex(Field_3_Title_L, Field_3_Index_L, _fileSchema[2]);
            UpdateFieldTitleAndIndex(Field_4_Title_L, Field_4_Index_L, _fileSchema[3]);
            UpdateFieldTitleAndIndex(Field_5_Title_L, Field_5_Index_L, _fileSchema[4]);
            UpdateFieldTitleAndIndex(Field_6_Title_L, Field_6_Index_L, _fileSchema[5]);
            UpdateFieldTitleAndIndex(Field_7_Title_L, Field_7_Index_L, _fileSchema[6]);
            UpdateFieldTitleAndIndex(Field_8_Title_L, Field_8_Index_L, _fileSchema[7]);
            UpdateFieldTitleAndIndex(Field_9_Title_L, Field_9_Index_L, _fileSchema[8]);
            UpdateFieldTitleAndIndex(Field_10_Title_L, Field_10_Index_L, _fileSchema[9]);
            UpdateFieldTitleAndIndex(Field_11_Title_L, Field_11_Index_L, _fileSchema[10]);
            UpdateFieldTitleAndIndex(Field_12_Title_L, Field_12_Index_L, _fileSchema[11]);

            UpdateFileSample_DG();
        }
        private void UpdateFieldTitleAndIndex(System.Windows.Controls.Label field_Title, System.Windows.Controls.Label field_index, string new_Content)
        {
            field_Title.Content = new_Content;

            if (new_Content == "pass")
            {
                field_Title.FontStyle = FontStyles.Italic;
                field_index.FontStyle = FontStyles.Italic;

                field_Title.Foreground = new SolidColorBrush(Colors.Yellow);
                field_index.Foreground = new SolidColorBrush(Colors.Yellow);
            }
            else
            {
                field_Title.FontStyle = FontStyles.Normal;
                field_index.FontStyle = FontStyles.Normal;

                field_Title.Foreground = new SolidColorBrush(Colors.White);
                field_index.Foreground = new SolidColorBrush(Colors.White);
            }
        }

        private void UpdateFileSample_DG()
        {
            float column_width = 77.9F;

            Sample_BED_lines_DG.Columns.Clear();

            Sample_BED_lines_DG.ItemsSource = _fileSamplePortion;

            for (byte i = 0; i < 12; i++)
            {
                InquireValidity converter = new InquireValidity();
                converter.Inquire_Key = _fileSchema[i];

                Binding triggerBinding = new Binding("column_" + i.ToString());
                triggerBinding.Converter = converter;
                triggerBinding.Mode = BindingMode.OneTime;

                DataTrigger trueValueTrigger = new DataTrigger();
                trueValueTrigger.Binding = triggerBinding;
                //trueValueTrigger.Setters.Add(new Setter(DataGridCell.BackgroundProperty, new SolidColorBrush(Colors.LimeGreen)));
                trueValueTrigger.Setters.Add(new Setter(DataGridCell.ForegroundProperty, new SolidColorBrush(Colors.White)));
                trueValueTrigger.Value = true;

                DataTrigger falseValueTrigger = new DataTrigger();
                falseValueTrigger.Binding = triggerBinding;
                falseValueTrigger.Setters.Add(new Setter(DataGridCell.BackgroundProperty, new SolidColorBrush(Colors.OrangeRed)));
                falseValueTrigger.Setters.Add(new Setter(DataGridCell.ForegroundProperty, new SolidColorBrush(Colors.White)));
                falseValueTrigger.Value = false;

                DataTrigger warningValueTrigger = new DataTrigger();
                warningValueTrigger.Binding = triggerBinding;
                warningValueTrigger.Setters.Add(new Setter(DataGridCell.BackgroundProperty, new SolidColorBrush(Colors.Yellow)));
                warningValueTrigger.Setters.Add(new Setter(DataGridCell.ForegroundProperty, new SolidColorBrush(Colors.Black)));
                warningValueTrigger.Value = '0';

                DataTrigger nullAcceptableValueTrigger = new DataTrigger();
                nullAcceptableValueTrigger.Binding = triggerBinding;
                nullAcceptableValueTrigger.Setters.Add(new Setter(DataGridCell.BackgroundProperty, new SolidColorBrush(Color.FromArgb(255, 220, 0, 220))));
                nullAcceptableValueTrigger.Setters.Add(new Setter(DataGridCell.ForegroundProperty, new SolidColorBrush(Colors.White)));
                nullAcceptableValueTrigger.Value = '1';

                Style style = new System.Windows.Style(typeof(DataGridCell));
                style.Setters.Add(new Setter(TextBlock.TextAlignmentProperty, TextAlignment.Center));
                style.Setters.Add(new Setter(DataGridCell.BorderThicknessProperty, new Thickness(0, 0, 0, 0)));
                style.Triggers.Add(trueValueTrigger);
                style.Triggers.Add(falseValueTrigger);
                style.Triggers.Add(warningValueTrigger);
                style.Triggers.Add(nullAcceptableValueTrigger);

                DataGridTextColumn column = new DataGridTextColumn();
                column.Binding = new Binding("column_" + i.ToString()) { Mode = BindingMode.OneTime };

                if (i == 11)
                {
                    column.Width = new DataGridLength(column_width, DataGridLengthUnitType.Star);
                }
                else
                {
                    column.Width = column_width;
                }

                column.CellStyle = style;

                Sample_BED_lines_DG.Columns.Add(column);
            }
        }


        /// <summary>
        /// When mouse enter event occured for Field_*_Title or Field_*_Index labels, 
        /// background of Field_*_Tilte or Field_*_Index will be highlighted. To 
        /// understand when mouse leaves the area, the same controles could not be used
        /// because their mouse enter event is used for highlighting, hence the mouse
        /// enter event of surrounding controlers is used. If the mouse enter event of 
        /// these controlers is raised, it indicates that mouse left Field_*_Title
        /// or Field_*_Index, hence their background needs to be reseted. 
        /// This event will be used commonly for "Sample_BED_lines_DG", 
        /// "Controls_Grid", "File_schema_L", and "Grid_B"
        /// </summary>
        /// <param propertyName="sender"></param>
        /// <param propertyName="e"></param>
        private void SurroundingControlers_MouseEnter(object sender, MouseEventArgs e)
        {
            Mouse_Exit__Control_EventHandler();
        }

        private void ChIPseqAssays_Checked(object sender, RoutedEventArgs e)
        {
            p_value_Conversion_ops_Canvas.IsEnabled = true;
            p_value_validity_ops_Canvas.IsEnabled = true;
            MidPointAsSummit.IsEnabled = true;

            _fileSchema = null;
            if (MidPointAsSummit.IsChecked == true)
                _fileSchema = new string[] { "chr", "start", "stop", "name", "p-value", "pass", "pass", "pass", "pass", "pass", "pass", "pass" };
            else
                _fileSchema = new string[] { "chr", "start", "stop", "name", "p-value", "summit", "pass", "pass", "pass", "pass", "pass", "pass" };

            UpdateFieldTitlesWithFileSchema();
        }
        private void RefseqGenes_Checked(object sender, RoutedEventArgs e)
        {
            p_value_Conversion_ops_Canvas.IsEnabled = false;
            p_value_validity_ops_Canvas.IsEnabled = false;
            MidPointAsSummit.IsEnabled = false;

            _fileSchema = null;
            _fileSchema = new string[] { "refseq.ID", "gene.symb", "chr", "pass", "start", "stop", "pass", "pass", "pass", "pass", "pass", "pass" };

            UpdateFieldTitlesWithFileSchema();
        }
        private void General_Feature__RB_Checked(object sender, RoutedEventArgs e)
        {
            p_value_Conversion_ops_Canvas.IsEnabled = false;
            p_value_validity_ops_Canvas.IsEnabled = false;
            MidPointAsSummit.IsEnabled = false;

            _fileSchema = null;
            _fileSchema = new string[] { "chr", "pass", "feature", "start", "stop", "pass", "pass", "pass", "attribute", "pass", "pass", "pass" };

            UpdateFieldTitlesWithFileSchema();
        }

        private void MidPointAsSummit_Checked(object sender, RoutedEventArgs e)
        {
            _fileSchema = new string[] { "chr", "start", "stop", "name", "p-value", "pass", "pass", "pass", "pass", "pass", "pass", "pass" };
            UpdateFieldTitlesWithFileSchema();
        }

        private void MidPointAsSummit_Unchecked(object sender, RoutedEventArgs e)
        {
            _fileSchema = new string[] { "chr", "start", "stop", "name", "p-value", "summit", "pass", "pass", "pass", "pass", "pass", "pass" };
            UpdateFieldTitlesWithFileSchema();
        }


        #region -_-     Browse/Add/Cancel Buttons Mouse Event Handlers    -_-

        private void Browse_BT_MouseEnter(object sender, MouseEventArgs e)
        {
            Browse_BT.Foreground = new SolidColorBrush(Colors.White);
            Browse_BT.Background = new SolidColorBrush(Color.FromArgb(255, 50, 155, 255));
        }
        private void Browse_BT_MouseLeave(object sender, MouseEventArgs e)
        {
            Browse_BT.Foreground = new SolidColorBrush(Color.FromArgb(255, 50, 155, 255));
            Browse_BT.Background = new SolidColorBrush(Color.FromArgb(20, 255, 255, 255));
        }
        private void Browse_BT_MouseDown(object sender, MouseButtonEventArgs e)
        {
            inputFileChooser.Multiselect = true;
            inputFileChooser.Filter = "Common BED files (*.bed) , (*.encodePeak)|*.bed;*.encodePeak|General Transfer Format (*.GTF)|*.GTF|All files (*.*)|*.*";
            Nullable<bool> result = inputFileChooser.ShowDialog();

            if (result == true && inputFileChooser.FileNames.Length != 0)
            {
                Add_BT.IsEnabled = true;
                Grid_AL.IsEnabled = true;
                Grid_AR.IsEnabled = true;
                Grid_B.IsEnabled = true;
                Grid_C.IsEnabled = true;
                Browse_BT.IsEnabled = false;

                DisplayFractionOfSample();
            }
        }

        private void Add_BT_MouseEnter(object sender, MouseEventArgs e)
        {
            Add_BT.Foreground = new SolidColorBrush(Colors.White);
            Add_BT.Background = new SolidColorBrush(Color.FromArgb(255, 50, 155, 255));
        }
        private void Add_BT_MouseLeave(object sender, MouseEventArgs e)
        {
            if (Add_BT.IsEnabled == true)
            {
                Add_BT.Foreground = new SolidColorBrush(Color.FromArgb(255, 50, 155, 255));
                Add_BT.Background = new SolidColorBrush(Color.FromArgb(20, 255, 255, 255));
            }
        }
        private void Add_BT_MouseDown(object sender, MouseButtonEventArgs e)
        {
            OverallProcess_PB.Maximum = inputFileChooser.FileNames.Length;

            ParserOptions runKey = new ParserOptions();

            foreach (var genome in _genomeInfo)
                if (genome.Value.genomeTitle == (string)Genome_CB.SelectedItem)
                {
                    runKey.genome = genome.Key;
                    foreach (var assembly in genome.Value.genomeAssemblies)
                        if (assembly.Value == (string)Assembly_CB.SelectedItem)
                        {
                            runKey.assembly = assembly.Key;
                            break;
                        }
                    break;
                }


            if (ReadOnlyValidChrs.IsChecked == true) runKey.readOnlyValidChrs = true;
            else runKey.readOnlyValidChrs = false;

            runKey.startOffset = byte.Parse(Off_Set_Line_TB.Text);
            runKey.summitColumn = -1;
            if (Assays_RB.IsChecked == true)
            {
                for (byte i = 0; i < _fileSchema.Length; i++)
                {
                    switch (_fileSchema[i])
                    {
                        case "chr": runKey.chrColumn = i; break;
                        case "start": runKey.leftColumn = i; break;
                        case "stop": runKey.rightColumn = i; break;
                        case "name": runKey.nameColumn = i; break;
                        case "p-value": runKey.pValueColumn = i; break;
                        case "summit": runKey.summitColumn = (sbyte)i; break;
                    }
                }
            }
            else if (Refseq_genes_RB.IsChecked == true)
            {
                for (byte i = 0; i < _fileSchema.Length; i++)
                {
                    switch (_fileSchema[i])
                    {
                        case "chr": runKey.chrColumn = i; break;
                        case "start": runKey.leftColumn = i; break;
                        case "stop": runKey.rightColumn = i; break;
                        case "refseq.ID": runKey.refseqIDColum = i; break;
                        case "gene.symb": runKey.geneSymbolColumn = i; break;
                    }
                }
            }
            else if (General_Feature__RB.IsChecked == true)
            {
                for (byte i = 0; i < _fileSchema.Length; i++)
                {
                    switch (_fileSchema[i])
                    {
                        case "chr": runKey.chrColumn = i; break;
                        case "start": runKey.leftColumn = i; break;
                        case "stop": runKey.rightColumn = i; break;
                        case "feature": runKey.featureColumn = i; break;
                        case "attribute": runKey.attributeColumn = i; break;
                    }
                }
            }
            runKey.strandColumn = -1;

            runKey.defaultpValue = double.Parse(Default_p_value.Text);
            if (p_value_op1.IsChecked == true)
                runKey.pValueConversion = pValueFormat.minus100_Log10_pValue;
            else if (p_value_op2.IsChecked == true)
                runKey.pValueConversion = pValueFormat.minus10_Log10_pValue;
            else runKey.pValueConversion = pValueFormat.minus1_Log10_pValue;

            if (No_p_Value_C1.IsChecked == true) runKey.dropIfNopValue = true; else runKey.dropIfNopValue = false;
            if (Assays_RB.IsChecked == true) runKey.inputType = ParserOptions.InputType.ChIPseqAssays;
            else if (Refseq_genes_RB.IsChecked == true) runKey.inputType = ParserOptions.InputType.RefseqGene;
            else if (General_Feature__RB.IsChecked == true) runKey.inputType = ParserOptions.InputType.GeneralFeatures;


            Add_BT.Content = "Caching  ... please wait     ";
            Add_BT.Foreground = new SolidColorBrush(Colors.White);
            Add_BT.Background = new SolidColorBrush(Color.FromArgb(255, 50, 155, 255));

            Cancel_BT.Content = "The window will close when all files cached     ";
            Cancel_BT.Foreground = new SolidColorBrush(Colors.White);
            Cancel_BT.Background = new SolidColorBrush(Color.FromArgb(255, 50, 155, 255));

            Add_BT.IsEnabled = false;
            Cancel_BT.IsEnabled = false;
            Add_BT.IsEnabled = false;
            Grid_AL.IsEnabled = false;
            Grid_AR.IsEnabled = false;
            Grid_B.IsEnabled = false;
            Grid_C.IsEnabled = false;

            Process_Current_Sample_L.Visibility = Visibility.Visible;
            Process_Overall_L.Visibility = Visibility.Visible;
            CurrentSample_PB.Visibility = Visibility.Visible;
            OverallProcess_PB.Visibility = Visibility.Visible;

            parser_BGW.RunWorkerAsync(runKey);
        }

        private void parser_BGW_DoWork(object sender, DoWorkEventArgs e)
        {
            switch (((ParserOptions)e.Argument).inputType)
            {
                case ParserOptions.InputType.ChIPseqAssays:
                    Parser_BGW__ReadChIPseqAssays((ParserOptions)e.Argument);
                    break;

                case ParserOptions.InputType.RefseqGene:
                    Parser_BGW__ReadRefseqGenes((ParserOptions)e.Argument);
                    break;

                case ParserOptions.InputType.GeneralFeatures:
                    Parser_BGW__ReadGeneralFeatures((ParserOptions)e.Argument);
                    break;
            }
        }
        private void parser_BGW_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Add_BT.Content = "Add     ";
            Add_BT.Foreground = new SolidColorBrush(Color.FromArgb(255, 50, 155, 255));
            Add_BT.Background = new SolidColorBrush(Color.FromArgb(20, 255, 255, 255));

            Cancel_BT.Content = "Cancel     ";
            Cancel_BT.Foreground = new SolidColorBrush(Color.FromArgb(255, 50, 155, 255));
            Cancel_BT.Background = new SolidColorBrush(Color.FromArgb(20, 255, 255, 255));

            Process_Current_Sample_L.Visibility = Visibility.Hidden;
            Process_Overall_L.Visibility = Visibility.Hidden;
            CurrentSample_PB.Visibility = Visibility.Hidden;
            OverallProcess_PB.Visibility = Visibility.Hidden;

            Add_BT.IsEnabled = true;
            Cancel_BT.IsEnabled = true;
            Add_BT.IsEnabled = true;
            Grid_AL.IsEnabled = true;
            Grid_AR.IsEnabled = true;
            Grid_B.IsEnabled = true;
            Grid_C.IsEnabled = true;

            this.Close();
        }
        private void Parser_BGW__ReadChIPseqAssays(ParserOptions runKey)
        {
            var ChIPSeqData = Samples<Interval<int, MChIPSeqPeak>, MChIPSeqPeak>.Data;

            foreach (string file in inputFileChooser.FileNames)
            {
                if (ChIPSeqData.Where(x => x.Value.filePath == file).ToList().Count != 0)
                {
                    MessageBox.Show("The following sample is already cached into the program." +
                        "The sample is ignored due to disallowed duplications." +
                        "\n\nDuplicated sample :\n" + file + "\n",
                        "Duplication encountered", MessageBoxButton.OK, MessageBoxImage.Information);
                    continue;
                }

                var bedParser = new BEDParser<Interval<int, MChIPSeqPeak>, MChIPSeqPeak>(
                        source: file,
                        species: runKey.genome,
                        assembly: runKey.assembly,
                        readOnlyValidChrs: runKey.readOnlyValidChrs,
                        startOffset: runKey.startOffset,
                        chrColumn: runKey.chrColumn,
                        leftEndColumn: runKey.leftColumn,
                        rightEndColumn: runKey.rightColumn,
                        nameColumn: runKey.nameColumn,
                        valueColumn: runKey.pValueColumn,
                        summitColumn: runKey.summitColumn,
                        strandColumn: runKey.strandColumn,
                        defaultValue: runKey.defaultpValue,
                        pValueFormat: runKey.pValueConversion,
                        dropPeakIfInvalidValue: runKey.dropIfNopValue);

                bedParser.decimalPlaces = 5;
                bedParser.StatusChanged += ParserStatusChanged;

                /// This informs the function that a new sample is 
                /// started to be parsed, hence overall progress bar 
                /// needs to be updated
                ParserStatusChanged(this, new ParserEventArgs("s"));

                var data = bedParser.Parse();

                if (bedParser.missingChrs.Count != 0 && bedParser.excessChrs.Count != 0)
                {
                    #region -_-     Message Creation                   -_-

                    string message = "The following sample is expected to be of type " + runKey.genome + ", however it is incommensurable in terms of present chromosomes.";

                    if (bedParser.missingChrs.Count != 0)
                    {
                        message += "\n\nThe sample lacks " + bedParser.missingChrs.Count.ToString() + " ";

                        if (bedParser.missingChrs.Count == 1) message += "chromosome";
                        else message += "chromosomes";

                        message += " (i.e., " + bedParser.missingChrs[0];

                        for (int i = 1; i < bedParser.missingChrs.Count; i++)
                            if (i != bedParser.missingChrs.Count - 1)
                                message += ", " + bedParser.missingChrs[i];
                            else
                                message += ", and " + bedParser.missingChrs[i];

                        message += " ).";

                        if (bedParser.missingChrs.Count == 1) message += "This chromosome ";
                        else message += "These chromosomes ";

                        message += "will be considered as 'null' without effecting the analysis process.";
                    }

                    if (bedParser.excessChrs.Count != 0)
                    {
                        message += "\n\nThe sample encompasses " + bedParser.excessChrs.Count.ToString() + " excess ";

                        if (bedParser.excessChrs.Count == 1) message += "chromosome";
                        else message += "chromosomes";

                        message += " (i.e., " + bedParser.excessChrs[0];

                        for (int i = 1; i < bedParser.excessChrs.Count; i++)
                            if (i != bedParser.excessChrs.Count - 1)
                                message += ", " + bedParser.excessChrs[i];
                            else
                                message += ", and " + bedParser.excessChrs[i];


                        message += " ).The reads on ";

                        if (bedParser.excessChrs.Count == 1) message += "this chromosome ";
                        else message += "these chromosomes ";

                        message += "are not cached and hence the analysis will not be effected.";
                    }

                    message += "\n\nDo you want to keep the sample anyway ?";
                    message += "\n\nThe ambiguous sample is :\n" + file + "\n";

                    #endregion

                    MessageBoxResult r = MessageBox.Show(
                        message,
                        "Sample chromosome count warning",
                        MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);

                    if (r == MessageBoxResult.Cancel) break;
                    else if (r == MessageBoxResult.No) continue;
                }

                Dispatcher.Invoke((Action)(() =>
                {
                    ChIPSeqData.Add(data.fileHashKey, data);
                    ApplicationViewModel.Default.cachedDataSummary.Add(new Models.CachedDataSummary(
                        selected: false,
                        fileHashKey: data.fileHashKey,
                        label: data.fileName,
                        featureCount: data.intervalsCount,
                        genome: data.genome,
                        assembly: data.assembly));
                }));
            }
        }
        private void Parser_BGW__ReadRefseqGenes(ParserOptions runKey)
        {
            var refSeqGenes = RefSeqGenes<Interval<int, MRefSeqGenes>, MRefSeqGenes>.Data;

            foreach (string file in inputFileChooser.FileNames)
            {
                if (refSeqGenes.Where(x => x.Value.filePath == file).ToList().Count != 0)
                {
                    MessageBox.Show("The following sample is already cached into the program." +
                        "The sample is ignored due to disallowed duplications." +
                        "\n\nDuplicated sample :\n" + file + "\n",
                        "Duplication encountered", MessageBoxButton.OK, MessageBoxImage.Information);
                    continue;
                }

                var refSeqGenesParser = new RefSeqGenesParser<Interval<int, MRefSeqGenes>, MRefSeqGenes>(
                    file,
                    runKey.genome,
                    runKey.assembly,
                    runKey.readOnlyValidChrs,
                    runKey.startOffset,
                    runKey.chrColumn,
                    runKey.leftColumn,
                    runKey.rightColumn,
                    runKey.refseqIDColum,
                    runKey.geneSymbolColumn,
                    runKey.strandColumn);
                refSeqGenesParser.decimalPlaces = 5;
                refSeqGenesParser.StatusChanged += ParserStatusChanged;

                /// This informs the function that a new sample is 
                /// started to be parsed, hence overall progress bar 
                /// needs to be updated
                ParserStatusChanged(this, new ParserEventArgs("s"));

                var data = refSeqGenesParser.Parse();

                if (refSeqGenesParser.missingChrs.Count != 0 && refSeqGenesParser.excessChrs.Count != 0)
                {
                    #region -_-     Message Creation                   -_-

                    string message = "The following sample is expected to be of type " + runKey.genome + ", however it is incommensurable in terms of present chromosomes.";

                    if (refSeqGenesParser.missingChrs.Count != 0)
                    {
                        message += "\n\nThe sample lacks " + refSeqGenesParser.missingChrs.Count.ToString() + " ";
                        if (refSeqGenesParser.missingChrs.Count == 1) message += "chromosome";
                        else message += "chromosomes";

                        message += " (i.e., " + refSeqGenesParser.missingChrs[0];

                        for (int i = 1; i < refSeqGenesParser.missingChrs.Count; i++)
                            if (i != refSeqGenesParser.missingChrs.Count - 1)
                                message += ", " + refSeqGenesParser.missingChrs[i];
                            else
                                message += ", and " + refSeqGenesParser.missingChrs[i];

                        message += " ).";
                        if (refSeqGenesParser.missingChrs.Count == 1) message += "This chromosome ";
                        else message += "These chromosomes ";

                        message += "will be considered as 'null' without effecting the analysis process.";
                    }

                    if (refSeqGenesParser.excessChrs.Count != 0)
                    {
                        message += "\n\nThe sample encompasses " + refSeqGenesParser.excessChrs.Count.ToString() + " excess ";

                        if (refSeqGenesParser.excessChrs.Count == 1) message += "chromosome";
                        else message += "chromosomes";

                        message += " (i.e., " + refSeqGenesParser.excessChrs[0];
                        for (int i = 1; i < refSeqGenesParser.excessChrs.Count; i++)
                            if (i != refSeqGenesParser.excessChrs.Count - 1)
                                message += ", " + refSeqGenesParser.excessChrs[i];
                            else
                                message += ", and " + refSeqGenesParser.excessChrs[i];

                        message += " ).The reads on ";
                        if (refSeqGenesParser.excessChrs.Count == 1) message += "this chromosome ";
                        else message += "these chromosomes ";


                        message += "are not cached and hence the analysis will not be effected.";
                    }

                    message += "\n\nDo you want to keep the sample anyway ?";
                    message += "\n\nThe ambiguous sample is :\n" + file + "\n";

                    #endregion
                    #region -_-     Message Display + Upload data_____________________1      -_-

                    MessageBoxResult r = MessageBox.Show(
                        message,
                        "Sample chromosome count warning",
                        MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);

                    if (r == MessageBoxResult.Cancel || r == MessageBoxResult.No) return;

                    #endregion
                }

                Dispatcher.Invoke((Action)(() =>
                {
                    RefSeqGenes<Interval<int, MRefSeqGenes>, MRefSeqGenes>.Data.Add(data.fileHashKey, data);
                    ApplicationViewModel.Default.cachedFeaturesSummary.Add(new Models.CachedFeaturesSummary(
                        selected: false,
                                fileHashKey: data.fileHashKey,
                                label: data.fileName,
                                featureCount: data.intervalsCount,
                                genome: data.genome,
                                assembly: data.assembly,
                                dataType: Models.DataType.RefSeqGenes));
                }));
            }
        }
        private void Parser_BGW__ReadGeneralFeatures(ParserOptions runKey)
        {
            var generalFeatures = GeneralFeatures<Interval<int, MGeneralFeatures>, MGeneralFeatures>.Data;
            foreach (string file in inputFileChooser.FileNames)
            {
                if (generalFeatures.Where(x => x.Value.filePath == file).ToList().Count != 0)
                {
                    MessageBox.Show("The following sample is already cached into the program." +
                        "The sample is ignored due to disallowed duplications." +
                        "\n\nDuplicated sample :\n" + file + "\n",
                        "Duplication encountered", MessageBoxButton.OK, MessageBoxImage.Information);
                    continue;
                }

                var generalFeaturesParser = new GeneralFeaturesParser<Interval<int, MGeneralFeatures>, MGeneralFeatures>(
                    file,
                    runKey.genome,
                    runKey.assembly,
                    runKey.readOnlyValidChrs,
                    runKey.startOffset,
                    runKey.chrColumn,
                    runKey.leftColumn,
                    runKey.rightColumn,
                    runKey.featureColumn,
                    runKey.attributeColumn);
                generalFeaturesParser.decimalPlaces = 5;
                generalFeaturesParser.StatusChanged += ParserStatusChanged;

                /// This informs the function that a new sample is 
                /// started to be parsed, hence overall progress bar 
                /// needs to be updated
                ParserStatusChanged(this, new ParserEventArgs("s"));

                var data = generalFeaturesParser.Parse();

                if (generalFeaturesParser.missingChrs.Count != 0 && generalFeaturesParser.excessChrs.Count != 0)
                {
                    #region -_-     Message Creation                   -_-

                    string message = "The following sample is expected to be of type " + runKey.genome + ", however it is incommensurable in terms of present chromosomes.";

                    if (generalFeaturesParser.missingChrs.Count != 0)
                    {
                        message += "\n\nThe sample lacks " + generalFeaturesParser.missingChrs.Count.ToString() + " ";

                        if (generalFeaturesParser.missingChrs.Count == 1) message += "chromosome";
                        else message += "chromosomes";

                        message += " (i.e., " + generalFeaturesParser.missingChrs[0];

                        for (int i = 1; i < generalFeaturesParser.missingChrs.Count; i++)
                            if (i != generalFeaturesParser.missingChrs.Count - 1)
                                message += ", " + generalFeaturesParser.missingChrs[i];
                            else
                                message += ", and " + generalFeaturesParser.missingChrs[i];

                        message += " ).";
                        if (generalFeaturesParser.missingChrs.Count == 1) message += "This chromosome ";
                        else message += "These chromosomes ";

                        message += "will be considered as 'null' without effecting the analysis process.";
                    }

                    if (generalFeaturesParser.excessChrs.Count != 0)
                    {
                        message += "\n\nThe sample encompasses " + generalFeaturesParser.excessChrs.Count.ToString() + " excess ";

                        if (generalFeaturesParser.excessChrs.Count == 1) message += "chromosome";
                        else message += "chromosomes";

                        message += " (i.e., " + generalFeaturesParser.excessChrs[0];
                        for (int i = 1; i < generalFeaturesParser.excessChrs.Count; i++)
                            if (i != generalFeaturesParser.excessChrs.Count - 1)
                                message += ", " + generalFeaturesParser.excessChrs[i];
                            else
                                message += ", and " + generalFeaturesParser.excessChrs[i];

                        message += " ).The reads on ";
                        if (generalFeaturesParser.excessChrs.Count == 1) message += "this chromosome ";
                        else message += "these chromosomes ";

                        message += "are not cached and hence the analysis will not be effected.";
                    }

                    message += "\n\nDo you want to keep the sample anyway ?";
                    message += "\n\nThe ambiguous sample is :\n" + file + "\n";

                    #endregion
                    #region -_-     Message Display + Upload data_____________________1      -_-

                    MessageBoxResult r = MessageBox.Show(
                        message,
                        "Sample chromosome count warning",
                        MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);

                    if (r == MessageBoxResult.Cancel || r == MessageBoxResult.No) return;

                    #endregion
                }

                Dispatcher.Invoke((Action)(() =>
                {
                    GeneralFeatures<Interval<int, MGeneralFeatures>, MGeneralFeatures>.Data.Add(data.fileHashKey, data);
                    ApplicationViewModel.Default.cachedFeaturesSummary.Add(new Models.CachedFeaturesSummary(
                        selected: false,
                                fileHashKey: data.fileHashKey,
                                label: data.fileName,
                                featureCount: data.intervalsCount,
                                genome: data.genome,
                                assembly: data.assembly,
                                dataType: Models.DataType.GeneralFeatures));
                }));
            }
        }
        void ParserStatusChanged(object sender, ParserEventArgs e)
        {
            if (e.Value[0] == 's') Dispatcher.Invoke((Action)(() => { OverallProcess_PB.Value++; }));
            else Dispatcher.Invoke((Action)(() => { CurrentSample_PB.Value = Convert.ToDouble(e.Value); }));
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
            this.Close();
        }

        #endregion

        #region -_-     Field_*_Title MouseEnter                          -_-

        private void Field_1_Title_L_MouseEnter(object sender, MouseEventArgs e)
        {
            Mouse_Exit__Control_EventHandler();
            Mouse_Enter_Control_EventHandler(Field_1_Title_L, Field_1_Index_L);
        }
        private void Field_2_Title_L_MouseEnter(object sender, MouseEventArgs e)
        {
            Mouse_Exit__Control_EventHandler();
            Mouse_Enter_Control_EventHandler(Field_2_Title_L, Field_2_Index_L);
        }
        private void Field_3_Title_L_MouseEnter(object sender, MouseEventArgs e)
        {
            Mouse_Exit__Control_EventHandler();
            Mouse_Enter_Control_EventHandler(Field_3_Title_L, Field_3_Index_L);
        }
        private void Field_4_Title_L_MouseEnter(object sender, MouseEventArgs e)
        {
            Mouse_Exit__Control_EventHandler();
            Mouse_Enter_Control_EventHandler(Field_4_Title_L, Field_4_Index_L);
        }
        private void Field_5_Title_L_MouseEnter(object sender, MouseEventArgs e)
        {
            Mouse_Exit__Control_EventHandler();
            Mouse_Enter_Control_EventHandler(Field_5_Title_L, Field_5_Index_L);
        }
        private void Field_6_Title_L_MouseEnter(object sender, MouseEventArgs e)
        {
            Mouse_Exit__Control_EventHandler();
            Mouse_Enter_Control_EventHandler(Field_6_Title_L, Field_6_Index_L);
        }
        private void Field_7_Title_L_MouseEnter(object sender, MouseEventArgs e)
        {
            Mouse_Exit__Control_EventHandler();
            Mouse_Enter_Control_EventHandler(Field_7_Title_L, Field_7_Index_L);
        }
        private void Field_8_Title_L_MouseEnter(object sender, MouseEventArgs e)
        {
            Mouse_Exit__Control_EventHandler();
            Mouse_Enter_Control_EventHandler(Field_8_Title_L, Field_8_Index_L);
        }
        private void Field_9_Title_L_MouseEnter(object sender, MouseEventArgs e)
        {
            Mouse_Exit__Control_EventHandler();
            Mouse_Enter_Control_EventHandler(Field_9_Title_L, Field_9_Index_L);
        }
        private void Field_10_Title_L_MouseEnter(object sender, MouseEventArgs e)
        {
            Mouse_Exit__Control_EventHandler();
            Mouse_Enter_Control_EventHandler(Field_10_Title_L, Field_10_Index_L);
        }
        private void Field_11_Title_L_MouseEnter(object sender, MouseEventArgs e)
        {
            Mouse_Exit__Control_EventHandler();
            Mouse_Enter_Control_EventHandler(Field_11_Title_L, Field_11_Index_L);
        }
        private void Field_12_Title_L_MouseEnter(object sender, MouseEventArgs e)
        {
            Mouse_Exit__Control_EventHandler();
            Mouse_Enter_Control_EventHandler(Field_12_Title_L, Field_12_Index_L);
        }

        #endregion

        #region -_-     Field_*_Index MouseEnter                          -_-

        private void Field_1_Index_L_MouseEnter(object sender, MouseEventArgs e)
        {
            Mouse_Exit__Control_EventHandler();
            Mouse_Enter_Control_EventHandler(Field_1_Title_L, Field_1_Index_L);
        }
        private void Field_2_Index_L_MouseEnter(object sender, MouseEventArgs e)
        {
            Mouse_Exit__Control_EventHandler();
            Mouse_Enter_Control_EventHandler(Field_2_Title_L, Field_2_Index_L);
        }
        private void Field_3_Index_L_MouseEnter(object sender, MouseEventArgs e)
        {
            Mouse_Exit__Control_EventHandler();
            Mouse_Enter_Control_EventHandler(Field_3_Title_L, Field_3_Index_L);
        }
        private void Field_4_Index_L_MouseEnter(object sender, MouseEventArgs e)
        {
            Mouse_Exit__Control_EventHandler();
            Mouse_Enter_Control_EventHandler(Field_4_Title_L, Field_4_Index_L);
        }
        private void Field_5_Index_L_MouseEnter(object sender, MouseEventArgs e)
        {
            Mouse_Exit__Control_EventHandler();
            Mouse_Enter_Control_EventHandler(Field_5_Title_L, Field_5_Index_L);
        }
        private void Field_6_Index_L_MouseEnter(object sender, MouseEventArgs e)
        {
            Mouse_Exit__Control_EventHandler();
            Mouse_Enter_Control_EventHandler(Field_6_Title_L, Field_6_Index_L);
        }
        private void Field_7_Index_L_MouseEnter(object sender, MouseEventArgs e)
        {
            Mouse_Exit__Control_EventHandler();
            Mouse_Enter_Control_EventHandler(Field_7_Title_L, Field_7_Index_L);
        }
        private void Field_8_Index_L_MouseEnter(object sender, MouseEventArgs e)
        {
            Mouse_Exit__Control_EventHandler();
            Mouse_Enter_Control_EventHandler(Field_8_Title_L, Field_8_Index_L);
        }
        private void Field_9_Index_L_MouseEnter(object sender, MouseEventArgs e)
        {
            Mouse_Exit__Control_EventHandler();
            Mouse_Enter_Control_EventHandler(Field_9_Title_L, Field_9_Index_L);
        }
        private void Field_10_Index_L_MouseEnter(object sender, MouseEventArgs e)
        {
            Mouse_Exit__Control_EventHandler();
            Mouse_Enter_Control_EventHandler(Field_10_Title_L, Field_10_Index_L);
        }
        private void Field_11_Index_L_MouseEnter(object sender, MouseEventArgs e)
        {
            Mouse_Exit__Control_EventHandler();
            Mouse_Enter_Control_EventHandler(Field_11_Title_L, Field_11_Index_L);
        }
        private void Field_12_Index_L_MouseEnter(object sender, MouseEventArgs e)
        {
            Mouse_Exit__Control_EventHandler();
            Mouse_Enter_Control_EventHandler(Field_12_Title_L, Field_12_Index_L);
        }

        #endregion

        #region -_-     Fields Re-Arrangemnt Fucntions                    -_-

        private void Mouse_Exit__Control_EventHandler()
        {
            #region -_-     Clear Field Title Background        -_-

            switch (ActiveColumnIndex(Canvas.GetLeft(ShiftLeft_L)))
            {
                case 0: Field_1_Title_L.Background = _MouseLeave_BC; Field_1_Index_L.Background = _MouseLeave_BC; break;
                case 1: Field_2_Title_L.Background = _MouseLeave_BC; Field_2_Index_L.Background = _MouseLeave_BC; break;
                case 2: Field_3_Title_L.Background = _MouseLeave_BC; Field_3_Index_L.Background = _MouseLeave_BC; break;
                case 3: Field_4_Title_L.Background = _MouseLeave_BC; Field_4_Index_L.Background = _MouseLeave_BC; break;
                case 4: Field_5_Title_L.Background = _MouseLeave_BC; Field_5_Index_L.Background = _MouseLeave_BC; break;
                case 5: Field_6_Title_L.Background = _MouseLeave_BC; Field_6_Index_L.Background = _MouseLeave_BC; break;
                case 6: Field_7_Title_L.Background = _MouseLeave_BC; Field_7_Index_L.Background = _MouseLeave_BC; break;
                case 7: Field_8_Title_L.Background = _MouseLeave_BC; Field_8_Index_L.Background = _MouseLeave_BC; break;
                case 8: Field_9_Title_L.Background = _MouseLeave_BC; Field_9_Index_L.Background = _MouseLeave_BC; break;
                case 9: Field_10_Title_L.Background = _MouseLeave_BC; Field_10_Index_L.Background = _MouseLeave_BC; break;
                case 10: Field_11_Title_L.Background = _MouseLeave_BC; Field_11_Index_L.Background = _MouseLeave_BC; break;
                case 11: Field_12_Title_L.Background = _MouseLeave_BC; Field_12_Index_L.Background = _MouseLeave_BC; break;
            }

            #endregion

            ShiftLeft_L.Visibility = Visibility.Hidden;
            ShiftRight_L.Visibility = Visibility.Hidden;

            ShiftLeft_L.Foreground = new SolidColorBrush(Colors.White);
            ShiftRight_L.Foreground = new SolidColorBrush(Colors.White);
        }
        private void Mouse_Enter_Control_EventHandler(System.Windows.Controls.Label field_title, System.Windows.Controls.Label field_index)
        {
            field_title.Background = _MouseEnter_BC;
            field_index.Background = _MouseEnter_BC;

            ShiftLeft_L.Visibility = Visibility.Visible;
            ShiftRight_L.Visibility = Visibility.Visible;

            Canvas.SetLeft(ShiftLeft_L, Canvas.GetLeft(field_title));
            Canvas.SetLeft(ShiftRight_L, Canvas.GetLeft(field_title) + _sbw);
        }

        private void Shift_Left_L_MouseEnter(object sender, MouseEventArgs e)
        {
            ShiftLeft_L.Foreground = new SolidColorBrush(Colors.OrangeRed);
            ShiftRight_L.Foreground = new SolidColorBrush(Colors.White);
        }
        private void Shift_Left_L_MouseDown(object sender, MouseButtonEventArgs e)
        {
            int current_position = ActiveColumnIndex(Canvas.GetLeft(ShiftLeft_L));

            if (current_position != 0)
            {
                string temp_title = _fileSchema[current_position - 1];
                _fileSchema[current_position - 1] = _fileSchema[current_position];
                _fileSchema[current_position] = temp_title;
            }
            else
            {
                string temp_title = _fileSchema[11];
                _fileSchema[11] = _fileSchema[0];
                _fileSchema[0] = temp_title;
            }

            UpdateFieldTitlesWithFileSchema();
        }

        private void Shift_Right_L_MouseEnter(object sender, MouseEventArgs e)
        {
            ShiftLeft_L.Foreground = new SolidColorBrush(Colors.White);
            ShiftRight_L.Foreground = new SolidColorBrush(Colors.OrangeRed);
        }
        private void Shift_Right_L_MouseDown(object sender, MouseButtonEventArgs e)
        {
            int current_position = ActiveColumnIndex(Canvas.GetLeft(ShiftLeft_L));

            if (current_position != 11)
            {
                string temp_title = _fileSchema[current_position + 1];
                _fileSchema[current_position + 1] = _fileSchema[current_position];
                _fileSchema[current_position] = temp_title;
            }
            else
            {
                string temp_title = _fileSchema[0];
                _fileSchema[0] = _fileSchema[11];
                _fileSchema[11] = temp_title;
            }

            UpdateFieldTitlesWithFileSchema();
        }

        /// <summary>
        /// Returns the column number which is active (i.e., it is highlighted)
        /// </summary>
        /// <param propertyName="position">The left position of column title or left-arrow</param>
        /// <returns>Zero-based number of active column</returns>
        private int ActiveColumnIndex(double position)
        {
            if (!double.IsNaN(position))
                return (Convert.ToInt32(position) - _o) / (_w + _d);
            else return -1;
        }

        #endregion


        /// <summary>
        /// Display'flowDocument a small sample portion of the first selected file.
        /// This helps user having an insight on file to be able to simply specifying
        /// proper parameters for parsing.
        /// </summary>
        private void DisplayFractionOfSample()
        {
            _fileSamplePortion.Clear();

            if (inputFileChooser.FileNames.Length > 0)
            {
                StreamReader fileReader = new StreamReader(inputFileChooser.FileNames[0]);

                string line;
                byte lineCounter = 0;

                for (int i = 0; i < Convert.ToInt32(Off_Set_Line_TB.Text); i++)
                {
                    line = fileReader.ReadLine();
                    lineCounter++;
                }

                while ((line = fileReader.ReadLine()) != null && lineCounter < 30)
                {
                    lineCounter++;
                    string[] splitted_Line = line.Split('\t');
                    FileSample temp_line = new FileSample();
                    temp_line.column_0 = splitted_Line[0];

                    /// IMPORTANT NOTE
                    /// InquireValidity relies on "null" Value to determine if propertyName column is correctly mapping or not ?
                    /// Hence, if "null" is changed here, InquireValidity must also be updated.
                    if (splitted_Line.Length >= 2) temp_line.column_1 = splitted_Line[1]; else temp_line.column_1 = "null";
                    if (splitted_Line.Length >= 3) temp_line.column_2 = splitted_Line[2]; else temp_line.column_2 = "null";
                    if (splitted_Line.Length >= 4) temp_line.column_3 = splitted_Line[3]; else temp_line.column_3 = "null";
                    if (splitted_Line.Length >= 5) temp_line.column_4 = splitted_Line[4]; else temp_line.column_4 = "null";
                    if (splitted_Line.Length >= 6) temp_line.column_5 = splitted_Line[5]; else temp_line.column_5 = "null";
                    if (splitted_Line.Length >= 7) temp_line.column_6 = splitted_Line[6]; else temp_line.column_6 = "null";
                    if (splitted_Line.Length >= 8) temp_line.column_7 = splitted_Line[7]; else temp_line.column_7 = "null";
                    if (splitted_Line.Length >= 9) temp_line.column_8 = splitted_Line[8]; else temp_line.column_8 = "null";
                    if (splitted_Line.Length >= 10) temp_line.column_9 = splitted_Line[9]; else temp_line.column_9 = "null";
                    if (splitted_Line.Length >= 11) temp_line.column_10 = splitted_Line[10]; else temp_line.column_10 = "null";
                    if (splitted_Line.Length >= 12) temp_line.column_11 = splitted_Line[11]; else temp_line.column_11 = "null";

                    _fileSamplePortion.Add(temp_line);
                }
            }
        }


        private class InquireValidity : IValueConverter
        {
            public object Convert(object obj, Type targetType, object parameter, CultureInfo culture)
            {
                switch (Inquire_Key)
                {
                    case "chr":
                        string[] splitted_chromosome_number = ((string)obj).Split('r');
                        int chr_number = 0;

                        if (splitted_chromosome_number.Length == 2 && splitted_chromosome_number[0] == "ch")
                        {
                            if (int.TryParse(splitted_chromosome_number[1], out chr_number))
                                return true;
                            else
                            {
                                if (splitted_chromosome_number[1].ToLower() == "x" ||
                                    splitted_chromosome_number[1].ToLower() == "y" ||
                                    splitted_chromosome_number[1].ToLower() == "m")
                                    return true;
                                else
                                    return false;
                            }
                        }
                        else
                            return false;

                    case "start":
                    case "stop":
                    case "summit":
                        uint start = 0;
                        return uint.TryParse((string)obj, out start);

                    case "name":
                        double t_double_number;
                        int t_int_number;
                        if (double.TryParse((string)obj, out t_double_number)) return '0';
                        if (int.TryParse((string)obj, out t_int_number)) return '0';
                        if ((string)obj == "null") return '1';
                        return true;


                    case "p-value": double p_value = 0.0; return double.TryParse((string)obj, out p_value);

                    default: return true;
                }
            }

            public object ConvertBack(object value, Type targetTypes, object parameter, CultureInfo culture)
            {
                return null;
            }

            public string Inquire_Key { set; get; }
        }

        private void Genome_CB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (var genome in _genomeInfo)
                if (genome.Value.genomeTitle == (string)Genome_CB.SelectedItem)
                {
                    Assembly_CB.Items.Clear();
                    foreach (var assembly in genome.Value.genomeAssemblies)
                        Assembly_CB.Items.Add((string)assembly.Value);
                    Assembly_CB.SelectedIndex = 0;
                    break;
                }
        }
    }
}
