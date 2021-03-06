﻿/** Copyright © 2013-2015 Vahid Jalili
 * 
 * This file is part of MuSERA project.
 * MuSERA is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation,
 * either version 3 of the License, or (at your option) any later version.
 * MuSERA is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A 
 * PARTICULAR PURPOSE. See the GNU General Public License for more details.
 * You should have received a copy of the GNU General Public License along with Foobar. If not, see http://www.gnu.org/licenses/.
 **/

using System;

namespace Polimi.DEIB.VahidJalili.MuSERA.XSquaredData
{
    public class ChiSquaredCache
    {
        /// <summary>
        /// Returns the right-tailed probability of the chi-squared distribution.
        /// <para>Is equivalent to Excel CHISQ.DIST.RT(x,deg_freedom) command.</para>
        /// <para>.</para>
        /// <para>[http://office.microsoft.com]</para>
        /// <para>The χ2 distribution is associated with a χ2 test. Use the χ2 test to
        /// compare observed and expected values. For example, a genetic experiment
        /// might hypothesize that the next generation of plants will exhibit a
        /// certain set of colors. By comparing the observed results with the 
        /// expected ones, you can decide whether your original hypothesis is valid.</para>
        /// </summary>
        /// <param name="x">The value at which you want to evaluate the distribution.</param>
        /// <param name="df">The number of degrees of freedom.</param>
        /// <returns></returns>
        public static double ChiSqrdDistRTP(double x, int df)
        {
            switch (df)
            {
                case 04: return new DF04().ChiSqrd(x);
                case 06: return new DF06().ChiSqrd(x);
                case 08: return new DF08().ChiSqrd(x);
                case 10: return new DF10().ChiSqrd(x);
                case 12: return new DF12().ChiSqrd(x);
                case 16: return new DF16().ChiSqrd(x);
                case 18: return new DF18().ChiSqrd(x);
                case 20: return new DF20().ChiSqrd(x);
                default: return 0;
            }
        }


        /// <summary>
        /// Returns the inverse of the right-tailed probability of the chi-squared distribution.
        /// <para>Is equivalent to Excel CHISQ.INV.RT(probability,deg_freedom) command.</para>
        /// <para>.</para>
        /// <para>[http://office.microsoft.com]</para>
        /// <para>If probability = CHISQ.DIST.RT(x,...), then CHISQ.INV.RT(probability,...) = x.
        /// Use this function to compare observed results with expected ones in order to decide
        /// whether your original hypothesis is valid.</para>
        /// <para>Given a value for probability, CHISQ.INV.RT seeks that value x such that 
        /// CHISQ.DIST.RT(x, deg_freedom) = probability. Thus, precision of CHISQ.INV.RT depends
        /// on precision of CHISQ.DIST.RT. CHISQ.INV.RT uses an iterative search technique. </para>
        /// </summary>
        /// <param name="probability">A probability associated with the chi-squared distribution.</param>
        /// <param name="df">The number of degrees of freedom.</param>
        /// <returns></returns>
        public static double ChiSqrdINVRTP(double probability, int df)
        {
            if (!((df / 2) - 1 > 7 || (df / 2) - 1 < 0))
            {
                byte num_power = (byte)Math.Ceiling(Math.Abs(Math.Log10(probability)));
                byte num_base = (byte)Math.Floor(probability * Math.Pow(10, num_power));

                return INVRTPData.data[num_base - 1, num_power - 1, (df / 2) - 1];
            }
            else
            {
                return -1;
            }
        }
    }
}
