using System;
using System.IO;
using System.Text;
using MakoIoT.Device.Services.FileStorage.Interface;
using MakoIoT.Device.Services.Interface;
using Microsoft.Extensions.Logging;

namespace MakoIoT.Device.Services.FileStorage
{
    public class FileStorageService : IStorageService, IStreamStorageService
    {
        private readonly ILog _logger;

        public const string Root = "I:";

        public FileStorageService(ILog logger)
        {
            _logger = logger;
        }


        public void WriteToFile(string fileName, string text)
        {
            _logger.Trace($"Writing to file [{fileName}]");

            var filePath = GetFilePath(fileName);
            File.Create(filePath);

            byte[] buffer = Encoding.UTF8.GetBytes(text);

            using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite))
            {
                fs.Write(buffer, 0, buffer.Length);
                fs.Close();
            }

            _logger.Trace($"File {fileName} written.");
        }

        public StreamWriter WriteToFileStream(string fileName)
        {
            var filePath = GetFilePath(fileName);
            File.Create(filePath);

            return new StreamWriter(new FileStream(filePath, FileMode.Open, FileAccess.Write));
        }


        public bool FileExists(string fileName) => File.Exists(GetFilePath(fileName));


        public string ReadFile(string fileName)
        {
            using var fs = new FileStream(GetFilePath(fileName), FileMode.Open, FileAccess.Read);
            var buffer = new byte[fs.Length];
            fs.Read(buffer, 0, buffer.Length);
            fs.Close();
            return Encoding.UTF8.GetString(buffer, 0, buffer.Length);
        }

        public StreamReader ReadFileStream(string fileName)
        {
            var filePath = GetFilePath(fileName);
            return new StreamReader(new FileStream(filePath, FileMode.Open, FileAccess.Read));
        }

        public void DeleteFile(string fileName)
        {
            File.Delete(GetFilePath(fileName));
        }

        public string[] GetFiles()
        {
            return Directory.GetFiles(Root);
        }

        public string[] GetFileNames()
        {
            var files = Directory.GetFiles(Root);
            for (int i = 0; i < files.Length; i++)
            {
                files[i] = Path.GetFileName(files[i]);
            }
            return files;
        }

        public string GetFilePath(string fileName) => fileName.StartsWith(Root) ? fileName : Path.Combine(Root, fileName);
    }
}
