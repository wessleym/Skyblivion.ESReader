using System;
using System.Linq;

namespace Skyblivion.ESReader.TES4
{
    public class TES4SubrecordData
    {
        private readonly byte[] bytes;
        public TES4SubrecordData(byte[] bytes)
        {
            this.bytes = bytes;
        }

        public int Length => bytes.Length;

        public byte FirstByte()
        {
            return bytes[0];
        }

        public byte SecondByte()
        {
            return bytes[1];
        }

        public byte[] FirstFourBytes()
        {
            return bytes.Take(4).ToArray();
        }

        public override string ToString()
        {
            return TES4File.ISO_8859_1.Value.GetString(bytes);
        }

        public string ToStringTrim()
        {
            return ToString().Trim('\0').Trim();
        }

        public string ToStringTrimLower()
        {
            return ToStringTrim().ToLower();
        }

        public int ToInt()
        {
            return BitConverter.ToInt32(bytes, 0);
        }
    }
}
