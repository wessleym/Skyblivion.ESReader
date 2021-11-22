namespace Skyblivion.ESReader.TES4
{
    public static class TES4CollectionFactory
    {
        public static TES4Collection Create(string dataFileDirectory, string dataFile)
        {
            //NOTE - SCRI record load scheme is a copypasta, as in, i didnt check which records do actually might have SCRI
            //Doesnt really matter for other purposes than cleaniness
            TES4FileLoadScheme fileScheme = new TES4FileLoadScheme
            {
                {
                    TES4RecordType.GMST,
                    new TES4GrupLoadScheme()
                    {
                        { TES4RecordType.GMST, new TES4RecordLoadScheme(new string[] { "EDID", "SCRI" }) }
                    }
                },
                {
                    TES4RecordType.GLOB,
                    new TES4GrupLoadScheme()
                    {
                        { TES4RecordType.GLOB, new TES4RecordLoadScheme(new string[] { "EDID", "SCRI" }) }
                    }
                },
                {
                    TES4RecordType.CLAS,
                    new TES4GrupLoadScheme()
                    {
                        { TES4RecordType.CLAS, new TES4RecordLoadScheme(new string[] { "EDID", "SCRI" }) }
                    }
                },
                {
                    TES4RecordType.FACT,
                    new TES4GrupLoadScheme()
                    {
                        { TES4RecordType.FACT, new TES4RecordLoadScheme(new string[] { "EDID", "SCRI" }) }
                    }
                },
                {
                    TES4RecordType.HAIR,
                    new TES4GrupLoadScheme()
                    {
                        { TES4RecordType.HAIR, new TES4RecordLoadScheme(new string[] { "EDID", "SCRI" }) }
                    }
                },
                {
                    TES4RecordType.EYES,
                    new TES4GrupLoadScheme()
                    {
                        { TES4RecordType.EYES, new TES4RecordLoadScheme(new string[] { "EDID", "SCRI" }) }
                    }
                },
                {
                    TES4RecordType.RACE,
                    new TES4GrupLoadScheme()
                    {
                        { TES4RecordType.RACE, new TES4RecordLoadScheme(new string[] { "EDID", "SCRI" }) }
                    }
                },
                {
                    TES4RecordType.SOUN,
                    new TES4GrupLoadScheme()
                    {
                        { TES4RecordType.SOUN, new TES4RecordLoadScheme(new string[] { "EDID", "SCRI" }) }
                    }
                },
                {
                    TES4RecordType.SKIL,
                    new TES4GrupLoadScheme()
                    {
                        { TES4RecordType.SKIL, new TES4RecordLoadScheme(new string[] { "EDID", "SCRI" }) }
                    }
                },
                {
                    TES4RecordType.MGEF,
                    new TES4GrupLoadScheme()
                    {
                        { TES4RecordType.MGEF, new TES4RecordLoadScheme(new string[] { "EDID", "SCRI" }) }
                    }
                },
                {
                    TES4RecordType.SCPT,
                    new TES4GrupLoadScheme()
                    {
                        { TES4RecordType.SCPT, new TES4RecordLoadScheme(new string[] { "EDID", "SCRI", "SCHR", "SCRO" }) }//WTM:  Change:  Added SCRO for use by ESMAnalyzer.GetTypesFromSCRO.
                    }
                },
                {
                    TES4RecordType.LTEX,
                    new TES4GrupLoadScheme()
                    {
                        { TES4RecordType.LTEX, new TES4RecordLoadScheme(new string[] { "EDID", "SCRI" }) }
                    }
                },
                {
                    TES4RecordType.ENCH,
                    new TES4GrupLoadScheme()
                    {
                        { TES4RecordType.ENCH, new TES4RecordLoadScheme(new string[] { "EDID", "SCRI" }) }
                    }
                },
                {
                    TES4RecordType.SPEL,
                    new TES4GrupLoadScheme()
                    {
                        { TES4RecordType.SPEL, new TES4RecordLoadScheme(new string[] { "EDID", "SCRI" }) }
                    }
                },
                {
                    TES4RecordType.BSGN,
                    new TES4GrupLoadScheme()
                    {
                        { TES4RecordType.BSGN, new TES4RecordLoadScheme(new string[] { "EDID", "SCRI" }) }
                    }
                },
                {
                    TES4RecordType.ACTI,
                    new TES4GrupLoadScheme()
                    {
                        { TES4RecordType.ACTI, new TES4RecordLoadScheme(new string[] { "EDID", "SCRI" }) }
                    }
                },
                {
                    TES4RecordType.APPA,
                    new TES4GrupLoadScheme()
                    {
                        { TES4RecordType.APPA, new TES4RecordLoadScheme(new string[] { "EDID", "SCRI" }) }
                    }
                },
                {
                    TES4RecordType.ARMO,
                    new TES4GrupLoadScheme()
                    {
                        { TES4RecordType.ARMO, new TES4RecordLoadScheme(new string[] { "EDID", "SCRI" }) }
                    }
                },
                {
                    TES4RecordType.BOOK,
                    new TES4GrupLoadScheme()
                    {
                        { TES4RecordType.BOOK, new TES4RecordLoadScheme(new string[] { "EDID", "SCRI" }) }
                    }
                },
                {
                    TES4RecordType.CLOT,
                    new TES4GrupLoadScheme()
                    {
                        { TES4RecordType.CLOT, new TES4RecordLoadScheme(new string[] { "EDID", "SCRI" }) }
                    }
                },
                {
                    TES4RecordType.CONT,
                    new TES4GrupLoadScheme()
                    {
                        { TES4RecordType.CONT, new TES4RecordLoadScheme(new string[] { "EDID", "SCRI" }) }
                    }
                },
                {
                    TES4RecordType.DOOR,
                    new TES4GrupLoadScheme()
                    {
                        { TES4RecordType.DOOR, new TES4RecordLoadScheme(new string[] { "EDID", "SCRI" }) }
                    }
                },
                {
                    TES4RecordType.INGR,
                    new TES4GrupLoadScheme()
                    {
                        { TES4RecordType.INGR, new TES4RecordLoadScheme(new string[] { "EDID", "SCRI" }) }
                    }
                },
                {
                    TES4RecordType.LIGH,
                    new TES4GrupLoadScheme()
                    {
                        { TES4RecordType.LIGH, new TES4RecordLoadScheme(new string[] { "EDID", "SCRI" }) }
                    }
                },
                {
                    TES4RecordType.MISC,
                    new TES4GrupLoadScheme()
                    {
                        { TES4RecordType.MISC, new TES4RecordLoadScheme(new string[] { "EDID", "SCRI" }) }
                    }
                },
                {
                    TES4RecordType.STAT,
                    new TES4GrupLoadScheme()
                    {
                        { TES4RecordType.STAT, new TES4RecordLoadScheme(new string[] { "EDID", "SCRI" }) }
                    }
                },
                {
                    TES4RecordType.GRAS,
                    new TES4GrupLoadScheme()
                    {
                        { TES4RecordType.GRAS, new TES4RecordLoadScheme(new string[] { "EDID", "SCRI" }) }
                    }
                },
                {
                    TES4RecordType.TREE,
                    new TES4GrupLoadScheme()
                    {
                        { TES4RecordType.TREE, new TES4RecordLoadScheme(new string[] { "EDID", "SCRI" }) }
                    }
                },
                {
                    TES4RecordType.FLOR,
                    new TES4GrupLoadScheme()
                    {
                        { TES4RecordType.FLOR, new TES4RecordLoadScheme(new string[] { "EDID", "SCRI" }) }
                    }
                },
                {
                    TES4RecordType.FURN,
                    new TES4GrupLoadScheme()
                    {
                        { TES4RecordType.FURN, new TES4RecordLoadScheme(new string[] { "EDID", "SCRI" }) }
                    }
                },
                {
                    TES4RecordType.WEAP,
                    new TES4GrupLoadScheme()
                    {
                        { TES4RecordType.WEAP, new TES4RecordLoadScheme(new string[] { "EDID", "SCRI" }) }
                    }
                },
                {
                    TES4RecordType.AMMO,
                    new TES4GrupLoadScheme()
                    {
                        { TES4RecordType.AMMO, new TES4RecordLoadScheme(new string[] { "EDID", "SCRI" }) }
                    }
                },
                {
                    TES4RecordType.NPC_,
                    new TES4GrupLoadScheme()
                    {
                        { TES4RecordType.NPC_, new TES4RecordLoadScheme(new string[] { "EDID", "SCRI" }) }
                    }
                },
                {
                    TES4RecordType.CREA,
                    new TES4GrupLoadScheme()
                    {
                        { TES4RecordType.CREA, new TES4RecordLoadScheme(new string[] { "EDID", "SCRI" }) }
                    }
                },
                {
                    TES4RecordType.LVLC,
                    new TES4GrupLoadScheme()
                    {
                        { TES4RecordType.LVLC, new TES4RecordLoadScheme(new string[] { "EDID", "SCRI" }) }
                    }
                },
                {
                    TES4RecordType.SLGM,
                    new TES4GrupLoadScheme()
                    {
                        { TES4RecordType.SLGM, new TES4RecordLoadScheme(new string[] { "EDID", "SCRI" }) }
                    }
                },
                {
                    TES4RecordType.KEYM,
                    new TES4GrupLoadScheme()
                    {
                        { TES4RecordType.KEYM, new TES4RecordLoadScheme(new string[] { "EDID", "SCRI" }) }
                    }
                },
                {
                    TES4RecordType.ALCH,
                    new TES4GrupLoadScheme()
                    {
                        { TES4RecordType.ALCH, new TES4RecordLoadScheme(new string[] { "EDID", "SCRI" }) }
                    }
                },
                {
                    TES4RecordType.SBSP,
                    new TES4GrupLoadScheme()
                    {
                        { TES4RecordType.SBSP, new TES4RecordLoadScheme(new string[] { "EDID", "SCRI" }) }
                    }
                },
                {
                    TES4RecordType.SGST,
                    new TES4GrupLoadScheme()
                    {
                        { TES4RecordType.SGST, new TES4RecordLoadScheme(new string[] { "EDID", "SCRI" }) }
                    }
                },
                {
                    TES4RecordType.LVLI,
                    new TES4GrupLoadScheme()
                    {
                        { TES4RecordType.LVLI, new TES4RecordLoadScheme(new string[] { "EDID", "SCRI" }) }
                    }
                },
                {
                    TES4RecordType.WTHR,
                    new TES4GrupLoadScheme()
                    {
                        { TES4RecordType.WTHR, new TES4RecordLoadScheme(new string[] { "EDID", "SCRI" }) }
                    }
                },
                {
                    TES4RecordType.CLMT,
                    new TES4GrupLoadScheme()
                    {
                        { TES4RecordType.CLMT, new TES4RecordLoadScheme(new string[] { "EDID", "SCRI" }) }
                    }
                },
                {
                    TES4RecordType.REGN,
                    new TES4GrupLoadScheme()
                    {
                        { TES4RecordType.REGN, new TES4RecordLoadScheme(new string[] { "EDID", "SCRI" }) }
                    }
                },
                {
                    TES4RecordType.CELL,
                    new TES4GrupLoadScheme()
                    {
                        { TES4RecordType.CELL, new TES4RecordLoadScheme(new string[] { "EDID", "SCRI", "FULL" }) },//WTM:  Change:  Added FULL for use from GetInCellFactory.cs.
                        { TES4RecordType.REFR, new TES4RecordLoadScheme(new string[] { "EDID", "NAME" }) },
                        { TES4RecordType.ACHR, new TES4RecordLoadScheme(new string[] { "EDID", "NAME" }) },
                        { TES4RecordType.ACRE, new TES4RecordLoadScheme(new string[] { "EDID", "NAME" }) }
                    }
                },
                {
                    TES4RecordType.WRLD,
                    new TES4GrupLoadScheme()
                    {
                        { TES4RecordType.WRLD, new TES4RecordLoadScheme(new string[] { "EDID", "SCRI" }) },
                        { TES4RecordType.CELL, new TES4RecordLoadScheme(new string[] { "EDID", "SCRI" }) },
                        { TES4RecordType.REFR, new TES4RecordLoadScheme(new string[] { "EDID", "NAME" }) },
                        { TES4RecordType.ACHR, new TES4RecordLoadScheme(new string[] { "EDID", "NAME" }) },
                        { TES4RecordType.ACRE, new TES4RecordLoadScheme(new string[] { "EDID", "NAME" }) }
                    }
                },
                {
                    TES4RecordType.DIAL,
                    new TES4GrupLoadScheme()
                    {
                        { TES4RecordType.DIAL, new TES4RecordLoadScheme(new string[] { "EDID", "SCRI" }) },
                        { TES4RecordType.INFO, new TES4RecordLoadScheme(new string[] { "NAME", "SCRO" }) }//WTM:  Change:  Added INFO line and SCRO argument for use by ESMAnalyzer.GetTypesFromSCRO.  Added NAME argument for use by TranspileChunkJob.GenerateINFOAddTopicScripts.
                    }
                },
                {
                    TES4RecordType.QUST,
                    new TES4GrupLoadScheme()
                    {
                        { TES4RecordType.QUST, new TES4RecordLoadScheme(new string[] { "EDID", "SCRI", "INDX", "SCRO", "QSTA", "QSDT", "CTDA" }) }//WTM:  Change:  Added INDX and SCRO for use by ESMAnalyzer.GetTypesFromSCRO.  Added QSTA, QSDT, and CTDA to retrieve information previously retrieved from BuildTarget files.
                    }
                },
                {
                    TES4RecordType.IDLE,
                    new TES4GrupLoadScheme()
                    {
                        { TES4RecordType.IDLE, new TES4RecordLoadScheme(new string[] { "EDID", "SCRI" }) }
                    }
                },
                {
                    TES4RecordType.PACK,
                    new TES4GrupLoadScheme()
                    {
                        { TES4RecordType.PACK, new TES4RecordLoadScheme(new string[] { "EDID", "SCRI" }) }
                    }
                },
                {
                    TES4RecordType.CSTY,
                    new TES4GrupLoadScheme()
                    {
                        { TES4RecordType.CSTY, new TES4RecordLoadScheme(new string[] { "EDID", "SCRI" }) }
                    }
                },
                {
                    TES4RecordType.LSCR,
                    new TES4GrupLoadScheme()
                    {
                        { TES4RecordType.LSCR, new TES4RecordLoadScheme(new string[] { "EDID", "SCRI" }) }
                    }
                },
                {
                    TES4RecordType.LVSP,
                    new TES4GrupLoadScheme()
                    {
                        { TES4RecordType.LVSP, new TES4RecordLoadScheme(new string[] { "EDID", "SCRI" }) }
                    }
                },
                {
                    TES4RecordType.ANIO,
                    new TES4GrupLoadScheme()
                    {
                        { TES4RecordType.ANIO, new TES4RecordLoadScheme(new string[] { "EDID", "SCRI" }) }
                    }
                },
                {
                    TES4RecordType.WATR,
                    new TES4GrupLoadScheme()
                    {
                        { TES4RecordType.WATR, new TES4RecordLoadScheme(new string[] { "EDID", "SCRI" }) }
                    }
                },
                {
                    TES4RecordType.EFSH,
                    new TES4GrupLoadScheme()
                    {
                        { TES4RecordType.EFSH, new TES4RecordLoadScheme(new string[] { "EDID", "SCRI" }) }
                    }
                }
            };
            TES4Collection collection = new TES4Collection(dataFileDirectory);
            collection.Add(dataFile);
            collection.Load(fileScheme);
            return collection;
        }

        public static TES4Collection CreateForScriptExporting(string dataFileDirectory, string dataFile)
        {
            TES4FileLoadScheme fileScheme = new TES4FileLoadScheme()
            {
                {
                    TES4RecordType.SCPT,
                    new TES4GrupLoadScheme()
                    {
                        { TES4RecordType.SCPT, new TES4RecordLoadScheme(new string[] { "EDID", "SCTX" }) }
                    }
                },
                {
                    TES4RecordType.DIAL,
                    new TES4GrupLoadScheme()
                    {
                        { TES4RecordType.DIAL, new TES4RecordLoadScheme(new string[] { "EDID", "SCRI" }) },
                        { TES4RecordType.INFO, new TES4RecordLoadScheme(new string[] { "EDID", "SCTX" }) }
                    }
                },
                {
                    TES4RecordType.QUST,
                    new TES4GrupLoadScheme()
                    {
                        { TES4RecordType.QUST, new TES4RecordLoadScheme(new string[] { "EDID", "INDX", "QSDT", "SCTX" }) }
                    }
                }
            };
            TES4Collection collection = new TES4Collection(dataFileDirectory);
            collection.Add(dataFile);
            collection.Load(fileScheme);
            return collection;
        }

        public static TES4Collection CreateForQUSTReferenceAliasExporting(string dataFileDirectory, string dataFile)
        {
            TES4FileLoadScheme fileScheme = new TES4FileLoadScheme()
            {
                {
                    TES4RecordType.QUST,
                    new TES4GrupLoadScheme()
                    {
                        { TES4RecordType.QUST, new TES4RecordLoadScheme(new string[] { "EDID", "QSTA" }) }
                    }
                }
            };
            TES4Collection collection = new TES4Collection(dataFileDirectory);
            collection.Add(dataFile);
            collection.Load(fileScheme);
            return collection;
        }

        public static TES4Collection CreateForQUSTStageMapExportingFromPSCFiles(string dataFileDirectory, string dataFile)
        {
            TES4FileLoadScheme fileScheme = new TES4FileLoadScheme
            {
                {
                    TES4RecordType.QUST,
                    new TES4GrupLoadScheme()
                    {
                        { TES4RecordType.QUST, new TES4RecordLoadScheme(new string[] { "EDID" }) }
                    }
                }
            };
            TES4Collection collection = new TES4Collection(dataFileDirectory);
            collection.Add(dataFile);
            collection.Load(fileScheme);
            return collection;
        }

        public static TES4Collection CreateForQUSTStageMapExportingFromESM(string dataFileDirectory, string dataFile)
        {
            TES4FileLoadScheme fileScheme = new TES4FileLoadScheme
            {
                {
                    TES4RecordType.QUST,
                    new TES4GrupLoadScheme()
                    {
                        { TES4RecordType.QUST, new TES4RecordLoadScheme(new string[] { "EDID", "INDX", "QSTA", "CTDA" }) }
                    }
                }
            };
            TES4Collection collection = new TES4Collection(dataFileDirectory);
            collection.Add(dataFile);
            collection.Load(fileScheme);
            return collection;
        }
    }
}
