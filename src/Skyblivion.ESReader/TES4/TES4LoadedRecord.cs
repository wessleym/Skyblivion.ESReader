using Skyblivion.ESReader.PHP;
using Skyblivion.ESReader.QueueExtensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Skyblivion.ESReader.TES4
{
    class TES4LoadedRecord : ITES4Record
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
            if (!this.data.ContainsKey(type))
            {
                return new List<string>();
            }

            return this.data[type];
        }

        public string getSubrecord(string type)
        {
            if (!this.data.ContainsKey(type) || !this.data[type].Any()) { return null; }
            return this.data[type][0];
        }

        public Nullable<int> getSubrecordAsFormid(string type)
        {
            if (!this.dataAsFormidCache.ContainsKey(type))
            {
                string subrecord = this.getSubrecord(type);
                if (null == subrecord) { return null; }
                if (subrecord.Length < 4)
                {
                    return null;
                }
                this.dataAsFormidCache[type] = this.placedFile.expand(int.Parse(subrecord.Substring(0, 4)));
            }
            return this.dataAsFormidCache[type];
        }

        public int getFormId()
        {
            if (this.expandedFormid == null)
            {
                this.expandedFormid = this.placedFile.expand(this.formid);
            }

            return this.expandedFormid.Value;
        }

        public void load(Queue<char> file, TES4RecordLoadScheme scheme)
        {
            if (this.size == 0)
            {
                return;
            }

            string fileData = new string(file.Dequeue(this.size).ToArray());
            //Decompression
            if ((this.flags & 0x00040000) == 0x00040000)
            {
                //Skip the uncompressed data size
                this.size = int.Parse(fileData.Substring(0, 4));
                fileData = fileData.Substring(4);
                fileData = PHPFunction.GZUncompress(fileData);
            }

            int i = 0;
            while (i < this.size)
            {
                string subrecordType = fileData.Substring(i, 4);
                int subrecordSize = int.Parse(fileData.Substring(i + 4, 2));
                if (scheme.shouldLoad(subrecordType))
                {
                    string subrecordData = fileData.Substring(i + 6, subrecordSize);
                    if (!this.data.ContainsKey(subrecordType))
                    {
                        this.data[subrecordType] = new List<string>();
                    }

                    this.data[subrecordType].Add(subrecordData);
                }

                i += (subrecordSize + 6);
            }
        }
    }
}