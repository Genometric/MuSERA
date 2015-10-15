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
using Polimi.DEIB.VahidJalili.MuSERA.ImportData;
using Polimi.DEIB.VahidJalili.MuSERA.Models;
using Polimi.DEIB.VahidJalili.MuSERA.Warehouse;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Xml;

namespace Polimi.DEIB.VahidJalili.MuSERA.Functions.BatchMode
{
    internal class AtJobParser
    {
        public AtJobParser(string file)
        {
            _file = file;
        }

        private string _file { set; get; }

        public BatchOptions Parse()
        {
            var rtv = new BatchOptions();
            var document = new XmlDocument();
            document.Load(_file);

            try
            {
                foreach (XmlNode childNode in document.DocumentElement.ChildNodes)
                {
                    switch (childNode.Name)
                    {
                        case "Plot":
                            rtv.plotOptions = ParsePlotInformation(childNode);
                            break;

                        case "LogFile":
                            rtv.logFile = ParseLogFileInformation(childNode);
                            break;

                        case "Session":
                            // A session should have a title.
                            if (childNode.Attributes == null || childNode.Attributes.Count == 0) continue;
                            if (rtv.sessions == null) rtv.sessions = new List<AtJobSession>();
                            rtv.sessions.Add(ParseLoadAndAnalysisInformation(childNode));
                            break;
                    }
                }

                if (rtv.plotOptions == null || rtv.logFile == null || rtv.sessions == null)
                    throw new Exception();

                return rtv;
            }
            catch (Exception e)
            {
                MessageBox.Show("The provided at-Job file (i.e., " + Path.GetFileNameWithoutExtension(_file) + " ) contains errors." +
                    "\nPlease check if the file is correctly structured, and \"Value\"s are compliant with corresponding \"Property\"." +
                    "\nAlternatively, you may use the provided template to create your desired at-job in correct format." +
                    "\nPlease retry having validated the file structure",
                    "Invalid XML structure", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }
        private BatchPlotOptions ParsePlotInformation(XmlNode parent)
        {
            var rtv = new BatchPlotOptions();
            foreach (XmlNode childNode in parent.ChildNodes)
            {
                if (childNode.Attributes == null || childNode.Attributes.Count == 0) continue;
                switch (childNode.Attributes[0].Value)
                {
                    case "Width":
                        rtv.plotWidth = Convert.ToInt32(childNode.Attributes[1].Value);
                        break;

                    case "Height":
                        rtv.plotHeight = Convert.ToInt32(childNode.Attributes[1].Value);
                        break;

                    case "Marker Size":
                        rtv.markerSize = Convert.ToInt32(childNode.Attributes[1].Value);
                        break;

                    case "Font Size":
                        rtv.fontSize = Convert.ToInt32(childNode.Attributes[1].Value);
                        break;

                    case "Axis Font Size":
                        rtv.axisFontSize = Convert.ToInt32(childNode.Attributes[1].Value);
                        break;

                    case "Data Label Font Size":
                        rtv.actualDataLabelFontSize = Convert.ToInt32(childNode.Attributes[1].Value);
                        break;

                    case "Header font size":
                        rtv.headerFontSize = Convert.ToInt32(childNode.Attributes[1].Value);
                        break;

                    case "Save Path":
                        rtv.savePath = childNode.Attributes[1].Value;
                        break;

                    case "Overview":
                        switch (childNode.Attributes[1].Value)
                        {
                            case "Enabled":
                                rtv.saveOverview = true;
                                break;

                            case "Disabled":
                                rtv.saveOverview = false;
                                break;
                        }
                        break;
                }
            }
            return rtv;
        }
        private string ParseLogFileInformation(XmlNode parent)
        {
            foreach (XmlNode childNode in parent.ChildNodes)
            {
                if (childNode.Attributes == null || childNode.Attributes.Count == 0) continue;
                switch (childNode.Attributes[0].Value)
                {
                    case "Path":
                        return Convert.ToString(childNode.Attributes[1].Value);
                }
            }
            return "";
        }
        private AtJobSession ParseLoadAndAnalysisInformation(XmlNode parent)
        {
            bool processTGamma = false;
            double gammaIsDetermined = 0.0;
            var session = new AtJobSession();
            session.analysisOptions = new AnalysisOptions();
            session.parserParameters = new ParserOptions();
            session.title = parent.Attributes[0].Value;

            foreach (XmlNode childNode in parent.ChildNodes)
            {
                switch (childNode.Name)
                {
                    case "Load_and_Save_Parameters":
                        #region .::.     Load and Save Parameters        .::.

                        bool Export_R_j__o_BED = false;
                        bool Export_R_j__o_XML = false;
                        bool Export_R_j__s_BED = false;
                        bool Export_R_j__w_BED = false;
                        bool Export_R_j__b_BED = false;
                        bool Export_R_j__c_BED = false;
                        bool Export_R_j__c_XML = false;
                        bool Export_R_j__d_BED = false;
                        bool Export_R_j__d_XML = false;
                        bool Export_Chromosomewide_stats = false;

                        foreach (XmlNode setter in childNode)
                        {
                            if (setter.Attributes == null || setter.Attributes.Count == 0) continue;
                            switch (setter.Attributes[0].Value)
                            {
                                case "Input Sample":
                                    session.samples.Add(setter.Attributes[1].Value);
                                    break;

                                case "Output Path":
                                    session.outputPath = setter.Attributes[1].Value;
                                    break;

                                case "Export Output Set (BED)":
                                    if (!Boolean.TryParse(setter.Attributes[1].Value, out Export_R_j__o_BED))
                                        throw new Exception();
                                    break;

                                case "Export Output Set (XML)":
                                    if (!Boolean.TryParse(setter.Attributes[1].Value, out Export_R_j__o_XML))
                                        throw new Exception();
                                    break;

                                case "Export Stringent ERs":
                                    if (!Boolean.TryParse(setter.Attributes[1].Value, out Export_R_j__s_BED))
                                        throw new Exception();
                                    break;

                                case "Export Weak ERs":
                                    if (!Boolean.TryParse(setter.Attributes[1].Value, out Export_R_j__w_BED))
                                        throw new Exception();
                                    break;

                                case "Export Background ERs":
                                    if (!Boolean.TryParse(setter.Attributes[1].Value, out Export_R_j__b_BED))
                                        throw new Exception();
                                    break;

                                case "Export Confirmed ERs (BED)":
                                    if (!Boolean.TryParse(setter.Attributes[1].Value, out Export_R_j__c_BED))
                                        throw new Exception();
                                    break;

                                case "Export Confirmed ERs (XML)":
                                    if (!Boolean.TryParse(setter.Attributes[1].Value, out Export_R_j__c_XML))
                                        throw new Exception();
                                    break;

                                case "Export Discarded ERs (BED)":
                                    if (!Boolean.TryParse(setter.Attributes[1].Value, out Export_R_j__d_BED))
                                        throw new Exception();
                                    break;

                                case "Export Discarded ERs (XML)":
                                    if (!Boolean.TryParse(setter.Attributes[1].Value, out Export_R_j__d_XML))
                                        throw new Exception();
                                    break;

                                case "Export Chromosome-wide statistics":
                                    if (!Boolean.TryParse(setter.Attributes[1].Value, out Export_Chromosomewide_stats))
                                        throw new Exception();
                                    break;
                            }
                        }

                        session.exportOptions = new Polimi.DEIB.VahidJalili.MuSERA.Exporter.ExportOptions(
                            sessionPath: session.outputPath,
                            includeBEDHeader: true,
                            Export_R_j__o_BED: Export_R_j__o_BED,
                            Export_R_j__o_XML: Export_R_j__o_XML,
                            Export_R_j__s_BED: Export_R_j__s_BED,
                            Export_R_j__w_BED: Export_R_j__w_BED,
                            Export_R_j__b_BED: Export_R_j__b_BED,
                            Export_R_j__c_BED: Export_R_j__c_BED,
                            Export_R_j__c_XML: Export_R_j__c_XML,
                            Export_R_j__d_BED: Export_R_j__d_BED,
                            Export_R_j__d_XML: Export_R_j__d_XML,
                            Export_Chromosomewide_stats: Export_Chromosomewide_stats);

                        #endregion
                        break;

                    case "Analysis_Parameters":
                        #region .::.     Analysis Parameters              .::.

                        foreach (XmlNode setter in childNode)
                        {
                            if (setter.Attributes == null || setter.Attributes.Count == 0) continue;
                            switch (setter.Attributes[0].Value)
                            {
                                case "Replicate Type":
                                    if (setter.Attributes[1].Value.ToLower() == "biological")
                                        session.analysisOptions.replicateType = ReplicateType.Biological;
                                    else if (setter.Attributes[1].Value.ToLower() == "technical")
                                        session.analysisOptions.replicateType = ReplicateType.Technical;
                                    break;

                                case "TauS":
                                    session.analysisOptions.tauS = Double.Parse(setter.Attributes[1].Value);
                                    break;

                                case "TauW":
                                    session.analysisOptions.tauW = Double.Parse(setter.Attributes[1].Value);
                                    break;

                                case "Gamma":
                                    gammaIsDetermined = Double.Parse(setter.Attributes[1].Value);
                                    processTGamma = true;
                                    break;

                                case "Alpha":
                                    session.analysisOptions.alpha = float.Parse(setter.Attributes[1].Value);
                                    break;

                                case "C":
                                    session.analysisOptions.C = Byte.Parse(setter.Attributes[1].Value);
                                    break;

                                case "Multiple Testing Correction":
                                    switch (setter.Attributes[1].Value)
                                    {
                                        case "BH FDR":
                                            session.analysisOptions.fDRProcedure = FDRProcedure.BenjaminiHochberg;
                                            break;
                                    }
                                    break;
                            }
                        }

                        #endregion
                        break;

                    case "BED_Parser_Parameters":
                        #region .::.     BED Parser Parameters            .::.
                        var parserParameters = new ParserOptions();
                        foreach (XmlNode setter in childNode)
                        {
                            if (setter.Attributes == null || setter.Attributes.Count == 0) continue;
                            switch (setter.Attributes[0].Value)
                            {
                                case "Start Offset":
                                    parserParameters.startOffset = Byte.Parse(setter.Attributes[1].Value);
                                    break;

                                case "Chr Column":
                                    parserParameters.chrColumn = Byte.Parse(setter.Attributes[1].Value);
                                    break;

                                case "Start Column":
                                    parserParameters.leftColumn = Byte.Parse(setter.Attributes[1].Value);
                                    break;

                                case "Stop Column":
                                    parserParameters.rightColumn = Byte.Parse(setter.Attributes[1].Value);
                                    break;

                                case "Name Column":
                                    parserParameters.nameColumn = Byte.Parse(setter.Attributes[1].Value);
                                    break;

                                case "p-value Column":
                                    parserParameters.pValueColumn = Byte.Parse(setter.Attributes[1].Value);
                                    break;

                                case "Drop Line if no p-value":
                                    parserParameters.dropIfNopValue = Boolean.Parse(setter.Attributes[1].Value);
                                    break;

                                case "Default p-value":
                                    parserParameters.defaultpValue = Double.Parse(setter.Attributes[1].Value);
                                    break;

                                case "p-value Convertion Option":
                                    switch (setter.Attributes[1].Value.Trim().ToLower())
                                    {
                                        case "-10 x log10 (p-value)":
                                        case "-10 * log10 (p-value)":
                                        case "-10xlog10(p-value)":
                                        case "-10*log10(p-value)":
                                            parserParameters.pValueConversion = pValueFormat.minus10_Log10_pValue;
                                            break;

                                        case "-1 x log10 (p-value)":
                                        case "-1 * log10 (p-value)":
                                        case "-1xlog10(p-value)":
                                        case "-1*log10(p-value)":
                                            parserParameters.pValueConversion = pValueFormat.minus1_Log10_pValue;
                                            break;

                                        default: // No convertion.
                                            parserParameters.pValueConversion = pValueFormat.SameAsInput;
                                            break;
                                    }
                                    break;
                            }
                        }

                        session.parserParameters = parserParameters;

                        #endregion
                        break;
                }
            }

            if (processTGamma == true)
            {
                if (gammaIsDetermined < 1.0 && gammaIsDetermined >= 0.0 && gammaIsDetermined > 1E-20)
                    session.analysisOptions.gamma = gammaIsDetermined;// Don't modify "gammaIsDetermined"                    
                else
                    gammaIsDetermined = 1E-19;
            }
            return session;
        }

        public void WriteSampleAtJob()
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.ConformanceLevel = ConformanceLevel.Document;
            settings.Encoding = Encoding.UTF8;
            settings.Indent = true;
            settings.IndentChars = "\t";
            settings.CloseOutput = true;
            settings.NewLineOnAttributes = false;
            settings.NewLineChars = "\n";

            var desktopFolder = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            var fullFileName = Path.Combine(desktopFolder, "MuSERA batch-mode sample at-Job.xml");
            if (File.Exists(fullFileName)) File.Delete(fullFileName);
            var fileStream = new FileStream(fullFileName, FileMode.Create);
            using (XmlWriter writer = XmlWriter.Create(fileStream, settings))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("atJob", "Polimi.DEIB.VahidJalili.MuSERA");
                writer.WriteComment("Note: Labels are case sensitive.");

                writer.WriteStartElement("Plot");
                #region .::.   Width                            .::.
                writer.WriteStartElement("Setter");
                writer.WriteAttributeString("Property", "Width");
                writer.WriteAttributeString("Value", "3500");
                writer.WriteEndElement();
                #endregion
                #region .::.   Height                           .::.
                writer.WriteStartElement("Setter");
                writer.WriteAttributeString("Property", "Height");
                writer.WriteAttributeString("Value", "1500");
                writer.WriteEndElement();
                #endregion
                #region .::.   Marker Size                      .::.
                writer.WriteStartElement("Setter");
                writer.WriteAttributeString("Property", "Marker Size");
                writer.WriteAttributeString("Value", "24");
                writer.WriteEndElement();
                #endregion
                #region .::.   Font Size                        .::.
                writer.WriteStartElement("Setter");
                writer.WriteAttributeString("Property", "Font Size");
                writer.WriteAttributeString("Value", "24");
                writer.WriteEndElement();
                #endregion
                #region .::.   Axis Font Size                   .::.
                writer.WriteStartElement("Setter");
                writer.WriteAttributeString("Property", "Axis Font Size");
                writer.WriteAttributeString("Value", "32");
                writer.WriteEndElement();
                #endregion
                #region .::.   Data Label Font Size             .::.
                writer.WriteStartElement("Setter");
                writer.WriteAttributeString("Property", "Data Label Font Size");
                writer.WriteAttributeString("Value", "24");
                writer.WriteEndElement();
                #endregion
                #region .::.   Header font size                 .::.
                writer.WriteStartElement("Setter");
                writer.WriteAttributeString("Property", "Header font size");
                writer.WriteAttributeString("Value", "40");
                writer.WriteEndElement();
                #endregion
                #region .::.   Save Path                        .::.
                writer.WriteStartElement("Setter");
                writer.WriteAttributeString("Property", "Save Path");
                writer.WriteAttributeString("Value", "E:\\myStudies\\");
                writer.WriteEndElement();
                #endregion
                #region .::.   Overview                         .::.
                writer.WriteStartElement("Setter");
                writer.WriteAttributeString("Property", "Overview");
                writer.WriteAttributeString("Value", "Enabled");
                writer.WriteEndElement();
                writer.WriteComment("Enables/Disables plotting overview for each analysis session.");
                #endregion
                writer.WriteEndElement();
                #region .::.   Log File                         .::.
                writer.WriteStartElement("LogFile");
                writer.WriteStartElement("Setter");
                writer.WriteAttributeString("Property", "Path");
                writer.WriteAttributeString("Value", "E:\\myStudies\\");
                writer.WriteEndElement();
                writer.WriteEndElement();
                #endregion
                #region .::.   Session 01                       .::.
                writer.WriteStartElement("Session");
                writer.WriteAttributeString("Title", "Session_01");
                writer.WriteStartElement("Load_and_Save_Parameters");
                #region .::.   Input Sample                     .::.
                writer.WriteStartElement("Setter");
                writer.WriteAttributeString("Property", "Input Sample");
                writer.WriteAttributeString("Value", "E:\\mySamples\\K562CmycAlnRep1.BED");
                writer.WriteEndElement();
                #endregion
                #region .::.   Input Sample                     .::.
                writer.WriteStartElement("Setter");
                writer.WriteAttributeString("Property", "Input Sample");
                writer.WriteAttributeString("Value", "E:\\mySamples\\K562CmycAlnRep2.BED");
                writer.WriteEndElement();
                #endregion
                #region .::.   Output Path                      .::.
                writer.WriteStartElement("Setter");
                writer.WriteAttributeString("Property", "Output Path");
                writer.WriteAttributeString("Value", "E:\\myStudies\\");
                writer.WriteEndElement();
                #endregion
                #region .::.   Export Output Set (BED)          .::.
                writer.WriteStartElement("Setter");
                writer.WriteAttributeString("Property", "Export Output Set (BED)");
                writer.WriteAttributeString("Value", "true");
                writer.WriteEndElement();
                #endregion
                #region .::.   Export Output Set (XML)          .::.
                writer.WriteStartElement("Setter");
                writer.WriteAttributeString("Property", "Export Output Set (XML)");
                writer.WriteAttributeString("Value", "true");
                writer.WriteEndElement();
                #endregion
                #region .::.   Export Stringent ERs             .::.
                writer.WriteStartElement("Setter");
                writer.WriteAttributeString("Property", "Export Stringent ERs");
                writer.WriteAttributeString("Value", "true");
                writer.WriteEndElement();
                #endregion
                #region .::.   Export Weak ERs                  .::.
                writer.WriteStartElement("Setter");
                writer.WriteAttributeString("Property", "Export Weak ERs");
                writer.WriteAttributeString("Value", "true");
                writer.WriteEndElement();
                #endregion
                #region .::.   Export Background ERs            .::.
                writer.WriteStartElement("Setter");
                writer.WriteAttributeString("Property", "Export Background ERs");
                writer.WriteAttributeString("Value", "true");
                writer.WriteEndElement();
                #endregion
                #region .::.   Export Confirmed ERs (BED)       .::.
                writer.WriteStartElement("Setter");
                writer.WriteAttributeString("Property", "Export Confirmed ERs (BED)");
                writer.WriteAttributeString("Value", "true");
                writer.WriteEndElement();
                #endregion
                #region .::.   Export Confirmed ERs (XML)       .::.
                writer.WriteStartElement("Setter");
                writer.WriteAttributeString("Property", "Export Confirmed ERs (XML)");
                writer.WriteAttributeString("Value", "true");
                writer.WriteEndElement();
                #endregion
                #region .::.   Export Discarded ERs (BED)       .::.
                writer.WriteStartElement("Setter");
                writer.WriteAttributeString("Property", "Export Discarded ERs (BED)");
                writer.WriteAttributeString("Value", "true");
                writer.WriteEndElement();
                #endregion
                #region .::.   Export Discarded ERs (XML)       .::.
                writer.WriteStartElement("Setter");
                writer.WriteAttributeString("Property", "Export Discarded ERs (XML)");
                writer.WriteAttributeString("Value", "true");
                writer.WriteEndElement();
                #endregion
                #region .::.   Export Chromosome-wide statistics.::.
                writer.WriteStartElement("Setter");
                writer.WriteAttributeString("Property", "Export Chromosome-wide statistics");
                writer.WriteAttributeString("Value", "true");
                writer.WriteEndElement();
                #endregion
                writer.WriteEndElement();

                writer.WriteStartElement("Analysis_Parameters");
                #region .::.   Replicate Type                   .::.
                writer.WriteStartElement("Setter");
                writer.WriteAttributeString("Property", "Replicate Type");
                writer.WriteAttributeString("Value", "biological");
                writer.WriteEndElement();
                writer.WriteComment("possible values are (without square bracket): [biological] or [technical]");
                #endregion
                #region .::.   TauS                             .::.
                writer.WriteStartElement("Setter");
                writer.WriteAttributeString("Property", "TauS");
                writer.WriteAttributeString("Value", "1.00E-8");
                writer.WriteEndElement();
                #endregion
                #region .::.   TauW                             .::.
                writer.WriteStartElement("Setter");
                writer.WriteAttributeString("Property", "TauW");
                writer.WriteAttributeString("Value", "1.00E-2");
                writer.WriteEndElement();
                #endregion
                #region .::.   Gamma                            .::.
                writer.WriteStartElement("Setter");
                writer.WriteAttributeString("Property", "Gamma");
                writer.WriteAttributeString("Value", "1.00E-8");
                writer.WriteEndElement();
                #endregion
                #region .::.   Alpha                            .::.
                writer.WriteStartElement("Setter");
                writer.WriteAttributeString("Property", "Alpha");
                writer.WriteAttributeString("Value", "0.05");
                writer.WriteEndElement();
                #endregion
                #region .::.   C                                .::.
                writer.WriteStartElement("Setter");
                writer.WriteAttributeString("Property", "C");
                writer.WriteAttributeString("Value", "2");
                writer.WriteEndElement();
                #endregion
                #region .::.   Multiple Testing Correction      .::.
                writer.WriteStartElement("Setter");
                writer.WriteAttributeString("Property", "Multiple Testing Correction");
                writer.WriteAttributeString("Value", "BH FDR");
                writer.WriteEndElement();
                #endregion
                writer.WriteEndElement();

                writer.WriteStartElement("BED_Parser_Parameters");
                #region .::.   Start Offset                     .::.
                writer.WriteStartElement("Setter");
                writer.WriteAttributeString("Property", "Start Offset");
                writer.WriteAttributeString("Value", "1");
                writer.WriteEndElement();
                #endregion
                #region .::.   Chr Column                       .::.
                writer.WriteStartElement("Setter");
                writer.WriteAttributeString("Property", "Chr Column");
                writer.WriteAttributeString("Value", "0");
                writer.WriteEndElement();
                #endregion
                #region .::.   Start Column                     .::.
                writer.WriteStartElement("Setter");
                writer.WriteAttributeString("Property", "Start Column");
                writer.WriteAttributeString("Value", "1");
                writer.WriteEndElement();
                #endregion
                #region .::.   Stop Column                      .::.
                writer.WriteStartElement("Setter");
                writer.WriteAttributeString("Property", "Stop Column");
                writer.WriteAttributeString("Value", "2");
                writer.WriteEndElement();
                #endregion
                #region .::.   Name Column                      .::.
                writer.WriteStartElement("Setter");
                writer.WriteAttributeString("Property", "Name Column");
                writer.WriteAttributeString("Value", "3");
                writer.WriteEndElement();
                #endregion
                #region .::.   p-value Column                   .::.
                writer.WriteStartElement("Setter");
                writer.WriteAttributeString("Property", "p-value Column");
                writer.WriteAttributeString("Value", "4");
                writer.WriteEndElement();
                #endregion
                #region .::.   Drop Line if no p-value          .::.
                writer.WriteStartElement("Setter");
                writer.WriteAttributeString("Property", "Drop Line if no p-value");
                writer.WriteAttributeString("Value", "true");
                writer.WriteEndElement();
                #endregion
                #region .::.   Default p-value                  .::.
                writer.WriteStartElement("Setter");
                writer.WriteAttributeString("Property", "Default p-value");
                writer.WriteAttributeString("Value", "1.00E-6");
                writer.WriteEndElement();
                #endregion
                #region .::.   p-value Convertion Option        .::.
                writer.WriteStartElement("Setter");
                writer.WriteAttributeString("Property", "p-value Convertion Option");
                writer.WriteAttributeString("Value", "-1 x Log10 (p-value)");
                writer.WriteEndElement();
                writer.WriteComment("Possible values are (without square bracket):");
                writer.WriteComment("1: none");
                writer.WriteComment("2: [-1 x log10 (p-value)], or [-1 * log10 (p-value)], or [-1xlog10(p-value)], or [-1*log10(p-value)]");
                writer.WriteComment("3: [-10 x log10 (p-value)], or [-10 * log10 (p-value)], or [-10xlog10(p-value)], or [-10*log10(p-value)]");
                #endregion
                writer.WriteEndElement();
                writer.WriteEndElement();
                #endregion
                #region .::.   Session 02                       .::.
                writer.WriteStartElement("Session");
                writer.WriteAttributeString("Title", "mySecondSession");
                writer.WriteStartElement("Load_and_Save_Parameters");
                #region .::.   Input Sample                     .::.
                writer.WriteStartElement("Setter");
                writer.WriteAttributeString("Property", "Input Sample");
                writer.WriteAttributeString("Value", "E:\\ENCODE Samples\\K562CmycIfng30StdAlnRep1.BED");
                writer.WriteEndElement();
                #endregion
                #region .::.   Input Sample                     .::.
                writer.WriteStartElement("Setter");
                writer.WriteAttributeString("Property", "Input Sample");
                writer.WriteAttributeString("Value", "E:\\ENCODE Samples\\K562CmycIfng30StdAlnRep2.BED");
                writer.WriteEndElement();
                #endregion
                #region .::.   Input Sample                     .::.
                writer.WriteStartElement("Setter");
                writer.WriteAttributeString("Property", "Input Sample");
                writer.WriteAttributeString("Value", "E:\\ENCODE Samples\\K562CmycIfng30StdAlnRep3.BED");
                writer.WriteEndElement();
                #endregion
                #region .::.   Output Path                      .::.
                writer.WriteStartElement("Setter");
                writer.WriteAttributeString("Property", "Output Path");
                writer.WriteAttributeString("Value", "E:\\myStudies\\");
                writer.WriteEndElement();
                #endregion
                #region .::.   Export Output Set (BED)          .::.
                writer.WriteStartElement("Setter");
                writer.WriteAttributeString("Property", "Export Output Set (BED)");
                writer.WriteAttributeString("Value", "true");
                writer.WriteEndElement();
                #endregion
                #region .::.   Export Output Set (XML)          .::.
                writer.WriteStartElement("Setter");
                writer.WriteAttributeString("Property", "Export Output Set (XML)");
                writer.WriteAttributeString("Value", "true");
                writer.WriteEndElement();
                #endregion
                #region .::.   Export Stringent ERs             .::.
                writer.WriteStartElement("Setter");
                writer.WriteAttributeString("Property", "Export Stringent ERs");
                writer.WriteAttributeString("Value", "true");
                writer.WriteEndElement();
                #endregion
                #region .::.   Export Weak ERs                  .::.
                writer.WriteStartElement("Setter");
                writer.WriteAttributeString("Property", "Export Weak ERs");
                writer.WriteAttributeString("Value", "true");
                writer.WriteEndElement();
                #endregion
                #region .::.   Export Background ERs            .::.
                writer.WriteStartElement("Setter");
                writer.WriteAttributeString("Property", "Export Background ERs");
                writer.WriteAttributeString("Value", "true");
                writer.WriteEndElement();
                #endregion
                #region .::.   Export Confirmed ERs (BED)       .::.
                writer.WriteStartElement("Setter");
                writer.WriteAttributeString("Property", "Export Confirmed ERs (BED)");
                writer.WriteAttributeString("Value", "true");
                writer.WriteEndElement();
                #endregion
                #region .::.   Export Confirmed ERs (XML)       .::.
                writer.WriteStartElement("Setter");
                writer.WriteAttributeString("Property", "Export Confirmed ERs (XML)");
                writer.WriteAttributeString("Value", "true");
                writer.WriteEndElement();
                #endregion
                #region .::.   Export Discarded ERs (BED)       .::.
                writer.WriteStartElement("Setter");
                writer.WriteAttributeString("Property", "Export Discarded ERs (BED)");
                writer.WriteAttributeString("Value", "true");
                writer.WriteEndElement();
                #endregion
                #region .::.   Export Discarded ERs (XML)       .::.
                writer.WriteStartElement("Setter");
                writer.WriteAttributeString("Property", "Export Discarded ERs (XML)");
                writer.WriteAttributeString("Value", "true");
                writer.WriteEndElement();
                #endregion
                #region .::.   Export Chromosome-wide statistics.::.
                writer.WriteStartElement("Setter");
                writer.WriteAttributeString("Property", "Export Chromosome-wide statistics");
                writer.WriteAttributeString("Value", "true");
                writer.WriteEndElement();
                #endregion
                writer.WriteEndElement();

                writer.WriteStartElement("Analysis_Parameters");
                #region .::.   Replicate Type                   .::.
                writer.WriteStartElement("Setter");
                writer.WriteAttributeString("Property", "Replicate Type");
                writer.WriteAttributeString("Value", "technical");
                writer.WriteEndElement();
                writer.WriteComment("possible values are (without square bracket): [biological] or [technical]");
                #endregion
                #region .::.   TauS                             .::.
                writer.WriteStartElement("Setter");
                writer.WriteAttributeString("Property", "TauS");
                writer.WriteAttributeString("Value", "1.00E-12");
                writer.WriteEndElement();
                #endregion
                #region .::.   TauW                             .::.
                writer.WriteStartElement("Setter");
                writer.WriteAttributeString("Property", "TauW");
                writer.WriteAttributeString("Value", "1.00E-4");
                writer.WriteEndElement();
                #endregion
                #region .::.   Gamma                            .::.
                writer.WriteStartElement("Setter");
                writer.WriteAttributeString("Property", "Gamma");
                writer.WriteAttributeString("Value", "1.00E-12");
                writer.WriteEndElement();
                #endregion
                #region .::.   Alpha                            .::.
                writer.WriteStartElement("Setter");
                writer.WriteAttributeString("Property", "Alpha");
                writer.WriteAttributeString("Value", "0.05");
                writer.WriteEndElement();
                #endregion
                #region .::.   C                                .::.
                writer.WriteStartElement("Setter");
                writer.WriteAttributeString("Property", "C");
                writer.WriteAttributeString("Value", "3");
                writer.WriteEndElement();
                #endregion
                #region .::.   Multiple Testing Correction      .::.
                writer.WriteStartElement("Setter");
                writer.WriteAttributeString("Property", "Multiple Testing Correction");
                writer.WriteAttributeString("Value", "BH FDR");
                writer.WriteEndElement();
                #endregion
                writer.WriteEndElement();

                writer.WriteStartElement("BED_Parser_Parameters");
                #region .::.   Start Offset                     .::.
                writer.WriteStartElement("Setter");
                writer.WriteAttributeString("Property", "Start Offset");
                writer.WriteAttributeString("Value", "0");
                writer.WriteEndElement();
                #endregion
                #region .::.   Chr Column                       .::.
                writer.WriteStartElement("Setter");
                writer.WriteAttributeString("Property", "Chr Column");
                writer.WriteAttributeString("Value", "0");
                writer.WriteEndElement();
                #endregion
                #region .::.   Start Column                     .::.
                writer.WriteStartElement("Setter");
                writer.WriteAttributeString("Property", "Start Column");
                writer.WriteAttributeString("Value", "1");
                writer.WriteEndElement();
                #endregion
                #region .::.   Stop Column                      .::.
                writer.WriteStartElement("Setter");
                writer.WriteAttributeString("Property", "Stop Column");
                writer.WriteAttributeString("Value", "2");
                writer.WriteEndElement();
                #endregion
                #region .::.   Name Column                      .::.
                writer.WriteStartElement("Setter");
                writer.WriteAttributeString("Property", "Name Column");
                writer.WriteAttributeString("Value", "3");
                writer.WriteEndElement();
                #endregion
                #region .::.   p-value Column                   .::.
                writer.WriteStartElement("Setter");
                writer.WriteAttributeString("Property", "p-value Column");
                writer.WriteAttributeString("Value", "6");
                writer.WriteEndElement();
                #endregion
                #region .::.   Drop Line if no p-value          .::.
                writer.WriteStartElement("Setter");
                writer.WriteAttributeString("Property", "Drop Line if no p-value");
                writer.WriteAttributeString("Value", "false");
                writer.WriteEndElement();
                #endregion
                #region .::.   Default p-value                  .::.
                writer.WriteStartElement("Setter");
                writer.WriteAttributeString("Property", "Default p-value");
                writer.WriteAttributeString("Value", "1.00E-65");
                writer.WriteEndElement();
                #endregion
                #region .::.   p-value Convertion Option        .::.
                writer.WriteStartElement("Setter");
                writer.WriteAttributeString("Property", "p-value Convertion Option");
                writer.WriteAttributeString("Value", "-10 x Log10 (p-value)");
                writer.WriteEndElement();
                writer.WriteComment("Possible values are (without square bracket):");
                writer.WriteComment("1: none");
                writer.WriteComment("2: [-1 x log10 (p-value)], or [-1 * log10 (p-value)], or [-1xlog10(p-value)], or [-1*log10(p-value)]");
                writer.WriteComment("3: [-10 x log10 (p-value)], or [-10 * log10 (p-value)], or [-10xlog10(p-value)], or [-10*log10(p-value)]");
                #endregion
                writer.WriteEndElement();
                writer.WriteEndElement();
                #endregion

                writer.Flush();
                writer.WriteEndElement();
                writer.WriteEndDocument();
            }
        }
    }
}
