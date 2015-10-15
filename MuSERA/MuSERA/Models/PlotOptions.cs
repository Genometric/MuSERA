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

namespace Polimi.DEIB.VahidJalili.MuSERA.Models
{
    internal class PlotOptions : PlotOptionsColors
    {
        public PlotOptions()
        {
            showLegend = true;
            showOverviewLegend = false;
            interBarGap = 0.01;
            actualInterBarGap = -1;
            barWidth = 0.1;
            actualBarWidth = -1;
            dataLabelDistance = 0.08;
            actualDataLabelDistance = -1;
            dataLabelFontSize = 0.0291;
            actualDataLabelFontSize = -1;
            strokeThickness = 1;
            legendLineThickness = 5;
        }
        public PlotOptions(double plotHeight, double plotWidth, uint totalERsCount)
            : this() // calls the parameters less constructor of PlotOptions
        {
            this.plotHeight = plotHeight;
            this.plotWidth = plotWidth;
            this.totalERsCount = totalERsCount;
        }

        public bool showLegend { set; get; }
        public bool showOverviewLegend { set; get; }
        public double strokeThickness { set; get; }
        public double legendLineThickness { set; get; }


        public double dataLabelFontSize { set; get; }
        public double actualDataLabelFontSize
        {
            set { _actualDataLabelFontSize = value; }
            get
            {
                if (_actualDataLabelFontSize != -1) return _actualDataLabelFontSize;
                if (Math.Round(plotHeight * dataLabelFontSize, 3) >= 1)
                    return Math.Round(plotHeight * dataLabelFontSize, 3);
                else
                    return 1;
            }
        }
        private double _actualDataLabelFontSize;


        public double dataLabelDistance { set; get; }
        public double actualDataLabelDistance
        {
            set { _actualDataLabelDistance = value; }
            get { return _actualDataLabelDistance == -1 ? Math.Round(totalERsCount * dataLabelDistance, 3) : _actualDataLabelDistance; }
        }
        private double _actualDataLabelDistance;


        public double interBarGap { set; get; }
        public double actualInterBarGap
        {
            set { _actualInterBarGap = value; }
            get { return _actualInterBarGap == -1 ? Math.Round(totalERsCount * interBarGap, 3) : _actualInterBarGap; }
        }
        private double _actualInterBarGap;


        public double barWidth { set; get; }
        public double actualBarWidth
        {
            set { _actualBarWidth = value; }
            get { return _actualBarWidth == -1 ? Math.Round(totalERsCount * barWidth, 3) : _actualBarWidth; }
        }
        private double _actualBarWidth;

        /// <summary>
        /// This number will be used to calculate ER-count-related
        /// actual sizes using corresponding proportional sizes.
        /// </summary>
        public uint totalERsCount { set; get; }

        /// <summary>
        /// This number will be used to calculate plot-height-related
        /// actual sizes using corresponding proportional sizes.
        /// </summary>
        public double plotHeight { set; get; }

        /// <summary>
        /// This number will be used to calculate plot-width-related
        /// actual sizes using corresponding proportional sizes.
        /// </summary>
        public double plotWidth { set; get; }
    }
}
