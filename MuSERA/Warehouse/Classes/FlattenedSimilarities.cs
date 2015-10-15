/** Copyright © 2013-2015 Vahid Jalili
 * 
 * This file is part of MuSERA project.
 * MuSERA is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation,
 * either version 3 of the License, or (at your option) any later version.
 * MuSERA is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A 
 * PARTICULAR PURPOSE. See the GNU General Public License for more details.
 * You should have received a copy of the GNU General Public License along with Foobar. If not, see http://www.gnu.org/licenses/.
 **/

using Polimi.DEIB.VahidJalili.DI3.AuxiliaryComponents;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Polimi.DEIB.VahidJalili.MuSERA.Warehouse.Classes
{
    public class FlattenedSimilarities : ObservableCollection<FlattenedSimilarity>
    {
        public FlattenedSimilarities(Dictionary<ERClassificationType, Similarity> similarities, Dictionary<UInt32, string> samples)
        {
            foreach (var type in similarities)
            {
                this.Add(new FlattenedSimilarity(
                     source: "Overall",
                     erClassificationType: type.Key.ToString(),
                     bpInter: type.Value.basePairLevel.globalSummary.intersection,
                     bpUnion: type.Value.basePairLevel.globalSummary.union,
                     bpJIndx: type.Value.basePairLevel.globalSummary.JaccardSimilarity,
                     reInter: type.Value.regionLevel.globalSummary.intersection,
                     reUnion: type.Value.regionLevel.globalSummary.union,
                     reJIndx: type.Value.regionLevel.globalSummary.JaccardSimilarity
                    ));

                foreach (var sample in type.Value.basePairLevel.sampleWideSummary)
                {
                    this.Add(new FlattenedSimilarity(
                     source: samples[sample.Key],
                     erClassificationType: type.Key.ToString(),
                     bpInter: type.Value.basePairLevel.sampleWideSummary[sample.Key].intersection,
                     bpUnion: type.Value.basePairLevel.sampleWideSummary[sample.Key].union,
                     bpJIndx: type.Value.basePairLevel.sampleWideSummary[sample.Key].JaccardSimilarity,
                     reInter: type.Value.regionLevel.sampleWideSummary[sample.Key].intersection,
                     reUnion: type.Value.regionLevel.sampleWideSummary[sample.Key].union,
                     reJIndx: type.Value.regionLevel.sampleWideSummary[sample.Key].JaccardSimilarity
                    ));
                }
            }
        }
    }

    public class FlattenedSimilarity : INotifyPropertyChanged
    {
        public FlattenedSimilarity(string source, string erClassificationType, double bpInter, double bpUnion, double bpJIndx, double reInter, double reUnion, double reJIndx)
        {
            this.source = source;
            this.erClassificationType = erClassificationType;
            this.bpInter = bpInter;
            this.bpUnion = bpUnion;
            this.bpJIndx = bpJIndx;
            this.reInter = reInter;
            this.reUnion = reUnion;
            this.reJIndx = reJIndx;
        }

        /// <summary>
        /// Sets and gets the source of estimated type.
        /// <para>It could be overall or per-sample type estimation.</para>
        /// </summary>
        public string source
        {
            private set
            {
                _source = value;
                NotifyPropertyChanged("source");
            }
            get { return _source; }
        }
        private string _source;

        /// <summary>
        /// Sets and gets the ER Classification Type.
        /// </summary>
        public string erClassificationType
        {
            private set
            {
                _erClassificationType = value;
                NotifyPropertyChanged("erClassificationType");
            }
            get { return _erClassificationType; }
        }
        private string _erClassificationType;

        /// <summary>
        /// Sets and gets base-pair level intersection cardinality.
        /// </summary>
        public double bpInter
        {
            private set
            {
                _bpInter = value;
                NotifyPropertyChanged("bpInter");
            }
            get { return _bpInter; }
        }
        private double _bpInter;

        /// <summary>
        /// Sets and gets base-pair level union cardinality.
        /// </summary>
        public double bpUnion
        {
            private set
            {
                _bpUnion = value;
                NotifyPropertyChanged("bpUnion");
            }
            get { return _bpUnion; }
        }
        private double _bpUnion;

        /// <summary>
        /// Sets and gets base-pair level Jaccard index.
        /// </summary>
        public double bpJIndx
        {
            private set
            {
                _bpJIndx = value;
                NotifyPropertyChanged("bpJIndx");
            }
            get { return _bpJIndx; }
        }
        private double _bpJIndx;

        /// <summary>
        /// Sets and gets region level intersection cardinality.
        /// </summary>
        public double reInter
        {
            private set
            {
                _reInter = value;
                NotifyPropertyChanged("reInter");
            }
            get { return _reInter; }
        }
        private double _reInter;

        /// <summary>
        /// Sets and get region level union cardinality.
        /// </summary>
        public double reUnion
        {
            private set
            {
                _reUnion = value;
                NotifyPropertyChanged("reUnion");
            }
            get { return _reUnion; }
        }
        private double _reUnion;

        /// <summary>
        /// Sets and gets region level Jaccard index.
        /// </summary>
        public double reJIndx
        {
            private set
            {
                _reJIndx = value;
                NotifyPropertyChanged("reJIndx");
            }
            get { return _reJIndx; }
        }
        private double _reJIndx;


        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
