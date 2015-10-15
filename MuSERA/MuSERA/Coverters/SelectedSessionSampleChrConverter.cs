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
using System.Collections.Generic;
using System.Windows.Data;

namespace Polimi.DEIB.VahidJalili.MuSERA.Coverters
{
    internal class SelectedSessionSampleChrConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            // value is : sessionsViewModel.selectedAnalysisResult
            if (value == null) return null;
            var selectedAnalysisResults = (AnalysisResult<Interval<int, MChIPSeqPeak>, MChIPSeqPeak>)value;
            var chrs = new SortedDictionary<string, bool>();
            foreach (var chr in selectedAnalysisResults.R_j__s.Keys)
                if (!chrs.ContainsKey(chr)) chrs.Add(chr, true);
            foreach (var chr in selectedAnalysisResults.R_j__w.Keys)
                if (!chrs.ContainsKey(chr)) chrs.Add(chr, true);
            foreach (var chr in selectedAnalysisResults.R_j__c.Keys)
                if (!chrs.ContainsKey(chr)) chrs.Add(chr, true);
            foreach (var chr in selectedAnalysisResults.R_j__d.Keys)
                if (!chrs.ContainsKey(chr)) chrs.Add(chr, true);
            foreach (var chr in selectedAnalysisResults.R_j__o.Keys)
                if (!chrs.ContainsKey(chr)) chrs.Add(chr, true);
            foreach (var chr in selectedAnalysisResults.R_j__b.Keys)
                if (!chrs.ContainsKey(chr)) chrs.Add(chr, true);
            return chrs.Keys;
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
