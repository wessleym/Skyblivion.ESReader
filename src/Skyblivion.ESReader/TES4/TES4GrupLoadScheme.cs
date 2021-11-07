using System.Collections;
using System.Collections.Generic;

namespace Skyblivion.ESReader.TES4
{
    public class TES4GrupLoadScheme : IEnumerable<KeyValuePair<TES4RecordType, TES4RecordLoadScheme>>
    {
        private readonly Dictionary<TES4RecordType, TES4RecordLoadScheme> records;
        public TES4GrupLoadScheme()
        {
            records = new Dictionary<TES4RecordType, TES4RecordLoadScheme>();
        }

        public IEnumerator<KeyValuePair<TES4RecordType, TES4RecordLoadScheme>> GetEnumerator()
        {
            return records.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(TES4RecordType type, TES4RecordLoadScheme scheme)
        {
            this.records.Add(type, scheme);
        }

        public bool ShouldLoad(TES4RecordType type)
        {
            return this.records.ContainsKey(type);
        }

        public TES4RecordLoadScheme GetRulesFor(TES4RecordType type)
        {
            return this.records[type];
        }
    }
}