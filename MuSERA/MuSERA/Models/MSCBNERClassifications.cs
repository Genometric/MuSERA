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

namespace Polimi.DEIB.VahidJalili.MuSERA.Models
{
    /// <summary>
    /// Multi-Select ComboBox Node for ER Classifications.
    /// </summary>
    internal class MSCBNERClassifications : INotifyPropertyChanged
    {
        public MSCBNERClassifications(bool isChecked, ERClassificationType classification)
        {
            this.isChecked = isChecked;
            this.classification = classification;
        }

        public bool isChecked
        {
            set
            {
                _isChecked = value;
                NotifyPropertyChanged("isChecked");
            }
            get { return _isChecked; }
        }
        private bool _isChecked;

        public ERClassificationType classification
        {
            set
            {
                _classification = value;
                NotifyPropertyChanged("classification");
            }
            get { return _classification; }
        }
        private ERClassificationType _classification;

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
