using System;
using System.Collections.Generic;

namespace Skyblivion.ESReader.TES4
{
    public interface ITES4Record
    {
        int getFormId();
        TES4RecordType getType();
        Nullable<int> getSubrecordAsFormid(string type);
        string getSubrecord(string type);
        List<string> getSubrecords(string type);
    }
}