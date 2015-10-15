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
using Polimi.DEIB.VahidJalili.MuSERA.Views;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Input;
using System.Windows.Media.Effects;

namespace Polimi.DEIB.VahidJalili.MuSERA.Commands
{
    internal class SaveCommand : ICommand
    {
        public SaveCommand(BlurEffect uiElementBlurEffect)
        {
            _uiElementBlurEffect = uiElementBlurEffect;
        }

        private BlurEffect _uiElementBlurEffect { set; get; }

        public event EventHandler CanExecuteChanged
        {
            /// to wire back to WPF command system
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
        public bool CanExecute(object parameter)
        {
            if (parameter == null) return false;
            return ((ObservableCollection<SessionSummary>)parameter).Count > 0 ? true : false;
        }
        public void Execute(object parameter)
        {
            _uiElementBlurEffect.Radius = 10;
            SaveView saveView = new SaveView((
                ObservableCollection<SessionSummary>)parameter,
                Environment.CurrentDirectory + Path.DirectorySeparatorChar);
            saveView.ShowDialog();
            _uiElementBlurEffect.Radius = 0;
        }
    }
}
