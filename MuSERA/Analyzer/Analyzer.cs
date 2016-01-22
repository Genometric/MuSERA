/** Copyright © 2013-2015 Vahid Jalili
 * 
 * This file is part of MuSERA project.
 * MuSERA is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation,
 * either version 3 of the License, or (at your option) any later version.
 * MuSERA is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A 
 * PARTICULAR PURPOSE. See the GNU General Public License for more details.
 * You should have received a copy of the GNU General Public License along with Foobar. If not, see http://www.gnu.org/licenses/.
 **/

using Polimi.DEIB.VahidJalili.GIFP;
using Polimi.DEIB.VahidJalili.IGenomics;
using Polimi.DEIB.VahidJalili.MuSERA.Warehouse;
using Polimi.DEIB.VahidJalili.MuSERA.XSquaredData;
using System;
using System.Collections.Generic;

namespace Polimi.DEIB.VahidJalili.MuSERA.Analyzer
{
    public class Analyzer<Peak, Metadata>
        where Peak : IInterval<int, Metadata>, IComparable<Peak>, new()
        where Metadata : IChIPSeqPeak, IComparable<Metadata>, new()
    {
        #region .::.      Status Change        .::.

        private Progress _status;
        public Progress Status
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

        public event EventHandler<AnalyzerEventArgs> StatusChanged;
        private void OnStatusValueChaned(Progress value)
        {
            if (StatusChanged != null)
                StatusChanged(this, new AnalyzerEventArgs(value));
        }

        #endregion

        private Processor<Peak, Metadata> processor { set; get; }
        private Data<Peak, Metadata> _data { set; get; }

        public Analyzer()
        {
            _data = new Data<Peak, Metadata>();
        }
        public void AddSample(ParsedChIPseqPeaks<int, Peak, Metadata> sample)
        {
            _data.samples.Add(sample.fileHashKey, sample);
        }
        public void Run(AnalysisOptions options)
        {
            Options.replicateType = options.replicateType;
            Options.C = options.C;
            Options.tauS = options.tauS;
            Options.tauW = options.tauW;
            Options.gamma = options.gamma;
            Options.alpha = options.alpha;
            Options.multipleIntersections = options.multipleIntersections;

            _data.cachedChiSqrd.Clear();
            for (int i = 1; i <= _data.samples.Count; i++)
                _data.cachedChiSqrd.Add(Math.Round(ChiSquaredCache.ChiSqrdINVRTP(Options.gamma, (byte)(i * 2)), 3));

            _data.BuildSharedItems();

            processor = new Processor<Peak, Metadata>(_data);
            int totalSteps = _data.samples.Count + 3;
            int stepCounter = -1;

            //Console.WriteLine("");
            foreach (var sample in _data.samples)
            {
                #region .::.    Status      .::.
                if (sample.Value.fileName.Length > 36)
                {
                    Status = new Progress(++stepCounter / totalSteps, totalSteps, stepCounter, "processing sample: ..." + sample.Value.fileName.Substring(sample.Value.fileName.Length - 35, 35));
                    //Console.Write("\r[" + (++stepCounter).ToString() + "\\" + totalSteps + "] processing sample: ..." + sampleKey.Value.Substring(sampleKey.Value.Length - 35, 35));
                }
                else
                {
                    Status = new Progress(++stepCounter / totalSteps, totalSteps, stepCounter, "processing sample: ..." + sample.Value.fileName);
                    //Console.Write("\r[" + (++stepCounter).ToString() + "\\" + totalSteps + "] processing sample: " + sampleKey.Value);
                }
                //Console.WriteLine("");


                #endregion

                foreach (var chr in sample.Value.intervals)
                    foreach (var strand in chr.Value)
                    {
                        //int currentLineCursor = Console.CursorTop;
                        //Console.SetCursorPosition(0, Console.CursorTop);
                        //Console.Write(new string(' ', Console.WindowWidth));
                        //Console.SetCursorPosition(0, currentLineCursor);
                        //Console.Write("\r .::. Processing {0}", chr.Key);
                        Status = new Progress(Status.percentage, Status.totalStepsCount, Status.currentStepNumber, "Processing " + chr.Key);
                        processor.Run(sample.Key, chr.Key, strand.Key);
                    }
            }

            #region .::.    Status      .::.
            //Console.Write("\r[" + (++stepCounter).ToString() + "\\" + totalSteps + "] Purifying intermediate sets.");
            Status = new Progress(++stepCounter / totalSteps, totalSteps, stepCounter, "Purifying intermediate sets.");
            #endregion
            processor.IntermediateSetsPurification();

            #region .::.    Status      .::.
            //Console.WriteLine("[" + (++stepCounter).ToString() + "\\" + totalSteps + "] Creating output set.");
            Status = new Progress(++stepCounter / totalSteps, totalSteps, stepCounter, "Creating output set.");
            #endregion
            processor.CreateOuputSet();

            #region .::.    Status      .::.
            //Console.WriteLine("[" + stepCounter + "\\" + totalSteps + "] Performing Multiple testing correction.");
            Status = new Progress(++stepCounter / totalSteps, totalSteps, stepCounter, "Performing Multiple testing correction.");
            #endregion
            processor.EstimateFalseDiscoveryRate();

            #region .::.    Status      .::.
            Status = new Progress(stepCounter / totalSteps, totalSteps, stepCounter, "Creating combined output set.");
            #endregion
            processor.CreateCombinedOutputSet();
        }
        public Dictionary<uint, AnalysisResult<Peak, Metadata>> GetResults()
        {
            foreach (var sample in _data.analysisResults)
                sample.Value.ReadOverallStats();

            return _data.analysisResults;
        }
        public Dictionary<string, List<Peak>> GetMergedReplicates()
        {
            var rtv = new Dictionary<string, List<Peak>>();
            int counter = 0;

            foreach(var chr in _data.mergedReplicates)
            {
                var tmpPeaks = new List<Peak>();
                foreach(var peak in chr.Value)
                {
                    peak.Value.metadata.name = "MuSERA_Peak_" + (counter++).ToString();
                    tmpPeaks.Add(peak.Value);
                }

                rtv.Add(chr.Key, tmpPeaks);
            }

            return rtv;
        }
        public Dictionary<uint, string> GetSamples()
        {
            var rtv = new Dictionary<uint, string>();
            foreach (var sample in _data.samples)
                rtv.Add(sample.Key, sample.Value.fileName);
            return rtv;
        }
    }
}
