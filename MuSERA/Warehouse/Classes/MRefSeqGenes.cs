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
    /// Metadata : RefSeq Genes.
    /// </summary>
    public class MRefSeqGenes : IGene, IComparable<MRefSeqGenes>
    {
        public MRefSeqGenes() { }
        public string officialGeneSymbol { set; get; }
        public string refSeqID { set; get; }
        public char strand { set; get; }
        public double value { set; get; }
        public uint hashKey { set; get; }

        public int CompareTo(MRefSeqGenes other)
        {
            // TODO: update this.
            return 0;
        }
    }
}
