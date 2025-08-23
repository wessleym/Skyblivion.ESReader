using System.Collections.Generic;
using System.Linq;

namespace Skyblivion.ESReader
{
    public class ESRecordType
    {
        public string Name { get; }
        protected ESRecordType(string name)
        {
            Name = name;
        }

        public static readonly ESRecordType
            ACHR = new ESRecordType("ACHR"),
            ACTI = new ESRecordType("ACTI"),
            ALCH = new ESRecordType("ALCH"),
            AMMO = new ESRecordType("AMMO"),
            ANIO = new ESRecordType("ANIO"),
            APPA = new ESRecordType("APPA"),
            ARMO = new ESRecordType("ARMO"),
            BOOK = new ESRecordType("BOOK"),
            CELL = new ESRecordType("CELL"),
            CLAS = new ESRecordType("CLAS"),
            CLMT = new ESRecordType("CLMT"),
            CONT = new ESRecordType("CONT"),
            CSTY = new ESRecordType("CSTY"),
            DIAL = new ESRecordType("DIAL"),
            DOOR = new ESRecordType("DOOR"),
            EFSH = new ESRecordType("EFSH"),
            ENCH = new ESRecordType("ENCH"),
            EYES = new ESRecordType("EYES"),
            FACT = new ESRecordType("FACT"),
            FLOR = new ESRecordType("FLOR"),
            FURN = new ESRecordType("FURN"),
            GLOB = new ESRecordType("GLOB"),
            GMST = new ESRecordType("GMST"),
            GRAS = new ESRecordType("GRAS"),
            HAIR = new ESRecordType("HAIR"),//deprecated in TES5
            IDLE = new ESRecordType("IDLE"),
            INFO = new ESRecordType("INFO"),
            INGR = new ESRecordType("INGR"),
            KEYM = new ESRecordType("KEYM"),
            LAND = new ESRecordType("LAND"),
            LIGH = new ESRecordType("LIGH"),
            LSCR = new ESRecordType("LSCR"),
            LTEX = new ESRecordType("LTEX"),
            LVLI = new ESRecordType("LVLI"),
            LVSP = new ESRecordType("LVSP"),
            MGEF = new ESRecordType("MGEF"),
            MISC = new ESRecordType("MISC"),
            NPC_ = new ESRecordType("NPC_"),
            PACK = new ESRecordType("PACK"),
            QUST = new ESRecordType("QUST"),
            RACE = new ESRecordType("RACE"),
            REFR = new ESRecordType("REFR"),
            REGN = new ESRecordType("REGN"),
            SLGM = new ESRecordType("SLGM"),
            SOUN = new ESRecordType("SOUN"),
            SPEL = new ESRecordType("SPEL"),
            STAT = new ESRecordType("STAT"),
            TES4 = new ESRecordType("TES4"),
            TREE = new ESRecordType("TREE"),
            WATR = new ESRecordType("WATR"),
            WEAP = new ESRecordType("WEAP"),
            WRLD = new ESRecordType("WRLD"),
            WTHR = new ESRecordType("WTHR");

        private static readonly ESRecordType[] all = new ESRecordType[]
        {
            ACHR,
            ACTI,
            ALCH,
            AMMO,
            ANIO,
            APPA,
            ARMO,
            BOOK,
            CELL,
            CLAS,
            CLMT,
            CONT,
            CSTY,
            DIAL,
            DOOR,
            EFSH,
            ENCH,
            EYES,
            FACT,
            FLOR,
            FURN,
            GLOB,
            GMST,
            GRAS,
            HAIR,
            IDLE,
            INFO,
            INGR,
            KEYM,
            LAND,
            LIGH,
            LSCR,
            LTEX,
            LVLI,
            LVSP,
            MGEF,
            MISC,
            NPC_,
            PACK,
            QUST,
            RACE,
            REFR,
            REGN,
            SLGM,
            SOUN,
            SPEL,
            STAT,
            TES4,
            TREE,
            WATR,
            WEAP,
            WRLD,
            WTHR
        };

        private static IEnumerable<ESRecordType> Where(string name)
        {
            return all.Where(t => t.Name == name);
        }

        public static ESRecordType First(string name)
        {
            return Where(name).First();
        }

        public static ESRecordType? FirstOrNull(string name)
        {
            return Where(name).FirstOrDefault();
        }
    }
}