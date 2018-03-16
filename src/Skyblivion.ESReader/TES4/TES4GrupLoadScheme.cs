using System.Collections.Generic;

namespace Skyblivion.ESReader.TES4
{
    public class TES4GrupLoadScheme
    {
        private Dictionary<TES4RecordType, TES4RecordLoadScheme> records = new Dictionary<TES4RecordType, TES4RecordLoadScheme>();
        public void add(TES4RecordType type, TES4RecordLoadScheme scheme)
        {
            this.records[type] = scheme;
        }

        public bool shouldLoad(TES4RecordType type)
        {
            return this.records.ContainsKey(type);
        }

        public TES4RecordLoadScheme getRulesFor(TES4RecordType type)
        {
            if (!this.records.ContainsKey(type)) { return null; }
            return this.records[type];
        }
    }
}