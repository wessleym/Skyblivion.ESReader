using Skyblivion.ESReader.Exceptions;
using Skyblivion.ESReader.Extensions;
using Skyblivion.ESReader.PHP;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace Skyblivion.ESReader
{
    public class ESFile
    {
        private readonly string path;
        public string Name { get; }
        private readonly Lazy<string[]> masters;
        private readonly Dictionary<ESRecordType, Grup> grups = new Dictionary<ESRecordType, Grup>();
        private readonly ESCollection collection;
        /*
        * File constructor.
        */
        public ESFile(ESCollection collection, string path, string name)
        {
            this.collection = collection;
            this.path = path;
            this.Name = name;
            masters = new Lazy<string[]>(() =>
              {
                  ESRecord tes4record;
                  using (FileStream file = GetFile())
                  {
                      tes4record = this.FetchTES4(file);
                  }
                  return tes4record.GetSubrecordsStringsTrim("MAST").ToArray();
              });
        }

        public string[] Masters => this.masters.Value;

        public static readonly Lazy<Encoding> ISO_8859_1 = new Lazy<Encoding>(() => Encoding.GetEncoding("iso-8859-1"));

        private FileStream GetFile()
        {
            string filePath = Path.Combine(this.path, this.Name);
            return new FileStream(filePath, FileMode.Open);
        }

        public IEnumerable<ESRecord> Load(FileLoadScheme scheme)
        {
            Console.Write("Processing " + nameof(ESFile) + " Data...");
            Stopwatch stopwatch = Stopwatch.StartNew();
            using (FileStream contents = GetFile())
            {
                this.FetchTES4(contents);
                while (true)
                {
                    byte[] headerBytes = new byte[ESRecord.RECORD_HEADER_SIZE];
                    int read = contents.Read(headerBytes);
                    if (read == 0) { break; }
                    string headerString = ISO_8859_1.Value.GetString(headerBytes);
                    if (headerString.Substring(0, 4) != "GRUP")
                    {
                        throw new InvalidESFileException("Invalid GRUP magic, found " + headerString.Substring(0, 4));
                    }
                    contents.Seek(-ESRecord.RECORD_HEADER_SIZE, SeekOrigin.Current);

                    int grupSize = PHPFunction.UnpackV(headerBytes.Skip(4).Take(4).ToArray());
                    ESRecordType grupType = RecordTypeParser.Parse(headerString.Substring(8, 4));
                    if (scheme.ShouldLoad(grupType))
                    {
                        GrupLoadScheme? rules = scheme.GetRulesFor(grupType);
                        if (rules == null) { throw new InvalidOperationException(nameof(rules) + " was null for " + nameof(grupType) + " " + grupType.Name + "."); }
                        Grup grup = Grup.Load(contents, true);
                        foreach (var loadedRecord in grup.LoadSubRecords(contents, this, rules))
                        {
                            yield return loadedRecord;
                        }
                        if (grup.Type == null) { throw new InvalidOperationException(nameof(grup.Type) + " was null."); }
                        this.grups.Add(grup.Type, grup);
                    }
                    else
                    {
                        contents.Seek(grupSize, SeekOrigin.Current);
                    }
                }
            }
            stopwatch.Stop();
            Console.WriteLine("\rProcessing " + nameof(ESFile) + " Complete (" + stopwatch.ElapsedMilliseconds + " ms)");
        }

        public Grup? TryGetGrup(ESRecordType type)
        {
            return this.grups.GetWithFallbackNullable(type, () => null);
        }

        public int Expand(int formid)
        {
            return this.collection.Expand(formid, this.Name);
        }

        private ESRecord FetchTES4(FileStream stream)
        {
            return RecordReader.ReadTES4Record(this, stream);
        }
    }
}