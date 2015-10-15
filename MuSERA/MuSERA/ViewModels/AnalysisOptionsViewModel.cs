/** Copyright © 2013-2015 Vahid Jalili
 * 
 * This file is part of MuSERA project.
 * MuSERA is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation,
 * either version 3 of the License, or (at your option) any later version.
 * MuSERA is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A 
 * PARTICULAR PURPOSE. See the GNU General Public License for more details.
 * You should have received a copy of the GNU General Public License along with Foobar. If not, see http://www.gnu.org/licenses/.
 **/

using Polimi.DEIB.VahidJalili.MuSERA.Warehouse;
using System.ComponentModel;

namespace Polimi.DEIB.VahidJalili.MuSERA.ViewModels
{
    internal class AnalysisOptionsViewModel : INotifyPropertyChanged
    {
        private AnalysisOptions _analysisOptions;
        public AnalysisOptions analysisOptions { get { return _analysisOptions; } set { _analysisOptions = value; NotifyPropertyChanged("analysisOptions"); } }


        public bool technicalReplicate { set; get; }
        public bool biologicalReplicate { set; get; }


        private string _info;
        public string info
        {
            get { return _info; }
            set
            {
                _info = value;
                NotifyPropertyChanged("info");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
