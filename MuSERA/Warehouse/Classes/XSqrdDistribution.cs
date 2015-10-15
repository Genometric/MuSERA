/** Copyright © 2013-2015 Vahid Jalili
 * 
 * This file is part of MuSERA project.
 * MuSERA is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation,
 * either version 3 of the License, or (at your option) any later version.
 * MuSERA is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A 
 * PARTICULAR PURPOSE. See the GNU General Public License for more details.
 * You should have received a copy of the GNU General Public License along with Foobar. If not, see http://www.gnu.org/licenses/.
 **/

using System.Collections.ObjectModel;

namespace Polimi.DEIB.VahidJalili.MuSERA.Warehouse
{
    public class XSqrdDistributions : ObservableCollection<XSqrdDistribution>
    { }
    public class XSqrdDistribution
    {
        /// <summary>
        /// Provides information for the frequency of a x-squared value.
        /// </summary>
        /// <param name="pValue">The x-squarded value of fisher's method.</param>
        /// <param name="rtp">Right-tail probability of x-squarded value which is 
        /// calculated with correct degree-of-freedom.</param>
        /// <param name="frequency">The frequency of x-squarded value.</param>
        public XSqrdDistribution(double xSqrd, double rtp, int frequency)
        {
            this.xSqrd = xSqrd;
            this.rtp = rtp;
            this.frequency = frequency;
        }
        /// <summary>
        /// Gets the x-squarded value.
        /// </summary>
        public double xSqrd { private set; get; }

        /// <summary>
        /// Gets the right-tail probability of x-squarded. 
        /// </summary>
        public double rtp { private set; get; }

        /// <summary>
        /// Gets the frequency of x-squarded value.
        /// </summary>
        public int frequency { private set; get; }
    }
}
