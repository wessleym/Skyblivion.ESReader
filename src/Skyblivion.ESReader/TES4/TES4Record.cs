using Skyblivion.ESReader.Extensions;
using Skyblivion.ESReader.PHP;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Skyblivion.ESReader.TES4
{
    public class TES4Record
    {
        public const int RECORD_HEADER_SIZE = 20;
        private readonly TES4File placedFile;
        private readonly int formIDPrivate;
        private readonly int flags;
        private int size;
        public TES4RecordType RecordType { get; }
        private readonly Lazy<int> formIDLazy;
        public int FormID => formIDLazy.Value;
        private readonly List<KeyValuePair<string, TES4SubrecordData>> subrecords = new List<KeyValuePair<string, TES4SubrecordData>>();
        private readonly Dictionary<string, int> dataAsFormidCache = new Dictionary<string, int>();
        /*
        * TES4Record constructor.
        */
        public TES4Record(TES4File placedFile, TES4RecordType type, int formid, int size, int flags)
        {
            this.placedFile = placedFile;
            this.RecordType = type;
            this.formIDPrivate = formid;
            this.size = size;
            this.flags = flags;
            formIDLazy = new Lazy<int>(() => this.placedFile.Expand(this.formIDPrivate));
        }

        public int ExpandBytesIntoFormID(TES4SubrecordData subrecord)
        {
            return this.placedFile.Expand(PHPFunction.UnpackV(subrecord.FirstFourBytes()));
        }

        public IEnumerable<TES4SubrecordData> GetSubrecords(string type)
        {
            return this.subrecords.Where(d => d.Key == type).Select(d => d.Value);
        }

        public IEnumerable<KeyValuePair<string, TES4SubrecordData>> GetSubrecords(IReadOnlyList<string> types)
        {
            return this.subrecords.Where(d => types.Contains(d.Key));
        }

        public IEnumerable<string> GetSubrecordsStrings(string type)
        {
            return GetSubrecords(type).Select(r => r.ToString());
        }

        public TES4SubrecordData? TryGetSubrecord(string type)
        {
            TES4SubrecordData? subrecord = this.subrecords.Where(d => d.Key == type).Select(d => d.Value).FirstOrDefault();
            if (subrecord != null) { return subrecord; }
            return null;
        }

        public TES4SubrecordData GetSubrecord(string type)
        {
            TES4SubrecordData? subrecord = TryGetSubrecord(type);
            if (subrecord == null) { throw new InvalidOperationException(nameof(type) + " " + type + " resulted in a null " + nameof(subrecord) + "."); }
            return subrecord;
        }

        public string? TryGetSubrecordTrim(string type)
        {
            TES4SubrecordData? subrecord = TryGetSubrecord(type);
            if (subrecord == null) { return null; }
            return subrecord.ToStringTrim();
        }

        public Nullable<int> TryGetSubrecordAsFormID(string type)
        {
            int value;
            if (this.dataAsFormidCache.TryGetValue(type, out value)) { return value; }
            TES4SubrecordData? subrecord = this.TryGetSubrecord(type);
            if (subrecord == null || subrecord.BytesCount < 4) { return null; }
            value = ExpandBytesIntoFormID(subrecord);
            this.dataAsFormidCache.Add(type, value);
            return value;
        }
        public int GetSubrecordAsFormID(string type)
        {
            Nullable<int> formid = TryGetSubrecordAsFormID(type);
            if (formid != null) { return formid.Value; }
            throw new InvalidOperationException(nameof(formid) + " was null for " + nameof(type) + " " + type + ".");
        }

        public string? TryGetEditorID()
        {
            return TryGetSubrecordTrim("EDID");
        }

        public string GetEditorID()
        {
            string? editorID = TryGetEditorID();
            if (editorID != null) { return editorID; }
            throw new InvalidOperationException(nameof(editorID) + " was null.");
        }

        public IEnumerable<Tuple<KeyValuePair<string, TES4SubrecordData>, StageIndexAndLogIndex?>> GetSubrecordsWithNullableStageIndexAndLogIndex(string returnRecordType)
        {
            const string indx = "INDX", qsdt = "QSDT";
            Nullable<int> stageIndex = null, logIndex = null;
            foreach (var subrecord in GetSubrecords(new string[] { indx, qsdt, returnRecordType }))
            {
                if (subrecord.Key == indx)
                {
                    stageIndex = (int)subrecord.Value.FirstByte();
                    logIndex = null;
                    if (subrecord.Value.SecondByte() != 0)
                    {
                        throw new InvalidOperationException(nameof(subrecord) + "." + nameof(subrecord.Value) + "[1] was non-zero.");
                    }
                }
                else if (subrecord.Key == qsdt)
                {
                    logIndex = logIndex == null ? 0 : logIndex.Value + 1;
                }
                else if (subrecord.Key == returnRecordType)
                {
                    if ((stageIndex != null) != (logIndex != null)) { throw new InvalidDataException("(stageIndex != null) != (logIndex != null)"); }
                    StageIndexAndLogIndex? stageIndexAndLogIndex = stageIndex != null && logIndex != null ? new StageIndexAndLogIndex(stageIndex.Value, logIndex.Value) : null;
                    yield return new Tuple<KeyValuePair<string, TES4SubrecordData>, StageIndexAndLogIndex?>(subrecord, stageIndexAndLogIndex);
                }
                else { throw new InvalidOperationException(nameof(subrecord) + "." + nameof(subrecord.Key) + " was " + subrecord.Key + "."); }
            }
        }

        public IEnumerable<Tuple<KeyValuePair<string, TES4SubrecordData>, StageIndexAndLogIndex>> GetSubrecordsWithStageIndexAndLogIndex(string returnRecordType)
        {
            return GetSubrecordsWithNullableStageIndexAndLogIndex(returnRecordType).Select(x =>
            {
                if (x.Item2 == null) { throw new InvalidOperationException(nameof(StageIndexAndLogIndex) + " was null."); }
                return x;
            })!;
        }

        public IEnumerable<TES4SubrecordData> GetSCRORecords(StageIndexAndLogIndex? stageIndexAndLogIndex)
        {
            var scroRecords = GetSubrecordsWithNullableStageIndexAndLogIndex("SCRO");
            if (stageIndexAndLogIndex != null)
            {
                scroRecords = scroRecords.Where(x =>
                {
                    if (x.Item2 == null) { throw new InvalidOperationException(nameof(StageIndexAndLogIndex) + " was null."); }
                    return x.Item2 == stageIndexAndLogIndex;
                });
            }
            return scroRecords.Select(x => x.Item1.Value);
        }

        public static string ReplaceSCTXSpecialCharacters(string sctx)
        {
            //To be more through and less performant, I could scan for char.IsControl(c) && !char.IsWhiteSpace(c).  But I've only witnessed \u0092 in VampireScript.
            return sctx.Replace('\u0092', '’');
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
                    this.subrecords.Add(new KeyValuePair<string, TES4SubrecordData>(subrecordType, new TES4SubrecordData(subrecordData)));
                }

                i += (subrecordSize + 6);
            }
        }
    }
}