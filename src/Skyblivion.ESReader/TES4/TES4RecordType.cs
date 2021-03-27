using System.Linq;

namespace Skyblivion.ESReader.TES4
{
    /*
     * Class TES4RecordType
     * @method static TES4RecordType ACHR()
     * @method static TES4RecordType ACRE()
     * @method static TES4RecordType ACTI()
     * @method static TES4RecordType ALCH()
     * @method static TES4RecordType AMMO()
     * @method static TES4RecordType ANIO()
     * @method static TES4RecordType APPA()
     * @method static TES4RecordType ARMO()
     * @method static TES4RecordType BOOK()
     * @method static TES4RecordType BSGN()
     * @method static TES4RecordType CELL()
     * @method static TES4RecordType CLAS()
     * @method static TES4RecordType CLMT()
     * @method static TES4RecordType CLOT()
     * @method static TES4RecordType CONT()
     * @method static TES4RecordType CREA()
     * @method static TES4RecordType CSTY()
     * @method static TES4RecordType DIAL()
     * @method static TES4RecordType DOOR()
     * @method static TES4RecordType EFSH()
     * @method static TES4RecordType ENCH()
     * @method static TES4RecordType EYES()
     * @method static TES4RecordType FACT()
     * @method static TES4RecordType FLOR()
     * @method static TES4RecordType FURN()
     * @method static TES4RecordType GLOB()
     * @method static TES4RecordType GMST()
     * @method static TES4RecordType GRAS()
     * @method static TES4RecordType HAIR()
     * @method static TES4RecordType IDLE()
     * @method static TES4RecordType INFO()
     * @method static TES4RecordType INGR()
     * @method static TES4RecordType KEYM()
     * @method static TES4RecordType LAND()
     * @method static TES4RecordType LIGH()
     * @method static TES4RecordType LSCR()
     * @method static TES4RecordType LTEX()
     * @method static TES4RecordType LVLC()
     * @method static TES4RecordType LVLI()
     * @method static TES4RecordType LVSP()
     * @method static TES4RecordType MGEF()
     * @method static TES4RecordType MISC()
     * @method static TES4RecordType NPC_()
     * @method static TES4RecordType PACK()
     * @method static TES4RecordType PGRD()
     * @method static TES4RecordType QUST()
     * @method static TES4RecordType RACE()
     * @method static TES4RecordType REFR()
     * @method static TES4RecordType REGN()
     * @method static TES4RecordType ROAD()
     * @method static TES4RecordType SBSP()
     * @method static TES4RecordType SCPT()
     * @method static TES4RecordType SGST()
     * @method static TES4RecordType SKIL()
     * @method static TES4RecordType SLGM()
     * @method static TES4RecordType SOUN()
     * @method static TES4RecordType SPEL()
     * @method static TES4RecordType STAT()
     * @method static TES4RecordType TES4()
     * @method static TES4RecordType TREE()
     * @method static TES4RecordType WATR()
     * @method static TES4RecordType WEAP()
     * @method static TES4RecordType WRLD()
     * @method static TES4RecordType WTHR()
     */
    public class TES4RecordType
    {
        public string Name { get; }
        private TES4RecordType(string name)
        {
            Name = name;
        }

        public static readonly TES4RecordType
            ACHR = new TES4RecordType("ACHR"),
            ACRE = new TES4RecordType("ACRE"),
            ACTI = new TES4RecordType("ACTI"),
            ALCH = new TES4RecordType("ALCH"),
            AMMO = new TES4RecordType("AMMO"),
            ANIO = new TES4RecordType("ANIO"),
            APPA = new TES4RecordType("APPA"),
            ARMO = new TES4RecordType("ARMO"),
            BOOK = new TES4RecordType("BOOK"),
            BSGN = new TES4RecordType("BSGN"),
            CELL = new TES4RecordType("CELL"),
            CLAS = new TES4RecordType("CLAS"),
            CLMT = new TES4RecordType("CLMT"),
            CLOT = new TES4RecordType("CLOT"),
            CONT = new TES4RecordType("CONT"),
            CREA = new TES4RecordType("CREA"),
            CSTY = new TES4RecordType("CSTY"),
            DIAL = new TES4RecordType("DIAL"),
            DOOR = new TES4RecordType("DOOR"),
            EFSH = new TES4RecordType("EFSH"),
            ENCH = new TES4RecordType("ENCH"),
            EYES = new TES4RecordType("EYES"),
            FACT = new TES4RecordType("FACT"),
            FLOR = new TES4RecordType("FLOR"),
            FURN = new TES4RecordType("FURN"),
            GLOB = new TES4RecordType("GLOB"),
            GMST = new TES4RecordType("GMST"),
            GRAS = new TES4RecordType("GRAS"),
            HAIR = new TES4RecordType("HAIR"),
            IDLE = new TES4RecordType("IDLE"),
            INFO = new TES4RecordType("INFO"),
            INGR = new TES4RecordType("INGR"),
            KEYM = new TES4RecordType("KEYM"),
            LAND = new TES4RecordType("LAND"),
            LIGH = new TES4RecordType("LIGH"),
            LSCR = new TES4RecordType("LSCR"),
            LTEX = new TES4RecordType("LTEX"),
            LVLC = new TES4RecordType("LVLC"),
            LVLI = new TES4RecordType("LVLI"),
            LVSP = new TES4RecordType("LVSP"),
            MGEF = new TES4RecordType("MGEF"),
            MISC = new TES4RecordType("MISC"),
            NPC_ = new TES4RecordType("NPC_"),
            PACK = new TES4RecordType("PACK"),
            PGRD = new TES4RecordType("PGRD"),
            QUST = new TES4RecordType("QUST"),
            RACE = new TES4RecordType("RACE"),
            REFR = new TES4RecordType("REFR"),
            REGN = new TES4RecordType("REGN"),
            ROAD = new TES4RecordType("ROAD"),
            SBSP = new TES4RecordType("SBSP"),
            SCPT = new TES4RecordType("SCPT"),
            SGST = new TES4RecordType("SGST"),
            SKIL = new TES4RecordType("SKIL"),
            SLGM = new TES4RecordType("SLGM"),
            SOUN = new TES4RecordType("SOUN"),
            SPEL = new TES4RecordType("SPEL"),
            STAT = new TES4RecordType("STAT"),
            TES4 = new TES4RecordType("TES4"),
            TREE = new TES4RecordType("TREE"),
            WATR = new TES4RecordType("WATR"),
            WEAP = new TES4RecordType("WEAP"),
            WRLD = new TES4RecordType("WRLD"),
            WTHR = new TES4RecordType("WTHR");

        private static readonly TES4RecordType[] all = new TES4RecordType[]
        {
            ACHR,
            ACRE,
            ACTI,
            ALCH,
            AMMO,
            ANIO,
            APPA,
            ARMO,
            BOOK,
            BSGN,
            CELL,
            CLAS,
            CLMT,
            CLOT,
            CONT,
            CREA,
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
            LVLC,
            LVLI,
            LVSP,
            MGEF,
            MISC,
            NPC_,
            PACK,
            PGRD,
            QUST,
            RACE,
            REFR,
            REGN,
            ROAD,
            SBSP,
            SCPT,
            SGST,
            SKIL,
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

        public static TES4RecordType First(string name)
        {
            return all.Where(t => t.Name == name).First();
        }
    }
}