using Skyblivion.ESReader.Exceptions;
using Skyblivion.ESReader.Struct;
using System.Collections.Generic;

namespace Skyblivion.ESReader.TES4
{
    public class TES4Collection
    {
        private string path;
        private Dictionary<int, TES4LoadedRecord> records = new Dictionary<int, TES4LoadedRecord>();
        private Trie edidIndex;
        private List<TES4File> files = new List<TES4File>();
        private Dictionary<string, TES4File> indexedFiles = new Dictionary<string, TES4File>();
        private Dictionary<string, Dictionary<int, int>> expandTables = new Dictionary<string, Dictionary<int, int>>();
        /*
        * TES4Collection constructor.
        */
        public TES4Collection(string path)
        {
            this.path = path;
            this.edidIndex = new Trie();
        }

        public void add(string name)
        {
            TES4File file = new TES4File(this, this.path, name);
            this.files.Add(file);
            this.indexedFiles[name] = file;
        }

        public void load(TES4FileLoadScheme scheme)
        {
            this.buildExpandTables();
            foreach (var file in this.files)
            {
                foreach (TES4LoadedRecord loadedRecord in file.load(scheme))
                {
                    //no FORMID class encapsulation due to memory budgeting ;)
                    int formid = loadedRecord.getFormId();
                    //TODO resolve conflicts
                    this.records[formid] = loadedRecord;
                    string edid = loadedRecord.getSubrecord("EDID");
                    if (edid != null)
                    {
                        this.edidIndex.add(edid.Trim().ToLower(), loadedRecord);
                    }
                }
            }
        }

        public ITES4Record findByFormid(int formid)
        {
            if (!this.records.ContainsKey(formid))
            {
                throw new RecordNotFoundException("Form "+formid.ToString()+" not found.");
            }

            return this.records[formid];
        }

        public ITES4Record findByEDID(string edid)
        {
            string lowerEdid = edid.ToLower();
            var val = this.edidIndex.search(lowerEdid);
            if (null == val)
            {
                throw new RecordNotFoundException("EDID "+edid+" not found.");
            }

            return (ITES4Record)val;
        }

        public TrieIterator findByEDIDPrefix(string edid)
        {
            string lowerEdid = edid.ToLower();
            return this.edidIndex.searchPrefix(lowerEdid);
        }

        public List<TES4Grup> getGrup(TES4RecordType type)
        {
            List<TES4Grup> grups = new List<TES4Grup>();
            foreach (var file in this.files)
            {
                TES4Grup grup = file.getGrup(type);
                if (grup != null)
                {
                    grups.Add(grup);
                }
            }
            return grups;
        }

        public int expand(int formid, string file)
        {
            if (!this.expandTables.ContainsKey(file))
            {
                throw new InconsistentESFilesException("Cannot find file "+file+" in expand table.");
            }

            int index = formid >> 24;
            if (!this.expandTables[file].ContainsKey(index))
            {
                throw new InconsistentESFilesException("Cannot expand formid index "+index+" in file "+file);
            }

            return (this.expandTables[file][index] << 24) | (formid & 0x00FFFFFF);
        }

        private void buildExpandTables()
        {
            //Index
            Dictionary<string, int> fileToIndex = new Dictionary<string, int>();
            for (int i = 0; i < this.files.Count; i++)
            {
                fileToIndex[files[i].getName()] = i;
            }

            for (int i = 0; i < this.files.Count; i++)
            {
                var file = files[i];
                List<string> masters = file.getMasters();
                //Index the file so it can see itself
                //this.expandTables[file.getName()] = [count(masters) => index];
                for (int x = 0; x <= 0xFF; ++x)
                {
                    this.expandTables[file.getName()][x] = i;
                }

                for(int j=0;j<masters.Count;j++)
                {
                    var masterId = j;
                    var masterName = masters[j];
                    if (!fileToIndex.ContainsKey(masterName))
                    {
                        throw new InconsistentESFilesException("File "+file.getName()+" references a master not present in collection.");
                    }

                    int expandedIndex = fileToIndex[masterName];
                    this.expandTables[file.getName()][masterId] = expandedIndex;
                }
            }
        }
    }
}