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
using Polimi.DEIB.VahidJalili.MuSERA.Warehouse.Classes;
using Polimi.DEIB.VahidJalili.MuSERA.XSquaredData;
using System;
using System.Collections.Generic;

namespace Polimi.DEIB.VahidJalili.MuSERA.Warehouse
{
    public class AnalysisResult<ER, Metadata>
        where ER : IInterval<int, Metadata>, IComparable<ER>, new()
        where Metadata : IChIPSeqPeak, new()
    {
        public AnalysisResult(string fileName, string filePath, int degreeOfFreedom)
        {
            FileName = fileName;
            FilePath = filePath;
            _degreeOfFreedom = degreeOfFreedom;

            total_scom = 0;
            total_wcom = 0;

            R_j_FP = new Dictionary<string, uint>();
            R_j_TP = new Dictionary<string, uint>();

            R_j__s = new Dictionary<string, List<ER>>();
            R_j__w = new Dictionary<string, List<ER>>();
            R_j__b = new Dictionary<string, List<ER>>();

            R_j__c = new Dictionary<string, Dictionary<ulong, ProcessedER>>();
            R_j__d = new Dictionary<string, Dictionary<ulong, ProcessedER>>();
            R_j__o = new Dictionary<string, List<ProcessedER>>();

            R_j___sc = new Dictionary<string, uint>();
            R_j__sdc = new Dictionary<string, uint>();
            R_j__sdt = new Dictionary<string, uint>();
            R_j___so = new Dictionary<string, uint>();

            R_j___wc = new Dictionary<string, uint>();
            R_j__wdc = new Dictionary<string, uint>();
            R_j__wdt = new Dictionary<string, uint>();
            R_j___wo = new Dictionary<string, uint>();

            messages = new List<string>();

            /// The analyzer points to these messages by their index,
            /// hence if the order would change, the indexes will change
            /// as well resulting in improper messages
            messages.Add("X-squared is below chi-squared of Gamma");
            messages.Add("Intersecting peaks count doesn't comply minimum requirement");
        }

        /// <summary>
        /// Sets and gets the degree-of-freedom of analysis.
        /// </summary>
        private int _degreeOfFreedom { set; get; }

        /// <summary>
        /// Represents the default value for maximum log of p-value. 
        /// p-value lower than this value will be truncated. 
        /// </summary>
        private const double _defaultMaxLogOfPVvalue = 3300.0;

        /// <summary>
        /// Sample'flowDocument file name.
        /// </summary>
        public string FileName { private set; get; }

        /// <summary>
        /// Sample'flowDocument full path.
        /// </summary>
        public string FilePath { private set; get; }

        /// <summary>
        /// Chromosome-wide stringent peaks of sample j
        /// </summary>
        public Dictionary<string, List<ER>> R_j__s { set; get; }

        /// <summary>
        /// Chromosome-wide weak peaks of sample j
        /// </summary>
        public Dictionary<string, List<ER>> R_j__w { set; get; }

        /// <summary>
        /// Chromosome-wide background peaks of sample j (i.e., the peaks with p-value > T_w ).
        /// </summary>
        public Dictionary<string, List<ER>> R_j__b { set; get; }



        /// <summary>
        /// Chromosome-wide Confirmed peaks of sample j (i.e., the peaks that passed both intersecting
        /// peaks count threshold and X-squared test).
        /// </summary>
        public Dictionary<string, Dictionary<ulong, ProcessedER>> R_j__c { set; get; }

        /// <summary>
        /// Chromosome-wide Discarded peaks of sample j (i.e., the peaks that either failed intersecting
        /// peaks count threshold or X-squared test).
        /// </summary>
        public Dictionary<string, Dictionary<ulong, ProcessedER>> R_j__d { set; get; }

        /// <summary>
        /// Chromosome-wide set of peaks as the result of the algorithm. The peaks of this set passed
        /// three tests (i.e., intersecting peaks count, X-squared test, and benjamini-hochberg
        /// multiple testing correction).
        /// </summary>
        public Dictionary<string, List<ProcessedER>> R_j__o { set; get; }

        /// <summary>
        /// Chromosome-wide Multiple-Testing Discarded counter.
        /// </summary>
        public Dictionary<string, uint> R_j_FP { set; get; }

        /// <summary>
        /// Chromosome-wide Multiple-Testing Confirmed counter.
        /// </summary>
        public Dictionary<string, uint> R_j_TP { set; get; }


        /// <summary>
        /// Chromosome-wide Stringent-Confirmed peaks count.
        /// </summary>
        public Dictionary<string, uint> R_j___sc { set; get; }
        /// <summary>
        /// Chromosome-wide Stringent-Discarded peaks count 
        /// failing intersecting regions count test
        /// </summary>
        public Dictionary<string, uint> R_j__sdc { set; get; }
        /// <summary>
        /// Chromosome-wide Stringent-Discarded peaks count failing x-squared test.
        /// </summary>
        public Dictionary<string, uint> R_j__sdt { set; get; }
        /// <summary>
        /// Chromosome-wide Stringent peaks in output set count.
        /// </summary>
        public Dictionary<string, uint> R_j___so { set; get; }



        /// <summary>
        /// Chromosome-wide Weak-Confirmed peaks count.
        /// </summary>
        public Dictionary<string, uint> R_j___wc { set; get; }
        /// <summary>
        /// Chromosome-wide Weak-Discarded peaks count
        /// failing intersecting regions count test
        /// </summary>
        public Dictionary<string, uint> R_j__wdc { set; get; }
        /// <summary>
        /// Chromosome-wide Weak-Discarded peaks count failing x-squared test.
        /// </summary>
        public Dictionary<string, uint> R_j__wdt { set; get; }
        /// <summary>
        /// Chromosome-wide Weak peaks in output set count.
        /// </summary>
        public Dictionary<string, uint> R_j___wo { set; get; }


        /// <summary>
        /// gets total number of stringent peaks.
        /// </summary>
        public uint total____s { private set; get; }
        /// <summary>
        /// gets total number of weak peaks.
        /// </summary>
        public uint total____w { private set; get; }
        /// <summary>
        /// gets total number of garbage peaks.
        /// </summary>
        public uint total____g { private set; get; }
        /// <summary>
        /// gets total number of peaks in output set.
        /// </summary>
        public uint total____o { private set; get; }

        /// <summary>
        /// Total number of Multiple-Testing Discarded peaks in output set.
        /// </summary>
        public uint total___FP { private set; get; }
        /// <summary>
        /// Total number of Multiple-Testing Confirmed peaks in output set.
        /// </summary>
        public uint total___TP { private set; get; }

        /// <summary>
        /// Total number of stringent peaks that are both validated and discarded
        /// through multiple tests.
        /// </summary>
        public uint total_scom { set; get; }
        /// <summary>
        /// Total number of weak peaks that are both validated and discarded
        /// through multiple tests.
        /// </summary>
        public uint total_wcom { set; get; }



        /// <summary>
        /// Total number of Stringent-Confirmed peaks.
        /// </summary>
        public uint total___sc { private set; get; }
        /// <summary>
        /// Total number of Stringent-Discarded peaks
        /// failing intersecting regions count test
        /// </summary>
        public uint total__sdc { private set; get; }
        /// <summary>
        /// Total number of Stringent-Discarded peaks failing x-squared test.
        /// </summary>
        public uint total__sdt { private set; get; }
        /// <summary>
        /// Total number of Stringent peaks in output set.
        /// </summary>
        public uint total___so { private set; get; }


        /// <summary>
        /// Total number of Weak-Confirmed peaks.
        /// </summary>
        public uint total___wc { private set; get; }
        /// <summary>
        /// Total number of Weak-Discarded peaks
        /// failing intersecting regions count test.
        /// </summary>
        public uint total__wdc { private set; get; }
        /// <summary>
        /// Total number of Weak-Discarded peaks failing x-squared test.
        /// </summary>
        public uint total__wdt { private set; get; }
        /// <summary>
        /// Total number of Weak peaks in Output set.
        /// </summary>
        public uint total___wo { private set; get; }


        /// <summary>
        /// Gets Chromosome-wide analysis summary statistics.
        /// </summary>
        public Dictionary<string, ChrWideStat> chrwideStats { private set; get; }

        public List<string> messages { private set; get; }

        public void ReadOverallStats()
        {
            total____s = 0;
            total____w = 0;
            total____g = 0;
            total____o = 0;

            total___TP = 0;
            total___FP = 0;

            total___sc = 0;
            total__sdc = 0;
            total__sdt = 0;
            total___so = 0;

            total___wc = 0;
            total__wdc = 0;
            total__wdt = 0;
            total___wo = 0;

            double totalERsCount = 0;
            chrwideStats = new Dictionary<string, ChrWideStat>();
            foreach (KeyValuePair<string, List<ER>> chr in R_j__s)
            {
                total____s += (uint)R_j__s[chr.Key].Count;
                total____w += (uint)R_j__w[chr.Key].Count;
                total____g += (uint)R_j__b[chr.Key].Count;
                total____o += (uint)R_j__o[chr.Key].Count;

                total___TP += R_j_TP[chr.Key];
                total___FP += R_j_FP[chr.Key];

                total___sc += R_j___sc[chr.Key];
                total__sdc += R_j__sdc[chr.Key];
                total__sdt += R_j__sdt[chr.Key];
                total___so += R_j___so[chr.Key];

                total___wc += R_j___wc[chr.Key];
                total__wdc += R_j__wdc[chr.Key];
                total__wdt += R_j__wdt[chr.Key];
                total___wo += R_j___wo[chr.Key];

                totalERsCount = R_j__s[chr.Key].Count + R_j__w[chr.Key].Count;
                chrwideStats.Add(chr.Key, new ChrWideStat()
                {
                    R_j__t_c = (uint)totalERsCount,
                    R_j__s_c = (uint)R_j__s[chr.Key].Count,
                    R_j__s_p = (Math.Round((R_j__s[chr.Key].Count * 100) / totalERsCount, 2)).ToString() + " %",
                    R_j__w_c = (uint)R_j__w[chr.Key].Count,
                    R_j__w_p = (Math.Round((R_j__w[chr.Key].Count * 100) / totalERsCount, 2)).ToString() + " %",
                    R_j__c_c = (uint)R_j__c[chr.Key].Count,
                    R_j__c_p = (Math.Round((R_j__c[chr.Key].Count * 100) / totalERsCount, 2)).ToString() + " %",
                    R_j__d_c = (uint)R_j__d[chr.Key].Count,
                    R_j__d_p = (Math.Round((R_j__d[chr.Key].Count * 100) / totalERsCount, 2)).ToString() + " %",
                    R_j__o_c = (uint)R_j__o[chr.Key].Count,
                    R_j__o_p = (Math.Round((R_j__o[chr.Key].Count * 100) / totalERsCount, 2)).ToString() + " %",
                    R_j_TP_c = R_j_TP[chr.Key],
                    R_j_TP_p = (Math.Round(((double)R_j_TP[chr.Key] * 100) / R_j__o[chr.Key].Count, 2)).ToString() + " %",
                    R_j_FP_c = R_j_FP[chr.Key],
                    R_j_FP_p = (Math.Round(((double)R_j_FP[chr.Key] * 100) / R_j__o[chr.Key].Count, 2)).ToString() + " %"
                });
            }
        }
        public void AddChromosome(string chromosome)
        {
            R_j__s.Add(chromosome, new List<ER>());
            R_j__w.Add(chromosome, new List<ER>());
            R_j__b.Add(chromosome, new List<ER>());

            R_j__c.Add(chromosome, new Dictionary<ulong, ProcessedER>());
            R_j__d.Add(chromosome, new Dictionary<ulong, ProcessedER>());
            R_j__o.Add(chromosome, new List<ProcessedER>());

            R_j_FP.Add(chromosome, 0);
            R_j_TP.Add(chromosome, 0);

            R_j___sc.Add(chromosome, 0);
            R_j__sdc.Add(chromosome, 0);
            R_j__sdt.Add(chromosome, 0);
            R_j___so.Add(chromosome, 0);

            R_j___wc.Add(chromosome, 0);
            R_j__wdc.Add(chromosome, 0);
            R_j__wdt.Add(chromosome, 0);
            R_j___wo.Add(chromosome, 0);
        }
        public XSqrdDistributions GetXSqrdDistributions(int binWidth)
        {
            double xSqrd = 0.0;
            var tDic = new Dictionary<double, int>();
            var rtv = new XSqrdDistributions();

            foreach (var chr in R_j__c)
                foreach (var er in chr.Value)
                {
                    xSqrd = (Math.Floor(er.Value.xSquared / binWidth)) * binWidth;
                    if (tDic.ContainsKey(xSqrd))
                        tDic[xSqrd]++;
                    else
                        tDic.Add(xSqrd, 1);
                }

            foreach (var entry in tDic)
                rtv.Add(new XSqrdDistribution(
                    xSqrd: entry.Key,
                    rtp: ChiSquaredCache.ChiSqrdDistRTP(entry.Key, _degreeOfFreedom),
                    frequency: entry.Value));

            return rtv;
        }

        public PValueDistributions GetInputPValueDistribution(int binWidth)
        {
            var data = Get_1st_CD(binWidth, false);
            var rtv = new PValueDistributions();
            var tDic = new Dictionary<double, int>();

            foreach (var dis in data)
                if (tDic.ContainsKey(dis.logpValue)) tDic[dis.logpValue]++;
                else tDic.Add(dis.logpValue, dis.frequency);

            foreach (var entry in tDic)
                rtv.Add(new PValueDistribution(
                    type: ERClassificationType.Input,
                    frequency: entry.Value,
                    logpValue: entry.Key));

            return rtv;
        }

        /// <summary>
        /// Returns the specified Classification Category p-value Distribution.
        /// </summary>
        /// <param name="category">The ER classification category which it's distribution is to be returned.</param>
        /// <param name="binWidth">The distribution binWidth.</param>
        /// <returns></returns>
        public PValueDistributions CCD(ERClassificationCategory category, int binWidth)
        {
            switch (category)
            {
                case ERClassificationCategory.First: return Get_1st_CD(binWidth, true);
                case ERClassificationCategory.Second: return Get_2nd_CD(binWidth);
                case ERClassificationCategory.Third: return Get_3rd_CD(binWidth);
                case ERClassificationCategory.Fourth: return Get_4th_CD(binWidth);
                default: return null;
            }
        }

        /// <summary>
        /// Returns First level classification distribution.
        /// <para>The distribution of Stringent and Weak enriched regions.</para>
        /// </summary>
        /// <param name="binWidth">The binWidth of distribution.</param>
        /// <returns></returns>
        private PValueDistributions Get_1st_CD(int binWidth, bool avoid_R_j__b)
        {
            double pValue = 0.0;
            var tDic = new Dictionary<double, int>();
            var rtv = new PValueDistributions();

            #region .::.   Stringent ERs   .::.

            foreach (var chr in R_j__s)
                foreach (var er in chr.Value)
                {
                    pValue = Math.Floor(((-10) * Math.Log10(er.metadata.value)) / binWidth) * binWidth;
                    if (double.IsInfinity(pValue)) pValue = _defaultMaxLogOfPVvalue;
                    if (tDic.ContainsKey(pValue))
                        tDic[pValue]++;
                    else
                        tDic.Add(pValue, 1);
                }
            foreach (var entry in tDic)
                rtv.Add(new PValueDistribution(
                    type: ERClassificationType.Stringent,
                    frequency: entry.Value,
                    logpValue: entry.Key));

            #endregion
            #region .::.      Weak ERs     .::.

            tDic.Clear();
            foreach (var chr in R_j__w)
                foreach (var er in chr.Value)
                {
                    pValue = Math.Floor(((-10) * Math.Log10(er.metadata.value)) / binWidth) * binWidth;
                    if (double.IsInfinity(pValue)) pValue = _defaultMaxLogOfPVvalue;
                    if (tDic.ContainsKey(pValue))
                        tDic[pValue]++;
                    else
                        tDic.Add(pValue, 1);
                }
            foreach (var entry in tDic)
                rtv.Add(new PValueDistribution(
                    type: ERClassificationType.Weak,
                    frequency: entry.Value,
                    logpValue: entry.Key));

            #endregion
            #region .::.   Background ERs  .::.

            if (!avoid_R_j__b)
            {
                tDic.Clear();
                foreach (var chr in R_j__b)
                    foreach (var er in chr.Value)
                    {
                        pValue = Math.Floor(((-10) * Math.Log10(er.metadata.value)) / binWidth) * binWidth;
                        if (double.IsInfinity(pValue)) pValue = _defaultMaxLogOfPVvalue;
                        if (tDic.ContainsKey(pValue))
                            tDic[pValue]++;
                        else
                            tDic.Add(pValue, 1);
                    }
                foreach (var entry in tDic)
                    rtv.Add(new PValueDistribution(
                        type: ERClassificationType.Background,
                        frequency: entry.Value,
                        logpValue: entry.Key));
            }

            #endregion

            return rtv;
        }

        /// <summary>
        /// Returns Second level classification distribution.
        /// <para>The distribution of stringent/weak Confirmed and 
        /// stringent/weak Discarded enriched regions.</para>
        /// </summary>
        /// <param name="binWidth">The binWidth of distribution.</param>
        /// <returns></returns>
        private PValueDistributions Get_2nd_CD(int binWidth)
        {
            double pValue = 0.0;
            var tDicA = new Dictionary<double, int>();
            var tDicB = new Dictionary<double, int>();
            var rtv = new PValueDistributions();

            #region .::.   Confirmed ERs   .::.

            foreach (var chr in R_j__c)
                foreach (var er in chr.Value)
                {
                    pValue = Math.Floor(((-10) * Math.Log10(er.Value.er.metadata.value)) / binWidth) * binWidth;
                    if (double.IsInfinity(pValue)) pValue = _defaultMaxLogOfPVvalue;
                    switch (er.Value.classification)
                    {
                        case ERClassificationType.StringentConfirmed:
                            if (tDicA.ContainsKey(pValue)) tDicA[pValue]++;
                            else tDicA.Add(pValue, 1);
                            break;

                        case ERClassificationType.WeakConfirmed:
                            if (tDicB.ContainsKey(pValue)) tDicB[pValue]++;
                            else tDicB.Add(pValue, 1);
                            break;
                    }
                }

            foreach (var entry in tDicA)
                rtv.Add(new PValueDistribution(
                    type: ERClassificationType.StringentConfirmed,
                    frequency: entry.Value,
                    logpValue: entry.Key));

            foreach (var entry in tDicB)
                rtv.Add(new PValueDistribution(
                    type: ERClassificationType.WeakConfirmed,
                    frequency: entry.Value,
                    logpValue: entry.Key));

            #endregion

            #region .::.   Discarded ERs   .::.

            tDicA.Clear();
            tDicB.Clear();
            foreach (var chr in R_j__d)
                foreach (var er in chr.Value)
                {
                    pValue = Math.Floor(((-10) * Math.Log10(er.Value.er.metadata.value)) / binWidth) * binWidth;
                    if (double.IsInfinity(pValue)) pValue = _defaultMaxLogOfPVvalue;
                    switch (er.Value.classification)
                    {
                        case ERClassificationType.StringentDiscarded:
                            if (tDicA.ContainsKey(pValue)) tDicA[pValue]++;
                            else tDicA.Add(pValue, 1);
                            break;

                        case ERClassificationType.WeakDiscarded:
                            if (tDicB.ContainsKey(pValue)) tDicB[pValue]++;
                            else tDicB.Add(pValue, 1);
                            break;
                    }
                }

            foreach (var entry in tDicA)
                rtv.Add(new PValueDistribution(
                    type: ERClassificationType.StringentDiscarded,
                    frequency: entry.Value,
                    logpValue: entry.Key));

            foreach (var entry in tDicB)
                rtv.Add(new PValueDistribution(
                    type: ERClassificationType.WeakDiscarded,
                    frequency: entry.Value,
                    logpValue: entry.Key));

            #endregion

            return rtv;
        }

        /// <summary>
        /// Returns Third level classification distribution.
        /// <para>The distribution of stringent/weak confirmed enriched regions 
        /// that made-up to output set.</para>
        /// </summary>
        /// <param name="binWidth">The binWidth of distribution.</param>
        /// <returns></returns>
        private PValueDistributions Get_3rd_CD(int binWidth)
        {
            double pValue = 0.0;
            var tDicA = new Dictionary<double, int>();
            var tDicB = new Dictionary<double, int>();
            var rtv = new PValueDistributions();

            foreach (var chr in R_j__o)
                foreach (var er in chr.Value)
                {
                    pValue = Math.Floor(((-10) * Math.Log10(er.er.metadata.value)) / binWidth) * binWidth;
                    if (double.IsInfinity(pValue)) pValue = _defaultMaxLogOfPVvalue;
                    switch (er.classification)
                    {
                        case ERClassificationType.StringentConfirmed:
                            if (tDicA.ContainsKey(pValue)) tDicA[pValue]++;
                            else tDicA.Add(pValue, 1);
                            break;

                        case ERClassificationType.WeakConfirmed:
                            if (tDicB.ContainsKey(pValue)) tDicB[pValue]++;
                            else tDicB.Add(pValue, 1);
                            break;
                    }
                }

            foreach (var entry in tDicA)
                rtv.Add(new PValueDistribution(
                    type: ERClassificationType.StringentConfirmed,
                    frequency: entry.Value,
                    logpValue: entry.Key));

            foreach (var entry in tDicB)
                rtv.Add(new PValueDistribution(
                    type: ERClassificationType.WeakConfirmed,
                    frequency: entry.Value,
                    logpValue: entry.Key));

            return rtv;
        }

        /// <summary>
        /// Returns Forth level classification distribution.
        /// The distribution of Multiple-Testing Confirmed and Multiple-Testing Discarded enriched regions
        /// in output set.
        /// </summary>
        /// <param name="binWidth">The binWidth of distribution.</param>
        /// <returns></returns>
        private PValueDistributions Get_4th_CD(int binWidth)
        {
            double pValue = 0.0;
            var tDicA = new Dictionary<double, int>();
            var tDicB = new Dictionary<double, int>();
            var rtv = new PValueDistributions();

            foreach (var chr in R_j__o)
                foreach (var er in chr.Value)
                {
                    pValue = Math.Floor(((-10) * Math.Log10(er.er.metadata.value)) / binWidth) * binWidth;
                    if (double.IsInfinity(pValue)) pValue = _defaultMaxLogOfPVvalue;
                    switch (er.statisticalClassification)
                    {
                        case ERClassificationType.TruePositive:
                            if (tDicA.ContainsKey(pValue)) tDicA[pValue]++;
                            else tDicA.Add(pValue, 1);
                            break;

                        case ERClassificationType.FalsePositive:
                            if (tDicB.ContainsKey(pValue)) tDicB[pValue]++;
                            else tDicB.Add(pValue, 1);
                            break;
                    }
                }

            foreach (var entry in tDicA)
                rtv.Add(new PValueDistribution(
                    type: ERClassificationType.TruePositive,
                    frequency: entry.Value,
                    logpValue: entry.Key));

            foreach (var entry in tDicB)
                rtv.Add(new PValueDistribution(
                    type: ERClassificationType.FalsePositive,
                    frequency: entry.Value,
                    logpValue: entry.Key));

            return rtv;
        }

        public class ProcessedER : IComparable<ProcessedER>
        {
            /// <summary>
            /// Sets and gets the Confirmed I. Is a reference to the original er in cached data.
            /// </summary>
            public ER er { set; get; }

            /// <summary>
            /// Sets and gets X-squared of test
            /// </summary>
            public double xSquared { set; get; }

            /// <summary>
            /// Right tailed probability of x-squared.
            /// </summary>
            public double rtp { set; get; }

            /// <summary>
            /// Sets and gets the set of peaks intersecting with confirmed er
            /// </summary>
            public List<SupportingERs> supportingERs { set; get; }

            /// <summary>
            /// Sets and gets the reason of discarding the er. It points to an index of
            /// predefined messages.
            /// </summary>
            public byte reason { set; get; }

            /// <summary>
            /// Sets and gets classification type. 
            /// </summary>
            public ERClassificationType classification { set; get; }

            /// <summary>
            /// Sets and gets adjusted p-value using the multiple testing correction method of choice.
            /// </summary>
            public double adjPValue { set; get; }

            /// <summary>
            /// Set and gets whether the peaks is identified as Multiple-Testing Discarded or Multiple-Testing Confirmed 
            /// based on multiple testing correction threshold. 
            /// </summary>
            public ERClassificationType statisticalClassification { set; get; }

            /// <summary>
            /// Contains different classification types.
            /// </summary>
            int IComparable<ProcessedER>.CompareTo(ProcessedER that)
            {
                if (that == null) return 1;

                return er.CompareTo(that.er);
            }
        }
        public class SupportingERs : IComparable<SupportingERs>
        {
            /// <summary>
            /// Sets and gets the source of the er in cached data. 
            /// </summary>
            public uint sampleIndex { set; get; }

            /// <summary>
            /// Sets and gets the supporting er.
            /// </summary>
            public ER er { set; get; }

            int IComparable<SupportingERs>.CompareTo(SupportingERs that)
            {
                return CompareWithThis(that);
            }

            public int CompareTo(object obj)
            {
                return CompareWithThis((SupportingERs)obj);
            }

            private int CompareWithThis(SupportingERs that)
            {
                if (that == null) return 1;

                if (sampleIndex != that.sampleIndex)
                    return sampleIndex.CompareTo(that.sampleIndex);

                if (er.left != that.er.left)
                    return er.left.CompareTo(that.er.left);

                if (er.right != that.er.right)
                    return er.right.CompareTo(that.er.right);

                /*if (this.er.metadata.strand != that.er.metadata.strand)
                    return this.er.metadata.strand.CompareTo(that.er.metadata.strand);*/

                if (er.metadata.value != that.er.metadata.value)
                    return er.metadata.value.CompareTo(that.er.metadata.value);

                /*if (this.er.metadata.name != that.er.metadata.name)
                    return this.er.metadata.name.CompareTo(that.er.metadata.name);*/

                return 0;
            }
        }
    }
}