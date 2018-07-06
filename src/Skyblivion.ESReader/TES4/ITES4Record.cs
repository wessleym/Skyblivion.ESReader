using System;
using System.Collections.Generic;

namespace Skyblivion.ESReader.TES4
{
    public interface ITES4Record
    {
        int GetFormId();
        TES4RecordType RecordType { get; }

        Nullable<int> GetSubrecordAsFormid(string type);
        byte[] GetSubrecord(string type);
        string GetSubrecordString(string type);
        string GetSubrecordTrim(string type);
        string GetSubrecordTrimLower(string type);
        List<byte[]> GetSubrecords(string type);
    }
}