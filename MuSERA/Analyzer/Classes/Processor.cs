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
using Polimi.DEIB.VahidJalili.IGenomics;
using Polimi.DEIB.VahidJalili.MuSERA.Warehouse;
using Polimi.DEIB.VahidJalili.MuSERA.XSquaredData;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Polimi.DEIB.VahidJalili.MuSERA.Analyzer
{
    internal class Processor<Peak, Metadata>
        where Peak : IInterval<int, Metadata>, IComparable<Peak>, new()
        where Metadata : IChIPSeqPeak, IComparable<Metadata>, new()
    {
        internal Processor(Data<Peak, Metadata> data)
        {
            _data = data;
        }

        private double _tXsqrd { set; get; }
        private AnalysisResult<Peak, Metadata> _analysisResults { set; get; }
        private Data<Peak, Metadata> _data { set; get; }
        private List<Peak> _sourcePeaks { set; get; }
        private string _chrTitle { set; get; }
        private uint _sampleHashKey { set; get; }
        private Dictionary<uint, Dictionary<string, IntervalTree<Peak, int>>> _trees { set; get; }



        internal void Run(uint sampleHashKey, string ChrTitle, char strand)
        {
            _sampleHashKey = sampleHashKey;
            _chrTitle = ChrTitle;
            _sourcePeaks = _data.samples[_sampleHashKey].intervals[_chrTitle][strand];
            _analysisResults = _data.analysisResults[_sampleHashKey];
            _trees = _data.trees;

            foreach (Peak p in _sourcePeaks)
            {
                _tXsqrd = 0;
                InitialClassification(p);
                if (p.metadata.value <= Options.tauS || p.metadata.value <= Options.tauW)
                    SecondaryClassification(p, FindSupportingPeaks(p));
            }
        }

        private void InitialClassification(Peak p)
        {
            if (p.metadata.value <= Options.tauS)
                _analysisResults.R_j__s[_chrTitle].Add(p);
            else if (p.metadata.value <= Options.tauW)
                _analysisResults.R_j__w[_chrTitle].Add(p);
            else
                _analysisResults.R_j__b[_chrTitle].Add(p);
        }

        private List<AnalysisResult<Peak, Metadata>.SupportingERs> FindSupportingPeaks(Peak p)
        {
            var supPeak = new List<AnalysisResult<Peak, Metadata>.SupportingERs>();
            foreach (var sample in _data.samples)
            {
                if (sample.Key != _sampleHashKey)
                {
                    var interPeaks = new List<IntervalTreeLib.Interval<Peak, int>>();
                    if (_trees[sample.Key].ContainsKey(_chrTitle))
                        interPeaks = _trees[sample.Key][_chrTitle].GetIntervals(p.left, p.right);

                    switch (interPeaks.Count)
                    {
                        case 0: break;

                        case 1:
                            supPeak.Add(new AnalysisResult<Peak, Metadata>.SupportingERs()
                            {
                                er = interPeaks[0].Data,
                                sampleIndex = sample.Key
                            });
                            break;

                        default:
                            var chosenPeak = interPeaks[0];
                            foreach (var tIp in interPeaks.Skip(1))
                                if ((Options.multipleIntersections == MultipleIntersections.UseLowestPValue && tIp.Data.metadata.value < chosenPeak.Data.metadata.value) ||
                                    (Options.multipleIntersections == MultipleIntersections.UseHighestPValue && tIp.Data.metadata.value > chosenPeak.Data.metadata.value))
                                    chosenPeak = tIp;

                            supPeak.Add(new AnalysisResult<Peak, Metadata>.SupportingERs()
                            {
                                er = chosenPeak.Data,
                                sampleIndex = sample.Key
                            });
                            break;
                    }
                }
            }

            return supPeak;
        }

        private void SecondaryClassification(Peak p, List<AnalysisResult<Peak, Metadata>.SupportingERs> supportingPeaks)
        {
            if (supportingPeaks.Count + 1 >= Options.C)
            {
                CalculateXsqrd(p, supportingPeaks);

                if (_tXsqrd >= _data.cachedChiSqrd[supportingPeaks.Count])
                    ConfirmPeak(p, supportingPeaks);
                else
                    DiscardPeak(p, supportingPeaks, 0);
            }
            else
            {
                DiscardPeak(p, supportingPeaks, 1);
            }
        }

        private void ConfirmPeak(Peak p, List<AnalysisResult<Peak, Metadata>.SupportingERs> supportingPeaks)
        {
            var anRe = new AnalysisResult<Peak, Metadata>.ProcessedER()
            {
                er = p,
                xSquared = _tXsqrd,
                rtp = ChiSquaredCache.ChiSqrdDistRTP(_tXsqrd, 2 + (supportingPeaks.Count * 2)),
                supportingERs = supportingPeaks
            };

            if (p.metadata.value <= Options.tauS)
            {
                _analysisResults.R_j___sc[_chrTitle]++;
                anRe.classification = ERClassificationType.StringentConfirmed;
            }
            else
            {
                _analysisResults.R_j___wc[_chrTitle]++;
                anRe.classification = ERClassificationType.WeakConfirmed;
            }

            if (!_analysisResults.R_j__c[_chrTitle].ContainsKey(p.metadata.hashKey))
                _analysisResults.R_j__c[_chrTitle].Add(p.metadata.hashKey, anRe);

            ConfirmeSupportingPeaks(p, supportingPeaks);
        }

        private void ConfirmeSupportingPeaks(Peak p, List<AnalysisResult<Peak, Metadata>.SupportingERs> supportingPeaks)
        {
            foreach (var supPeak in supportingPeaks)
            {
                if (!_data.analysisResults[supPeak.sampleIndex].R_j__c[_chrTitle].ContainsKey(supPeak.er.metadata.hashKey))
                {
                    var tSupPeak = new List<AnalysisResult<Peak, Metadata>.SupportingERs>();
                    tSupPeak.Add(new AnalysisResult<Peak, Metadata>.SupportingERs() { er = p, sampleIndex = _sampleHashKey });

                    foreach (var sP in supportingPeaks)
                        if (supPeak.CompareTo(sP) != 0)
                            tSupPeak.Add(sP);

                    var anRe = new AnalysisResult<Peak, Metadata>.ProcessedER()
                    {
                        er = supPeak.er,
                        xSquared = _tXsqrd,
                        rtp = ChiSquaredCache.ChiSqrdDistRTP(_tXsqrd, 2 + (supportingPeaks.Count * 2)),
                        supportingERs = tSupPeak
                    };

                    if (supPeak.er.metadata.value <= Options.tauS)
                    {
                        _data.analysisResults[supPeak.sampleIndex].R_j___sc[_chrTitle]++;
                        anRe.classification = ERClassificationType.StringentConfirmed;
                    }
                    else
                    {
                        _data.analysisResults[supPeak.sampleIndex].R_j___wc[_chrTitle]++;
                        anRe.classification = ERClassificationType.WeakConfirmed;
                    }

                    _data.analysisResults[supPeak.sampleIndex].R_j__c[_chrTitle].Add(supPeak.er.metadata.hashKey, anRe);
                }
            }
        }

        private void DiscardPeak(Peak p, List<AnalysisResult<Peak, Metadata>.SupportingERs> supportingPeaks, byte discardReason)
        {
            var anRe = new AnalysisResult<Peak, Metadata>.ProcessedER
            {
                er = p,
                xSquared = _tXsqrd,
                reason = discardReason,
                supportingERs = supportingPeaks
            };

            if (p.metadata.value <= Options.tauS)
            {
                // The cause of discarding the region is :
                if (supportingPeaks.Count + 1 >= Options.C)
                    _analysisResults.R_j__sdt[_chrTitle]++;  // - Test failure
                else _analysisResults.R_j__sdc[_chrTitle]++; // - insufficient intersecting regions count

                anRe.classification = ERClassificationType.StringentDiscarded;
            }
            else
            {
                // The cause of discarding the region is :
                if (supportingPeaks.Count + 1 >= Options.C)
                    _analysisResults.R_j__wdt[_chrTitle]++;  // - Test failure
                else _analysisResults.R_j__wdc[_chrTitle]++; // - insufficient intersecting regions count

                anRe.classification = ERClassificationType.WeakDiscarded;
            }

            if (!_analysisResults.R_j__d[_chrTitle].ContainsKey(p.metadata.hashKey))
                _analysisResults.R_j__d[_chrTitle].Add(p.metadata.hashKey, anRe);

            if (supportingPeaks.Count + 1 >= Options.C)
                DiscardSupportingPeaks(p, supportingPeaks, discardReason);
        }

        private void DiscardSupportingPeaks(Peak p, List<AnalysisResult<Peak, Metadata>.SupportingERs> supportingPeaks, byte discardReason)
        {
            foreach (var supPeak in supportingPeaks)
            {
                if (!_data.analysisResults[supPeak.sampleIndex].R_j__d[_chrTitle].ContainsKey(supPeak.er.metadata.hashKey))
                {
                    var tSupPeak = new List<AnalysisResult<Peak, Metadata>.SupportingERs>();
                    var targetSample = _data.analysisResults[supPeak.sampleIndex];
                    tSupPeak.Add(new AnalysisResult<Peak, Metadata>.SupportingERs() { er = p, sampleIndex = _sampleHashKey });

                    foreach (var sP in supportingPeaks)
                        if (supPeak.CompareTo(sP) != 0)
                            tSupPeak.Add(sP);

                    var anRe = new AnalysisResult<Peak, Metadata>.ProcessedER()
                    {
                        er = supPeak.er,
                        xSquared = _tXsqrd,
                        reason = discardReason,
                        rtp = ChiSquaredCache.ChiSqrdDistRTP(_tXsqrd, 2 + (supportingPeaks.Count * 2)),
                        supportingERs = tSupPeak
                    };


                    if (supPeak.er.metadata.value <= Options.tauS)
                    {
                        targetSample.R_j__sdt[_chrTitle]++;
                        anRe.classification = ERClassificationType.StringentDiscarded;
                    }
                    else
                    {
                        targetSample.R_j__wdt[_chrTitle]++;
                        anRe.classification = ERClassificationType.WeakDiscarded;
                    }

                    targetSample.R_j__d[_chrTitle].Add(supPeak.er.metadata.hashKey, anRe);
                }
            }
        }

        private void CalculateXsqrd(Peak p, List<AnalysisResult<Peak, Metadata>.SupportingERs> supportingPeaks)
        {
            if (p.metadata.value != 0)
                _tXsqrd = Math.Log(p.metadata.value, Math.E);
            else
                _tXsqrd = Math.Log(Options.default0PValue, Math.E);

            foreach (var supPeak in supportingPeaks)
            {
                if (supPeak.er.metadata.value != 0)
                    _tXsqrd += Math.Log(supPeak.er.metadata.value, Math.E);
                else
                    _tXsqrd += Math.Log(Options.default0PValue, Math.E);
            }

            _tXsqrd = _tXsqrd * (-2);
            if (_tXsqrd >= Math.Abs(Options.defaultMaxLogOfPVvalue))
                _tXsqrd = Math.Abs(Options.defaultMaxLogOfPVvalue);
        }

        internal void IntermediateSetsPurification()
        {
            if (Options.replicateType == ReplicateType.Biological)
            {
                // Performe : R_j__d = R_j__d \ { R_j__d intersection R_j__c }

                foreach (var sample in _data.samples)
                {
                    foreach (var chr in _data.analysisResults[sample.Key].R_j__c)
                    {
                        foreach (var confirmedPeak in chr.Value)
                        {
                            if (_data.analysisResults[sample.Key].R_j__d[chr.Key].ContainsKey(confirmedPeak.Key))
                            {
                                if (confirmedPeak.Value.er.metadata.value <= Options.tauS)
                                    _data.analysisResults[sample.Key].total_scom++;
                                else if (confirmedPeak.Value.er.metadata.value <= Options.tauW)
                                    _data.analysisResults[sample.Key].total_wcom++;

                                _data.analysisResults[sample.Key].R_j__d[chr.Key].Remove(confirmedPeak.Key);
                            }
                        }
                    }
                }
            }
            else
            {
                // Performe : R_j__c = R_j__c \ { R_j__c intersection R_j__d }

                foreach (var sample in _data.samples)
                {
                    foreach (var chr in _data.analysisResults[sample.Key].R_j__d)
                    {
                        foreach (var discardedPeak in chr.Value)
                        {
                            if (_data.analysisResults[sample.Key].R_j__c[chr.Key].ContainsKey(discardedPeak.Key))
                            {
                                if (discardedPeak.Value.er.metadata.value <= Options.tauS)
                                    _data.analysisResults[sample.Key].total_scom++;
                                else if (discardedPeak.Value.er.metadata.value <= Options.tauW)
                                    _data.analysisResults[sample.Key].total_wcom++;

                                _data.analysisResults[sample.Key].R_j__c[chr.Key].Remove(discardedPeak.Key);
                            }
                        }
                    }
                }
            }
        }

        internal void CreateOuputSet()
        {
            foreach (var sample in _data.samples)
            {
                foreach (var chr in _data.analysisResults[sample.Key].R_j__c)
                {
                    foreach (var confirmedPeak in chr.Value)
                    {
                        var outputPeak = new AnalysisResult<Peak, Metadata>.ProcessedER()
                        {
                            er = confirmedPeak.Value.er,
                            rtp = confirmedPeak.Value.rtp,
                            xSquared = confirmedPeak.Value.xSquared,
                            statisticalClassification = ERClassificationType.TruePositive,
                            supportingERs = confirmedPeak.Value.supportingERs,
                        };

                        if (confirmedPeak.Value.er.metadata.value <= Options.tauS)
                        {
                            outputPeak.classification = ERClassificationType.StringentConfirmed;
                            _data.analysisResults[sample.Key].R_j___so[chr.Key]++;
                        }
                        else if (confirmedPeak.Value.er.metadata.value <= Options.tauW)
                        {
                            outputPeak.classification = ERClassificationType.WeakConfirmed;
                            _data.analysisResults[sample.Key].R_j___wo[chr.Key]++;
                        }

                        _data.analysisResults[sample.Key].R_j__o[chr.Key].Add(outputPeak);
                    }
                }
            }
        }

        /// <summary>
        /// Benjamini–Hochberg procedure (step-up procedure)
        /// </summary>
        internal void EstimateFalseDiscoveryRate()
        {
            foreach (var sample in _data.samples)
            {
                foreach (var chr in _data.analysisResults[sample.Key].R_j__o)
                {
                    _data.analysisResults[sample.Key].R_j_TP[chr.Key] = (uint)chr.Value.Count;
                    _data.analysisResults[sample.Key].R_j_FP[chr.Key] = 0;

                    var outputSet = _data.analysisResults[sample.Key].R_j__o[chr.Key];

                    int m = outputSet.Count;

                    // Sorts output set based on the values of peaks. 
                    outputSet.Sort(new CompareProcessedPeakByValue<Peak, Metadata>());

                    for (int k = 1; k <= m; k++)
                    {
                        if (outputSet[k - 1].er.metadata.value > k / (double)m * Options.alpha)
                        {
                            k--;

                            for (int l = 1; l < k; l++)
                            {
                                // This should update the [analysisResults[sample.Key].R_j__o[chr.Key]] ; is it updating ?
                                outputSet[l].adjPValue = k * outputSet[l].er.metadata.value / m * Options.alpha;
                                outputSet[l].statisticalClassification = ERClassificationType.TruePositive;
                            }
                            for (int l = k; l < m; l++)
                            {
                                outputSet[l].adjPValue = k * outputSet[l].er.metadata.value / m * Options.alpha;
                                outputSet[l].statisticalClassification = ERClassificationType.FalsePositive;
                            }

                            _data.analysisResults[sample.Key].R_j_TP[chr.Key] = (uint)k;
                            _data.analysisResults[sample.Key].R_j_FP[chr.Key] = (uint)(m - k);

                            break;
                        }
                    }

                    // Sorts output set using default comparer. 
                    // The default sorter gives higher priority to two ends than values. 
                    outputSet.Sort();
                }
            }
        }
    }
}
