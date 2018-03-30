using Skyblivion.ESReader.Exceptions;
using Skyblivion.ESReader.Extensions.IDictionaryExtensions;
using Skyblivion.ESReader.Struct;
using System;
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
            this.indexedFiles.Add(name, file);
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
                    this.records.Add(formid, loadedRecord);
                    string edid = loadedRecord.getSubrecordTrimLower("EDID");
                    if (edid != null)
                    {
                        this.edidIndex.add(edid, loadedRecord);
                    }
                }
            }
        }

        public TES4LoadedRecord findByFormid(int formid)
        {
            TES4LoadedRecord record;
            if (this.records.TryGetValue(formid, out record))
            {
                return record;
            }
            throw new RecordNotFoundException("Form " + formid.ToString() + " not found.");
        }

        public ITES4Record findByEDID(string edid, bool throwNotFoundException)
        {
            string lowerEdid = edid.ToLower();
            var val = this.edidIndex.search(lowerEdid);
            if (val == null)
            {
                if (throwNotFoundException) { throw new RecordNotFoundException("EDID " + edid + " not found."); }
                return null;
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
            Dictionary<int, int> expandTable;
            if (!this.expandTables.TryGetValue(file, out expandTable))
            {
                throw new InconsistentESFilesException("Cannot find file "+file+" in expand table.");
            }

            int index = formid >> 24;
            int newValue;
            try
            {
                newValue = expandTable[index] << 24;
            }
            catch (KeyNotFoundException)
            {
                throw new InconsistentESFilesException("Cannot expand formid index "+index+" in file "+file);
            }

            return newValue | (formid & 0x00FFFFFF);
        }

        private void AddToExpandTables(string fileName, int dictionaryKey, int dictionaryValue)
        {
            Dictionary<int, int> expandTable = expandTables.GetOrAdd(fileName, () => new Dictionary<int, int>());
            expandTable.Add(dictionaryKey, dictionaryValue);
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
                //this.expandTables.Add(file.getName(), new Dictionary<int, int>() { { masters.Count, index } });
                for (int x = 0; x <= 0xFF; ++x)
                {
                    AddToExpandTables(file.getName(), x, i);
                }

                for(int j=0;j<masters.Count;j++)
                {
                    var masterId = j;
                    var masterName = masters[j];
                    int expandedIndex;
                    try
                    {
                        expandedIndex = fileToIndex[masterName];
                    }
                    catch (KeyNotFoundException)
                    {
                        throw new InconsistentESFilesException("File "+file.getName()+" references a master not present in collection.");
                    }
                    AddToExpandTables(file.getName(), masterId, expandedIndex);
                }
            }
        }
    }
}