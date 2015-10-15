/** Copyright © 2013-2015 Vahid Jalili
 * 
 * This file is part of MuSERA project.
 * MuSERA is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation,
 * either version 3 of the License, or (at your option) any later version.
 * MuSERA is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A 
 * PARTICULAR PURPOSE. See the GNU General Public License for more details.
 * You should have received a copy of the GNU General Public License along with Foobar. If not, see http://www.gnu.org/licenses/.
 **/

using System;
using System.ComponentModel;

namespace Polimi.DEIB.VahidJalili.MuSERA.Warehouse
{
    public class PValueFrequency : INotifyPropertyChanged, IEditableObject
    {
        public int frequency { set { _frequency = value; NotifyPropertyChanged("frequency"); } get { return _frequency; } }
        private int _frequency;

        public double logpValue { protected set { _logpValue = value; NotifyPropertyChanged("logpValue"); } get { return _logpValue; } }
        private double _logpValue;

        public double pValue { get { return (double)(Math.Pow(10, _logpValue / -10.0)); } }


        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(name));
        }

        public void BeginEdit()
        {
            return;
        }
        public void CancelEdit()
        {
            return;
        }
        public void EndEdit()
        {
            return;
        }
    }
}
