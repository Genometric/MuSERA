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

namespace Polimi.DEIB.VahidJalili.MuSERA.Models
{
    /// <summary>
    /// Multi-Select ComboBox Node for Chromosomes.
    /// </summary>
    internal class MSCBNChr : INotifyPropertyChanged
    {
        public MSCBNChr(bool isChecked, string chr)
        {
            this.isChecked = isChecked;
            this.chr = chr;
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

        public string chr
        {
            set
            {
                _chr = value;
                NotifyPropertyChanged("chr");
            }
            get { return _chr; }
        }
        private string _chr;

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
