using System;
using System.Collections.Generic;

namespace Skyblivion.ESReader.TES4
{
    public interface ITES4Record
    {
        int GetFormId();
        TES4RecordType getType();
        Nullable<int> getSubrecordAsFormid(string type);
        byte[] getSubrecord(string type);
        string getSubrecordString(string type);
        string getSubrecordTrim(string type);
        string getSubrecordTrimLower(string type);
        List<byte[]> getSubrecords(string type);
    }
}