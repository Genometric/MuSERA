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

using Polimi.DEIB.VahidJalili.MuSERA.Commands;
using Polimi.DEIB.VahidJalili.MuSERA.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Polimi.DEIB.VahidJalili.MuSERA.ViewModels
{
    /// <summary>
    /// Self-register view model for cached features.
    /// </summary>
    internal class CachedFeaturesSummaryViewModel
    {
        public CachedFeaturesSummaryViewModel()
        {
            // Self-registration requirement.
            Default = this;
            _cachedDataSummary = new ObservableCollection<CachedFeaturesSummary>();
        }

        private ObservableCollection<CachedFeaturesSummary> _cachedDataSummary;
        public ObservableCollection<CachedFeaturesSummary> cachedDataSummary { get { return _cachedDataSummary; } }

        // Self-registration requirement.
        public static CachedFeaturesSummaryViewModel Default { get; private set; }

        private ICommand _command;
        public ICommand SwitchFeatureInUse
        {
            get
            {
                if (_command == null)
                    _command = new DelegateCommand(CanExecute, Execute);
                return _command;
            }
        }
        private void Execute(object parameter)
        {
            var selectedRow = parameter as CachedFeaturesSummary;
            foreach (var cachedFeature in _cachedDataSummary)
                if (cachedFeature.fileHashKey == selectedRow.fileHashKey)
                    cachedFeature.selected = true;
                else if (cachedFeature.selected)
                    cachedFeature.selected = false;
        }
        private bool CanExecute(object parameter)
        {
            return true;
        }
    }
}
