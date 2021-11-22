using System;
using System.Collections.Generic;
using System.Linq;

namespace Skyblivion.ESReader.TES4
{
    public class TES4SubrecordData
    {
        public byte[] Bytes { get; }
        public TES4SubrecordData(byte[] bytes)
        {
            Bytes = bytes;
        }

        public int BytesLength => Bytes.Length;

        public byte FirstByte()
        {
            return Bytes[0];
        }

        public byte SecondByte()
        {
            return Bytes[1];
        }

        public byte[] FirstFourBytes()
        {
            return Bytes.Take(4).ToArray();
        }

        public override string ToString()
        {
            return TES4File.ISO_8859_1.Value.GetString(Bytes);
        }

        public string ToStringTrim()
        {
            return ToString().Trim('\0').Trim();
        }

        public string ToStringTrimLower()
        {
            return ToStringTrim().ToLower();
        }

        public int FirstFourBytesToInt()
        {
            return BitConverter.ToInt32(Bytes, 0);//This method only uses the first four bytes.
        }
    }
}
