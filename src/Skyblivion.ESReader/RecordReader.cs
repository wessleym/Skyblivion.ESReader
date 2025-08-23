using Skyblivion.ESReader.Extensions;
using Skyblivion.ESReader.PHP;
using System.IO;

namespace Skyblivion.ESReader
{
    internal static class RecordReader
    {
        private static int GetRecordSize(Stream stream)
        {
            return PHPFunction.UnpackV(stream.Read(4));
        }

        public static (int RecordFlags, int RecordFormid) GetRecordFlagsAndFormid(Stream stream)
        {
            int recordFlags = PHPFunction.UnpackV(stream.Read(4));
            int recordFormid = PHPFunction.UnpackV(stream.Read(4));
            return (recordFlags, recordFormid);
        }

        public static void ReadRestOfRecordHeader(Stream stream, int bytesRead)
        {
            int bytesRemaining = ESRecord.RECORD_HEADER_SIZE - bytesRead;
            stream.Seek(bytesRemaining, SeekOrigin.Current);//Discard
        }

        public static ESRecordType? Read(GrupLoadScheme scheme, Stream stream, out int recordSize)
        {
            string recordTypeString = ESFile.ISO_8859_1.Value.GetString(stream.Read(4));
            recordSize = GetRecordSize(stream);
            ESRecordType recordType = RecordTypeParser.Parse(recordTypeString);
            return scheme.ShouldLoad(recordType) ? recordType : null;
        }

        public static ESRecord ReadTES4Record(ESFile file, Stream stream)
        {
            stream.Seek(4, SeekOrigin.Current);//"TES4" (Even in TES5 ESMs). Reads 4 bytes.
            int recordSize = GetRecordSize(stream);//Reads 4 bytes.
            var (recordFlags, recordFormid) = GetRecordFlagsAndFormid(stream);//Reads 8 bytes.
            ReadRestOfRecordHeader(stream, 16);
            ESRecord tes4record = new ESRecord(file, ESRecordType.TES4, recordFormid, recordSize, recordFlags);
            tes4record.Load(stream, new RecordLoadScheme(new string[] { "MAST" }));
            return tes4record;
        }
    }
}
