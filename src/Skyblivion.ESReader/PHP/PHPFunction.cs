using Skyblivion.ESReader.Extensions;
using System;
using System.Collections.Generic;
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
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);
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
                object? obj = serializer.ReadObject(stream);
                if (obj == null) { throw new NullableException(nameof(obj)); }
                return (T)obj;
            }
        }

        public static string UCWords(string s)
        {
            if (s.Contains(" "))
            {
                throw new InvalidOperationException("This implementation of UCWords is only meant to make the first character uppercase, not process words like the real PHP version of UCWords");
            }
            return s.Substring(0, 1).ToUpper() + s.Substring(1);
        }

        public static byte[] GZUncompress(byte[] compressed)
        {//WTM:  Note:  PHP invoked gzuncompress, but from what I understand, GZip = header + deflate + footer.
            //Since the data is missing a header and footer (as stated by GZipStream), DeflateStream will work.
            //But I do need to skip the first two bytes.  See below.
            using (MemoryStream input = new MemoryStream())
            {
                input.Write(compressed, 0, compressed.Length);
                input.Position = 2;//Skip the first two bytes:  http://george.chiramattel.com/blog/2007/09/deflatestream-block-length-does-not-match.html
                using (DeflateStream deflate = new DeflateStream(input, CompressionMode.Decompress))
                {
                    using (MemoryStream output = new MemoryStream())
                    {
                        deflate.CopyTo(output);
                        return output.ToArray();
                    }
                }
            }
        }

        public static int UnpackV(IReadOnlyList<byte> bytes)
        {
            if (bytes.Count != 2 && bytes.Count != 4) { throw new ArgumentException("Length did not equal 2 or 4.", nameof(bytes)); }
            const int baseNum = 256;
            int sum = 0;
            for (int i = 0; i < bytes.Count; i++)
            {
                int byteInt = (int)bytes[i];
                sum += byteInt * (int)Math.Pow(baseNum, i);
            }
            return sum;
        }

        public static string Substr(string str, int length)
        {
            if (length > str.Length) { length = str.Length; }
            return str.Substring(0, length);
        }
    }
}
