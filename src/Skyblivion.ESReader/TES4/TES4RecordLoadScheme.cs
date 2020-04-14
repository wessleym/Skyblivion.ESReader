using System.Linq;

namespace Skyblivion.ESReader.TES4
{
    public class TES4RecordLoadScheme
    {
        private readonly string[] subrecords;
        public TES4RecordLoadScheme(string[] subrecords)
        {
            this.subrecords = subrecords;
        }

        public bool ShouldLoad(string subrecord)
        {
            return this.subrecords.Contains(subrecord);
        }
    }
}