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
    internal class ClassificationDistributionConverter : IMultiValueConverter
    {
        // values are : 0 sessionsViewModel.selectedAnalysisResult
        //              1 hAxisBinWidth
        //              2 classification
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (values == null || values.Length != 3 || values[0] == null || values[1] == null || values[2] == null) return null;
            var selectedAnalysisResult = (AnalysisResult<Interval<int, MChIPSeqPeak>, MChIPSeqPeak>)values[0];
            switch ((string)values[2])
            {
                case "1st": return selectedAnalysisResult.CCD(ERClassificationCategory.First, (int)values[1]);
                case "2nd": return selectedAnalysisResult.CCD(ERClassificationCategory.Second, (int)values[1]);
                case "3rd": return selectedAnalysisResult.CCD(ERClassificationCategory.Third, (int)values[1]);
                case "4th": return selectedAnalysisResult.CCD(ERClassificationCategory.Fourth, (int)values[1]);
                default: return null;
            }
        }
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
