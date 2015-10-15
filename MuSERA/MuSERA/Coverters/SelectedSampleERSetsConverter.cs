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
    internal class SelectedSampleERSetsConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (values == null || values.Length != 3 || values[0] == null || values[1] == null || values[2] == null) return null;
            var analysisResults = (AnalysisResult<Interval<int, MChIPSeqPeak>, MChIPSeqPeak>)values[0];

            switch ((string)values[2])
            {
                case "stringent": return analysisResults.R_j__s.ContainsKey((string)values[1]) ? analysisResults.R_j__s[(string)values[1]] : null;
                case "weak": return analysisResults.R_j__w.ContainsKey((string)values[1]) ? analysisResults.R_j__w[(string)values[1]] : null;
                case "confirmed": return analysisResults.R_j__c.ContainsKey((string)values[1]) ? analysisResults.R_j__c[(string)values[1]] : null;
                case "discarded": return analysisResults.R_j__d.ContainsKey((string)values[1]) ? analysisResults.R_j__d[(string)values[1]] : null;
                case "output": return analysisResults.R_j__o.ContainsKey((string)values[1]) ? analysisResults.R_j__o[(string)values[1]] : null;
                case "garbage": return analysisResults.R_j__b.ContainsKey((string)values[1]) ? analysisResults.R_j__b[(string)values[1]] : null;
                default: return null;
            }
        }
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
