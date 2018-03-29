using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;

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
            using (MemoryStream stream = new MemoryStream())
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType());
                serializer.WriteObject(stream, obj);
                stream.Position = 0;
                using (StreamReader reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        public static T Deserialize<T>(string str)
        {
            using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(str)))
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
                return (T)serializer.ReadObject(stream);
            }
        }

        public static string UCWords(string s)
        {
            if (s == null) { throw new ArgumentNullException(nameof(s)); }
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

        public static byte[] GZUncompress(byte[] compressed)
        {//https://stackoverflow.com/a/4080983/1325179
            using (MemoryStream input = new MemoryStream())
            {
                input.Write(new byte[] { 0x1f, 0x8b, 0x08, 0x00, 0x00, 0x00, 0x00, 0x00 }, 0, 8);
                input.Write(compressed, 0, compressed.Length);
                input.Position = 0;
                using (GZipStream gzip = new GZipStream(input, CompressionMode.Decompress))
                {
                    using (MemoryStream output = new MemoryStream())
                    {
                        gzip.CopyTo(output);//In .NET Core, this line throws at System.IO.InvalidDataException:  "The archive entry was compressed using an unsupported compression method."
                        return output.ToArray();
                    }
                }
            }
        }

        public static byte[] GZCompress(byte[] uncompressed)
        {
            using (var memoryStream = new MemoryStream())
            {
                using (var gZipStream = new GZipStream(memoryStream, CompressionMode.Compress, true))
                {
                    gZipStream.Write(uncompressed, 0, uncompressed.Length);
                }
                return memoryStream.ToArray();
            }
        }

        public static int UnpackV(byte[] bytes)
        {
            if (bytes.Length != 2 && bytes.Length != 4) { throw new ArgumentException("Length did not equal 4.", nameof(bytes)); }
            const int baseNum = 256;
            int sum = 0;
            for (int i = 0; i < bytes.Length; i++)
            {
                int byteInt = (int)bytes[i];
                sum += byteInt * (int)Math.Pow(baseNum, i);
            }
            return sum;
        }
        public static int UnpackV(string str)
        {
            return UnpackV(str.Select(c=>(byte)c).ToArray());
        }

        public static string Substr(string str, int length)
        {
            if (length > str.Length) { length = str.Length; }
            return str.Substring(0, length);
        }
    }
}
