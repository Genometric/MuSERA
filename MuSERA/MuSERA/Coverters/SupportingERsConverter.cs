﻿/** Copyright © 2013-2015 Vahid Jalili
 * 
 * This file is part of MuSERA project.
 * MuSERA is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation,
 * either version 3 of the License, or (at your option) any later version.
 * MuSERA is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A 
 * PARTICULAR PURPOSE. See the GNU General Public License for more details.
 * You should have received a copy of the GNU General Public License along with Foobar. If not, see http://www.gnu.org/licenses/.
 **/

using Polimi.DEIB.VahidJalili.MuSERA.ViewModels;
using Polimi.DEIB.VahidJalili.MuSERA.Warehouse;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace Polimi.DEIB.VahidJalili.MuSERA.Coverters
{
    internal class SupportingERsConverter : IValueConverter
    {
        object System.Windows.Data.IValueConverter.Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null) return "";
            else
            {
                var T = (List<AnalysisResult<Interval<int, MChIPSeqPeak>, MChIPSeqPeak>.SupportingERs>)value;
                string rtv = "";
                foreach (var er in T)
                {
                    rtv += rtv.Trim() == "" ? "" : "\n";
                    rtv +=
                        "\t< " +
                        er.er.left + " , " +
                        er.er.right + " , " +
                        er.er.metadata.summit + " , " +
                        er.er.metadata.name + " , " +
                        er.er.metadata.value.ToString("E01", CultureInfo.InvariantCulture) + " , " +
                        ApplicationViewModel.Default.cachedDataSummary.First(s => s.fileHashKey == er.sampleIndex).label +
                        " >\t";
                }
                return rtv;
            }
        }

        object System.Windows.Data.IValueConverter.ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
