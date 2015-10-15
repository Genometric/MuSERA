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
using System.Windows.Media;

namespace Polimi.DEIB.VahidJalili.MuSERA.Models
{
    internal static class SamplesDefaultColors
    {
        public static SolidColorBrush GetRandomColor()
        {
            return _colors[_random.Next(0, _colors.Length)];
        }

        private static Random _random = new Random();
        private static SolidColorBrush[] _colors = new SolidColorBrush[]
        {
            new SolidColorBrush(Color.FromArgb(255, 206, 0,255)),
            new SolidColorBrush(Color.FromArgb(255, 255,0,0)),
            new SolidColorBrush(Color.FromArgb(255, 0,216,255)),
            new SolidColorBrush(Color.FromArgb(255, 0,255,0)),
            new SolidColorBrush(Color.FromArgb(255, 240,255,0)),
            new SolidColorBrush(Color.FromArgb(255, 60,70,255)),
            new SolidColorBrush(Color.FromArgb(255, 255,120,0)),
            new SolidColorBrush(Color.FromArgb(255, 255,0,150)),
            new SolidColorBrush(Color.FromArgb(255, 130, 250, 255))
        };
    }
}
