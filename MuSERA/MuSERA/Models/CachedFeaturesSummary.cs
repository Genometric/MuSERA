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

namespace Polimi.DEIB.VahidJalili.MuSERA.Models
{
    internal class CachedFeaturesSummary : INotifyPropertyChanged
    {
        public CachedFeaturesSummary(bool selected, uint fileHashKey, string label, int featureCount, Genomes genome, Assemblies assembly, DataType dataType)
        {
            _selected = selected;
            _fileHashKey = fileHashKey;
            _label = label;
            _featureCount = featureCount;
            _genome = genome;
            _assembly = assembly;
            _dataType = dataType;
        }

        private bool _selected;
        public bool selected
        {
            set
            {
                _selected = value;
                NotifyPropertyChanged("selected");
            }
            get { return _selected; }
        }

        private uint _fileHashKey;
        public uint fileHashKey
        {
            private set
            {
                _fileHashKey = value;
                NotifyPropertyChanged("hashKey");
            }
            get { return _fileHashKey; }
        }

        private string _label;
        public string label
        {
            set
            {
                _label = value;
                NotifyPropertyChanged("label");
            }
            get { return _label; }
        }

        private int _featureCount;
        public int featureCount
        {
            private set
            {
                _featureCount = value;
                NotifyPropertyChanged("featureCount");
            }
            get { return _featureCount; }
        }

        private Genomes _genome;
        public Genomes genome
        {
            private set
            {
                _genome = value;
                NotifyPropertyChanged("genome");
            }
            get { return _genome; }
        }

        private Assemblies _assembly;
        public Assemblies assembly
        {
            private set
            {
                _assembly = value;
                NotifyPropertyChanged("assembly");
            }
            get { return _assembly; }
        }

        private DataType _dataType;
        public DataType dataType
        {
            private set
            {
                _dataType = value;
                NotifyPropertyChanged("dataType");
            }
            get { return _dataType; }
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
