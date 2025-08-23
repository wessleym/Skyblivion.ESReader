using System.Linq;

namespace Skyblivion.ESReader
{
    public class TES4RecordType : ESRecordType
    {
        private TES4RecordType(string name) : base(name) { }

        public static readonly TES4RecordType
            ACRE = new TES4RecordType("ACRE"),
            BSGN = new TES4RecordType("BSGN"),
            CLOT = new TES4RecordType("CLOT"),
            CREA = new TES4RecordType("CREA"),
            LVLC = new TES4RecordType("LVLC"),
            PGRD = new TES4RecordType("PGRD"),
            ROAD = new TES4RecordType("ROAD"),
            SBSP = new TES4RecordType("SBSP"),
            SCPT = new TES4RecordType("SCPT"),
            SGST = new TES4RecordType("SGST"),
            SKIL = new TES4RecordType("SKIL");

        private static readonly TES4RecordType[] all = new TES4RecordType[]
        {
            ACRE,
            BSGN,
            CLOT,
            CREA,
            LVLC,
            PGRD,
            ROAD,
            SBSP,
            SCPT,
            SGST,
            SKIL
        };

        public static new ESRecordType? FirstOrNull(string name)
        {
            return all.Where(t => t.Name == name).FirstOrDefault();
        }
    }
}