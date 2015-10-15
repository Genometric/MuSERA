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
    /// Metadata : General Features.
    /// </summary>
    public class MGeneralFeatures : IGeneralFeature, IComparable<MGeneralFeatures>
    {
        public MGeneralFeatures() { }

        /// <summary>
        /// Sets and gets the attribute of the general feature.
        /// </summary>
        public string attribute { set; get; }

        /// <summary>
        /// Sets and gets the feature of the general feature.
        /// </summary>
        public byte feature { set; get; }

        /// <summary>
        /// Sets and gets the value, if any, of the general feature.
        /// </summary>
        public double value { set; get; }

        /// <summary>
        /// Sets and gets the hashKey of general feature. 
        /// </summary>
        public uint hashKey { set; get; }

        public int CompareTo(MGeneralFeatures other)
        {
            return 0;
        }
    }
}
