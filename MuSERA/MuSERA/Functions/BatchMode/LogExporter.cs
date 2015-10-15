/** Copyright © 2013-2015 Vahid Jalili
 * 
 * This file is part of MuSERA project.
 * MuSERA is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation,
 * either version 3 of the License, or (at your option) any later version.
 * MuSERA is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A 
 * PARTICULAR PURPOSE. See the GNU General Public License for more details.
 * You should have received a copy of the GNU General Public License along with Foobar. If not, see http://www.gnu.org/licenses/.
 **/

using Polimi.DEIB.VahidJalili.MuSERA.Warehouse;
using System;
using System.Collections.Generic;
using System.IO;

namespace Polimi.DEIB.VahidJalili.MuSERA.Functions.BatchMode
{
    internal class LogExporter
    {
        public LogExporter(
            string IDsFile,
            string statsFile,
            string sessionTitle,
            string sessionIndex,
            string sampleFile,
            string sampleIndex,
            List<string> supSamples,
            AnalysisOptions analysisOptions,
            AnalysisResult<Interval<int, MChIPSeqPeak>, MChIPSeqPeak> analysisResult)
        {
            _IDsFile = IDsFile;
            _statsFile = statsFile;
            _sessionTitle = sessionTitle;
            _sessionIndex = sessionIndex;
            _sampleFile = sampleFile;
            _sampleIndex = sampleIndex;
            _supSamples = supSamples;
            _analysisOptions = analysisOptions;
            _analysisResult = analysisResult;
        }

        private string _IDsFile { set; get; }
        private string _statsFile { set; get; }
        private string _sessionTitle { set; get; }
        private string _sessionIndex { set; get; }
        private string _sampleFile { set; get; }
        private string _sampleIndex { set; get; }
        private List<string> _supSamples { set; get; }
        private AnalysisOptions _analysisOptions { set; get; }
        private AnalysisResult<Interval<int, MChIPSeqPeak>, MChIPSeqPeak> _analysisResult { set; get; }
        private string testID { set; get; }

        public void Export()
        {
            _IDsFile += "Log_TestIDs.txt";
            _statsFile += "Log_TestStats.txt";

            Export____Test_ID();
            Export_Test_Stats();
        }
        private void Export____Test_ID()
        {
            if (!Directory.Exists(Path.GetDirectoryName(_IDsFile)))
                Directory.CreateDirectory(Path.GetDirectoryName(_IDsFile));

            if (!File.Exists(_IDsFile))
            {
                using (FileStream fs = File.Create(_IDsFile))
                    testID = "Test_1";
            }
            else
            {
                using (StreamReader reader = new StreamReader(_IDsFile))
                {
                    string
                        currentLine = "",
                        lastLine = "";

                    while ((currentLine = reader.ReadLine()) != null)
                        lastLine = currentLine;

                    if (lastLine != null)
                    {
                        if (lastLine.Trim() != "")
                        {
                            string[] splitted_last_line = lastLine.Split('\t');
                            string[] last_test_ID = splitted_last_line[1].Split('_');
                            testID = "Test_" + (Math.Floor(Double.Parse(last_test_ID[1])) + 1).ToString();
                        }
                        else
                        {
                            testID = "Test_1";
                        }
                    }
                    else
                    {
                        testID = "Test_1";
                    }
                }
            }

            testID = _sessionTitle + "_" + _sessionIndex + "." + _sampleIndex;
            using (FileStream fileStream = new FileStream(_IDsFile, FileMode.Append, FileAccess.Write))
            {
                using (StreamWriter writer = new StreamWriter(fileStream))
                {
                    string line = "[" + DateTime.Now.ToShortDateString() + " - " + DateTime.Now.ToLongTimeString() + "]\t";

                    line += testID + "\t";
                    line += _analysisOptions.replicateType.ToString() + "\t";
                    line += _analysisOptions.tauS.ToString() + "\t";
                    line += _analysisOptions.tauW.ToString() + "\t";
                    line += _analysisOptions.gamma.ToString() + "\t";
                    line += _analysisOptions.fDRProcedure.ToString() + "\t";
                    line += _analysisOptions.alpha.ToString() + "\t";
                    line += _sampleFile + "\t";

                    foreach (string s in _supSamples)
                        line += s + ";";

                    writer.WriteLine(line);
                }
            }
        }
        private void Export_Test_Stats()
        {
            string line = "Vahid";
            if (!Directory.Exists(Path.GetDirectoryName(_statsFile)))
                Directory.CreateDirectory(Path.GetDirectoryName(_statsFile));

            if (!File.Exists(_statsFile))
            {
                using (FileStream fileStream = File.Create(_statsFile))
                {
                    using (StreamWriter writer = new StreamWriter(fileStream))
                    {
                        line = "Test_ID\t";
                        line += "# Total ERs\t";
                        line += "# Stringent\t";
                        line += "% Stringent\t";

                        line += "# Weak\t";
                        line += "% Weak\t";

                        line += "# Confirmed\t";
                        line += "% Confirmed\t";

                        line += "# Discarded\t";
                        line += "% Discarded\t";

                        line += "# Output\t";
                        line += "% Output\t";

                        line += "# TP\t";
                        line += "% TP\t";

                        line += "# FP\t";
                        line += "% FP\t";

                        line += "# Stringent.Confirmed\t";
                        line += "% Stringent.Confirmed\t";

                        line += "# Stringent.Discarded(C)\t";
                        line += "% Stringent.Discarded(C)\t";

                        line += "# Stringent.Discarded(Test)\t";
                        line += "% Stringent.Discarded(Test)\t";

                        line += "# Weak.Confirmed\t";
                        line += "% Weak.Confirmed\t";

                        line += "# Weak.Discarded(C)\t";
                        line += "% Weak.Discarded(C)\t";

                        line += "# Weak.Discarded(Test)\t";
                        line += "% Weak.Discarded(Test)\t";

                        line += "# Stringent ERs both Confirmed and Discarded\t";
                        line += "% Stringent ERs both Confirmed and Discarded\t";

                        line += "# Weak ERs both Confirmed and Discarded\t";
                        line += "% Weak ERs both Confirmed and Discarded\t";

                        line += "# Stringent.Confirmed/output\t";
                        line += "% Stringent.Confirmed/output\t";

                        line += "# Weak.Confirmed/output\t";
                        line += "% Weak.Confirmed/output\t";

                        line += "# Stringent.Confirmed.in.Output/output\t";
                        line += "% Stringent.Confirmed.in.Output/output\t";

                        line += "# Weak.Confirmed.in.Output/output\t";
                        line += "% Weak.Confirmed.in.Output/output\t";

                        line += "# Stringent.Confirmed/total\t";
                        line += "% Stringent.Confirmed/total\t";

                        line += "# Stringent.Discarded(C)/total\t";
                        line += "% Stringent.Discarded(C)/total\t";

                        line += "# Stringent.Discarded(Test)/total\t";
                        line += "% Stringent.Discarded(Test)/total\t";

                        line += "# Weak.Confirmed/total\t";
                        line += "% Weak.Confirmed/total\t";

                        line += "# Weak.Discarded(C)/total\t";
                        line += "% Weak.Discarded(C)/total\t";

                        line += "# Weak.Discarded(Test)/total\t";
                        line += "% Weak.Discarded(Test)/total\t";
                        writer.WriteLine(line);
                    }
                }
            }

            using (FileStream fileStream = new FileStream(_statsFile, FileMode.Append, FileAccess.Write))
            {
                using (StreamWriter writer = new StreamWriter(fileStream))
                {
                    UInt32 totalERsCount = _analysisResult.total____s + _analysisResult.total____w;
                    line = testID + "\t";
                    line += totalERsCount.ToString() + "\t";
                    line += _analysisResult.total____s.ToString() + "\t" + (Math.Round(((double)(_analysisResult.total____s * 100) / (double)totalERsCount), 2)).ToString() + "\t";
                    line += _analysisResult.total____w.ToString() + "\t" + (Math.Round(((double)(_analysisResult.total____w * 100) / (double)totalERsCount), 2)).ToString() + "\t";
                    line += (_analysisResult.total___sc + _analysisResult.total___wc).ToString() + "\t" + (Math.Round(((double)((_analysisResult.total___sc + _analysisResult.total___wc) * 100.0) / (double)totalERsCount), 2)).ToString() + "\t";
                    line += (_analysisResult.total__sdc + _analysisResult.total__sdt + _analysisResult.total__wdc + _analysisResult.total__wdt).ToString() + "\t" + (Math.Round(((double)((_analysisResult.total__sdc + _analysisResult.total__sdt + _analysisResult.total__wdc + _analysisResult.total__wdt) * 100.0) / (double)totalERsCount), 2)).ToString() + "\t";
                    line += _analysisResult.total____o.ToString() + "\t" + (Math.Round(((double)(_analysisResult.total____o * 100) / (double)totalERsCount), 2)).ToString() + "\t";
                    line += _analysisResult.total___TP.ToString() + "\t" + (Math.Round(((double)(_analysisResult.total___TP * 100) / (double)_analysisResult.total____o), 2)).ToString() + "\t";
                    line += _analysisResult.total___FP.ToString() + "\t" + (Math.Round(((double)(_analysisResult.total___FP * 100) / (double)_analysisResult.total____o), 2)).ToString() + "\t";
                    line += _analysisResult.total___sc.ToString() + "\t" + (Math.Round(((double)(_analysisResult.total___sc * 100) / (double)_analysisResult.total____s), 2)).ToString() + "\t";
                    line += _analysisResult.total__sdc.ToString() + "\t" + (Math.Round(((double)(_analysisResult.total__sdc * 100) / (double)_analysisResult.total____s), 2)).ToString() + "\t";
                    line += _analysisResult.total__sdt.ToString() + "\t" + (Math.Round(((double)(_analysisResult.total__sdt * 100) / (double)_analysisResult.total____s), 2)).ToString() + "\t";
                    line += _analysisResult.total___wc.ToString() + "\t" + (Math.Round(((double)(_analysisResult.total___wc * 100) / (double)_analysisResult.total____w), 2)).ToString() + "\t";
                    line += _analysisResult.total__wdc.ToString() + "\t" + (Math.Round(((double)(_analysisResult.total__wdc * 100) / (double)_analysisResult.total____w), 2)).ToString() + "\t";
                    line += _analysisResult.total__wdt.ToString() + "\t" + (Math.Round(((double)(_analysisResult.total__wdt * 100) / (double)_analysisResult.total____w), 2)).ToString() + "\t";
                    line += _analysisResult.total_scom.ToString() + "\t" + (Math.Round(((double)(_analysisResult.total_scom * 100) / (double)_analysisResult.total____s), 2)).ToString() + "\t";
                    line += _analysisResult.total_wcom.ToString() + "\t" + (Math.Round(((double)(_analysisResult.total_wcom * 100) / (double)_analysisResult.total____w), 2)).ToString() + "\t";
                    line += _analysisResult.total___sc.ToString() + "\t" + (Math.Round(((double)(_analysisResult.total___sc * 100) / (double)_analysisResult.total____o), 2)).ToString() + "\t";
                    line += _analysisResult.total___wc.ToString() + "\t" + (Math.Round(((double)(_analysisResult.total___wc * 100) / (double)_analysisResult.total____o), 2)).ToString() + "\t";
                    line += _analysisResult.total___so.ToString() + "\t" + (Math.Round(((double)(_analysisResult.total___so * 100) / (double)_analysisResult.total____o), 2)).ToString() + "\t";
                    line += _analysisResult.total___wo.ToString() + "\t" + (Math.Round(((double)(_analysisResult.total___wo * 100) / (double)_analysisResult.total____o), 2)).ToString() + "\t";
                    line += _analysisResult.total___sc.ToString() + "\t" + (Math.Round(((double)(_analysisResult.total___sc * 100) / (double)totalERsCount), 2)).ToString() + "\t";
                    line += _analysisResult.total__sdc.ToString() + "\t" + (Math.Round(((double)(_analysisResult.total__sdc * 100) / (double)totalERsCount), 2)).ToString() + "\t";
                    line += _analysisResult.total__sdt.ToString() + "\t" + (Math.Round(((double)(_analysisResult.total__sdt * 100) / (double)totalERsCount), 2)).ToString() + "\t";
                    line += _analysisResult.total___wc.ToString() + "\t" + (Math.Round(((double)(_analysisResult.total___wc * 100) / (double)totalERsCount), 2)).ToString() + "\t";
                    line += _analysisResult.total__wdc.ToString() + "\t" + (Math.Round(((double)(_analysisResult.total__wdc * 100) / (double)totalERsCount), 2)).ToString() + "\t";
                    line += _analysisResult.total__wdt.ToString() + "\t" + (Math.Round(((double)(_analysisResult.total__wdt * 100) / (double)totalERsCount), 2)).ToString() + "\t";
                    writer.WriteLine(line);
                }
            }
        }
    }
}
