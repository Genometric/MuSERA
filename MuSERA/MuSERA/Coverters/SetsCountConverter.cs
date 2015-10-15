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
    internal class SetsCountConverter : IMultiValueConverter
    {
        // values are : 0 sessionsViewModel.selectedAnalysisResult
        //              1 the required info
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (values == null || values.Length != 2 || values[0] == null || values[1] == null) return null;
            var analysisResult = (AnalysisResult<Interval<int, MChIPSeqPeak>, MChIPSeqPeak>)values[0];
            double total = analysisResult.total____s + analysisResult.total____w;

            switch ((string)values[1])
            {
                case "total": return total.ToString();
                case "R_j__s_c": return analysisResult.total____s.ToString();
                case "R_j__s_p": return Math.Round((analysisResult.total____s * 100.0) / total, 2).ToString() + " %";
                case "R_j__w_c": return analysisResult.total____w.ToString();
                case "R_j__w_p": return Math.Round((analysisResult.total____w * 100.0) / total, 2).ToString() + " %";
                case "R_j__c_c": return (analysisResult.total___sc + analysisResult.total___wc).ToString();
                case "R_j__c_p": return Math.Round(((analysisResult.total___sc + analysisResult.total___wc) * 100.0) / total, 2).ToString() + " %";
                case "R_j__d_c": return (analysisResult.total__sdc + analysisResult.total__sdt + analysisResult.total__wdc + analysisResult.total__wdt).ToString();
                case "R_j__d_p": return Math.Round(((analysisResult.total__sdc + analysisResult.total__sdt + analysisResult.total__wdc + analysisResult.total__wdt) * 100.0) / total, 2).ToString() + " %";
                case "R_j__o_c": return analysisResult.total____o.ToString();
                case "R_j__o_p": return Math.Round((analysisResult.total____o * 100.0) / total, 2).ToString() + " %";
                case "R_j_TP_c": return analysisResult.total___TP.ToString();
                case "R_j_TP_p": return Math.Round((analysisResult.total___TP * 100.0) / analysisResult.total____o, 2).ToString() + " %";
                case "R_j_FP_c": return analysisResult.total___FP.ToString();
                case "R_j_FP_p": return Math.Round((analysisResult.total___FP * 100.0) / analysisResult.total____o, 2).ToString() + " %";
            }
            return analysisResult.chrwideStats; // why not null ?!
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
