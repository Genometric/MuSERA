/** Copyright © 2013-2015 Vahid Jalili
 * 
 * This file is part of MuSERA project.
 * MuSERA is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation,
 * either version 3 of the License, or (at your option) any later version.
 * MuSERA is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A 
 * PARTICULAR PURPOSE. See the GNU General Public License for more details.
 * You should have received a copy of the GNU General Public License along with Foobar. If not, see http://www.gnu.org/licenses/.
 **/

using Polimi.DEIB.VahidJalili.GIFP;
using Polimi.DEIB.VahidJalili.IGenomics;
using System;
using System.Collections.Generic;

namespace Polimi.DEIB.VahidJalili.MuSERA.Warehouse
{
    public static class Samples<ER, Metadata>
        where ER : IInterval<int, Metadata>, IComparable<ER>, new()
        where Metadata : IChIPSeqPeak, new()
    {
        public static Dictionary<uint, ParsedChIPseqPeaks<int, ER, Metadata>> Data { set; get; }
    }

    public static class RefSeqGenes<ER, Metadata>
        where ER : IInterval<int, Metadata>, IComparable<ER>, new()
        where Metadata : IGene, IComparable<Metadata>, new()
    {
        public static Dictionary<uint, ParsedRefSeqGenes<int, Interval<int, Metadata>, Metadata>> Data { set; get; }
    }

    public static class GeneralFeatures<ER, Metadata>
        where ER : IInterval<int, Metadata>, IComparable<ER>, new()
        where Metadata : IGeneralFeature, new()
    {
        public static Dictionary<uint, ParsedGeneralFeatures<int, ER, Metadata>> Data { set; get; }
    }

    public static class Sessions<ER, Metadata>
        where ER : IInterval<int, Metadata>, IComparable<ER>, new()
        where Metadata : IChIPSeqPeak, new()
    {
        public static Dictionary<string, Session<ER, Metadata>> Data { set; get; }

        public static string GetSessionTitle()
        {
            int c = 0;
            string counter = "";

            do counter = ++c < 10 ? "0" + c.ToString() : c.ToString();
            while (Data.ContainsKey("Session_" + counter));

            return "Session_" + counter;
        }
    }
}
