/** Copyright © 2013-2015 Vahid Jalili
 * 
 * This file is part of MuSERA project.
 * MuSERA is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation,
 * either version 3 of the License, or (at your option) any later version.
 * MuSERA is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A 
 * PARTICULAR PURPOSE. See the GNU General Public License for more details.
 * You should have received a copy of the GNU General Public License along with Foobar. If not, see http://www.gnu.org/licenses/.
 **/


using IntervalTreeLib;
using Polimi.DEIB.VahidJalili.GIFP;
using Polimi.DEIB.VahidJalili.IGenomics;
using Polimi.DEIB.VahidJalili.MuSERA.Warehouse;
using System;
using System.Collections.Generic;

namespace Polimi.DEIB.VahidJalili.MuSERA.Analyzer
{
    internal class Data<Peak, Metadata>
        where Peak : IInterval<int, Metadata>, IComparable<Peak>, new()
        where Metadata : IChIPSeqPeak, new()
    {
        public Data()
        {
            samples = new Dictionary<uint, ParsedChIPseqPeaks<int, Peak, Metadata>>();
            trees = new Dictionary<uint, Dictionary<string, IntervalTree<Peak, int>>>();
            analysisResults = new Dictionary<uint, AnalysisResult<Peak, Metadata>>();
            cachedChiSqrd = new List<double>();
        }
        internal Dictionary<uint, ParsedChIPseqPeaks<int, Peak, Metadata>> samples { set; get; }
        internal Dictionary<uint, Dictionary<string, IntervalTree<Peak, int>>> trees { set; get; }
        internal Dictionary<uint, AnalysisResult<Peak, Metadata>> analysisResults { set; get; }
        internal List<double> cachedChiSqrd { set; get; }
        internal void BuildSharedItems()
        {
            trees.Clear();
            analysisResults.Clear();

            foreach (var sample in samples)
            {
                analysisResults.Add(sample.Value.fileHashKey, new AnalysisResult<Peak, Metadata>(sample.Value.fileName, sample.Value.filePath, samples.Count * 2));

                Dictionary<string, IntervalTree<Peak, int>> sampleTree = new Dictionary<string, IntervalTree<Peak, int>>();

                foreach (var chromosome in sample.Value.intervals)
                    foreach (var strand in chromosome.Value)
                    {
                        sampleTree.Add(chromosome.Key, new IntervalTree<Peak, int>());
                        analysisResults[sample.Key].AddChromosome(chromosome.Key);

                        foreach (Peak p in strand.Value)
                            if (p.metadata.value <= Options.tauW)
                                sampleTree[chromosome.Key].AddInterval(p.left, p.right, p);
                            else
                                analysisResults[sample.Key].R_j__b[chromosome.Key].Add(p);
                    }

                trees.Add(sample.Value.fileHashKey, sampleTree);
            }
        }
    }
}
