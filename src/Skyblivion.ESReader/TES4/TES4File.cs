using Skyblivion.ESReader.Exceptions;
using Skyblivion.ESReader.Extensions.IDictionaryExtensions;
using Skyblivion.ESReader.Extensions.StreamExtensions;
using Skyblivion.ESReader.Extensions.TextReaderExtensions;
using Skyblivion.ESReader.PHP;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Skyblivion.ESReader.TES4
{
    public class TES4File
    {
        const int TES4_HEADER_SIZE = 0x18;
        private string path;
        private string name;
        private List<string> masters = new List<string>();
        private bool initialized = false;
        private Dictionary<TES4RecordType, TES4Grup> grups = new Dictionary<TES4RecordType, TES4Grup>();
        private TES4Collection collection;
        /*
        * File constructor.
        */
        public TES4File(TES4Collection collection, string path, string name)
        {
            this.collection = collection;
            this.path = path;
            this.name = name;
        }

        public string getName()
        {
            return this.name;
        }

        public List<string> getMasters()
        {
            if (!this.initialized)
            {
                this.initialize();
            }

            return this.masters;
        }

        public static readonly Lazy<Encoding> ISO_8859_1 = new Lazy<Encoding>(() => Encoding.GetEncoding("iso-8859-1"));
        private FileStream GetFile()
        {
            string filePath = this.path + Path.DirectorySeparatorChar + this.name;
            return new FileStream(filePath, FileMode.Open);
        }

        public IEnumerable<ITES4Record> load(TES4FileLoadScheme scheme)
        {
            Console.Write("Processing " + nameof(TES4File) + " Data...");
            using (FileStream contents = GetFile())
            {
                this.fetchTES4(contents);
                while (true)
                {
                    byte[] headerBytes = new byte[TES4Grup.GRUP_HEADER_SIZE];
                    int read = contents.Read(headerBytes);
                    if (read == 0) { break; }
                    string header = ISO_8859_1.Value.GetString(headerBytes);
                    if (header.Substring(0, 4) != "GRUP")
                    {
                        throw new InvalidESFileException("Invalid GRUP magic, found " + header.Substring(0, 4));
                    }
                    contents.Seek(-TES4Grup.GRUP_HEADER_SIZE, SeekOrigin.Current);

                    int grupSize = PHPFunction.UnpackV(header.Substring(4, 4));
                    TES4RecordType grupType = TES4RecordType.First(header.Substring(8, 4));
                    TES4Grup grup = new TES4Grup();
                    if (scheme.shouldLoad(grupType))
                    {
                        foreach (var loadedRecord in grup.load(contents, this, scheme.getRulesFor(grupType), true))
                        {
                            yield return loadedRecord;
                        }
                    }
                    else
                    {
                        contents.Seek(grupSize, SeekOrigin.Current);
                    }

                    this.grups.Add(grup.getType(), grup);
                }
            }
            Console.WriteLine("\rProcessing " + nameof(TES4File) + " Complete");
        }

        public TES4Grup getGrup(TES4RecordType type)
        {
            return this.grups.GetWithFallback(type, () => null);
        }

        public int expand(int formid)
        {
            return this.collection.expand(formid, this.getName());
        }

        private TES4LoadedRecord fetchTES4(FileStream stream)
        {
            byte[] recordHeader = stream.Read(TES4LoadedRecord.RECORD_HEADER_SIZE);
            int recordSize = PHPFunction.UnpackV(recordHeader.Skip(4).Take(4).ToArray());//Throw away the first four bytes.
            int recordFlags = PHPFunction.UnpackV(recordHeader.Skip(8).Take(4).ToArray());
            int recordFormid = PHPFunction.UnpackV(recordHeader.Skip(12).Take(4).ToArray());
            TES4LoadedRecord tes4record = new TES4LoadedRecord(this, TES4RecordType.TES4, recordFormid, recordSize, recordFlags);
            tes4record.load(stream, new TES4RecordLoadScheme(new string[] { "MAST" }));
            return tes4record;
        }

        private void initialize()
        {
            TES4LoadedRecord tes4record;
            using (FileStream file = GetFile())
            {
                tes4record = this.fetchTES4(file);
            }
            masters = tes4record.getSubrecords("MAST");
            this.initialized = true;
        }
    }
}