using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using CrcStudio.Zip;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CrcStudio.Test.Zip
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class ZipEntryTest
    {
        public ZipEntryTest()
        {
        }

        [TestMethod]
        public void CompressionTypeTest()
        {
            File.Delete("test.zip");
            using (var zf = new ZipFile("test.zip"))
            {
                zf.Add(new MemoryStream(new byte[0]), "ZeroBytes", CompressionType.Deflate);
                zf.Add(new MemoryStream(Encoding.ASCII.GetBytes("test")), "Test", CompressionType.Deflate);
                var test = "test";
                for (int i = 0; i < 1000; i++) test += "test";
                zf.Add(new MemoryStream(Encoding.ASCII.GetBytes(test)), "LargeTest", CompressionType.Deflate);
            }
            using (var zf = new ZipFile("test.zip"))
            {
                var zipEntry = zf.Entries.ToArray()[0];
                Assert.AreEqual(zipEntry.CompressionMethod, (ushort)CompressionType.Store);
                Assert.AreEqual((int)zipEntry.CompressedSize, 0);
                Assert.AreEqual((int)zipEntry.UncompressedSize, 0);

                for (int n = 1; n < 3; n++)
                {
                    zipEntry = zf.Entries.ToArray()[n];
                    if (zipEntry.CompressedSize == zipEntry.UncompressedSize)
                    {
                        Assert.AreEqual(zipEntry.CompressionMethod, (ushort) CompressionType.Store);
                    }
                    else
                    {
                        Assert.AreEqual(zipEntry.CompressionMethod, (ushort) CompressionType.Deflate);
                    }
                }
            }
        }
    }
}
