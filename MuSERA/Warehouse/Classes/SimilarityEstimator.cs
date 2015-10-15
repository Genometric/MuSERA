/** Copyright © 2013-2015 Vahid Jalili
 * 
 * This file is part of MuSERA project.
 * MuSERA is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation,
 * either version 3 of the License, or (at your option) any later version.
 * MuSERA is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A 
 * PARTICULAR PURPOSE. See the GNU General Public License for more details.
 * You should have received a copy of the GNU General Public License along with Foobar. If not, see http://www.gnu.org/licenses/.
 **/

using Polimi.DEIB.VahidJalili.DI3;
using Polimi.DEIB.VahidJalili.DI3.AuxiliaryComponents;
using Polimi.DEIB.VahidJalili.IGenomics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Polimi.DEIB.VahidJalili.MuSERA.Warehouse.Classes
{
    internal class SimilarityEstimator<ER, Metadata>
        where ER : IInterval<int, Metadata>, IComparable<ER>, new()
        where Metadata : IChIPSeqPeak, new()
    {
        public SimilarityEstimator(Session<ER, Metadata> source)
        {
            _source = source;
        }


        #region .::.         Status variable and it's event controlers   .::.

        private double _status;
        public double status
        {
            get { return _status; }
            set
            {
                if (_status != value)
                {
                    _status = value;
                    OnStatusValueChaned(value);
                }
            }
        }
        public event EventHandler<DValueEventArgs> StatusChanged;
        private void OnStatusValueChaned(double value)
        {
            if (StatusChanged != null)
                StatusChanged(this, new DValueEventArgs(value));
        }

        #endregion
        #region .::.         Private Members                             .::.

        private Session<ER, Metadata> _source { set; get; }

        /// <summary>
        /// Sets and gets a collection of estimated similarities.
        /// </summary>
        private Dictionary<ERClassificationType, Similarity> _similarities { set; get; }

        private Dictionary<string, Di3> _di3 { set; get; }

        #endregion


        /// <summary>
        /// Estimates the similarities between all set of provided session
        /// </summary>
        /// <returns>A list of determined type indexes.</returns>
        public Dictionary<ERClassificationType, Similarity> GetSimilarity()
        {
            _similarities = new Dictionary<ERClassificationType, Similarity>();

            status = 0.0;
            InitializeDi3();
            Get_Input____Similarities();

            status = 12.0;
            InitializeDi3();
            Get_R_j__o___Similarities();

            status = 25.0;
            InitializeDi3();
            Get_R_j__s___Similarities();

            status = 37.0;
            InitializeDi3();
            Get_R_j__sc__Similarities();

            status = 50.0;
            InitializeDi3();
            Get_R_j__sd__Similarities();

            status = 62.0;
            InitializeDi3();
            Get_R_j__w___Similarities();

            status = 75.0;
            InitializeDi3();
            Get_R_j__wc__Similarities();

            status = 87.0;
            InitializeDi3();
            Get_R_j__wd__Similarities();

            status = 100.0;

            return _similarities;
        }


        private void Get_Input____Similarities()
        {            
            foreach (var sample in _source.samples)
                foreach (var chr in Samples<ER, Metadata>.Data[sample.Key].intervals)
                    foreach (var strand in chr.Value)
                        foreach (var interval in strand.Value)
                            if (interval.metadata.value <= _source.options.tauW)                            
                                _di3[chr.Key].Insert(interval.left, interval.right, interval, IntervalType.ER, sample.Key, ERClassificationType.Input);                            
            
            UpdateSimilarities(ERClassificationType.Input);
        }
        private void Get_R_j__o___Similarities()
        {
            foreach (var sample in _source.analysisResults)
                foreach (var chr in sample.Value.R_j__o)
                    foreach (var interval in chr.Value)
                        _di3[chr.Key].Insert(interval.er.left, interval.er.right, interval.er, IntervalType.ER, sample.Key, ERClassificationType.Output);

            UpdateSimilarities(ERClassificationType.Output);
        }
        private void Get_R_j__s___Similarities()
        {
            foreach (var sample in _source.analysisResults)
                foreach (var chr in sample.Value.R_j__s)
                    foreach (var interval in chr.Value)
                        _di3[chr.Key].Insert(interval.left, interval.right, interval, IntervalType.ER, sample.Key, ERClassificationType.Stringent);

            UpdateSimilarities(ERClassificationType.Stringent);
        }
        private void Get_R_j__w___Similarities()
        {
            foreach (var sample in _source.analysisResults)
                foreach (var chr in sample.Value.R_j__w)
                    foreach (var interval in chr.Value)
                        _di3[chr.Key].Insert(interval.left, interval.right, interval, IntervalType.ER, sample.Key, ERClassificationType.Weak);

            UpdateSimilarities(ERClassificationType.Weak);
        }
        private void Get_R_j__sc__Similarities()
        {
            foreach (var sample in _source.analysisResults)
                foreach (var chr in sample.Value.R_j__c)
                    foreach (var interval in chr.Value)
                        if (interval.Value.classification == ERClassificationType.StringentConfirmed)
                            _di3[chr.Key].Insert(interval.Value.er.left, interval.Value.er.right, interval.Value.er, IntervalType.ER, sample.Key, ERClassificationType.StringentConfirmed);

            UpdateSimilarities(ERClassificationType.StringentConfirmed);
        }
        private void Get_R_j__sd__Similarities()
        {
            foreach (var sample in _source.analysisResults)
                foreach (var chr in sample.Value.R_j__d)
                    foreach (var interval in chr.Value)
                        if (interval.Value.classification == ERClassificationType.StringentDiscarded)
                            _di3[chr.Key].Insert(interval.Value.er.left, interval.Value.er.right, interval.Value.er, IntervalType.ER, sample.Key, ERClassificationType.StringentDiscarded);

            UpdateSimilarities(ERClassificationType.StringentDiscarded);
        }
        private void Get_R_j__wc__Similarities()
        {
            foreach (var sample in _source.analysisResults)
                foreach (var chr in sample.Value.R_j__c)
                    foreach (var interval in chr.Value)
                        if (interval.Value.classification == ERClassificationType.WeakConfirmed)
                            _di3[chr.Key].Insert(interval.Value.er.left, interval.Value.er.right, interval.Value.er, IntervalType.ER, sample.Key, ERClassificationType.WeakConfirmed);

            UpdateSimilarities(ERClassificationType.WeakConfirmed);
        }
        private void Get_R_j__wd__Similarities()
        {
            foreach (var sample in _source.analysisResults)
                foreach (var chr in sample.Value.R_j__d)
                    foreach (var interval in chr.Value)
                        if (interval.Value.classification == ERClassificationType.WeakDiscarded)
                            _di3[chr.Key].Insert(interval.Value.er.left, interval.Value.er.right, interval.Value.er, IntervalType.ER, sample.Key, ERClassificationType.WeakDiscarded);

            UpdateSimilarities(ERClassificationType.WeakDiscarded);
        }

        private void UpdateSimilarities(ERClassificationType classification)
        {
            _similarities.Add(classification, new Similarity(_source.samples.Keys.ToList<uint>()));
            foreach (var _di3Chr in _di3)
            {
                var tSimilarity = _di3Chr.Value.GetSimilarity();
                _similarities[classification].regionLevel.globalSummary.intersection += tSimilarity.regionLevel.globalSummary.intersection;
                _similarities[classification].regionLevel.globalSummary.union += tSimilarity.regionLevel.globalSummary.union;
                _similarities[classification].basePairLevel.globalSummary.intersection += tSimilarity.basePairLevel.globalSummary.intersection;
                _similarities[classification].basePairLevel.globalSummary.union += tSimilarity.basePairLevel.globalSummary.union;

                foreach (var sample in tSimilarity.basePairLevel.sampleWideSummary)
                {
                    _similarities[classification].basePairLevel.sampleWideSummary[sample.Key].intersection += sample.Value.intersection;
                    _similarities[classification].basePairLevel.sampleWideSummary[sample.Key].union += sample.Value.union;
                }
                foreach (var sample in tSimilarity.regionLevel.sampleWideSummary)
                {
                    _similarities[classification].regionLevel.sampleWideSummary[sample.Key].intersection += sample.Value.intersection;
                    _similarities[classification].regionLevel.sampleWideSummary[sample.Key].union += sample.Value.union;
                }
            }
        }

        private void InitializeDi3()
        {
            _di3 = new Dictionary<string, Di3>();
            foreach (var result in _source.analysisResults)
                foreach (var chr in result.Value.chrwideStats)
                    if (!_di3.ContainsKey(chr.Key))
                        _di3.Add(chr.Key, new Di3());
        }
    }
}
