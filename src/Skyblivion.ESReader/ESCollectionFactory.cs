namespace Skyblivion.ESReader
{
    public static class ESCollectionFactory
    {
        public static ESCollection Create(string dataFileDirectory, string dataFile)
        {
            FileLoadScheme fileScheme = new FileLoadScheme
            {
                {
                    ESRecordType.GMST,
                    new GrupLoadScheme()
                    {
                        { ESRecordType.GMST, new RecordLoadScheme(new string[] { "EDID" }) }
                    }
                },
                {
                    ESRecordType.GLOB,
                    new GrupLoadScheme()
                    {
                        { ESRecordType.GLOB, new RecordLoadScheme(new string[] { "EDID" }) }
                    }
                },
                {
                    ESRecordType.CLAS,
                    new GrupLoadScheme()
                    {
                        { ESRecordType.CLAS, new RecordLoadScheme(new string[] { "EDID" }) }
                    }
                },
                {
                    ESRecordType.FACT,
                    new GrupLoadScheme()
                    {
                        { ESRecordType.FACT, new RecordLoadScheme(new string[] { "EDID" }) }
                    }
                },
                {
                    ESRecordType.HAIR,
                    new GrupLoadScheme()
                    {
                        { ESRecordType.HAIR, new RecordLoadScheme(new string[] { "EDID" }) }
                    }
                },
                {
                    ESRecordType.EYES,
                    new GrupLoadScheme()
                    {
                        { ESRecordType.EYES, new RecordLoadScheme(new string[] { "EDID" }) }
                    }
                },
                {
                    ESRecordType.RACE,
                    new GrupLoadScheme()
                    {
                        { ESRecordType.RACE, new RecordLoadScheme(new string[] { "EDID" }) }
                    }
                },
                {
                    ESRecordType.SOUN,
                    new GrupLoadScheme()
                    {
                        { ESRecordType.SOUN, new RecordLoadScheme(new string[] { "EDID" }) }
                    }
                },
                {
                    TES4RecordType.SKIL,
                    new GrupLoadScheme()
                    {
                        { TES4RecordType.SKIL, new RecordLoadScheme(new string[] { "EDID" }) }
                    }
                },
                {
                    ESRecordType.MGEF,
                    new GrupLoadScheme()
                    {
                        { ESRecordType.MGEF, new RecordLoadScheme(new string[] { "EDID" }) }
                    }
                },
                {
                    TES4RecordType.SCPT,
                    new GrupLoadScheme()
                    {
                        { TES4RecordType.SCPT, new RecordLoadScheme(new string[] { "EDID", "SCHR", "SCRO" }) }//WTM:  Change:  Added SCRO for use by ESMAnalyzer.GetTypesFromSCRO.
                    }
                },
                {
                    ESRecordType.LTEX,
                    new GrupLoadScheme()
                    {
                        { ESRecordType.LTEX, new RecordLoadScheme(new string[] { "EDID" }) }
                    }
                },
                {
                    ESRecordType.ENCH,
                    new GrupLoadScheme()
                    {
                        { ESRecordType.ENCH, new RecordLoadScheme(new string[] { "EDID" }) }
                    }
                },
                {
                    ESRecordType.SPEL,
                    new GrupLoadScheme()
                    {
                        { ESRecordType.SPEL, new RecordLoadScheme(new string[] { "EDID" }) }
                    }
                },
                {
                    TES4RecordType.BSGN,
                    new GrupLoadScheme()
                    {
                        { TES4RecordType.BSGN, new RecordLoadScheme(new string[] { "EDID" }) }
                    }
                },
                {
                    ESRecordType.ACTI,
                    new GrupLoadScheme()
                    {
                        { ESRecordType.ACTI, new RecordLoadScheme(new string[] { "EDID", "SCRI" }) }
                    }
                },
                {
                    ESRecordType.APPA,
                    new GrupLoadScheme()
                    {
                        { ESRecordType.APPA, new RecordLoadScheme(new string[] { "EDID" }) }
                    }
                },
                {
                    ESRecordType.ARMO,
                    new GrupLoadScheme()
                    {
                        { ESRecordType.ARMO, new RecordLoadScheme(new string[] { "EDID" }) }
                    }
                },
                {
                    ESRecordType.BOOK,
                    new GrupLoadScheme()
                    {
                        { ESRecordType.BOOK, new RecordLoadScheme(new string[] { "EDID" }) }
                    }
                },
                {
                    TES4RecordType.CLOT,
                    new GrupLoadScheme()
                    {
                        { TES4RecordType.CLOT, new RecordLoadScheme(new string[] { "EDID" }) }
                    }
                },
                {
                    ESRecordType.CONT,
                    new GrupLoadScheme()
                    {
                        { ESRecordType.CONT, new RecordLoadScheme(new string[] { "EDID", "SCRI" }) }
                    }
                },
                {
                    ESRecordType.DOOR,
                    new GrupLoadScheme()
                    {
                        { ESRecordType.DOOR, new RecordLoadScheme(new string[] { "EDID", "SCRI" }) }
                    }
                },
                {
                    ESRecordType.INGR,
                    new GrupLoadScheme()
                    {
                        { ESRecordType.INGR, new RecordLoadScheme(new string[] { "EDID" }) }
                    }
                },
                {
                    ESRecordType.LIGH,
                    new GrupLoadScheme()
                    {
                        { ESRecordType.LIGH, new RecordLoadScheme(new string[] { "EDID" }) }
                    }
                },
                {
                    ESRecordType.MISC,
                    new GrupLoadScheme()
                    {
                        { ESRecordType.MISC, new RecordLoadScheme(new string[] { "EDID", "SCRI" }) }
                    }
                },
                {
                    ESRecordType.STAT,
                    new GrupLoadScheme()
                    {
                        { ESRecordType.STAT, new RecordLoadScheme(new string[] { "EDID" }) }
                    }
                },
                {
                    ESRecordType.GRAS,
                    new GrupLoadScheme()
                    {
                        { ESRecordType.GRAS, new RecordLoadScheme(new string[] { "EDID" }) }
                    }
                },
                {
                    ESRecordType.TREE,
                    new GrupLoadScheme()
                    {
                        { ESRecordType.TREE, new RecordLoadScheme(new string[] { "EDID" }) }
                    }
                },
                {
                    ESRecordType.FLOR,
                    new GrupLoadScheme()
                    {
                        { ESRecordType.FLOR, new RecordLoadScheme(new string[] { "EDID" }) }
                    }
                },
                {
                    ESRecordType.FURN,
                    new GrupLoadScheme()
                    {
                        { ESRecordType.FURN, new RecordLoadScheme(new string[] { "EDID" }) }
                    }
                },
                {
                    ESRecordType.WEAP,
                    new GrupLoadScheme()
                    {
                        { ESRecordType.WEAP, new RecordLoadScheme(new string[] { "EDID", "SCRI" }) }
                    }
                },
                {
                    ESRecordType.AMMO,
                    new GrupLoadScheme()
                    {
                        { ESRecordType.AMMO, new RecordLoadScheme(new string[] { "EDID" }) }
                    }
                },
                {
                    ESRecordType.NPC_,
                    new GrupLoadScheme()
                    {
                        { ESRecordType.NPC_, new RecordLoadScheme(new string[] { "EDID", "SCRI" }) }
                    }
                },
                {
                    TES4RecordType.CREA,
                    new GrupLoadScheme()
                    {
                        { TES4RecordType.CREA, new RecordLoadScheme(new string[] { "EDID", "SCRI" }) }
                    }
                },
                {
                    TES4RecordType.LVLC,
                    new GrupLoadScheme()
                    {
                        { TES4RecordType.LVLC, new RecordLoadScheme(new string[] { "EDID" }) }
                    }
                },
                {
                    ESRecordType.SLGM,
                    new GrupLoadScheme()
                    {
                        { ESRecordType.SLGM, new RecordLoadScheme(new string[] { "EDID" }) }
                    }
                },
                {
                    ESRecordType.KEYM,
                    new GrupLoadScheme()
                    {
                        { ESRecordType.KEYM, new RecordLoadScheme(new string[] { "EDID" }) }
                    }
                },
                {
                    ESRecordType.ALCH,
                    new GrupLoadScheme()
                    {
                        { ESRecordType.ALCH, new RecordLoadScheme(new string[] { "EDID" }) }
                    }
                },
                {
                    TES4RecordType.SBSP,
                    new GrupLoadScheme()
                    {
                        { TES4RecordType.SBSP, new RecordLoadScheme(new string[] { "EDID" }) }
                    }
                },
                {
                    TES4RecordType.SGST,
                    new GrupLoadScheme()
                    {
                        { TES4RecordType.SGST, new RecordLoadScheme(new string[] { "EDID" }) }
                    }
                },
                {
                    ESRecordType.LVLI,
                    new GrupLoadScheme()
                    {
                        { ESRecordType.LVLI, new RecordLoadScheme(new string[] { "EDID" }) }
                    }
                },
                {
                    ESRecordType.WTHR,
                    new GrupLoadScheme()
                    {
                        { ESRecordType.WTHR, new RecordLoadScheme(new string[] { "EDID" }) }
                    }
                },
                {
                    ESRecordType.CLMT,
                    new GrupLoadScheme()
                    {
                        { ESRecordType.CLMT, new RecordLoadScheme(new string[] { "EDID" }) }
                    }
                },
                {
                    ESRecordType.REGN,
                    new GrupLoadScheme()
                    {
                        { ESRecordType.REGN, new RecordLoadScheme(new string[] { "EDID" }) }
                    }
                },
                {
                    ESRecordType.CELL,
                    new GrupLoadScheme()
                    {
                        { ESRecordType.CELL, new RecordLoadScheme(new string[] { "EDID", "FULL" }) },//WTM:  Change:  Added FULL for use from GetInCellFactory.cs.
                        { ESRecordType.REFR, new RecordLoadScheme(new string[] { "EDID", "NAME" }) },
                        { ESRecordType.ACHR, new RecordLoadScheme(new string[] { "EDID", "NAME" }) },
                        { TES4RecordType.ACRE, new RecordLoadScheme(new string[] { "EDID", "NAME" }) }
                    }
                },
                {
                    ESRecordType.WRLD,
                    new GrupLoadScheme()
                    {
                        { ESRecordType.WRLD, new RecordLoadScheme(new string[] { "EDID" }) },
                        { ESRecordType.CELL, new RecordLoadScheme(new string[] { "EDID" }) },
                        { ESRecordType.REFR, new RecordLoadScheme(new string[] { "EDID", "NAME" }) },
                        { ESRecordType.ACHR, new RecordLoadScheme(new string[] { "EDID", "NAME" }) },
                        { TES4RecordType.ACRE, new RecordLoadScheme(new string[] { "EDID", "NAME" }) }
                    }
                },
                {
                    ESRecordType.DIAL,
                    new GrupLoadScheme()
                    {
                        { ESRecordType.DIAL, new RecordLoadScheme(new string[] { "EDID" }) },
                        { ESRecordType.INFO, new RecordLoadScheme(new string[] { "NAME", "SCRO" }) }//WTM:  Change:  Added INFO line and SCRO argument for use by ESMAnalyzer.GetTypesFromSCRO.  Added NAME argument for use by TranspileChunkJob.GenerateINFOAddTopicScripts.
                    }
                },
                {
                    ESRecordType.QUST,
                    new GrupLoadScheme()
                    {
                        { ESRecordType.QUST, new RecordLoadScheme(new string[] { "EDID", "SCRI", "INDX", "SCRO", "QSTA", "QSDT", "CTDA" }) }//WTM:  Change:  Added INDX and SCRO for use by ESMAnalyzer.GetTypesFromSCRO.  Added QSTA, QSDT, and CTDA to retrieve information previously retrieved from BuildTarget files.
                    }
                },
                {
                    ESRecordType.IDLE,
                    new GrupLoadScheme()
                    {
                        { ESRecordType.IDLE, new RecordLoadScheme(new string[] { "EDID" }) }
                    }
                },
                {
                    ESRecordType.PACK,
                    new GrupLoadScheme()
                    {
                        { ESRecordType.PACK, new RecordLoadScheme(new string[] { "EDID" }) }
                    }
                },
                {
                    ESRecordType.CSTY,
                    new GrupLoadScheme()
                    {
                        { ESRecordType.CSTY, new RecordLoadScheme(new string[] { "EDID" }) }
                    }
                },
                {
                    ESRecordType.LSCR,
                    new GrupLoadScheme()
                    {
                        { ESRecordType.LSCR, new RecordLoadScheme(new string[] { "EDID" }) }
                    }
                },
                {
                    ESRecordType.LVSP,
                    new GrupLoadScheme()
                    {
                        { ESRecordType.LVSP, new RecordLoadScheme(new string[] { "EDID" }) }
                    }
                },
                {
                    ESRecordType.ANIO,
                    new GrupLoadScheme()
                    {
                        { ESRecordType.ANIO, new RecordLoadScheme(new string[] { "EDID" }) }
                    }
                },
                {
                    ESRecordType.WATR,
                    new GrupLoadScheme()
                    {
                        { ESRecordType.WATR, new RecordLoadScheme(new string[] { "EDID" }) }
                    }
                },
                {
                    ESRecordType.EFSH,
                    new GrupLoadScheme()
                    {
                        { ESRecordType.EFSH, new RecordLoadScheme(new string[] { "EDID" }) }
                    }
                }
            };
            ESCollection collection = new ESCollection(dataFileDirectory);
            collection.Add(dataFile);
            collection.Load(fileScheme);
            return collection;
        }

        public static ESCollection CreateForScriptExporting(string dataFileDirectory, string dataFile)
        {
            FileLoadScheme fileScheme = new FileLoadScheme()
            {
                {
                    TES4RecordType.SCPT,
                    new GrupLoadScheme()
                    {
                        { TES4RecordType.SCPT, new RecordLoadScheme(new string[] { "EDID", "SCTX" }) }
                    }
                },
                {
                    ESRecordType.DIAL,
                    new GrupLoadScheme()
                    {
                        { ESRecordType.DIAL, new RecordLoadScheme(new string[] { "EDID" }) },
                        { ESRecordType.INFO, new RecordLoadScheme(new string[] { "EDID", "SCTX" }) }
                    }
                },
                {
                    ESRecordType.QUST,
                    new GrupLoadScheme()
                    {
                        { ESRecordType.QUST, new RecordLoadScheme(new string[] { "EDID", "INDX", "QSDT", "SCTX" }) }
                    }
                }
            };
            ESCollection collection = new ESCollection(dataFileDirectory);
            collection.Add(dataFile);
            collection.Load(fileScheme);
            return collection;
        }

        public static ESCollection CreateForQUSTReferenceAliasExporting(string dataFileDirectory, string dataFile)
        {
            FileLoadScheme fileScheme = new FileLoadScheme()
            {
                {
                    ESRecordType.QUST,
                    new GrupLoadScheme()
                    {
                        { ESRecordType.QUST, new RecordLoadScheme(new string[] { "EDID", "QSTA" }) }
                    }
                }
            };
            ESCollection collection = new ESCollection(dataFileDirectory);
            collection.Add(dataFile);
            collection.Load(fileScheme);
            return collection;
        }

        public static ESCollection CreateForQUSTStageMapExportingFromPSCFiles(string dataFileDirectory, string dataFile)
        {
            FileLoadScheme fileScheme = new FileLoadScheme
            {
                {
                    ESRecordType.QUST,
                    new GrupLoadScheme()
                    {
                        { ESRecordType.QUST, new RecordLoadScheme(new string[] { "EDID" }) }
                    }
                }
            };
            ESCollection collection = new ESCollection(dataFileDirectory);
            collection.Add(dataFile);
            collection.Load(fileScheme);
            return collection;
        }

        public static ESCollection CreateForQUSTStageMapExportingFromESM(string dataFileDirectory, string dataFile)
        {
            FileLoadScheme fileScheme = new FileLoadScheme
            {
                {
                    ESRecordType.QUST,
                    new GrupLoadScheme()
                    {
                        { ESRecordType.QUST, new RecordLoadScheme(new string[] { "EDID", "INDX", "QSTA", "CTDA" }) }
                    }
                }
            };
            ESCollection collection = new ESCollection(dataFileDirectory);
            collection.Add(dataFile);
            collection.Load(fileScheme);
            return collection;
        }
    }
}
