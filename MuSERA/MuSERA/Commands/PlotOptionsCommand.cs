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
using Polimi.DEIB.VahidJalili.MuSERA.Views;
using System;
using System.Windows.Input;
using System.Windows.Media.Effects;

namespace Polimi.DEIB.VahidJalili.MuSERA.Commands
{
    internal class PlotOptionsCommand : ICommand
    {
        public PlotOptionsCommand(ApplicationViewModel applicationViewModel, BlurEffect UIElementBlurEffect)
        {
            _applicationViewModel = applicationViewModel;
            _uiElementBlurEffect = UIElementBlurEffect;
        }

        ApplicationViewModel _applicationViewModel;
        private BlurEffect _uiElementBlurEffect;

        public event EventHandler CanExecuteChanged;
        public bool CanExecute(object parameter)
        {
            return _applicationViewModel.sessionsViewModel.selectedAnalysisResult == null ? false : true;
        }
        public void Execute(object parameter)
        {
            _uiElementBlurEffect.Radius = 10;
            PlotOptionsView plotOptionsView =
                new PlotOptionsView(
                    _applicationViewModel.sessionsViewModel.plotData.plotOptions,
                    _applicationViewModel.sessionsViewModel.selectedAnalysisResult.total____s +
                    _applicationViewModel.sessionsViewModel.selectedAnalysisResult.total____w);

            /// you may think of providing direct access to the total ER count of selected
            /// analysis result in PlotOptions class for instance. However, this will open issues
            /// with batch excecution that does not leverage on sessionsViewModel.

            plotOptionsView.ShowDialog();
            _uiElementBlurEffect.Radius = 0;
            if (plotOptionsView.DialogResult == true)
                _applicationViewModel.sessionsViewModel.plotData.plotOptions = plotOptionsView.Options;
        }
    }
}
