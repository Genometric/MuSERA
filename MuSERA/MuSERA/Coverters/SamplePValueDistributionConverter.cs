/** Copyright © 2013-2015 Vahid Jalili
 * 
 * This file is part of MuSERA project.
 * MuSERA is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation,
 * either version 3 of the License, or (at your option) any later version.
 * MuSERA is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A 
 * PARTICULAR PURPOSE. See the GNU General Public License for more details.
 * You should have received a copy of the GNU General Public License along with Foobar. If not, see http://www.gnu.org/licenses/.
 **/

using Polimi.DEIB.VahidJalili.MuSERA.Models;
using Polimi.DEIB.VahidJalili.MuSERA.Warehouse;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Data;

namespace Polimi.DEIB.VahidJalili.MuSERA.Coverters
{
    internal class SamplePValueDistributionConverter : IMultiValueConverter
    {
        // Values are : 0 cachedDataSummary
        //              1 sessionsViewModel.selectedSession
        //              2 sessionsViewModel.hAxisBinWidth
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (values == null || values.Length != 3 || values[0] == null || values[1] == null || values[2] == null) return null;

            int binWidth = (int)values[2];
            var rtv = new ObservableCollection<Tuple<string, PValueDistribution>>();
            var analysisResults = ((Session<Interval<int, MChIPSeqPeak>, MChIPSeqPeak>)values[1]).analysisResults;
            var cachedDataSummary = (ObservableCollection<CachedDataSummary>)values[0];
            string label;
            foreach (var result in analysisResults)
            {
                var pValues = result.Value.GetInputPValueDistribution(binWidth);
                label = cachedDataSummary.First(x => x.fileHashKey == result.Key).label;
                foreach (var pValue in pValues)
                    rtv.Add(new Tuple<string, PValueDistribution>(label, pValue));
            }

            return rtv;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
