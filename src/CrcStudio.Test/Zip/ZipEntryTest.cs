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
            var zf = new ZipFile("test.zip");
            zf.Add(new MemoryStream(new byte[0]), "ZeroBytes", CompressionType.Deflate);
            zf.Add(new MemoryStream(Encoding.ASCII.GetBytes("test")), "Test", CompressionType.Deflate);
            zf.Close();
        }
    }
}
