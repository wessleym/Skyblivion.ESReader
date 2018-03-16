using Skyblivion.ESReader.Exceptions;
using Skyblivion.ESReader.QueueExtensions;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Skyblivion.ESReader.TES4
{
    public class TES4File
    {
        const int TES4_HEADER_SIZE = 0x18;
        private string path;
        private string name;
        private List<string> masters = new List<string>();
        private bool initialized = false;
        private Dictionary<TES4RecordType, TES4Grup> grups = new Dictionary<TES4RecordType, TES4Grup>();
        private TES4Collection collection;
        /*
        * File constructor.
        */
        public TES4File(TES4Collection collection, string path, string name)
        {
            this.collection = collection;
            this.path = path;
            this.name = name;
        }

        public string getName()
        {
            return this.name;
        }

        public List<string> getMasters()
        {
            if (!this.initialized)
            {
                this.initialize();
            }

            return this.masters;
        }

        public IEnumerable<ITES4Record> load(TES4FileLoadScheme scheme)
        {
            string filepath = this.path+"/"+this.name;
            Queue<char> contents = new Queue<char>(File.ReadAllText(filepath));
            this.fetchTES4(contents);
            while (contents.Any())
            {
                TES4Grup grup = new TES4Grup();
                string header = new string(contents.Take(TES4Grup.GRUP_HEADER_SIZE).ToArray());
                if (header.Substring(0, 4) != "GRUP")
                {
                    throw new InvalidESFileException("Invalid GRUP magic, found "+header.Substring(0, 4));
                }

                TES4RecordType grupType = TES4RecordType.First(header.Substring(8, 4));
                int grupSize = int.Parse(header.Substring(4, 4));
                if (scheme.shouldLoad(grupType))
                {
                    foreach (var loadedRecord in grup.load(contents, this, scheme.getRulesFor(grupType), true))
                    {
                        yield return loadedRecord;
                    }
                }
                else
                {
                    contents.Dequeue(grupSize);
                }

                this.grups[grup.getType()] = grup;
            }
        }

        public TES4Grup getGrup(TES4RecordType type)
        {
            if (!this.grups.ContainsKey(type)) { return null; }
            return this.grups[type];
        }

    public int expand(int formid)
    {
        return this.collection.expand(formid, this.getName());
    }

    private TES4LoadedRecord fetchTES4(Queue<char> contents)
    {
        string recordHeader = new string(contents.Dequeue(TES4LoadedRecord.RECORD_HEADER_SIZE).ToArray());
        int recordSize = int.Parse(recordHeader.Substring(4, 4));
        int recordFormid = int.Parse(recordHeader.Substring(0xC, 4));
        int recordFlags = int.Parse(recordHeader.Substring(8, 4));
        TES4LoadedRecord tes4record = new TES4LoadedRecord(this, TES4RecordType.TES4, recordFormid, recordSize, recordFlags);
        tes4record.load(contents, new TES4RecordLoadScheme(new string[] { "MAST" }));
        return tes4record;
    }

    private void initialize()
    {
        string filepath = this.path+"/"+this.name;
        Queue<char> contents = new Queue<char>(File.ReadAllText(filepath));
        TES4LoadedRecord tes4record = this.fetchTES4(contents);
        masters = tes4record.getSubrecords("MAST");
        this.initialized = true;
    }
} }
