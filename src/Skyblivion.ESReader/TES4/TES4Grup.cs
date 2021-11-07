using Skyblivion.ESReader.Exceptions;
using Skyblivion.ESReader.Extensions;
using Skyblivion.ESReader.PHP;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Skyblivion.ESReader.TES4
{
    /*
     * Represents top level GRUP
     * Class TES4Grup
     * @package Skyblivion\ESReader\TES4
     */
    public class TES4Grup : IEnumerable<TES4Record>
    {
        public const int GRUP_HEADER_SIZE = 20;
        public int Size { get; private set; }
        public TES4RecordType? Type { get; private set; }
        private readonly List<TES4Record> records = new List<TES4Record>();

        public IEnumerator<TES4Record> GetEnumerator()
        {
            return records.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /*
             * @throws InvalidESFileException
        */
        public IEnumerable<TES4Record> Load(FileStream fileContents, TES4File file, TES4GrupLoadScheme scheme, bool isTopLevelGrup)
        {
            long startPosition = fileContents.Position;
            byte[] headerBytes = fileContents.Read(GRUP_HEADER_SIZE);
            string headerString = TES4File.ISO_8859_1.Value.GetString(headerBytes);
            if (headerString.Substring(0, 4) != "GRUP")
            {
                throw new InvalidESFileException("Invalid GRUP magic, found "+headerString.Substring(0, 4));
            }

            this.Size = PHPFunction.UnpackV(headerBytes.Skip(4).Take(4).ToArray());
            if (isTopLevelGrup)
            {
                this.Type = TES4RecordType.First(headerString.Substring(8, 4));
            }

            long end = startPosition + this.Size;
            while (fileContents.Position < end)
            {
                //Ineffective lookahead, but oh well
                byte[] nextEntryTypeBytes = new byte[4];
                int bytesRead = fileContents.Read(nextEntryTypeBytes);
                if (bytesRead == 0) { break; }
                fileContents.Seek(-4, SeekOrigin.Current);
                string nextEntryType = TES4File.ISO_8859_1.Value.GetString(nextEntryTypeBytes);
                switch (nextEntryType)
                {
                    case "GRUP":
                        {
                            TES4Grup nestedGrup = new TES4Grup();
                            foreach (var subrecord in nestedGrup.Load(fileContents, file, scheme, false))
                            {
                                yield return subrecord;
                            }
                            break;
                        }

                    default:
                        {
                            byte[] recordHeaderBytes = fileContents.Read(TES4Record.RECORD_HEADER_SIZE);
                            string recordTypeString = TES4File.ISO_8859_1.Value.GetString(recordHeaderBytes.Take(4).ToArray());
                            TES4RecordType recordType = TES4RecordType.First(recordTypeString);
                            int recordSize = PHPFunction.UnpackV(recordHeaderBytes.Skip(4).Take(4).ToArray());
                            int recordFlags = PHPFunction.UnpackV(recordHeaderBytes.Skip(8).Take(4).ToArray());
                            int recordFormid = PHPFunction.UnpackV(recordHeaderBytes.Skip(12).Take(4).ToArray());
                            if (scheme.ShouldLoad(recordType))
                            {
                                TES4Record record = new TES4Record(file, recordType, recordFormid, recordSize, recordFlags);
                                record.Load(fileContents, scheme.GetRulesFor(recordType));
                                this.records.Add(record);
                                yield return record;
                            }
                            else
                            {
                                fileContents.Seek(recordSize, SeekOrigin.Current);
                            }
                            break;
                        }
                }
            }
        }
    }
}