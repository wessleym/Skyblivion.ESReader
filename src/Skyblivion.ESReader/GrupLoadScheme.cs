using System.Collections;
using System.Collections.Generic;

namespace Skyblivion.ESReader
{
    public class GrupLoadScheme : IEnumerable<KeyValuePair<ESRecordType, RecordLoadScheme>>
    {
        private readonly Dictionary<ESRecordType, RecordLoadScheme> records;
        public GrupLoadScheme()
        {
            records = new Dictionary<ESRecordType, RecordLoadScheme>();
        }

        public IEnumerator<KeyValuePair<ESRecordType, RecordLoadScheme>> GetEnumerator()
        {
            return records.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(ESRecordType type, RecordLoadScheme scheme)
        {
            this.records.Add(type, scheme);
        }

        public bool ShouldLoad(ESRecordType type)
        {
            return this.records.ContainsKey(type);
        }

        public RecordLoadScheme GetRulesFor(ESRecordType type)
        {
            return this.records[type];
        }
    }
}