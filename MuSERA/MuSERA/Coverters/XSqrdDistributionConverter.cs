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
    internal class XSqrdDistributionConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            // values are : 0 sessionsViewModel.selectedAnalysisResult
            //              1 hAxisBinWidth
            if (values == null || values.Length != 2 || values[0] == null || values[1] == null) return null;
            return ((AnalysisResult<Interval<int, MChIPSeqPeak>, MChIPSeqPeak>)values[0]).GetXSqrdDistributions((int)values[1]);
        }
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
