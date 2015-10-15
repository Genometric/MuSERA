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
using System;
using System.Windows.Input;
using System.Windows.Media;

namespace Polimi.DEIB.VahidJalili.MuSERA.Commands
{
    internal class ChangeSampleColorCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        public bool CanExecute(object parameter)
        {
            return true;
        }
        public void Execute(object parameter)
        {
            var summary = (CachedDataSummary)parameter;
            var dialog = new System.Windows.Forms.ColorDialog();
            dialog.AllowFullOpen = true;
            dialog.AnyColor = true;
            dialog.FullOpen = true;
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string colorCode = System.Drawing.ColorTranslator.ToHtml(dialog.Color);
                summary.color = new SolidColorBrush((Color)ColorConverter.ConvertFromString(colorCode));
                ApplicationViewModel.Default.sessionsViewModel.plotData.plotOptions.SampleColor[summary.fileHashKey] = summary.color;
            }
        }
    }
}
