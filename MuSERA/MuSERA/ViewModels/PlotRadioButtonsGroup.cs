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

using Polimi.DEIB.VahidJalili.MuSERA.Models;
using System;
using System.ComponentModel;

namespace Polimi.DEIB.VahidJalili.MuSERA.ViewModels
{
    internal class PlotRadioButtonsViewModel : INotifyPropertyChanged
    {
        public PlotRadioButtonsViewModel()
        {
            UnCheckAll();
            Overview = true;
        }
        private void UnCheckAll()
        {
            _Overview = false;
            _SamplePValueDistribution = false;
            _ER = false;
            _Classification_1st = false;
            _Classification_2nd_2in1 = false;
            _Classification_2nd_4in1 = false;
            _Classification_3rd = false;
            _Classification_4th = false;
            _GenomeBrowser = false;
            _ERToFeature = false;
            _NND = false;
            _Xsqrd = false;
            _ChrwideStats = false;
            _Similarity = false;
            NotifyPropertyChanged("Overview");
            NotifyPropertyChanged("SamplePValueDistribution");
            NotifyPropertyChanged("ER");
            NotifyPropertyChanged("Classification_1st");
            NotifyPropertyChanged("Classification_2nd_2in1");
            NotifyPropertyChanged("Classification_2nd_4in1");
            NotifyPropertyChanged("Classification_3rd");
            NotifyPropertyChanged("Classification_4th");
            NotifyPropertyChanged("GenomeBrowser");
            NotifyPropertyChanged("ERToFeature");
            NotifyPropertyChanged("NND");
            NotifyPropertyChanged("Xsqrd");
            NotifyPropertyChanged("ChrwideStats");
            NotifyPropertyChanged("Similarity");
        }

        public bool Overview
        {
            set
            {
                UnCheckAll();
                _Overview = value;
                NotifyPropertyChanged("Overview");
                Type = PlotType.Overview;
            }
            get { return _Overview; }
        }
        private bool _Overview;

        public bool ER
        {
            set
            {
                UnCheckAll();
                _ER = value;
                NotifyPropertyChanged("ER");
                Type = PlotType.ER;
            }
            get { return _ER; }
        }
        private bool _ER;

        public bool Classification_1st
        {
            set
            {
                UnCheckAll();
                _Classification_1st = value;
                NotifyPropertyChanged("Classification_1st");
                Type = PlotType.Classification_1st;
            }
            get { return _Classification_1st; }
        }
        private bool _Classification_1st;

        public bool Classification_2nd_2in1
        {
            set
            {
                UnCheckAll();
                _Classification_2nd_2in1 = value;
                NotifyPropertyChanged("Classification_2nd_2in1");
                Type = PlotType.Classification_2nd_2in1;
            }
            get { return _Classification_2nd_2in1; }
        }
        private bool _Classification_2nd_2in1;

        public bool Classification_2nd_4in1
        {
            set
            {
                UnCheckAll();
                _Classification_2nd_4in1 = value;
                NotifyPropertyChanged("Classification_2nd_4in1");
                Type = PlotType.Classification_2nd_4in1;
            }
            get { return _Classification_2nd_4in1; }
        }
        private bool _Classification_2nd_4in1;

        public bool Classification_3rd
        {
            set
            {
                UnCheckAll();
                _Classification_3rd = value;
                NotifyPropertyChanged("Classification_3rd");
                Type = PlotType.Classification_3rd;
            }
            get { return _Classification_3rd; }
        }
        private bool _Classification_3rd;

        public bool Classification_4th
        {
            set
            {
                UnCheckAll();
                _Classification_4th = value;
                NotifyPropertyChanged("Classification_4th");
                Type = PlotType.Classification_4th;
            }
            get { return _Classification_4th; }
        }
        private bool _Classification_4th;

        public bool GenomeBrowser
        {
            set
            {
                UnCheckAll();
                _GenomeBrowser = value;
                NotifyPropertyChanged("GenomeBrowser");
                Type = PlotType.GenomeBrowser;
            }
            get { return _GenomeBrowser; }
        }
        private bool _GenomeBrowser;

        public bool ERToFeature
        {
            set
            {
                UnCheckAll();
                _ERToFeature = value;
                NotifyPropertyChanged("ERToFeature");
                Type = PlotType.ERToFeature;
            }
            get { return _ERToFeature; }
        }
        private bool _ERToFeature;

        public bool NND
        {
            set
            {
                UnCheckAll();
                _NND = value;
                NotifyPropertyChanged("NND");
                Type = PlotType.NND;
            }
            get { return _NND; }
        }
        private bool _NND;

        public bool Xsqrd
        {
            set
            {
                UnCheckAll();
                _Xsqrd = value;
                NotifyPropertyChanged("Xsqrd");
                Type = PlotType.Xsqrd;
            }
            get { return _Xsqrd; }
        }
        private bool _Xsqrd;

        public bool SamplePValueDistribution
        {
            set
            {
                UnCheckAll();
                _SamplePValueDistribution = value;
                NotifyPropertyChanged("SamplePValueDistribution");
                Type = PlotType.SamplePValueDistribution;
            }
            get { return _SamplePValueDistribution; }
        }
        private bool _SamplePValueDistribution;

        public bool ChrwideStats
        {
            set
            {
                UnCheckAll();
                _ChrwideStats = value;
                NotifyPropertyChanged("ChrwideStats");
                Type = PlotType.ChrwideStats;
            }
            get { return _ChrwideStats; }
        }
        private bool _ChrwideStats;

        public bool Similarity
        {
            set
            {
                UnCheckAll();
                _Similarity = value;
                NotifyPropertyChanged("Similarity");
                Type = PlotType.Similarity;
            }
            get { return _Similarity; }
        }
        private bool _Similarity;

        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }


        public PlotType Type
        {
            set
            {
                if (_type != value)
                {
                    _type = value;
                    OnTypeValueChanged(value);
                }
            }
            get { return _type; }
        }
        private PlotType _type;
        public event EventHandler<PlotTypeEventArgs> TypeChanged;
        private void OnTypeValueChanged(PlotType type)
        {
            if (TypeChanged != null)
                TypeChanged(this, new PlotTypeEventArgs(type));
        }
    }
}
