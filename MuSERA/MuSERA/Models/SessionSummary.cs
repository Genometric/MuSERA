﻿/** Copyright © 2013-2015 Vahid Jalili
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
    public class SessionSummary : INotifyPropertyChanged
    {
        public SessionSummary(string id, int index, string label)
        {
            _id = id;
            _index = index;
            _label = label;
        }

        public string id
        {
            private set
            {
                _id = value;
                NotifyPropertyChanged("id");
            }
            get { return _id; }
        }
        private string _id;

        public int index // TODO : is this used anywhere ?
        {
            set
            {
                _index = value;
                NotifyPropertyChanged("index");
            }
            get { return _index; }
        }
        private int _index;

        public string label
        {
            set
            {
                _label = value;
                NotifyPropertyChanged("label");
            }
            get { return _label; }
        }
        private string _label;

        protected void NotifyPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
