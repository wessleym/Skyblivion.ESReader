using System.IO;

namespace Skyblivion.ESReader.Extensions.StreamExtensions
{
    public static class StreamExtensions
    {
        public static int Read(this Stream stream, byte[] buffer)
        {
            return stream.Read(buffer, 0, buffer.Length);
        }
        public static byte[] Read(this Stream stream, int length)
        {
            byte[] bytes = new byte[length];
            Read(stream, bytes);
            return bytes;
        }
    }
}
