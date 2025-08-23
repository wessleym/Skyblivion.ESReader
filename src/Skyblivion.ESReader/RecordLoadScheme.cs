using System.Linq;

namespace Skyblivion.ESReader
{
    public class RecordLoadScheme
    {
        private readonly string[] subrecords;
        public RecordLoadScheme(string[] subrecords)
        {
            this.subrecords = subrecords;
        }

        public bool ShouldLoad(string subrecord)
        {
            return this.subrecords.Contains(subrecord);
        }
    }
}