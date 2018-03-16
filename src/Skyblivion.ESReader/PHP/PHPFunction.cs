using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Skyblivion.ESReader.PHP
{
    public static class PHPFunction
    {
        public static string MD5(string input)
        {
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);
                // Convert the byte array to hexadecimal string
                return string.Join("", hashBytes.Select(b => b.ToString("X2"))).ToLower();
            }
        }

        public static string Serialize(object obj)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(obj.GetType());
            using (StringWriter writer = new StringWriter())
            {
                xmlSerializer.Serialize(writer, obj);
                return writer.ToString();
            }
        }

        public static T Deserialize<T>(string str)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
            using (StringReader reader = new StringReader(str))
            {
                return (T)xmlSerializer.Deserialize(reader);
            }
        }

        public static string UCWords(string s)
        {
            return s.Aggregate("",
              (working, next) =>
              {
                  return working.Length == 0 && next != ' ' ?
                    next.ToString().ToUpper() :
                    (
                        working.EndsWith(" ") ?
                        working + next.ToString().ToUpper() :
                        working + next.ToString()
                    );
              }
            );
        }

        private static byte[] GZUncompress(byte[] compressed)
        {
            using (var input = new MemoryStream(compressed))
            {
                using (var gzip = new GZipStream(input, CompressionMode.Decompress))
                {
                    using (var output = new MemoryStream())
                    {
                        gzip.CopyTo(output);
                        return output.ToArray();
                    }
                }
            }
        }
        public static string GZUncompress(string compressed)
        {
            byte[] compressedBytes = Encoding.UTF8.GetBytes(compressed);
            byte[] uncompressedBytes = GZUncompress(compressedBytes);
            return Encoding.UTF8.GetString(uncompressedBytes);
        }
    }
}
