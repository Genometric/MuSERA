/** Copyright © 2013-2015 Vahid Jalili
 * 
 * This file is part of MuSERA project.
 * MuSERA is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation,
 * either version 3 of the License, or (at your option) any later version.
 * MuSERA is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A 
 * PARTICULAR PURPOSE. See the GNU General Public License for more details.
 * You should have received a copy of the GNU General Public License along with Foobar. If not, see http://www.gnu.org/licenses/.
 **/

using Polimi.DEIB.VahidJalili.GIFP;
using System.ComponentModel;
using System.Windows.Media;

namespace Polimi.DEIB.VahidJalili.MuSERA.Models
{
    internal class CachedDataSummary : INotifyPropertyChanged
    {
        public CachedDataSummary(bool selected, uint fileHashKey, string label, int featureCount, Genomes genome, Assemblies assembly)
        {
            _selected = selected;
            _fileHashKey = fileHashKey;
            _label = label;
            _featureCount = featureCount;
            _genome = genome;
            _assembly = assembly;
            _color = SamplesDefaultColors.GetRandomColor();
        }

        public SolidColorBrush color
        {
            set
            {
                _color = value;
                NotifyPropertyChanged("color");
            }
            get { return _color; }
        }
        private SolidColorBrush _color;


        private bool _selected;
        public bool selected { set { _selected = value; NotifyPropertyChanged("selected"); } get { return _selected; } }

        private uint _fileHashKey;
        public uint fileHashKey { private set { _fileHashKey = value; NotifyPropertyChanged("hashKey"); } get { return _fileHashKey; } }

        private string _label;
        public string label { set { _label = value; NotifyPropertyChanged("label"); } get { return _label; } }

        private int _featureCount;
        public int featureCount { private set { _featureCount = value; NotifyPropertyChanged("featureCount"); } get { return _featureCount; } }

        private Genomes _genome;
        public Genomes genome { private set { _genome = value; NotifyPropertyChanged("genome"); } get { return _genome; } }

        private Assemblies _assembly;
        public Assemblies assembly { private set { _assembly = value; NotifyPropertyChanged("assembly"); } get { return _assembly; } }


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
