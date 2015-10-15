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
using Polimi.DEIB.VahidJalili.MuSERA.Warehouse;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Documents;
using System.Xml;

namespace Polimi.DEIB.VahidJalili.MuSERA.Exporter
{
    public class ExporterBase<Peak, Metadata>
        where Peak : IInterval<int, Metadata>, IComparable<Peak>, new()
        where Metadata : IChIPSeqPeak, new()
    {
        public string samplePath { set; get; }
        protected string sessionPath { set; get; }
        protected bool includeBEDHeader { set; get; }
        protected Dictionary<uint, string> samples { set; get; }
        protected AnalysisResult<Peak, Metadata> data { set; get; }
        protected void Export_Overview(FlowDocument overview)
        {
            using (FileStream fs = new FileStream(samplePath + Path.DirectorySeparatorChar + "Overview.xaml", FileMode.OpenOrCreate, FileAccess.Write))
            {
                TextRange textRange = new TextRange(overview.ContentStart, overview.ContentEnd);
                textRange.Save(fs, DataFormats.Xaml);
            }
        }
        protected void Export__R_j__o_BED()
        {
            using (File.Create(samplePath + Path.DirectorySeparatorChar + "A_Output_Set.bed")) { }
            using (StreamWriter writter = new StreamWriter(samplePath + Path.DirectorySeparatorChar + "A_Output_Set.bed"))
            {
                if (includeBEDHeader)
                    writter.WriteLine("chr\tstart\tstop\tname\tpValue(-10xlog10(p-value))");

                foreach (var chr in data.R_j__o)
                {
                    chr.Value.Sort();
                    foreach (var item in chr.Value)
                    {
                        if (item.statisticalClassification == ERClassificationType.TruePositive)
                        {
                            writter.WriteLine(
                                chr.Key + "\t" +
                                item.er.left.ToString() + "\t" +
                                item.er.right.ToString() + "\t" +
                                item.er.metadata.name + "\t" +
                                ConvertPValue(item.er.metadata.value));
                        }
                    }
                }
            }

            using (File.Create(samplePath + Path.DirectorySeparatorChar + "A_Output_Set_stringent_Confirmed_ERs.bed")) { }
            using (StreamWriter writter = new StreamWriter(samplePath + Path.DirectorySeparatorChar + "A_Output_Set_stringent_Confirmed_ERs.bed"))
            {
                if (includeBEDHeader)
                    writter.WriteLine("chr\tstart\tstop\tname\tpValue(-10xlog10(p-value))");

                foreach (var chr in data.R_j__o)
                {
                    chr.Value.Sort();
                    foreach (var item in chr.Value)
                    {
                        if (item.statisticalClassification == ERClassificationType.TruePositive && item.classification == ERClassificationType.StringentConfirmed)
                        {
                            writter.WriteLine(
                                chr.Key + "\t" +
                                item.er.left.ToString() + "\t" +
                                item.er.right.ToString() + "\t" +
                                item.er.metadata.name + "\t" +
                                ConvertPValue(item.er.metadata.value));
                        }
                    }
                }
            }

            using (File.Create(samplePath + Path.DirectorySeparatorChar + "A_Output_Set_weak_Confirmed_ERs.bed")) { }
            using (StreamWriter writter = new StreamWriter(samplePath + Path.DirectorySeparatorChar + "A_Output_Set_weak_Confirmed_ERs.bed"))
            {
                if (includeBEDHeader)
                    writter.WriteLine("chr\tstart\tstop\tname\tpValue(-10xlog10(p-value))");

                foreach (var chr in data.R_j__o)
                {
                    chr.Value.Sort();
                    foreach (var item in chr.Value)
                    {
                        if (item.statisticalClassification == ERClassificationType.TruePositive && item.classification == ERClassificationType.WeakConfirmed)
                        {
                            writter.WriteLine(
                                chr.Key + "\t" +
                                item.er.left.ToString() + "\t" +
                                item.er.right.ToString() + "\t" +
                                item.er.metadata.name + "\t" +
                                ConvertPValue(item.er.metadata.value));
                        }
                    }
                }
            }


            using (File.Create(samplePath + Path.DirectorySeparatorChar + "A_Output_Set_Multiple_Testing_Discarded.bed")) { }
            using (StreamWriter writter = new StreamWriter(samplePath + Path.DirectorySeparatorChar + "A_Output_Set_Multiple_Testing_Discarded.bed"))
            {
                if (includeBEDHeader)
                    writter.WriteLine("chr\tstart\tstop\tname\tpValue(-10xlog10(p-value))");

                foreach (var chr in data.R_j__o)
                {
                    chr.Value.Sort();
                    foreach (var item in chr.Value)
                    {
                        if (item.statisticalClassification == ERClassificationType.FalsePositive)
                        {
                            writter.WriteLine(
                                chr.Key + "\t" +
                                item.er.left.ToString() + "\t" +
                                item.er.right.ToString() + "\t" +
                                item.er.metadata.name + "\t" +
                                ConvertPValue(item.er.metadata.value));
                        }
                    }
                }
            }
        }
        protected void Export__R_j__o_XML()
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.ConformanceLevel = ConformanceLevel.Document;
            settings.Encoding = Encoding.UTF8;
            settings.Indent = true;
            settings.CloseOutput = true;
            settings.NewLineOnAttributes = false;

            using (XmlWriter writer = XmlWriter.Create(samplePath + Path.DirectorySeparatorChar + "A_Output_Set.xml", settings))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("OutputSet");

                writer.WriteComment("As for reduction in file size, following abbreviations are introduced:");
                writer.WriteComment("ER:Enriched Region");
                writer.WriteComment("X-sqrd:X-squared");
                writer.WriteComment("RTP:Right Tail Probability");
                writer.WriteComment("AdjP:adjusted p-value");
                writer.WriteComment("SupER:Supporting ER(s)");
                writer.WriteComment("file:file name");
                writer.WriteComment("");
                writer.WriteComment("NOTE: p-values of enriched regions are formated as -10xLog10(p-value).");
                writer.WriteComment("");

                foreach (var chr in data.R_j__o)
                {
                    foreach (var er in chr.Value)
                    {
                        writer.WriteStartElement("ER");
                        writer.WriteAttributeString("chr", chr.Key);
                        writer.WriteAttributeString("start", er.er.left.ToString());
                        writer.WriteAttributeString("stop", er.er.right.ToString());
                        writer.WriteAttributeString("name", er.er.metadata.name);
                        writer.WriteAttributeString("p-value", ConvertPValue(er.er.metadata.value));

                        writer.WriteElementString("X-sqrd", Math.Round(er.xSquared, 2).ToString());
                        writer.WriteElementString("RTP", er.rtp.ToString());
                        writer.WriteElementString("AdjP", er.adjPValue.ToString());

                        if (er.supportingERs.Count > 0)
                        {
                            writer.WriteStartElement("SupER");
                            foreach (var supER in er.supportingERs)
                            {
                                writer.WriteStartElement("ER");
                                writer.WriteAttributeString("chr", chr.Key);
                                writer.WriteAttributeString("start", supER.er.left.ToString());
                                writer.WriteAttributeString("stop", supER.er.right.ToString());
                                writer.WriteAttributeString("name", supER.er.metadata.name);
                                writer.WriteAttributeString("p-value", ConvertPValue(supER.er.metadata.value));
                                writer.WriteAttributeString("file", Path.GetFileNameWithoutExtension(samples[supER.sampleIndex]));
                                writer.WriteEndElement();
                            }

                            writer.WriteEndElement();
                        }

                        writer.WriteEndElement();
                        writer.Flush();
                    }
                }

                writer.WriteEndElement();
                writer.WriteEndDocument();
            }
        }
        protected void Export__R_j__s_BED()
        {
            using (File.Create(samplePath + Path.DirectorySeparatorChar + "B_Stringent_ERs.bed")) { }
            using (StreamWriter writter = new StreamWriter(samplePath + Path.DirectorySeparatorChar + "B_Stringent_ERs.bed"))
            {
                if (includeBEDHeader)
                    writter.WriteLine("chr\tstart\tstop\tname\tpValue(-10xlog10(p-value))");

                foreach (var chr in data.R_j__s)
                {
                    chr.Value.Sort();
                    foreach (var item in chr.Value)
                    {
                        writter.WriteLine(
                            chr.Key + "\t" +
                            item.left.ToString() + "\t" +
                            item.right.ToString() + "\t" +
                            item.metadata.name + "\t" +
                            ConvertPValue(item.metadata.value));
                    }
                }
            }
        }
        protected void Export__R_j__w_BED()
        {
            using (File.Create(samplePath + Path.DirectorySeparatorChar + "C_Weak_ERs.bed")) { }
            using (StreamWriter writter = new StreamWriter(samplePath + Path.DirectorySeparatorChar + "C_Weak_ERs.bed"))
            {
                if (includeBEDHeader)
                    writter.WriteLine("chr\tstart\tstop\tname\tpValue(-10xlog10(p-value))");

                foreach (var chr in data.R_j__w)
                {
                    chr.Value.Sort();
                    foreach (var item in chr.Value)
                    {
                        writter.WriteLine(
                            chr.Key + "\t" +
                            item.left.ToString() + "\t" +
                            item.right.ToString() + "\t" +
                            item.metadata.name + "\t" +
                            ConvertPValue(item.metadata.value));
                    }
                }
            }
        }
        protected void Export__R_j__b_BED()
        {
            using (File.Create(samplePath + Path.DirectorySeparatorChar + "D_Background_ERs.bed")) { }
            using (StreamWriter writter = new StreamWriter(samplePath + Path.DirectorySeparatorChar + "D_Background_ERs.bed"))
            {
                if (includeBEDHeader)
                    writter.WriteLine("chr\tstart\tstop\tname\tpValue(-10xlog10(p-value))");

                foreach (var chr in data.R_j__b)
                {
                    chr.Value.Sort();
                    foreach (var item in chr.Value)
                    {
                        writter.WriteLine(
                            chr.Key + "\t" +
                            item.left.ToString() + "\t" +
                            item.right.ToString() + "\t" +
                            item.metadata.name + "\t" +
                            ConvertPValue(item.metadata.value));
                    }
                }
            }
        }
        protected void Export__R_j__c_BED()
        {
            using (File.Create(samplePath + Path.DirectorySeparatorChar + "E_Confirmed_ERs.bed")) { }
            using (StreamWriter writter = new StreamWriter(samplePath + Path.DirectorySeparatorChar + "E_Confirmed_ERs.bed"))
            {
                if (includeBEDHeader)
                    writter.WriteLine("chr\tstart\tstop\tname\tpValue(-10xlog10(p-value))");

                foreach (var chr in data.R_j__c)
                {
                    var sortedDictionary = from entry in chr.Value orderby entry.Value ascending select entry;

                    foreach (var item in sortedDictionary)
                    {
                        writter.WriteLine(
                            chr.Key + "\t" +
                            item.Value.er.left.ToString() + "\t" +
                            item.Value.er.right.ToString() + "\t" +
                            item.Value.er.metadata.name + "\t" +
                            ConvertPValue(item.Value.er.metadata.value));
                    }
                }
            }

            using (File.Create(samplePath + Path.DirectorySeparatorChar + "E_Stringent_Confirmed_ERs.bed")) { }
            using (StreamWriter writter = new StreamWriter(samplePath + Path.DirectorySeparatorChar + "E_Stringent_Confirmed_ERs.bed"))
            {
                if (includeBEDHeader)
                    writter.WriteLine("chr\tstart\tstop\tname\tpValue(-10xlog10(p-value))");

                foreach (var chr in data.R_j__c)
                {
                    var sortedDictionary = from entry in chr.Value orderby entry.Value ascending select entry;

                    foreach (var item in sortedDictionary)
                    {
                        if (item.Value.classification == ERClassificationType.StringentConfirmed)
                        {
                            writter.WriteLine(
                                chr.Key + "\t" +
                                item.Value.er.left.ToString() + "\t" +
                                item.Value.er.right.ToString() + "\t" +
                                item.Value.er.metadata.name + "\t" +
                                ConvertPValue(item.Value.er.metadata.value));
                        }
                    }
                }
            }

            using (File.Create(samplePath + Path.DirectorySeparatorChar + "E_Weak_Confirmed_ERs.bed")) { }
            using (StreamWriter writter = new StreamWriter(samplePath + Path.DirectorySeparatorChar + "E_Weak_Confirmed_ERs.bed"))
            {
                if (includeBEDHeader)
                    writter.WriteLine("chr\tstart\tstop\tname\tpValue(-10xlog10(p-value))");

                foreach (var chr in data.R_j__c)
                {
                    var sortedDictionary = from entry in chr.Value orderby entry.Value ascending select entry;

                    foreach (var item in sortedDictionary)
                    {
                        if (item.Value.classification == ERClassificationType.WeakConfirmed)
                        {
                            writter.WriteLine(
                                chr.Key + "\t" +
                                item.Value.er.left.ToString() + "\t" +
                                item.Value.er.right.ToString() + "\t" +
                                item.Value.er.metadata.name + "\t" +
                                ConvertPValue(item.Value.er.metadata.value));
                        }
                    }
                }
            }
        }
        protected void Export__R_j__c_XML()
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.ConformanceLevel = ConformanceLevel.Document;
            settings.Encoding = Encoding.UTF8;
            settings.Indent = true;
            settings.CloseOutput = true;
            settings.NewLineOnAttributes = false;

            using (XmlWriter writer = XmlWriter.Create(samplePath + Path.DirectorySeparatorChar + "E_Confirmed_ERs.xml", settings))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("Confirmed-ERs");

                writer.WriteComment("As for reduction in file size, following abbreviations are introduced:");
                writer.WriteComment("ER:Enriched Region");
                writer.WriteComment("X-sqrd:X-squared");
                writer.WriteComment("RTP:Right Tail Probability");
                writer.WriteComment("SupER:Supporting ER(s)");
                writer.WriteComment("file:file name");
                writer.WriteComment("Class:Classification of the ER (e.g., stringent-confirmed, weak-confirmed)");
                writer.WriteComment("SC:Stringent Confirmed");
                writer.WriteComment("WC:Weak Confirmed");
                writer.WriteComment("");
                writer.WriteComment("NOTE: p-values of enriched regions are formated as -10xLog10(p-value).");
                writer.WriteComment("");

                foreach (var chr in data.R_j__c)
                {
                    foreach (var er in chr.Value)
                    {
                        writer.WriteStartElement("ER");
                        writer.WriteAttributeString("chr", chr.Key);
                        writer.WriteAttributeString("start", er.Value.er.left.ToString());
                        writer.WriteAttributeString("stop", er.Value.er.right.ToString());
                        writer.WriteAttributeString("name", er.Value.er.metadata.name);
                        writer.WriteAttributeString("p-value", ConvertPValue(er.Value.er.metadata.value));

                        writer.WriteElementString("X-sqrd", er.Value.xSquared.ToString());
                        writer.WriteElementString("RTP", er.Value.rtp.ToString());

                        if (er.Value.classification == ERClassificationType.StringentConfirmed)
                            writer.WriteElementString("Class", "SC");
                        else if (er.Value.classification == ERClassificationType.WeakConfirmed)
                            writer.WriteElementString("Class", "WC");

                        if (er.Value.supportingERs.Count > 0)
                        {
                            writer.WriteStartElement("SupER");

                            foreach (var supER in er.Value.supportingERs)
                            {
                                writer.WriteStartElement("ER");
                                writer.WriteAttributeString("chr", chr.Key);
                                writer.WriteAttributeString("start", supER.er.left.ToString());
                                writer.WriteAttributeString("stop", supER.er.right.ToString());
                                writer.WriteAttributeString("name", supER.er.metadata.name);
                                writer.WriteAttributeString("p-value", ConvertPValue(supER.er.metadata.value));
                                writer.WriteAttributeString("file", Path.GetFileNameWithoutExtension(samples[supER.sampleIndex]));
                                writer.WriteEndElement();
                            }

                            writer.WriteEndElement();
                        }

                        writer.WriteEndElement();
                        writer.Flush();
                    }
                }

                writer.WriteEndElement();
                writer.WriteEndDocument();
            }
        }
        protected void Export__R_j__d_BED()
        {
            using (File.Create(samplePath + Path.DirectorySeparatorChar + "F_Discarded_ERs.bed")) { }
            using (StreamWriter writter = new StreamWriter(samplePath + Path.DirectorySeparatorChar + "F_Discarded_ERs.bed"))
            {
                if (includeBEDHeader)
                    writter.WriteLine("chr\tstart\tstop\tname\tpValue(-10xlog10(p-value))");

                foreach (var chr in data.R_j__d)
                {
                    foreach (var item in chr.Value)
                    {
                        writter.WriteLine(
                            chr.Key + "\t" +
                            item.Value.er.left.ToString() + "\t" +
                            item.Value.er.right.ToString() + "\t" +
                            item.Value.er.metadata.name + "\t" +
                            ConvertPValue(item.Value.er.metadata.value));
                    }
                }
            }

            using (File.Create(samplePath + Path.DirectorySeparatorChar + "F_Stringent_Discarded_ERs.bed")) { }
            using (StreamWriter writter = new StreamWriter(samplePath + Path.DirectorySeparatorChar + "F_Stringent_Discarded_ERs.bed"))
            {
                if (includeBEDHeader)
                    writter.WriteLine("chr\tstart\tstop\tname\tpValue(-10xlog10(p-value))");

                foreach (var chr in data.R_j__d)
                {
                    foreach (var item in chr.Value)
                    {
                        if (item.Value.classification == ERClassificationType.StringentDiscarded)
                        {
                            writter.WriteLine(
                                chr.Key + "\t" +
                                item.Value.er.left.ToString() + "\t" +
                                item.Value.er.right.ToString() + "\t" +
                                item.Value.er.metadata.name + "\t" +
                                ConvertPValue(item.Value.er.metadata.value));
                        }
                    }
                }
            }

            using (File.Create(samplePath + Path.DirectorySeparatorChar + "F_Weak_Discarded_ERs.bed")) { }
            using (StreamWriter writter = new StreamWriter(samplePath + Path.DirectorySeparatorChar + "F_Weak_Discarded_ERs.bed"))
            {
                if (includeBEDHeader)
                    writter.WriteLine("chr\tstart\tstop\tname\tpValue(-10xlog10(p-value))");

                foreach (var chr in data.R_j__d)
                {
                    foreach (var item in chr.Value)
                    {
                        if (item.Value.classification == ERClassificationType.WeakDiscarded)
                        {
                            writter.WriteLine(
                                chr.Key + "\t" +
                                item.Value.er.left.ToString() + "\t" +
                                item.Value.er.right.ToString() + "\t" +
                                item.Value.er.metadata.name + "\t" +
                                ConvertPValue(item.Value.er.metadata.value));
                        }
                    }
                }
            }
        }
        protected void Export__R_j__d_XML()
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.ConformanceLevel = ConformanceLevel.Document;
            settings.Encoding = Encoding.UTF8;
            settings.Indent = true;
            settings.CloseOutput = true;
            settings.NewLineOnAttributes = false;

            using (XmlWriter writer = XmlWriter.Create(samplePath + Path.DirectorySeparatorChar + "F_Discarded_ERs.xml", settings))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("Discarded-ERs");

                writer.WriteComment("As for reduction in file size, following abbreviations are introduced:");
                writer.WriteComment("ER:Enriched Region");
                writer.WriteComment("X-sqrd:X-squared");
                writer.WriteComment("AdjP:adjusted p-value");
                writer.WriteComment("SupER:Supporting ER(s)");
                writer.WriteComment("file:file name");
                writer.WriteComment("Class:Classification of the ER (e.g., stringent-discarded, weak-discarded)");
                writer.WriteComment("SD:Stringent Discarded");
                writer.WriteComment("WD:Weak Discarded");
                writer.WriteComment("");
                writer.WriteComment("NOTE: p-values of enriched regions are formated as -10xLog10(p-value).");
                writer.WriteComment("");

                foreach (var chr in data.R_j__d)
                {
                    foreach (var er in chr.Value)
                    {
                        writer.WriteStartElement("ER");
                        writer.WriteAttributeString("chr", chr.Key);
                        writer.WriteAttributeString("start", er.Value.er.left.ToString());
                        writer.WriteAttributeString("stop", er.Value.er.right.ToString());
                        writer.WriteAttributeString("name", er.Value.er.metadata.name);
                        writer.WriteAttributeString("p-value", ConvertPValue(er.Value.er.metadata.value));

                        writer.WriteElementString("X-sqrd", er.Value.xSquared.ToString());
                        writer.WriteElementString("RTP", er.Value.rtp.ToString());
                        writer.WriteElementString("Reason", data.messages[er.Value.reason]);

                        if (er.Value.classification == ERClassificationType.StringentDiscarded)
                            writer.WriteElementString("Class", "SD");
                        else if (er.Value.classification == ERClassificationType.WeakDiscarded)
                            writer.WriteElementString("Class", "WD");

                        if (er.Value.supportingERs.Count > 0)
                        {
                            writer.WriteStartElement("SupER");

                            foreach (var supER in er.Value.supportingERs)
                            {
                                writer.WriteStartElement("ER");
                                writer.WriteAttributeString("chr", chr.Key);
                                writer.WriteAttributeString("start", supER.er.left.ToString());
                                writer.WriteAttributeString("stop", supER.er.right.ToString());
                                writer.WriteAttributeString("name", supER.er.metadata.name);
                                writer.WriteAttributeString("p-value", ConvertPValue(supER.er.metadata.value));
                                writer.WriteAttributeString("file", Path.GetFileNameWithoutExtension(samples[supER.sampleIndex]));
                                writer.WriteEndElement();
                            }

                            writer.WriteEndElement();
                        }

                        writer.WriteEndElement();
                        writer.Flush();
                    }
                }

                writer.WriteEndElement();
                writer.WriteEndDocument();
            }
        }
        private string ConvertPValue(double pValue)
        {
            if (pValue != 0)
                return (Math.Round((-10.0) * Math.Log10(pValue), 3)).ToString();
            return "0";
        }
    }
}
