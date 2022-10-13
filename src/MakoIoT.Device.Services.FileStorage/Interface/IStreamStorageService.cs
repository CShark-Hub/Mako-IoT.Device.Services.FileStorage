using System.IO;
using MakoIoT.Device.Services.Interface;

namespace MakoIoT.Device.Services.FileStorage.Interface
{
    public interface IStreamStorageService : IStorageService
    {
        StreamWriter WriteToFileStream(string fileName);
        StreamReader ReadFileStream(string fileName);
    }
}
