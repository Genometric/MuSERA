/** Copyright © 2013-2015 Vahid Jalili
 * 
 * This file is part of MuSERA project.
 * MuSERA is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation,
 * either version 3 of the License, or (at your option) any later version.
 * MuSERA is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A 
 * PARTICULAR PURPOSE. See the GNU General Public License for more details.
 * You should have received a copy of the GNU General Public License along with Foobar. If not, see http://www.gnu.org/licenses/.
 **/

namespace Polimi.DEIB.VahidJalili.MuSERA.Analyzer
{
    public class Progress
    {
        internal Progress(double percentage, int totalStepsCount, int currentStepNumber, string currentStepLabel)
        {
            this.percentage = percentage;
            this.totalStepsCount = totalStepsCount;
            this.currentStepNumber = currentStepNumber;
            this.currentStepLabel = currentStepLabel;
        }

        public double percentage { private set; get; }
        public int totalStepsCount { private set; get; }
        public int currentStepNumber { private set; get; }
        public string currentStepLabel { private set; get; }
        public override string ToString()
        {
            return "[" + currentStepLabel + "//" + totalStepsCount + "] : " + currentStepLabel;
        }
    }
}
