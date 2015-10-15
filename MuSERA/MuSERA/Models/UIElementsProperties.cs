/** Copyright © 2013-2015 Vahid Jalili
 * 
 * This file is part of MuSERA project.
 * MuSERA is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation,
 * either version 3 of the License, or (at your option) any later version.
 * MuSERA is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A 
 * PARTICULAR PURPOSE. See the GNU General Public License for more details.
 * You should have received a copy of the GNU General Public License along with Foobar. If not, see http://www.gnu.org/licenses/.
 **/

using System.ComponentModel;
using System.Windows;

namespace Polimi.DEIB.VahidJalili.MuSERA.Models
{
    internal class UIElementsProperties : INotifyPropertyChanged
    {
        public UIElementsProperties()
        {
            interactiveGUIState = InteractiveGUIState.Initial;
        }

        public int interactiveModeDetailsTabIndex
        {
            set
            {
                _interactiveModeDetailsTabIndex = value;
                NotifyPropertyChanged("interactiveModeParentTabIndex");
            }
            get
            { return _interactiveModeDetailsTabIndex; }
        }
        private int _interactiveModeDetailsTabIndex;

        public int resultsTabIndex
        {
            set
            {
                _resultsTabIndex = value;
                NotifyPropertyChanged("resultsTab");
            }
            get { return _resultsTabIndex; }
        }
        private int _resultsTabIndex;

        public InteractiveGUIState interactiveGUIState
        {
            set
            {
                _interactiveGUIState = value;
                ChangeUIState();
            }
            get { return _interactiveGUIState; }
        }
        private InteractiveGUIState _interactiveGUIState;

        public Visibility sAAnBTVisibility
        {
            private set
            {
                _sAAnBTVisibility = value;
                NotifyPropertyChanged("sAAnBTVisibility");
            }
            get { return _sAAnBTVisibility; }
        }
        private Visibility _sAAnBTVisibility;

        public Visibility analysisETVisibility
        {
            private set
            {
                _analysisETVisibility = value;
                NotifyPropertyChanged("analysisETVisibility");
            }
            get { return _analysisETVisibility; }
        }
        private Visibility _analysisETVisibility;

        public string analysisET
        {
            set
            {
                _analysisET = value;
                NotifyPropertyChanged("analysisET");
            }
            get { return _analysisET; }
        }
        private string _analysisET;

        private void ChangeUIState()
        {
            switch (_interactiveGUIState)
            {
                case InteractiveGUIState.Initial:
                    analysisET = "";
                    sAAnBTVisibility = Visibility.Visible;
                    analysisETVisibility = Visibility.Hidden;
                    interactiveModeDetailsTabIndex = 0;
                    break;

                case InteractiveGUIState.RunningAnalysis:
                    sAAnBTVisibility = Visibility.Hidden;
                    analysisETVisibility = Visibility.Visible;
                    interactiveModeDetailsTabIndex = 0;
                    break;

                case InteractiveGUIState.AnalysisFinished:
                    analysisET = "";
                    sAAnBTVisibility = Visibility.Visible;
                    analysisETVisibility = Visibility.Hidden;
                    break;

                case InteractiveGUIState.ShowSessionDetails:
                    interactiveModeDetailsTabIndex = 0;
                    resultsTabIndex = 0;
                    break;
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
