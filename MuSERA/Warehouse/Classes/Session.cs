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
using Polimi.DEIB.VahidJalili.IGenomics;
using Polimi.DEIB.VahidJalili.MuSERA.Warehouse.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Documents;

namespace Polimi.DEIB.VahidJalili.MuSERA.Warehouse
{
    public class Session<ER, Metadata> : INotifyPropertyChanged
        where ER : IInterval<int, Metadata>, IComparable<ER>, new()
        where Metadata : IChIPSeqPeak, new()
    {
        public bool isCompleted { set; get; }
        public int index { set; get; }
        public Dictionary<uint, string> samples { set; get; }
        public DateTime startTime { set; get; }
        public DateTime endTime { set; get; }
        public string elapsedTime { set; get; }
        public AnalysisOptions options { set; get; }
        public Dictionary<uint, AnalysisResult<ER, Metadata>> analysisResults { set; get; }

        public string status
        {
            set
            {
                _status = value;
                NotifyPropertyChanged("status");
            }
            get { return _status; }
        }
        private string _status;

        public double similarityEstimationProgress
        {
            private set
            {
                _similarityEstimationProgress = value;
                NotifyPropertyChanged("similarityEstimationProgress");
            }
            get { return _similarityEstimationProgress; }
        }
        private double _similarityEstimationProgress;

        public FlattenedSimilarities flattenedSimilarities
        {
            private set
            {
                _flattenedSimilarities = value;
                NotifyPropertyChanged("flattenedSimilarities");
            }
            get { return _flattenedSimilarities; }
        }
        private FlattenedSimilarities _flattenedSimilarities;

        public Dictionary<ERClassificationType, Similarity> similarity
        {
            private set
            {
                _similarity = value;
                NotifyPropertyChanged("similarity");
            }
            get { return _similarity; }
        }
        private Dictionary<ERClassificationType, Similarity> _similarity;


        private BackgroundWorker similarityEstimator_BGW { set; get; }

        // TODO : Can I dispose BGWs in Completed even ?
        // TODO : add progress report for both background workers

        public void CalculateSimilarities()
        {
            similarityEstimator_BGW = new BackgroundWorker();
            similarityEstimator_BGW.DoWork += similarityEstimator_BGW_DoWork;
            similarityEstimator_BGW.RunWorkerAsync();
        }
        private void similarityEstimator_BGW_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                var similarityEstimator = new SimilarityEstimator<ER, Metadata>(this);
                similarityEstimator.StatusChanged += SimilarityEstimatorStatusChanged;
                similarity = similarityEstimator.GetSimilarity();
                flattenedSimilarities = new FlattenedSimilarities(similarity, samples);
            }
            catch (OutOfMemoryException)
            {
                MessageBox.Show("Out-of-memory !!\n" +
                    "MuSERA does not have enough free memory space to execute the analysis. The program exits now.",
                    "OutOfMemory", MessageBoxButton.OK, MessageBoxImage.Stop);
                System.Environment.Exit(0);
            }
        }
        private void SimilarityEstimatorStatusChanged(object sender, DValueEventArgs e)
        {
            similarityEstimationProgress = e.Value;
        }

        public FlowDocument ToFlowDocument()
        {
            Paragraph paragraph = new Paragraph();

            paragraph.Inlines.Add("\nProcess started at :\n");
            paragraph.Inlines.Add(startTime.ToLongDateString() + " @\n" + startTime.ToLongTimeString() + "\n");

            if (elapsedTime == null)
                switch (status)
                {
                    case "Analysis completed":
                        paragraph.Inlines.Add("\nElapsed Time : " + DateTime.Now.Subtract(startTime).Duration().ToString());
                        break;

                    default:
                        paragraph.Inlines.Add("\nElapsed Time : in progress ...");
                        break;
                }
            else
                paragraph.Inlines.Add("\nElapsed Time : " + elapsedTime);

            paragraph.Inlines.Add("\n\nStatus : " + status.ToString());
            paragraph.Inlines.Add("\n\nSamples : ");

            int counter = 0;
            foreach (var sample in samples)
                paragraph.Inlines.Add("\n\n" + (++counter).ToString() + " : " + sample.Value);

            FlowDocument flowDocument = new FlowDocument();
            flowDocument.Blocks.Add(paragraph);
            return flowDocument;
        }

        /// <summary>
        /// Returns a FlowDocument containing analysis overview of given sampleID.
        /// </summary>
        /// <param name="sampleID">The ID of the sampleID to get it's overview.</param>
        /// <returns></returns>
        public FlowDocument Overview(uint sampleID)
        {
            Paragraph paragraph = new Paragraph();

            /// Selected Sample's Data
            var ssd = analysisResults[sampleID];

            paragraph.Inlines.Add("Analysis performed on :\n");
            paragraph.Inlines.Add("\t" + ssd.FileName);

            paragraph.Inlines.Add("\n\nwith support of :");
            foreach (var sample in samples)
                if (sample.Key != sampleID)
                    paragraph.Inlines.Add("\n\t" + sample.Value);

            if (options.replicateType == ReplicateType.Biological)
                paragraph.Inlines.Add("\n\nSamples are treated as Biological Replicates with at least " + options.C.ToString() + " intersecting samples (C) required");
            else
                paragraph.Inlines.Add("\n\nSamples are treated as Technical Replicates with at least " + options.C.ToString() + " intersecting samples (C) required");

            paragraph.Inlines.Add(
                "\n\nThresholds :\n\tTau^w = " + options.tauW.ToString() +
                "\n\tTau^s = " + options.tauS.ToString() +
                "\n\tGamma = " + options.gamma.ToString());

            switch (options.fDRProcedure)
            {
                case FDRProcedure.BenjaminiHochberg:
                    paragraph.Inlines.Add("\n\nBenjamini–Hochberg step-up procedure is used to control false discovery rate at level alpha = " + options.alpha.ToString());
                    break;
            }

            paragraph.Inlines.Add("\n\nWhen multiple enrichment regions from one sample intersect with single region, ");
            if (options.multipleIntersections == MultipleIntersections.UseLowestPValue)
                paragraph.Inlines.Add("\nthe one with LOWEST p-value is chosen (the most stringent one)");
            else
                paragraph.Inlines.Add("\nthe one with HIGHEST p-value is chosen (the least stringent one)");

            double totalERsCount = ssd.total____s + ssd.total____w;

            paragraph.Inlines.Add("\n\nSample File Name:\t" + ssd.FileName);
            paragraph.Inlines.Add("\nSample Full Name:\t" + ssd.FilePath);
            paragraph.Inlines.Add("\n.");
            paragraph.Inlines.Add("\n-\t" + totalERsCount.ToString() + "\t\tare total ERs count (Disregarding background ERs set)");
            paragraph.Inlines.Add("\n-\t" + ssd.total____s.ToString() + "\t" + (Math.Round(ssd.total____s * 100.0 / totalERsCount, 2)).ToString() + "%\tmarked as Stringent ERs (% estimated disregarding background ERs set)");
            paragraph.Inlines.Add("\n-\t" + ssd.total____w.ToString() + "\t" + (Math.Round(((ssd.total____w * 100.0) / totalERsCount), 2)).ToString() + "%\tmarked as Weak ERs (% estimated disregarding background ERs set)");
            paragraph.Inlines.Add("\n-\t" + ssd.total____g.ToString() + "\t" + (Math.Round(((ssd.total____g * 100.0) / (totalERsCount + ssd.total____g)), 2)).ToString() + "%\tmarked as background ERs (p-value > Tau_w)(% estimated considering all ERs in BED file)");
            paragraph.Inlines.Add("\n-\t" + (ssd.total___sc + ssd.total___wc).ToString() + "\t" + (Math.Round((ssd.total___sc + ssd.total___wc) * 100.0 / totalERsCount, 2)).ToString() + "%\tare Confirmed ERs (stringent and weak confirmed)");
            paragraph.Inlines.Add("\n-\t" + (ssd.total__sdc + ssd.total__sdt + ssd.total__wdc + ssd.total__wdt).ToString() + "\t" + (Math.Round((ssd.total__sdc + ssd.total__sdt + ssd.total__wdc + ssd.total__wdt) * 100.0 / totalERsCount, 2)).ToString() + "%\tare Discarded ERs (stringent and weak discarded)");
            paragraph.Inlines.Add("\n-\t" + ssd.total____o.ToString() + "\t" + (Math.Round(((ssd.total____o * 100.0) / totalERsCount), 2)).ToString() + "%\tERs in Output set");
            paragraph.Inlines.Add("\n-\t" + ssd.total___TP.ToString() + "\t" + (Math.Round(((ssd.total___TP * 100.0) / ssd.total____o), 2)).ToString() + "%\tMultiple-Testing Confirmed in Output set");
            paragraph.Inlines.Add("\n-\t" + ssd.total___FP.ToString() + "\t" + (Math.Round(((ssd.total___FP * 100.0) / ssd.total____o), 2)).ToString() + "%\tMultiple-Testing Discarded in Output set");
            paragraph.Inlines.Add("\n.");
            paragraph.Inlines.Add("\n-\t" + ssd.total___sc.ToString() + "\t" + (Math.Round(((ssd.total___sc * 100.0) / ssd.total____s), 2)).ToString() + "%\tStringent-Confirmed ERs (Ratio: of total stringent)");
            paragraph.Inlines.Add("\n-\t" + ssd.total__sdc.ToString() + "\t" + (Math.Round(((ssd.total__sdc * 100.0) / ssd.total____s), 2)).ToString() + "%\tStringent-Discarded ERs (Reason : failing |R_{ji,*}| < C) ; Ratio: of total stringent)");
            paragraph.Inlines.Add("\n-\t" + ssd.total__sdt.ToString() + "\t" + (Math.Round(((ssd.total__sdt * 100.0) / ssd.total____s), 2)).ToString() + "%\tStringent-Discarded ERs (Reason : failing {X-sqrd}_{ji} < {Chi-sqrd}_{Gamma,2K} ; Ratio: of total stringent)");
            paragraph.Inlines.Add("\n.");
            paragraph.Inlines.Add("\n-\t" + ssd.total___wc.ToString() + "\t" + (Math.Round(((ssd.total___wc * 100.0) / ssd.total____w), 2)).ToString() + "%\tWeak-Confirmed ERs");
            paragraph.Inlines.Add("\n-\t" + ssd.total__wdc.ToString() + "\t" + (Math.Round(((ssd.total__wdc * 100.0) / ssd.total____w), 2)).ToString() + "%\tWeak-Discarded ERs (Reason : failing |R_{ji,*}| < C) ; Ratio: of total Weak)");
            paragraph.Inlines.Add("\n-\t" + ssd.total__wdt.ToString() + "\t" + (Math.Round(((ssd.total__wdt * 100.0) / ssd.total____w), 2)).ToString() + "%\tWeak-Discarded ERs (Reason : failing {X-sqrd}_{ji} < {Chi-sqrd}_{Gamma,2K} ; Ratio: of total Weak)");
            paragraph.Inlines.Add("\n.");
            paragraph.Inlines.Add("\n.");
            paragraph.Inlines.Add("\n-\t\t\tInformative Ratios:");
            paragraph.Inlines.Add("\n.");
            paragraph.Inlines.Add("\n-\t\t" + (Math.Round((((ssd.total___sc) * 100.0) / ssd.total____o), 2)).ToString() + "%\t|{Stringent-Confirmed ERs}| / |{output set}| *");
            paragraph.Inlines.Add("\n-\t\t" + (Math.Round((((ssd.total___wc) * 100.0) / ssd.total____o), 2)).ToString() + "%\t|{Weak-Confirmed ERs}| / |{output set}| *");
            paragraph.Inlines.Add("\n.");
            paragraph.Inlines.Add("\n-\t\t" + (Math.Round((((ssd.total___so) * 100.0) / ssd.total____o), 2)).ToString() + "%\t|{Stringent-Confirmed ERs in output set}| / |{output set}| *");
            paragraph.Inlines.Add("\n-\t\t" + (Math.Round((((ssd.total___wo) * 100.0) / ssd.total____o), 2)).ToString() + "%\t|{Weak-Confirmed ERs in output set}| / |{output set}| *");
            paragraph.Inlines.Add("\n.");
            paragraph.Inlines.Add("\n-\t\t" + (Math.Round((((ssd.total___sc) * 100.0) / totalERsCount), 2)).ToString() + "%\t|{Stringent-Confirmed ERs}| / |{Total ERs}|");
            paragraph.Inlines.Add("\n-\t\t" + (Math.Round((((ssd.total__sdc) * 100.0) / totalERsCount), 2)).ToString() + "%\t|{Stringent-Discarded ERs}| / |{Total ERs}|  (Reason : failing |R_{ji,*}| < C)");
            paragraph.Inlines.Add("\n-\t\t" + (Math.Round((((ssd.total__sdt) * 100.0) / totalERsCount), 2)).ToString() + "%\t|{Stringent-Discarded ERs}| / |{Total ERs}|  (Reason : failing {X-sqrd}_{ji} < {Chi-sqrd}_{Gamma,2K})");
            paragraph.Inlines.Add("\n.");
            paragraph.Inlines.Add("\n-\t\t" + (Math.Round((((ssd.total___wc) * 100.0) / totalERsCount), 2)).ToString() + "%\t|{Weak-Confirmed ERs}| / |{Total ERs}|");
            paragraph.Inlines.Add("\n-\t\t" + (Math.Round((((ssd.total__wdc) * 100.0) / totalERsCount), 2)).ToString() + "%\t|{Weak-Discarded ERs}| / |{Total ERs}|  (Reason : failing |R_{ji,*}| < C)");
            paragraph.Inlines.Add("\n-\t\t" + (Math.Round((((ssd.total__wdt) * 100.0) / totalERsCount), 2)).ToString() + "%\t|{Weak-Discarded ERs}| / |{Total ERs}|  (Reason : failing {X-sqrd}_{ji} < {Chi-sqrd}_{Gamma,2K})");
            paragraph.Inlines.Add("\n.");
            paragraph.Inlines.Add("\n.");
            paragraph.Inlines.Add("\n.");

            paragraph.Inlines.Add("\n* A ER may pass tests and get marked as (weak/stringent)/Confirmed; but it may not apear in output set if it fails a test " +
                    "(with repect to the Replicate type), hence the two ratios may be different (i.e., [((weak/stringent)-Confirmed ERs) / (output set)] and [((weak/stringent)-Confirmed ERs in output set) / (output set)] ");


            FlowDocument flowDocument = new FlowDocument();
            flowDocument.Blocks.Add(paragraph);
            return flowDocument;
        }

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
