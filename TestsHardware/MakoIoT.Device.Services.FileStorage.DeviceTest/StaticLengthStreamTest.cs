using System.IO;
using System.Text;
using nanoFramework.TestFramework;

namespace MakoIoT.Device.Services.FileStorage.DeviceTest
{
    [TestClass]
    public class StaticLengthStreamTest
    {
        [TestMethod]
        public void After_getting_length_should_keep_stream_position()
        {

            File.WriteAllBytes("I:\\loremipsum.txt", Encoding.UTF8.GetBytes(Lorem));

            using (var fs = new StaticLengthStream(new FileStream("I:\\loremipsum.txt", FileMode.Open)))
            {
                var length = fs.Length;
                Assert.AreNotEqual(0, length);

                var buffer = new byte[10];
                //read 10 bytes
                fs.Read(buffer, 0, 10);

                //get position
                Assert.AreEqual(10, fs.Position, "before getting stream length");
                
                //get length
                Assert.AreEqual(length, fs.Length);
                
                //get position again
                Assert.AreEqual(10, fs.Position, "after getting stream length");
            }

            File.Delete("I:\\loremipsum.txt");
        }

        private const string Lorem = @"Lorem ipsum dolor sit amet, consectetur adipiscing elit. Quisque lacinia pellentesque pharetra. Nulla a tortor in nunc facilisis cursus nec id felis.";
    }
}
