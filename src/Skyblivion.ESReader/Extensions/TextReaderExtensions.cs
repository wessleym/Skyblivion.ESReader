using System.IO;

namespace Skyblivion.ESReader.Extensions.TextReaderExtensions
{
    public static class TextReaderExtensions
    {
        public static int ReadBlock(this TextReader reader, char[] buffer)
        {
            return reader.ReadBlock(buffer, 0, buffer.Length);
        }
    }
}
