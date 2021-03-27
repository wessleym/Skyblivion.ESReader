using Skyblivion.ESReader.Extensions;
using Skyblivion.ESReader.PHP;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Skyblivion.ESReader.TES4
{
    public class TES4LoadedRecord : ITES4Record
    {
        public const int RECORD_HEADER_SIZE = 20;
        private readonly TES4File placedFile;
        private readonly int formIDPrivate;
        private readonly int flags;
        private int size;
        public TES4RecordType RecordType { get; }
        private readonly Lazy<int> formIDLazy;
        public int FormID => formIDLazy.Value;
        private readonly List<KeyValuePair<string, byte[]>> data = new List<KeyValuePair<string, byte[]>>();
        private readonly Dictionary<string, int> dataAsFormidCache = new Dictionary<string, int>();
        /*
        * TES4LoadedRecord constructor.
        */
        public TES4LoadedRecord(TES4File placedFile, TES4RecordType type, int formid, int size, int flags)
        {
            this.placedFile = placedFile;
            this.RecordType = type;
            this.formIDPrivate = formid;
            this.size = size;
            this.flags = flags;
            formIDLazy = new Lazy<int>(() => this.placedFile.Expand(this.formIDPrivate));
        }

        public IEnumerable<byte[]> GetSubrecords(string type)
        {
            return this.data.Where(d => d.Key == type).Select(d => d.Value);
        }

        public IEnumerable<KeyValuePair<string, byte[]>> GetSubrecords(IList<string> types)
        {
            return this.data.Where(d => types.Contains(d.Key));
        }

        public IEnumerable<string> GetSubrecordsStrings(string type)
        {
            return GetSubrecords(type).Select(r => GetSubrecordString(r));
        }

        public byte[]? GetSubrecord(string type)
        {
            byte[]? subrecord = this.data.Where(d => d.Key == type).Select(d => d.Value).FirstOrDefault();
            if (subrecord != null) { return subrecord; }
            return null;
        }

        private static string GetSubrecordString(byte[] bytes)
        {
            return TES4File.ISO_8859_1.Value.GetString(bytes);
        }
        public string GetSubrecordString(string type)
        {
            byte[]? subrecord = GetSubrecord(type);
            if (subrecord == null) { throw new InvalidOperationException(nameof(type) + " " + type + " resulted in a null " + nameof(subrecord) + "."); }
            return GetSubrecordString(subrecord);
        }

        public string? GetSubrecordTrimNullable(string type)
        {
            byte[]? subrecord = GetSubrecord(type);
            if (subrecord == null) { return null; }
            string subrecordString = GetSubrecordString(subrecord);
            string trimmed = subrecordString.Trim('\0').Trim();
            return trimmed;
        }

        public string GetSubrecordTrim(string type)
        {
            string? trimmed = GetSubrecordTrimNullable(type);
            if (trimmed == null) { throw new InvalidOperationException(nameof(type) + " " + type + " resulted in null."); }
            return trimmed;
        }

        public string? GetSubrecordTrimLowerNullable(string type)
        {
            string? subrecord = GetSubrecordTrimNullable(type);
            if (subrecord == null) { return null; }
            return subrecord.ToLower();
        }

        public Nullable<int> GetSubrecordAsFormidNullable(string type)
        {
            int value;
            if (this.dataAsFormidCache.TryGetValue(type, out value)) { return value; }
            byte[]? subrecord = this.GetSubrecord(type);
            if (subrecord == null || subrecord.Length < 4) { return null; }
            value = ExpandBytesIntoFormID(subrecord.Take(4).ToArray());
            this.dataAsFormidCache.Add(type, value);
            return value;
        }
        public int GetSubrecordAsFormid(string type)
        {
            Nullable<int> formid = GetSubrecordAsFormidNullable(type);
            if (formid != null) { return formid.Value; }
            throw new InvalidOperationException(nameof(formid) + " was null for " + nameof(type) + " " + type + ".");
        }

        public int ExpandBytesIntoFormID(IList<byte> bytes)
        {
            return this.placedFile.Expand(PHPFunction.UnpackV(bytes));
        }

        public string? TryGetEditorID()
        {
            return GetSubrecordTrimNullable("EDID");
        }

        public string GetEditorID()
        {
            string? editorID = TryGetEditorID();
            if (editorID != null) { return editorID; }
            throw new InvalidOperationException(nameof(editorID) + " was null.");
        }

        public IEnumerable<byte[]> GetSCRORecords(Nullable<int> index)
        {
            const string indx = "INDX", scro = "SCRO";
            List<string> subrecordTypes = new List<string>();
            if (index != null) { subrecordTypes.Add(indx); }
            subrecordTypes.Add(scro);
            bool indexFound = false;
            foreach (var record in GetSubrecords(subrecordTypes))
            {
                if (index != null)
                {
                    bool isIndexRecord = record.Key == indx;
                    if (isIndexRecord)
                    {
                        if (!indexFound)
                        {
                            if (record.Value[0] == index) { indexFound = true; }
                        }
                        else
                        {
                            if (record.Value[0] != index) { yield break; }
                        }
                    }
                    if (!indexFound)
                    {
                        if (isIndexRecord && record.Value[0] == index) { indexFound = true; }
                        else { continue; }
                    }
                }
                if (record.Key == scro) { yield return record.Value; }
            }
        }

        public void Load(Stream file, TES4RecordLoadScheme scheme)
        {
            if (this.size == 0)
            {
                return;
            }

            byte[] fileData = file.Read(this.size);
            //Decompression
            if ((this.flags & 0x00040000) == 0x00040000)
            {
                //Skip the uncompressed data size
                this.size = PHPFunction.UnpackV(fileData.Take(4).ToArray());
                fileData = PHPFunction.GZUncompress(fileData.Skip(4).ToArray());
            }

            int i = 0;
            while (i < this.size)
            {
                string subrecordType = TES4File.ISO_8859_1.Value.GetString(fileData, i, 4);
                int subrecordSize = PHPFunction.UnpackV(fileData.Skip(i + 4).Take(2).ToArray());
                if (scheme.ShouldLoad(subrecordType))
                {
                    byte[] subrecordData = fileData.Skip(i + 6).Take(subrecordSize).ToArray();
                    this.data.Add(new KeyValuePair<string, byte[]>(subrecordType, subrecordData));
                }

                i += (subrecordSize + 6);
            }
        }
    }
}