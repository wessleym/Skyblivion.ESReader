using Skyblivion.ESReader.Extensions.IDictionaryExtensions;
using Skyblivion.ESReader.Extensions.StreamExtensions;
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
        private TES4File placedFile;
        private int formid;
        private Nullable<int> expandedFormid;
        private int flags;
        private int size;
        private TES4RecordType type;
        private Dictionary<string, List<string>> data = new Dictionary<string, List<string>>();
        private Dictionary<string, int> dataAsFormidCache = new Dictionary<string, int>();
        /*
        * TES4LoadedRecord constructor.
        */
        public TES4LoadedRecord(TES4File placedFile, TES4RecordType type, int formid, int size, int flags)
        {
            this.placedFile = placedFile;
            this.type = type;
            this.formid = formid;
            this.size = size;
            this.flags = flags;
        }

        public TES4RecordType getType()
        {
            return this.type;
        }

        public List<string> getSubrecords(string type)
        {
            return this.data.GetWithFallback(type, () => new List<string>());
        }

        public string getSubrecord(string type)
        {
            List<string> list;
            if (!this.data.TryGetValue(type, out list) || !this.data[type].Any()) { return null; }
            return list[0];
        }

        public string getSubrecordTrim(string type)
        {
            string subrecord = getSubrecord(type);
            if (subrecord == null) { return null; }
            string trimmed = subrecord.Trim('\0');
            if (trimmed.StartsWith(" ") || trimmed.EndsWith(" ")) { throw new InvalidOperationException("Value started or ended with space:" + Environment.NewLine + trimmed + Environment.NewLine + "string.Trim() should be called."); }
            return trimmed;
        }

        public string getSubrecordTrimLower(string type)
        {
            string subrecord = getSubrecordTrim(type);
            if (subrecord == null) { return null; }
            return subrecord.ToLower();
        }

        public Nullable<int> getSubrecordAsFormid(string type)
        {
            int value;
            if(this.dataAsFormidCache.TryGetValue(type, out value)) { return value; }
            string subrecord = this.getSubrecord(type);
            if (null == subrecord) { return null; }
            if (subrecord.Length < 4)
            {
                return null;
            }
            value = this.placedFile.expand(PHPFunction.UnpackV(subrecord.Substring(0, 4)));
            this.dataAsFormidCache.Add(type, value);
            return value;
        }

        public int getFormId()
        {
            if (this.expandedFormid == null)
            {
                this.expandedFormid = this.placedFile.expand(this.formid);
            }

            return this.expandedFormid.Value;
        }

        public void load(Stream file, TES4RecordLoadScheme scheme)
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
                if (scheme.shouldLoad(subrecordType))
                {
                    string subrecordData = TES4File.ISO_8859_1.Value.GetString(fileData, i + 6, subrecordSize);
                    this.data.AddNewListIfNotContainsKeyAndAddValueToList(subrecordType, subrecordData);
                }

                i += (subrecordSize + 6);
            }
        }
    }
}