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
using System.ComponentModel;

namespace Polimi.DEIB.VahidJalili.MuSERA.ViewModels
{
    internal class PlotOptionsViewModel : INotifyPropertyChanged
    {
        public PlotOptionsViewModel(PlotOptions defaultValues, uint totalERsCount)
        {
            _dummyPlotOptions = new PlotOptions(defaultValues.plotHeight, defaultValues.plotWidth, totalERsCount);
            _pDataLabelFontSize = defaultValues.dataLabelFontSize;
            _aDataLabelFontSize = defaultValues.actualDataLabelFontSize;
            _pDataLabelDistance = defaultValues.dataLabelDistance;
            _aDataLabelDistance = defaultValues.actualDataLabelDistance;
            _pBarWidth = defaultValues.barWidth;
            _aBarWidth = defaultValues.actualBarWidth;
            _pInterBarGap = defaultValues.interBarGap;
            _aInterBarGap = defaultValues.actualInterBarGap;
        }

        private PlotOptions _dummyPlotOptions { set; get; }

        public double pDataLabelFontSize
        {
            set
            {
                _pDataLabelFontSize = value;
                _dummyPlotOptions.dataLabelFontSize = value;
                _aDataLabelFontSize = _dummyPlotOptions.actualDataLabelFontSize;
                NotifyPropertyChanged("pDataLabelFontSize");
                NotifyPropertyChanged("aDataLabelFontSize");
            }
            get { return _pDataLabelFontSize; }
        }
        private double _pDataLabelFontSize;

        public double aDataLabelFontSize
        {
            set
            {
                _aDataLabelFontSize = value;
                _pDataLabelFontSize = 0;
                NotifyPropertyChanged("aDataLabelFontSize");
                NotifyPropertyChanged("pDataLabelFontSize");
            }
            get { return _aDataLabelFontSize; }
        }
        private double _aDataLabelFontSize;


        public double pDataLabelDistance
        {
            set
            {
                _pDataLabelDistance = value;
                _dummyPlotOptions.dataLabelDistance = value;
                _aDataLabelDistance = _dummyPlotOptions.actualDataLabelDistance;
                NotifyPropertyChanged("pDataLabelDistance");
                NotifyPropertyChanged("aDataLabelDistance");
            }
            get
            {
                return _pDataLabelDistance;
            }
        }
        private double _pDataLabelDistance;

        public double aDataLabelDistance
        {
            set
            {
                _aDataLabelDistance = value;
                _pDataLabelDistance = 0;
                NotifyPropertyChanged("aDataLabelDistance");
                NotifyPropertyChanged("pDataLabelDistance");
            }
            get
            {
                return _aDataLabelDistance;
            }
        }
        private double _aDataLabelDistance;


        public double pBarWidth
        {
            set
            {
                _pBarWidth = value;
                _dummyPlotOptions.barWidth = value;
                _aBarWidth = _dummyPlotOptions.actualBarWidth;
                NotifyPropertyChanged("pBarWidth");
                NotifyPropertyChanged("aBarWidth");
            }
            get
            {
                return _pBarWidth;
            }
        }
        private double _pBarWidth;

        public double aBarWidth
        {
            set
            {
                _aBarWidth = value;
                _pBarWidth = 0;
                NotifyPropertyChanged("aBarWidth");
                NotifyPropertyChanged("pBarWidth");
            }
            get
            {
                return _aBarWidth;
            }
        }
        private double _aBarWidth;


        public double pInterBarGap
        {
            set
            {
                _pInterBarGap = value;
                _dummyPlotOptions.interBarGap = value;
                _aInterBarGap = _dummyPlotOptions.actualInterBarGap;
                NotifyPropertyChanged("pInterBarGap");
                NotifyPropertyChanged("aInterBarGap");
            }
            get
            {
                return _pInterBarGap;
            }
        }
        private double _pInterBarGap;

        public double aInterBarGap
        {
            set
            {
                _aInterBarGap = value;
                _pInterBarGap = 0;
                NotifyPropertyChanged("aInterBarGap");
                NotifyPropertyChanged("pInterBarGap");
            }
            get
            {
                return _aInterBarGap;
            }
        }
        private double _aInterBarGap;

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
