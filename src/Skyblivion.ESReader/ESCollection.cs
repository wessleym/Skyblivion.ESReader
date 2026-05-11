using Skyblivion.ESReader.Exceptions;
using Skyblivion.ESReader.Extensions;
using Skyblivion.ESReader.Struct;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Skyblivion.ESReader
{
    public class ESCollection : IEnumerable<ESRecord>
    {
        private readonly string directoryPath;
        private readonly Dictionary<int, ESRecord> records;
        private readonly Trie<ESRecord> edidIndex;
        private readonly Dictionary<int, List<ESRecord>> scriIndex;
        private readonly List<ESFile> files;
        private readonly Dictionary<string, ESFile> indexedFiles;
        private readonly Dictionary<string, Dictionary<int, int>> expandTables;
        public ESCollection(string directoryPath)
        {
            this.directoryPath = directoryPath;
            this.records = new Dictionary<int, ESRecord>();
            this.edidIndex = new Trie<ESRecord>(StringComparer.OrdinalIgnoreCase, StringComparison.OrdinalIgnoreCase);
            this.scriIndex = new Dictionary<int, List<ESRecord>>();
            this.files = new List<ESFile>();
            this.indexedFiles = new Dictionary<string, ESFile>();
            this.expandTables = new Dictionary<string, Dictionary<int, int>>();
        }

        public void Add(string name)
        {
            ESFile file = new ESFile(this, this.directoryPath, name);
            this.files.Add(file);
            this.indexedFiles.Add(name, file);
        }

        public void Load(FileLoadScheme scheme)
        {
            this.BuildExpandTables();
            foreach (var file in this.files)
            {
                foreach (ESRecord loadedRecord in file.Load(scheme))
                {
                    //no FORMID class encapsulation due to memory budgeting ;)
                    int formid = loadedRecord.FormID;
                    //TODO resolve conflicts
                    this.records.Add(formid, loadedRecord);
                    SubrecordData? edid = loadedRecord.TryGetSubrecord("EDID");
                    if (edid != null)
                    {
                        this.edidIndex.Add(edid.ToStringTrim(), loadedRecord);
                    }
                    Nullable<int> scri = loadedRecord.TryGetSubrecordAsFormID("SCRI");
                    if (scri != null)
                    {
                        List<ESRecord> scriReferences = this.scriIndex.GetOrAdd(scri.Value, () => new List<ESRecord>(), out _);
                        scriReferences.Add(loadedRecord);
                    }
                }
            }
        }

        public ESRecord GetRecordByFormID(int formID)
        {
            ESRecord? record;
            if (this.records.TryGetValue(formID, out record))
            {
                return record;
            }
            throw new RecordNotFoundException("A record with form ID " + formID.ToString() + " was not found.");
        }

        public string? GetEDIDByFormIDNullable(int formID)
        {
            ESRecord record = GetRecordByFormID(formID);
            string? edid = record.TryGetEditorID();
            return edid;
        }

        public string GetEDIDByFormID(int formID)
        {
            string? edid = GetEDIDByFormIDNullable(formID);
            if (!string.IsNullOrWhiteSpace(edid)) { return edid!; }
            throw new InvalidOperationException(nameof(edid) + " was invalid:  " + edid);
        }

        public ESRecord[] GetRecordsBySCRI(int formID)
        {
            List<ESRecord>? list;
            return scriIndex.TryGetValue(formID, out list) ? list.ToArray() : [];
        }

        private ESRecord? GetRecordByEDID(string edid, bool throwNotFoundException)
        {
            ESRecord? record = this.edidIndex.Search(edid);
            if (record != null) { return record; }
            if (throwNotFoundException) { throw new RecordNotFoundException("EDID " + edid + " not found."); }
            return null;
        }
        public ESRecord? TryGetRecordByEDID(string edid)
        {
            return GetRecordByEDID(edid, false);
        }
        public ESRecord GetRecordByEDID(string edid)
        {
            return GetRecordByEDID(edid, true)!;
        }

        public TrieIterator<ESRecord> FindByEDIDPrefix(string edid)
        {
            return this.edidIndex.SearchPrefix(edid);
        }

        private IEnumerable<Grup> GetGrup(ESRecordType type)
        {
            foreach (var file in this.files)
            {
                Grup? grup = file.TryGetGrup(type);
                if (grup != null)
                {
                    yield return grup;
                }
            }
        }

        public IEnumerable<ESRecord> GetGrupRecords(ESRecordType type)
        {
            return GetGrup(type).SelectMany(g => g);
        }

        public int Expand(int formid, string file)
        {
            Dictionary<int, int>? expandTable;
            if (!this.expandTables.TryGetValue(file, out expandTable))
            {
                throw new InconsistentESFilesException("Cannot find file " + file + " in expand table.");
            }

            int index = formid >> 24;
            int expandTableValue;
            if (!expandTable.TryGetValue(index, out expandTableValue))
            {
                throw new InconsistentESFilesException("Cannot expand formid index " + index + " in file " + file);
            }
            int newValue = expandTableValue << 24;

            return newValue | (formid & 0x00FFFFFF);
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
                Dictionary<int, int> expandTable = [];
                //Index the file so it can see itself
                //expandTable.Add(masters.Length, i);
                for (int x = 0; x <= 0xFF; ++x)
                {
                    expandTable.Add(x, i);
                }

                for (int j = 0; j < masters.Length; j++)
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
                        throw new InconsistentESFilesException("File " + file.Name + " references a master not present in collection: " + masterName);
                    }
                    expandTable[masterId] = expandedIndex;
                }
                expandTables.Add(file.Name, expandTable);
            }
        }

        public IEnumerator<ESRecord> GetEnumerator()
        {
            return records.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}