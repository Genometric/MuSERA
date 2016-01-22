/** Copyright © 2013-2015 Vahid Jalili
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
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace Polimi.DEIB.VahidJalili.MuSERA.Commands
{
    internal class ChangeSelectedERCommand : ICommand
    {
        public event EventHandler CanExecuteChanged
        {
            /// to wire back to WPF command system
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
        public bool CanExecute(object parameter)
        {
            if (((object[])parameter)[0] == null)
            {
                MessageBox.Show(
                    "You have not selected an ER to be displayed on Genome Browser.\n"+
                    "Please select a chromosome and an ER.",
                    "No enriched region is selected",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
                return false;
            }

            if (ApplicationViewModel.Default.sessionsViewModel.di3.status != 0 &&
                ApplicationViewModel.Default.sessionsViewModel.di3.status != 100)
            {
                MessageBoxResult result = MessageBox.Show(
                    "MuSERA is busy preparing the data of the session.\n" +
                    "MuSERA can still plot you data, however, the displayed data might be incomplete. " +
                    "Therefore, it is recommend to wait for the data prepration completion.\n"+
                    "Do you want to wait ?",
                    "MuSERA is busy ...",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);

                return result == MessageBoxResult.Yes ? false : true;
            }

            return true;
        }
        public void Execute(object parameter)
        {
            var parameters = (object[])parameter;
            switch ((string)parameters[1])
            {
                case "stringent":
                    ApplicationViewModel.Default.sessionsViewModel.selectedER = new AnalysisResult<Interval<int, MChIPSeqPeak>, MChIPSeqPeak>.ProcessedER()
                    {
                        er = (Interval<int, MChIPSeqPeak>)parameters[0],
                        classification = ERClassificationType.Stringent
                    };
                    break;

                case "weak":
                    ApplicationViewModel.Default.sessionsViewModel.selectedER = new AnalysisResult<Interval<int, MChIPSeqPeak>, MChIPSeqPeak>.ProcessedER()
                    {
                        er = (Interval<int, MChIPSeqPeak>)parameters[0],
                        classification = ERClassificationType.Weak
                    };
                    break;

                case "noise":
                    ApplicationViewModel.Default.sessionsViewModel.selectedER = new AnalysisResult<Interval<int, MChIPSeqPeak>, MChIPSeqPeak>.ProcessedER()
                    {
                        er = (Interval<int, MChIPSeqPeak>)parameters[0],
                        classification = ERClassificationType.Background
                    };
                    break;

                case "confirmed":
                case "discarded":
                    ApplicationViewModel.Default.sessionsViewModel.selectedER = ((KeyValuePair<UInt64, AnalysisResult<Interval<int, MChIPSeqPeak>, MChIPSeqPeak>.ProcessedER>)parameters[0]).Value;
                    break;

                case "output":
                    ApplicationViewModel.Default.sessionsViewModel.selectedER = (AnalysisResult<Interval<int, MChIPSeqPeak>, MChIPSeqPeak>.ProcessedER)parameters[0];
                    break;
            }
        }
    }
}
