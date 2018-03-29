using Skyblivion.ESReader.Exceptions;
using Skyblivion.ESReader.Extensions.StreamExtensions;
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
    public class TES4Grup : IEnumerable<ITES4Record>
    {
        public const int GRUP_HEADER_SIZE = 20;
        private int size;
        private TES4RecordType type;
        private List<ITES4Record> records = new List<ITES4Record>();
        public int getSize()
        {
            return this.size;
        }

        public TES4RecordType getType()
        {
            return this.type;
        }

        public IEnumerator<ITES4Record> GetEnumerator()
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
        public IEnumerable<ITES4Record> load(FileStream fileContents, TES4File file, TES4GrupLoadScheme scheme, bool isTopLevelGrup)
        {
            long startPosition = fileContents.Position;
            byte[] headerBytes = fileContents.Read(GRUP_HEADER_SIZE);
            string header = TES4File.ISO_8859_1.Value.GetString(headerBytes);
            if (header.Substring(0, 4) != "GRUP")
            {
                throw new InvalidESFileException("Invalid GRUP magic, found "+header.Substring(0, 4));
            }

            this.size = PHPFunction.UnpackV(header.Substring(4, 4));
            if (isTopLevelGrup)
            {
                this.type = TES4RecordType.First(header.Substring(8, 4));
            }

            long end = startPosition + this.size;
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
                        foreach (var subrecord in nestedGrup.load(fileContents, file, scheme, false))
                        {
                            yield return subrecord;
                        }
                        break;
                    }

                    default:
                    {
                        string recordHeader = TES4File.ISO_8859_1.Value.GetString(fileContents.Read(TES4LoadedRecord.RECORD_HEADER_SIZE));
                        TES4RecordType recordType = TES4RecordType.First(recordHeader.Substring(0, 4));
                        int recordSize = PHPFunction.UnpackV(recordHeader.Substring(4, 4));
                        int recordFormid = PHPFunction.UnpackV(recordHeader.Substring(0xC, 4));
                        int recordFlags = PHPFunction.UnpackV(recordHeader.Substring(8, 4));
                        if (scheme.shouldLoad(recordType))
                        {
                            TES4LoadedRecord record = new TES4LoadedRecord(file, recordType, recordFormid, recordSize, recordFlags);
                            record.load(fileContents, scheme.getRulesFor(recordType));
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