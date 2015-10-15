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
using System;
using System.Windows.Data;

namespace Polimi.DEIB.VahidJalili.MuSERA.Coverters
{
    internal class ERClassificationConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            switch ((ERClassificationType)value)
            {
                case ERClassificationType.Stringent: return "Stringent";
                case ERClassificationType.Weak: return "Weak";
                case ERClassificationType.StringentConfirmed: return "Stringent-Confirmed";
                case ERClassificationType.StringentDiscarded: return "Stringent-Discarded";
                case ERClassificationType.WeakConfirmed: return "Weak-Confirmed";
                case ERClassificationType.WeakDiscarded: return "Weak-Discarded";
                case ERClassificationType.TruePositive: return "Multiple-Testing Confirmed";
                case ERClassificationType.FalsePositive: return "Multiple-Testing Discarded";
                case ERClassificationType.Input: return "Input";
                case ERClassificationType.Output: return "Output";
                default: return "";
            }
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
