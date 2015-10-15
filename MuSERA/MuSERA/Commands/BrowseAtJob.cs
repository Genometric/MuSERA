/** Copyright © 2013-2015 Vahid Jalili
 * 
 * This file is part of MuSERA project.
 * MuSERA is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation,
 * either version 3 of the License, or (at your option) any later version.
 * MuSERA is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A 
 * PARTICULAR PURPOSE. See the GNU General Public License for more details.
 * You should have received a copy of the GNU General Public License along with Foobar. If not, see http://www.gnu.org/licenses/.
 **/

using Microsoft.Win32;
using Polimi.DEIB.VahidJalili.MuSERA.ViewModels;
using System;
using System.Windows.Input;

namespace Polimi.DEIB.VahidJalili.MuSERA.Commands
{
    internal class BrowseAtJob : ICommand
    {
        public event EventHandler CanExecuteChanged
        {
            /// to wire back to WPF command system
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
        public bool CanExecute(object parameter)
        {
            return ApplicationViewModel.Default.batchModeViewModel.isFree;
        }
        public void Execute(object parameter)
        {
            var dialog = new OpenFileDialog();
            dialog.Multiselect = false;
            dialog.Filter = "XML files (*.XML)|*.xml";
            Nullable<bool> result = dialog.ShowDialog();
            if (result == true)
                ApplicationViewModel.Default.batchModeViewModel.atJobFileAbsolutePath = dialog.FileName;
        }
    }
}
