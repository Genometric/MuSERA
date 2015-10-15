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
using Polimi.DEIB.VahidJalili.MuSERA.Warehouse;
using System;
using System.Collections.Generic;

namespace Polimi.DEIB.VahidJalili.MuSERA.Analyzer
{
    class CompareProcessedPeakByValue<Peak, Metadata> : IComparer<AnalysisResult<Peak, Metadata>.ProcessedER>
        where Peak : IInterval<int, Metadata>, IComparable<Peak>, new()
        where Metadata : IChIPSeqPeak, IComparable<Metadata>, new()
    {
        public int Compare(AnalysisResult<Peak, Metadata>.ProcessedER A, AnalysisResult<Peak, Metadata>.ProcessedER B)
        {
            if (A == null)
            {
                if (B == null) return 0;// then they are equal
                else return -1; // then B is greater
            }
            else
            {
                if (B == null) return 1;// A is greater
                else
                {
                    if (A.er.metadata.value != B.er.metadata.value)
                        return A.er.metadata.value.CompareTo(B.er.metadata.value);
                    else if (A.er.left != B.er.left)
                        return A.er.left.CompareTo(B.er.left);
                    else
                        return A.er.right.CompareTo(B.er.right);
                }
            }
        }
    }
}
