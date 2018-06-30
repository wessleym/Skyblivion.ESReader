using Skyblivion.ESReader.Exceptions;
using Skyblivion.ESReader.Extensions.IDictionaryExtensions;
using Skyblivion.ESReader.Extensions.StreamExtensions;
using Skyblivion.ESReader.PHP;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace Skyblivion.ESReader.TES4
{
    public class TES4File
    {
        const int TES4_HEADER_SIZE = 0x18;
        private string path;
        public string Name { get; private set; }
        private Lazy<string[]> masters;
        private Dictionary<TES4RecordType, TES4Grup> grups = new Dictionary<TES4RecordType, TES4Grup>();
        private TES4Collection collection;
        /*
        * File constructor.
        */
        public TES4File(TES4Collection collection, string path, string name)
        {
            this.collection = collection;
            this.path = path;
            this.Name = name;
            masters = new Lazy<string[]>(() =>
              {
                  TES4LoadedRecord tes4record;
                  using (FileStream file = GetFile())
                  {
                      tes4record = this.FetchTES4(file);
                  }
                  return tes4record.getSubrecordsStrings("MAST");
              });
        }

        public string[] Masters => this.masters.Value;

        public static readonly Lazy<Encoding> ISO_8859_1 = new Lazy<Encoding>(() => Encoding.GetEncoding("iso-8859-1"));
        private FileStream GetFile()
        {
            string filePath = Path.Combine(this.path, this.Name);
            return new FileStream(filePath, FileMode.Open);
        }

        public IEnumerable<ITES4Record> load(TES4FileLoadScheme scheme)
        {
            Console.Write("Processing " + nameof(TES4File) + " Data...");
            Stopwatch stopwatch = Stopwatch.StartNew();
            using (FileStream contents = GetFile())
            {
                this.FetchTES4(contents);
                while (true)
                {
                    byte[] headerBytes = new byte[TES4Grup.GRUP_HEADER_SIZE];
                    int read = contents.Read(headerBytes);
                    if (read == 0) { break; }
                    string headerString = ISO_8859_1.Value.GetString(headerBytes);
                    if (headerString.Substring(0, 4) != "GRUP")
                    {
                        throw new InvalidESFileException("Invalid GRUP magic, found " + headerString.Substring(0, 4));
                    }
                    contents.Seek(-TES4Grup.GRUP_HEADER_SIZE, SeekOrigin.Current);

                    int grupSize = PHPFunction.UnpackV(headerBytes.Skip(4).Take(4).ToArray());
                    TES4RecordType grupType = TES4RecordType.First(headerString.Substring(8, 4));
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
            stopwatch.Stop();
            Console.WriteLine("\rProcessing " + nameof(TES4File) + " Complete (" + stopwatch.ElapsedMilliseconds + " ms)");
        }

        public TES4Grup GetGrup(TES4RecordType type)
        {
            return this.grups.GetWithFallback(type, () => null);
        }

        public int Expand(int formid)
        {
            return this.collection.expand(formid, this.Name);
        }

        private TES4LoadedRecord FetchTES4(FileStream stream)
        {
            byte[] recordHeader = stream.Read(TES4LoadedRecord.RECORD_HEADER_SIZE);
            int recordSize = PHPFunction.UnpackV(recordHeader.Skip(4).Take(4).ToArray());//Throw away the first four bytes.
            int recordFlags = PHPFunction.UnpackV(recordHeader.Skip(8).Take(4).ToArray());
            int recordFormid = PHPFunction.UnpackV(recordHeader.Skip(12).Take(4).ToArray());
            TES4LoadedRecord tes4record = new TES4LoadedRecord(this, TES4RecordType.TES4, recordFormid, recordSize, recordFlags);
            tes4record.load(stream, new TES4RecordLoadScheme(new string[] { "MAST" }));
            return tes4record;
        }
    }
}