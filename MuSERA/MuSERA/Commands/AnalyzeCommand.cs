/** Copyright © 2013-2015 Vahid Jalili
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
using Polimi.DEIB.VahidJalili.MuSERA.Views;
using Polimi.DEIB.VahidJalili.MuSERA.Warehouse;
using System;
using System.Collections;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Effects;

namespace Polimi.DEIB.VahidJalili.MuSERA.Commands
{
    internal class AnalyzeCommand : AnalyzeBase, ICommand
    {
        public AnalyzeCommand(ApplicationViewModel applicationViewModel, AnalysisOptionsViewModel analysisOptionsViewModel, BlurEffect UIElementBlurEffect)
        {
            _applicationViewModel = applicationViewModel;
            _applicationViewModel.uiProperties.analysisET = analysisET;
            _analysisOptionsViewModel = analysisOptionsViewModel;
            _uiElementBlurEffect = UIElementBlurEffect;
        }

        private ApplicationViewModel _applicationViewModel;
        private AnalysisOptionsViewModel _analysisOptionsViewModel;
        private BlurEffect _uiElementBlurEffect;

        public event EventHandler CanExecuteChanged
        {
            /// to wire back to WPF command system
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
        public bool CanExecute(object parameter)
        {
            if (parameter == null) return false;
            return ((IList)parameter).Count > 1 ? true : false;
        }
        public void Execute(object parameter)
        {
            var selectedSamples = ((IList)parameter).Cast<CachedDataSummary>().ToList<CachedDataSummary>();

            _uiElementBlurEffect.Radius = 10;
            AnalysisOptionsView analysisOptionsView = new AnalysisOptionsView();
            analysisOptionsView.selectedSampleCount = Convert.ToByte(selectedSamples.Count);
            analysisOptionsView.BioRep_RB.IsChecked = true;
            analysisOptionsView.C_TB.Text = Math.Ceiling(Convert.ToByte(selectedSamples.Count) / 2.0).ToString();
            analysisOptionsView.ShowDialog();
            _uiElementBlurEffect.Radius = 0;
            if (analysisOptionsView.DialogResult == true)
            {
                foreach (var selectedSample in selectedSamples)
                    AddSample(Samples<Interval<int, MChIPSeqPeak>, MChIPSeqPeak>.Data[selectedSample.fileHashKey]);

                analysisOptions = analysisOptionsView.Options;

                try { Execute(); }
                catch (OutOfMemoryException oom)
                {
                    MessageBox.Show("Out-of-memory !!\n" +
                    "MuSERA does not have enough free memory space to execute the analysis. The program exits now.",
                    "OutOfMemory", MessageBoxButton.OK, MessageBoxImage.Stop);
                    Application.Current.Shutdown();
                }
            }
        }
    }
}
