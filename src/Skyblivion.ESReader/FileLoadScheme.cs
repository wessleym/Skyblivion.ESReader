using Skyblivion.ESReader.Extensions;
using System.Collections;
using System.Collections.Generic;

namespace Skyblivion.ESReader
{
    public class FileLoadScheme : IEnumerable<KeyValuePair<ESRecordType, GrupLoadScheme>>
    {
        private readonly Dictionary<ESRecordType, GrupLoadScheme> grups;
        public FileLoadScheme()
        {
            grups = new Dictionary<ESRecordType, GrupLoadScheme>();
        }

        public IEnumerator<KeyValuePair<ESRecordType, GrupLoadScheme>> GetEnumerator()
        {
            return grups.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(ESRecordType type, GrupLoadScheme scheme)
        {
            this.grups.Add(type, scheme);
        }

        public bool ShouldLoad(ESRecordType type)
        {
            return this.grups.ContainsKey(type);
        }

        public GrupLoadScheme? GetRulesFor(ESRecordType type)
        {
            return this.grups.GetWithFallbackNullable(type, () => null);
        }
    }
}
