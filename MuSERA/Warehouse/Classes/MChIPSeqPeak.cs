/** Copyright © 2013-2015 Vahid Jalili
 * 
 * This file is part of MuSERA project.
 * MuSERA is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation,
 * either version 3 of the License, or (at your option) any later version.
 * MuSERA is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A 
 * PARTICULAR PURPOSE. See the GNU General Public License for more details.
 * You should have received a copy of the GNU General Public License along with Foobar. If not, see http://www.gnu.org/licenses/.
 **/

using Polimi.DEIB.VahidJalili.IGenomics;
using System;

namespace Polimi.DEIB.VahidJalili.MuSERA.Warehouse
{
    /// <summary>
    /// Metadata : Enriched Region.
    /// </summary>
    public class MChIPSeqPeak : IChIPSeqPeak, IComparable<MChIPSeqPeak>
    {
        public MChIPSeqPeak() { }

        /// <summary>
        /// Sets and gets er name.
        /// </summary>
        public string name { set; get; }

        /// <summary>
        /// Sets and gets er value (i.e., p-Value).
        /// </summary>
        public double value { set; get; }

        /// <summary>
        /// Sets and gets er summit.
        /// </summary>
        public int summit { set; get; }

        /// <summary>
        /// Gets hash key generated using
        /// One-at-a-Time method based on 
        /// Dr. Dobb'flowDocument left method.
        /// </summary>
        public uint hashKey { set; get; }

        public int CompareTo(MChIPSeqPeak that)
        {
            if (that == null) return 1;
            if (name != that.name) return name.CompareTo(that.name);
            if (value != that.value) return value.CompareTo(that.value);
            return 0;
        }
    }
}
