using System;

namespace Skyblivion.ESReader
{
    internal static class RecordTypeParser
    {
        public static ESRecordType Parse(string name)
        {
            var type = ESRecordType.FirstOrNull(name) ?? TES4RecordType.FirstOrNull(name) ?? TES5RecordType.FirstOrNull(name);
            return type ?? throw new ArgumentException(nameof(name) + " was not a record type abbreviation: " + name, nameof(name));
        }
    }
}
