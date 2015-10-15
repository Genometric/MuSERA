/** Copyright © 2013-2015 Vahid Jalili
 * 
 * This file is part of MuSERA project.
 * MuSERA is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation,
 * either version 3 of the License, or (at your option) any later version.
 * MuSERA is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A 
 * PARTICULAR PURPOSE. See the GNU General Public License for more details.
 * You should have received a copy of the GNU General Public License along with Foobar. If not, see http://www.gnu.org/licenses/.
 **/

namespace Polimi.DEIB.VahidJalili.MuSERA.Models
{
    internal class BatchPlotOptions : PlotOptions
    {
        public int width { set; get; }
        public int height { set; get; }
        public int markerSize { set; get; }
        public int axisFontSize { set; get; }
        public int fontSize { set; get; }
        public int headerFontSize { set; get; }
        public bool saveOverview { set; get; }
        public bool save_R_j__sc_by_R_j__total { set; get; }
        public bool save_R_j__sc_by_R_j__o { set; get; }
        public bool save_R_j__sd_by_total { set; get; }
        public bool save_R_j__sd_by_R_j__o { set; get; }
        public bool save_R_j__wc_by_total { set; get; }
        public bool save_R_j__wc_by_R_j__o { set; get; }
        public bool save_R_j__wd_by_total { set; get; }
        public bool save_R_j__wd_by_R_j__o { set; get; }
        public bool save_All_by_total { set; get; }
        public bool save_All_by_output { set; get; }
        public string savePath { set; get; }
    }
}
