using System;
using System.Collections.Generic;

namespace Skyblivion.ESReader.TES4
{
    public interface ITES4Record
    {
        int GetFormId();
        TES4RecordType RecordType { get; }

        Nullable<int> GetSubrecordAsFormidNullable(string type);
        byte[]? GetSubrecord(string type);
        string GetSubrecordString(string type);
        string? GetSubrecordTrimNullable(string type);
        string GetSubrecordTrim(string type);
        string? GetSubrecordTrimLowerNullable(string type);
        List<byte[]> GetSubrecords(string type);
    }
}