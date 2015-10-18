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
using System.Collections.Generic;
using System.Windows.Media;

namespace Polimi.DEIB.VahidJalili.MuSERA.Models
{
    internal class PlotOptionsColors
    {
        public PlotOptionsColors()
        {
            InitializeLineColors();
            InitializeMarkerColors();
            ColorGeneralDistributionLine = new SolidColorBrush(Colors.OrangeRed);
            ColorGeneralDistributionMarker = new SolidColorBrush(Colors.Red);
            SampleColor = new Dictionary<uint, SolidColorBrush>();
        }

        public SolidColorBrush ColorGeneralDistributionLine { set; get; }
        public SolidColorBrush ColorGeneralDistributionMarker { set; get; }
        public Dictionary<ERClassificationType, SolidColorBrush> ColorClassificationsLine { set; get; }
        public Dictionary<ERClassificationType, SolidColorBrush> ColorClassificationsMarker { set; get; }
        public SolidColorBrush ColorGenesLine { set; get; }
        public SolidColorBrush ColorGenesMarker { set; get; }
        public Dictionary<uint, SolidColorBrush> SampleColor { set; get; }


        public SolidColorBrush ColorSDT { get { return new SolidColorBrush(Color.FromArgb(255, 116, 2, 126)); } }
        public SolidColorBrush ColorWDT { get { return new SolidColorBrush(Color.FromArgb(255, 120, 0, 0)); } }
        public SolidColorBrush ColorSO { get { return new SolidColorBrush(Color.FromArgb(255, 0, 255, 0)); } }
        public SolidColorBrush ColorWO { get { return new SolidColorBrush(Color.FromArgb(255, 0, 255, 255)); } }


        private void InitializeLineColors()
        {
            ColorClassificationsLine = new Dictionary<ERClassificationType, SolidColorBrush>();
            ColorClassificationsLine.Add(ERClassificationType.Input, new SolidColorBrush(Color.FromArgb(255, 30, 144, 255)));
            ColorClassificationsLine.Add(ERClassificationType.Stringent, new SolidColorBrush(Color.FromArgb(255, 34, 139, 34)));
            ColorClassificationsLine.Add(ERClassificationType.Weak, new SolidColorBrush(Color.FromArgb(255, 250, 218, 5)));
            ColorClassificationsLine.Add(ERClassificationType.Background, new SolidColorBrush(Color.FromArgb(255, 255, 0, 0)));
            ColorClassificationsLine.Add(ERClassificationType.Confirmed, new SolidColorBrush(Color.FromArgb(255, 70, 255, 0)));
            ColorClassificationsLine.Add(ERClassificationType.Discarded, new SolidColorBrush(Color.FromArgb(255, 255, 70, 0)));
            ColorClassificationsLine.Add(ERClassificationType.StringentConfirmed, new SolidColorBrush(Color.FromArgb(255, 0, 100, 0)));
            ColorClassificationsLine.Add(ERClassificationType.StringentDiscarded, new SolidColorBrush(Color.FromArgb(255, 255, 20, 147)));
            ColorClassificationsLine.Add(ERClassificationType.WeakConfirmed, new SolidColorBrush(Color.FromArgb(255, 0, 200, 75)));
            ColorClassificationsLine.Add(ERClassificationType.WeakDiscarded, new SolidColorBrush(Color.FromArgb(255, 255, 69, 0)));
            ColorClassificationsLine.Add(ERClassificationType.Output, new SolidColorBrush(Color.FromArgb(255, 120, 230, 0)));
            ColorClassificationsLine.Add(ERClassificationType.TruePositive, new SolidColorBrush(Color.FromArgb(255, 124, 252, 0)));
            ColorClassificationsLine.Add(ERClassificationType.FalsePositive, new SolidColorBrush(Color.FromArgb(255, 255, 0, 255)));

            ColorGenesLine = new SolidColorBrush(Colors.Black);
        }
        private void InitializeMarkerColors()
        {
            ColorClassificationsMarker = new Dictionary<ERClassificationType, SolidColorBrush>();
            ColorClassificationsMarker.Add(ERClassificationType.Input, new SolidColorBrush(Color.FromArgb(255, 30, 144, 255)));
            ColorClassificationsMarker.Add(ERClassificationType.Stringent, new SolidColorBrush(Color.FromArgb(255, 34, 139, 34)));
            ColorClassificationsMarker.Add(ERClassificationType.Weak, new SolidColorBrush(Color.FromArgb(255, 255, 255, 0)));
            ColorClassificationsMarker.Add(ERClassificationType.Background, new SolidColorBrush(Color.FromArgb(255, 255, 0, 0)));
            ColorClassificationsMarker.Add(ERClassificationType.Confirmed, new SolidColorBrush(Color.FromArgb(255, 70, 255, 0)));
            ColorClassificationsMarker.Add(ERClassificationType.Discarded, new SolidColorBrush(Color.FromArgb(255, 255, 70, 0)));
            ColorClassificationsMarker.Add(ERClassificationType.StringentConfirmed, new SolidColorBrush(Color.FromArgb(255, 0, 100, 0)));
            ColorClassificationsMarker.Add(ERClassificationType.StringentDiscarded, new SolidColorBrush(Color.FromArgb(255, 255, 20, 147)));
            ColorClassificationsMarker.Add(ERClassificationType.WeakConfirmed, new SolidColorBrush(Color.FromArgb(255, 0, 250, 154)));
            ColorClassificationsMarker.Add(ERClassificationType.WeakDiscarded, new SolidColorBrush(Color.FromArgb(255, 255, 69, 0)));
            ColorClassificationsMarker.Add(ERClassificationType.Output, new SolidColorBrush(Color.FromArgb(255, 120, 230, 0)));
            ColorClassificationsMarker.Add(ERClassificationType.TruePositive, new SolidColorBrush(Color.FromArgb(255, 124, 252, 0)));
            ColorClassificationsMarker.Add(ERClassificationType.FalsePositive, new SolidColorBrush(Color.FromArgb(255, 255, 0, 255)));

            ColorGenesMarker = new SolidColorBrush(Colors.Black);
        }
    }
}
