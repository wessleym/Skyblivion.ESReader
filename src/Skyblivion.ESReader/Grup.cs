using Skyblivion.ESReader.Exceptions;
using Skyblivion.ESReader.Extensions;
using Skyblivion.ESReader.PHP;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Skyblivion.ESReader
{
    /*
     * Represents top level GRUP
     * Class TES4Grup
     * @package Skyblivion\ESReader\TES4
     */
    public class Grup : IEnumerable<ESRecord>
    {
        private readonly long endPosition;
        public ESRecordType? Type { get; }
        public Grup(long endPosition, ESRecordType? type)
        {
            this.endPosition = endPosition;
            Type = type;
        }

        private readonly List<ESRecord> records = new List<ESRecord>();

        public IEnumerator<ESRecord> GetEnumerator()
        {
            return records.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public static Grup Load(FileStream fileContents, bool isTopLevelGrup)
        {
            long startPosition = fileContents.Position;
            byte[] headerBytes = fileContents.Read(ESRecord.RECORD_HEADER_SIZE);
            string headerString = ESFile.ISO_8859_1.Value.GetString(headerBytes);
            if (headerString.Substring(0, 4) != "GRUP")
            {
                throw new InvalidESFileException("Invalid GRUP magic, found " + headerString.Substring(0, 4));
            }
            int size = PHPFunction.UnpackV(headerBytes.Skip(4).Take(4).ToArray());
            long endPosition = startPosition + size;
            ESRecordType? type = isTopLevelGrup ? RecordTypeParser.Parse(headerString.Substring(8, 4)) : null;
            return new Grup(endPosition, type);
        }

        public IEnumerable<ESRecord> LoadSubRecords(FileStream fileContents, ESFile file, GrupLoadScheme scheme)
        {
            while (fileContents.Position < endPosition)
            {
                //Ineffective lookahead, but oh well
                byte[] nextEntryTypeBytes = new byte[4];
                int bytesRead = fileContents.Read(nextEntryTypeBytes);
                if (bytesRead == 0) { break; }
                fileContents.Seek(-4, SeekOrigin.Current);
                string nextEntryType = ESFile.ISO_8859_1.Value.GetString(nextEntryTypeBytes);
                switch (nextEntryType)
                {
                    case "GRUP":
                        {
                            Grup nestedGrup = Load(fileContents, false);
                            foreach (var subrecord in nestedGrup.LoadSubRecords(fileContents, file, scheme))
                            {
                                yield return subrecord;
                            }
                            break;
                        }

                    default:
                        {
                            int recordSize;
                            ESRecordType? recordType = RecordReader.Read(scheme, fileContents, out recordSize);//Reads 8 bytes
                            if (recordType != null)
                            {
                                var (recordFlags, recordFormid) = RecordReader.GetRecordFlagsAndFormid(fileContents);//Reads 8 bytes
                                RecordReader.ReadRestOfRecordHeader(fileContents, 16);
                                ESRecord record = new ESRecord(file, recordType, recordFormid, recordSize, recordFlags);
                                record.Load(fileContents, scheme.GetRulesFor(recordType));
                                this.records.Add(record);
                                yield return record;
                            }
                            else
                            {
                                //Read rest of header and rest of record
                                fileContents.Seek((ESRecord.RECORD_HEADER_SIZE - 8) + recordSize, SeekOrigin.Current);
                            }
                            break;
                        }
                }
            }
        }
    }
}