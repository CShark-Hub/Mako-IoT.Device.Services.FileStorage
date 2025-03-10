using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using nanoFramework.TestFramework;

namespace MakoIoT.Device.Services.FileStorage.DeviceTest
{
    [TestClass]
    public class FileStorageServiceTest
    {
        [TestMethod]
        public void WriteToFile_should_create_file()
        {
            string fileName = $"mako-test1-{DateTime.UtcNow:HHmmss}.txt";
            string text = "Hello!";
        
            var sut = new FileStorageService(new DebugLog(nameof(FileStorageServiceTest)));
        
            sut.WriteToFile(fileName,text);
        
            Assert.IsNotNull(File.Exists(sut.GetFilePath(fileName))); 
            
            File.Delete(sut.GetFilePath(fileName));
        }
        
        [TestMethod]
        public void WriteToFile_should_write_text_to_file()
        {
            string fileName = $"mako-test2-{DateTime.UtcNow:HHmmss}.txt";
            string text = "{\"Enabled\":true,\"MaxFileSize\":102400,\"LogLevel\":0}";
        
            var sut = new FileStorageService(new DebugLog(nameof(FileStorageServiceTest)));
        
            sut.WriteToFile(fileName, text);
        
            Assert.IsNotNull(File.Exists(sut.GetFilePath(fileName)));
        
            using (var fs = new FileStream(sut.GetFilePath(fileName), FileMode.Open, FileAccess.Read))
            {
                var buffer = new byte[fs.Length];
                fs.Read(buffer, 0, buffer.Length);
                fs.Close();
                var readText = Encoding.UTF8.GetString(buffer, 0, buffer.Length);
                Assert.AreEqual(text, readText);
            }
        
            File.Delete(sut.GetFilePath(fileName));
        
        }
        
        [TestMethod]
        public void WriteToFile_should_overwrite_existing_file()
        {
            string fileName = $"mako-test3-{DateTime.UtcNow:HHmmss}.txt";
            string text1 = "Hello, World!";
            string text2 = "Hello!";
        
            var sut = new FileStorageService(new DebugLog(nameof(FileStorageServiceTest)));
        
            sut.WriteToFile(fileName, text1);
        
            Assert.IsNotNull(File.Exists(sut.GetFilePath(fileName)));
        
            sut.WriteToFile(fileName, text2);
        
            using (var fs = new FileStream(sut.GetFilePath(fileName), FileMode.Open, FileAccess.Read))
            {
                var buffer = new byte[fs.Length];
                fs.Read(buffer, 0, buffer.Length);
                fs.Close();
                var readText = Encoding.UTF8.GetString(buffer, 0, buffer.Length);
                Assert.AreEqual(text2, readText);
            }
        
            File.Delete(sut.GetFilePath(fileName));
        }
        
        [TestMethod]
        public void FileExists_should_detect_file()
        {
            string fileName = $"mako-test4-{DateTime.UtcNow:HHmmss}.txt";
        
            
            var sut = new FileStorageService(new DebugLog(nameof(FileStorageServiceTest)));

            using (var fs = File.Create(sut.GetFilePath(fileName)))
            {
                fs.Close();
            }
        
            bool exists = sut.FileExists(fileName);
        
            File.Delete(sut.GetFilePath(fileName));
        
            Assert.IsTrue(exists);
        
            Assert.IsFalse(sut.FileExists(fileName));
        }
        
        [TestMethod]
        public void ReadFile_should_return_text_from_file()
        {
            string fileName = $"mako-test5-{DateTime.UtcNow:HHmmss}.txt";
            string text = "Hello!";
        
            var sut = new FileStorageService(new DebugLog(nameof(FileStorageServiceTest)));
        
            byte[] buffer = Encoding.UTF8.GetBytes(text);
            using (var fs = new FileStream(sut.GetFilePath(fileName), FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                fs.Write(buffer, 0, buffer.Length);
                fs.Close();
            }
        
            string readText = sut.ReadFile(fileName);
        
            File.Delete(sut.GetFilePath(fileName));
        
            Assert.AreEqual(text, readText);
        }
        
        [TestMethod]
        public void ReadFile_given_no_file_should_throw_exception()
        {
            string fileName = $"mako-test6-{DateTime.UtcNow:HHmmss}.txt";
            var sut = new FileStorageService(new DebugLog(nameof(FileStorageServiceTest)));
        
            if (File.Exists(sut.GetFilePath(fileName)))
                File.Delete(sut.GetFilePath(fileName));
            
            Assert.ThrowsException(typeof(IOException), () => sut.ReadFile(fileName));
        }
        
        [DataRow("file.ext", "I:\\file.ext")]
        [DataRow("I:\\file.ext", "I:\\file.ext")]
        public void GetFilePath_without_directory(string filename, string result)
        {
            var sut = new FileStorageService(new DebugLog(nameof(FileStorageServiceTest)));
            Assert.AreEqual(result, sut.GetFilePath(filename));
        }
    }
}
