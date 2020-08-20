using Skyblivion.ESReader.Exceptions;
using Skyblivion.ESReader.Extensions.IDictionaryExtensions;
using Skyblivion.ESReader.Struct;
using System;
using System.Collections.Generic;

namespace Skyblivion.ESReader.TES4
{
    public class TES4Collection
    {
        private readonly string path;
        private readonly Dictionary<int, TES4LoadedRecord> records = new Dictionary<int, TES4LoadedRecord>();
        private readonly Trie<TES4LoadedRecord> edidIndex;
        private readonly Dictionary<int, List<int>> nameFormIDToFormIDIndex;//WTM:  Change:  Added
        private readonly Trie<List<TES4LoadedRecord>> scriIndex;
        private readonly List<TES4File> files = new List<TES4File>();
        private readonly Dictionary<string, TES4File> indexedFiles = new Dictionary<string, TES4File>();
        private readonly Dictionary<string, Dictionary<int, int>> expandTables = new Dictionary<string, Dictionary<int, int>>();
        /*
        * TES4Collection constructor.
        */
        public TES4Collection(string path)
        {
            this.path = path;
            this.edidIndex = new Trie<TES4LoadedRecord>();
            this.nameFormIDToFormIDIndex = new Dictionary<int, List<int>>();
            this.scriIndex = new Trie<List<TES4LoadedRecord>>();
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
                foreach (TES4LoadedRecord loadedRecord in file.Load(scheme))
                {
                    //no FORMID class encapsulation due to memory budgeting ;)
                    int formid = loadedRecord.GetFormId();
                    //TODO resolve conflicts
                    this.records.Add(formid, loadedRecord);
                    string? edid = loadedRecord.GetSubrecordTrimLowerNullable("EDID");
                    if (edid != null)
                    {
                        this.edidIndex.Add(edid, loadedRecord);
                    }
                    Nullable<int> nameFormID = loadedRecord.GetSubrecordAsFormidNullable("NAME");
                    if (nameFormID != null)
                    {
                        this.nameFormIDToFormIDIndex.AddNewListIfNotContainsKeyAndAddValueToList(nameFormID.Value, formid);
                    }
                    Nullable<int> scri = loadedRecord.GetSubrecordAsFormidNullable("SCRI");
                    if (scri != null)
                    {
                        string scriString = scri.ToString();
                        List<TES4LoadedRecord>? records = this.scriIndex.Search(scriString);
                        if (records == null)
                        {
                            records = new List<TES4LoadedRecord>();
                            this.scriIndex.Add(scri.ToString(), records);
                        }
                        records.Add(loadedRecord);
                    }
                }
            }
        }

        public TES4LoadedRecord GetRecordByFormID(int formID)
        {
            TES4LoadedRecord record;
            if (this.records.TryGetValue(formID, out record))
            {
                return record;
            }
            throw new RecordNotFoundException("A record with form ID " + formID.ToString() + " was not found.");
        }

        public string? GetEDIDByFormIDNullable(int formID)
        {
            TES4LoadedRecord record = GetRecordByFormID(formID);
            string? edid = record.GetSubrecordTrimNullable("EDID");
            return edid;
        }

        public string GetEDIDByFormID(int formID)
        {
            string? edid = GetEDIDByFormIDNullable(formID);
            if (!string.IsNullOrWhiteSpace(edid)) { return edid!; }
            throw new InvalidOperationException(nameof(edid) + " was invalid:  " + edid);
        }

        public TES4LoadedRecord[] GetRecordsBySCRI(int formID)
        {
            List<TES4LoadedRecord>? list = scriIndex.Search(formID.ToString());
            return list != null ? list.ToArray() : new TES4LoadedRecord[] { };
        }

        private TES4LoadedRecord? GetRecordByEDID(string edid, bool throwNotFoundException)
        {
            string lowerEdid = edid.ToLower();
            TES4LoadedRecord? record = this.edidIndex.Search(lowerEdid);
            if (record != null) { return record; }
            if (throwNotFoundException) { throw new RecordNotFoundException("EDID " + edid + " not found."); }
            return null;
        }
        public TES4LoadedRecord? TryGetRecordByEDID(string edid)
        {
            return GetRecordByEDID(edid, false);
        }
        public TES4LoadedRecord GetRecordByEDID(string edid)
        {
            return GetRecordByEDID(edid, true)!;
        }

        public TrieIterator<TES4LoadedRecord> FindByEDIDPrefix(string edid)
        {
            string lowerEdid = edid.ToLower();
            return this.edidIndex.SearchPrefix(lowerEdid);
        }

        public List<int>? TryGetFormIDsByName(int nameFormID)
        {
            List<int> formIDs;
            this.nameFormIDToFormIDIndex.TryGetValue(nameFormID, out formIDs);
            //Some values, like ArenaMouth "Arena Mouth" [NPC_:00046653], are sent into this method but return null since they are never used in NAME records.
            return formIDs;
        }

        public List<TES4Grup> GetGrup(TES4RecordType type)
        {
            List<TES4Grup> grups = new List<TES4Grup>();
            foreach (var file in this.files)
            {
                TES4Grup? grup = file.GetGrup(type);
                if (grup != null)
                {
                    grups.Add(grup);
                }
            }
            return grups;
        }

        public int Expand(int formid, string file)
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
    }
}