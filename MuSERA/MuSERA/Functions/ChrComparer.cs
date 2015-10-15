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
using System.Collections.Generic;

namespace Polimi.DEIB.VahidJalili.MuSERA.Functions
{
    internal class ChrComparer : IComparer<string>
    {
        public int Compare(string x, string y)
        {
            string xNoChr = x.Replace("chr", "");
            xNoChr = xNoChr.Replace("Chr", "");

            string yNoChr = y.Replace("chr", "");
            yNoChr = yNoChr.Replace("Chr", "");

            int xNum, yNum;
            bool xIsNum = int.TryParse(xNoChr, out xNum);
            bool yIsNum = int.TryParse(yNoChr, out yNum);

            if (xIsNum && yIsNum)
                return xNum.CompareTo(yNum);
            if (xIsNum) return -1;
            if (yIsNum) return 1;
            return xNoChr.CompareTo(yNoChr);
        }
    }
}
