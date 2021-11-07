using Skyblivion.ESReader.Exceptions;
using Skyblivion.ESReader.Extensions;
using Skyblivion.ESReader.Struct;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Skyblivion.ESReader.TES4
{
    public class TES4Collection : IEnumerable<TES4Record>
    {
        private readonly string path;
        private readonly Dictionary<int, TES4Record> records;
        private readonly Trie<TES4Record> edidIndex;
        private readonly Dictionary<int, List<TES4Record>> scriIndex;
        private readonly List<TES4File> files;
        private readonly Dictionary<string, TES4File> indexedFiles;
        private readonly Dictionary<string, Dictionary<int, int>> expandTables;
        /*
        * TES4Collection constructor.
        */
        public TES4Collection(string path)
        {
            this.path = path;
            this.records = new Dictionary<int, TES4Record>();
            this.edidIndex = new Trie<TES4Record>();
            this.scriIndex = new Dictionary<int, List<TES4Record>>();
            this.files = new List<TES4File>();
            this.indexedFiles = new Dictionary<string, TES4File>();
            this.expandTables = new Dictionary<string, Dictionary<int, int>>();
        }

        public void Add(string name)
        {
            TES4File file = new TES4File(this, this.path, name);
            this.files.Add(file);
            this.indexedFiles.Add(name, file);
        }

        public void Load(TES4FileLoadScheme scheme)
        {
            this.BuildExpandTables();
            foreach (var file in this.files)
            {
                foreach (TES4Record loadedRecord in file.Load(scheme))
                {
                    //no FORMID class encapsulation due to memory budgeting ;)
                    int formid = loadedRecord.FormID;
                    //TODO resolve conflicts
                    this.records.Add(formid, loadedRecord);
                    TES4SubrecordData? edid = loadedRecord.TryGetSubrecord("EDID");
                    if (edid != null)
                    {
                        this.edidIndex.Add(edid.ToStringTrimLower(), loadedRecord);
                    }
                    Nullable<int> scri = loadedRecord.TryGetSubrecordAsFormID("SCRI");
                    if (scri != null)
                    {
                        List<TES4Record> scriReferences = this.scriIndex.GetOrAdd(scri.Value, () => new List<TES4Record>(), out _);
                        scriReferences.Add(loadedRecord);
                    }
                }
            }
        }

        public TES4Record GetRecordByFormID(int formID)
        {
            TES4Record? record;
            if (this.records.TryGetValue(formID, out record))
            {
                return record;
            }
            throw new RecordNotFoundException("A record with form ID " + formID.ToString() + " was not found.");
        }

        public string? GetEDIDByFormIDNullable(int formID)
        {
            TES4Record record = GetRecordByFormID(formID);
            string? edid = record.TryGetEditorID();
            return edid;
        }

        public string GetEDIDByFormID(int formID)
        {
            string? edid = GetEDIDByFormIDNullable(formID);
            if (!string.IsNullOrWhiteSpace(edid)) { return edid!; }
            throw new InvalidOperationException(nameof(edid) + " was invalid:  " + edid);
        }

        public TES4Record[] GetRecordsBySCRI(int formID)
        {
            List<TES4Record>? list;
            return scriIndex.TryGetValue(formID, out list) ? list.ToArray() : new TES4Record[] { };
        }

        private TES4Record? GetRecordByEDID(string edid, bool throwNotFoundException)
        {
            string lowerEdid = edid.ToLower();
            TES4Record? record = this.edidIndex.Search(lowerEdid);
            if (record != null) { return record; }
            if (throwNotFoundException) { throw new RecordNotFoundException("EDID " + edid + " not found."); }
            return null;
        }
        public TES4Record? TryGetRecordByEDID(string edid)
        {
            return GetRecordByEDID(edid, false);
        }
        public TES4Record GetRecordByEDID(string edid)
        {
            return GetRecordByEDID(edid, true)!;
        }

        public TrieIterator<TES4Record> FindByEDIDPrefix(string edid)
        {
            string lowerEdid = edid.ToLower();
            return this.edidIndex.SearchPrefix(lowerEdid);
        }

        private IEnumerable<TES4Grup> GetGrup(TES4RecordType type)
        {
            foreach (var file in this.files)
            {
                TES4Grup? grup = file.TryGetGrup(type);
                if (grup != null)
                {
                    yield return grup;
                }
            }
        }

        public IEnumerable<TES4Record> GetGrupRecords(TES4RecordType type)
        {
            return GetGrup(type).SelectMany(g => g);
        }

        public int Expand(int formid, string file)
        {
            Dictionary<int, int>? expandTable;
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

        private void BuildExpandTables()
        {
            //Index
            Dictionary<string, int> fileToIndex = new Dictionary<string, int>();
            for (int i = 0; i < this.files.Count; i++)
            {
                fileToIndex[files[i].Name] = i;
            }

            for (int i = 0; i < this.files.Count; i++)
            {
                var file = files[i];
                string[] masters = file.Masters;
                //Index the file so it can see itself
                //this.expandTables.Add(file.getName(), new Dictionary<int, int>() { { masters.Count, index } });
                for (int x = 0; x <= 0xFF; ++x)
                {
                    AddToExpandTables(file.Name, x, i);
                }

                for(int j=0;j<masters.Length;j++)
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
                        throw new InconsistentESFilesException("File "+file.Name+" references a master not present in collection.");
                    }
                    AddToExpandTables(file.Name, masterId, expandedIndex);
                }
            }
        }

        public IEnumerator<TES4Record> GetEnumerator()
        {
            return records.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}