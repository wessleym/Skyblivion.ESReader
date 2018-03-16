using System.Collections.Generic;

namespace Skyblivion.ESReader.TES4
{
    public class TES4FileLoadScheme
    {
        private Dictionary<TES4RecordType, TES4GrupLoadScheme> grups = new Dictionary<TES4RecordType, TES4GrupLoadScheme>();
        public void add(TES4RecordType type, TES4GrupLoadScheme scheme)
        {
            this.grups[type] = scheme;
        }

        public bool shouldLoad(TES4RecordType type)
        {
            return this.grups.ContainsKey(type);
        }

        public TES4GrupLoadScheme getRulesFor(TES4RecordType type)
        {
            if (!this.grups.ContainsKey(type)) { return null; }
            return this.grups[type];
        }
    }
}
