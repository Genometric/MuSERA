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
    public enum DataType : byte { ChIPSeqAssay, RefSeqGenes, GeneralFeatures };
    public enum InteractiveGUIState : byte { Initial, LoadSamples, GettingAnalysisOptions, RunningAnalysis, AnalysisFinished, ShowSessionDetails };
    public enum Sets : byte { R_s, R_c, R_d, R_w, R_o, R_g };
    public enum PlotType : byte { Overview = 0, ER = 1, Classification_1st = 2, Classification_2nd_2in1 = 3, Classification_2nd_4in1 = 4, Classification_3rd = 5, Classification_4th = 6, GenomeBrowser = 7, ERToFeature = 8, NND = 9, Xsqrd = 10, SamplePValueDistribution = 11, ChrwideStats = 12, Similarity = 13 };
    internal enum OnBatchCompletionTask : byte { NoThing = 0, ExitProgram = 1, ForceRebood = 2, ForceShutdown = 3 };
}
