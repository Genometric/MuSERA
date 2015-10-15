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

namespace Polimi.DEIB.VahidJalili.MuSERA.ImportData
{
    internal struct ParserOptions
    {
        public Genomes genome { set; get; }
        public Assemblies assembly { set; get; }
        public bool readOnlyValidChrs { set; get; }
        public byte startOffset { set; get; }
        public byte chrColumn { set; get; }
        public byte leftColumn { set; get; }
        public byte rightColumn { set; get; }
        public byte nameColumn { set; get; }
        public byte pValueColumn { set; get; }
        public byte refseqIDColum { set; get; }
        public byte geneSymbolColumn { set; get; }
        public byte featureColumn { set; get; }
        public byte attributeColumn { set; get; }
        public sbyte summitColumn { set; get; }
        public sbyte strandColumn { set; get; }
        public double defaultpValue { set; get; }
        public pValueFormat pValueConversion { set; get; }
        public bool dropIfNopValue { set; get; }
        public InputType inputType { set; get; }
        public enum InputType { ChIPseqAssays, RefseqGene, GeneralFeatures };
    }
}
