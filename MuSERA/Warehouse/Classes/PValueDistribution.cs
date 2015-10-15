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
    public class PValueDistributions : ObservableCollection<PValueDistribution>
    { }

    public class PValueDistribution : PValueFrequency
    {
        public PValueDistribution(ERClassificationType type, int frequency, double logpValue)
        {
            this.type = type;
            this.frequency = frequency;
            this.logpValue = logpValue;
        }
        public ERClassificationType type { set; get; }
    }
}
