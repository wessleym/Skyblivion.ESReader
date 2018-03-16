using Skyblivion.ESReader.Exceptions;
using Skyblivion.ESReader.QueueExtensions;
using System.Collections;
using System.Collections.Generic;
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
        public IEnumerable<ITES4Record> load(Queue<char> fileContents, TES4File file, TES4GrupLoadScheme scheme, bool isTopLevelGrup)
        {
            string header = new string(fileContents.Dequeue(GRUP_HEADER_SIZE).ToArray());
            if (header.Substring(0, 4) != "GRUP")
            {
                throw new InvalidESFileException("Invalid GRUP magic, found "+header.Substring(0, 4));
            }

            this.size = int.Parse(header.Substring(4, 4));
            if (isTopLevelGrup)
            {
                this.type = TES4RecordType.First(header.Substring(8, 4));
            }

            while (fileContents.Any())
            {
                //Ineffective lookahead, but oh well
                string nextEntryType = new string(fileContents.Take(4).ToArray());
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
                        string recordHeader = new string(fileContents.Dequeue(TES4LoadedRecord.RECORD_HEADER_SIZE).ToArray());
                        TES4RecordType recordType = TES4RecordType.First(recordHeader.Substring(0, 4));
                        int recordSize = int.Parse(recordHeader.Substring(4, 4));
                        int recordFormid = int.Parse(recordHeader.Substring(0xC, 4));
                        int recordFlags = int.Parse(recordHeader.Substring(8, 4));
                        if (scheme.shouldLoad(recordType))
                        {
                            TES4LoadedRecord record = new TES4LoadedRecord(file, recordType, recordFormid, recordSize, recordFlags);
                            record.load(fileContents, scheme.getRulesFor(recordType));
                            this.records.Add(record);
                            yield return record;
                        }
                        else
                        {
                            fileContents.Dequeue(recordSize);
                        }

                        break;
                    }
                }
            }
        }
    }
}