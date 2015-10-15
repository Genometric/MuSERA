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
    internal class PointGene
    {
        public double x { set; get; }
        public double y { set; get; }
        public int left { set; get; }
        public int right { set; get; }
        public string refSeqID { set; get; }
        public string officialGeneSymbol { set; get; }
    }
}
