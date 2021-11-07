using Skyblivion.ESReader.Extensions;
using System.Collections;
using System.Collections.Generic;

namespace Skyblivion.ESReader.TES4
{
    public class TES4FileLoadScheme : IEnumerable<KeyValuePair<TES4RecordType, TES4GrupLoadScheme>>
    {
        private readonly Dictionary<TES4RecordType, TES4GrupLoadScheme> grups;
        public TES4FileLoadScheme()
        {
            grups = new Dictionary<TES4RecordType, TES4GrupLoadScheme>();
        }

        public IEnumerator<KeyValuePair<TES4RecordType, TES4GrupLoadScheme>> GetEnumerator()
        {
            return grups.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(TES4RecordType type, TES4GrupLoadScheme scheme)
        {
            this.grups.Add(type, scheme);
        }

        public bool ShouldLoad(TES4RecordType type)
        {
            return this.grups.ContainsKey(type);
        }

        public TES4GrupLoadScheme? GetRulesFor(TES4RecordType type)
        {
            return this.grups.GetWithFallbackNullable(type, () => null);
        }
    }
}
