/** Copyright © 2013-2015 Vahid Jalili
 * 
 * This file is part of MuSERA project.
 * MuSERA is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation,
 * either version 3 of the License, or (at your option) any later version.
 * MuSERA is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A 
 * PARTICULAR PURPOSE. See the GNU General Public License for more details.
 * You should have received a copy of the GNU General Public License along with Foobar. If not, see http://www.gnu.org/licenses/.
 **/

using System;

namespace Polimi.DEIB.VahidJalili.MuSERA.Analyzer
{
    /// <summary>
    /// Defines an interval structure used as a "key"
    /// for combining outputs of replicates into a single output.
    /// </summary>
    internal struct Interval : IComparable<Interval>
    {
        public int left { set; get; }
        public int right { set; get; }

        public void Merge(int left, int right)
        {
            this.left = Math.Min(this.left, left);
            this.right = Math.Max(this.right, right);
        }

        public int CompareTo(Interval other)
        {
            /*if (left.CompareTo(other.left) != 0) return left.CompareTo(other.left);
            if (right.CompareTo(other.right) != 0) return right.CompareTo(other.right);
            return 0;*/
            if (right <= other.left) return -1;
            if (left >= other.right) return 1;
            return 0;
        }
    }
}