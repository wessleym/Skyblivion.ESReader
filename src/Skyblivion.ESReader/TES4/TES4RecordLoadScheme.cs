using System.Collections.Generic;

namespace Skyblivion.ESReader.TES4
{
    public class TES4RecordLoadScheme
    {
        private readonly Dictionary<string, bool> subrecords;
        public TES4RecordLoadScheme(string[] subrecords)
        {
            this.subrecords = new Dictionary<string, bool>();
            foreach (var subrecord in subrecords)
            {
                this.subrecords.Add(subrecord, true);
            }
        }

        public bool ShouldLoad(string subrecord)
        {
            return this.subrecords.ContainsKey(subrecord);
        }
    }
}