using System.IO;

namespace MakoIoT.Device.Services.FileStorage.Extensions
{
    public static class StreamExtension
    {
        public static int SafeGetLength(this Stream stream)
        {
            var position = stream.Position;
            var length = (int)stream.Length;
            stream.Position = position;
            return length;
        }
    }
}
