using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PSI2;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Test_Convert_Endian_to_Int()
        {
            byte[] tab = { 50, 3, 0, 0 };
            int expected = 50 * 1 + 3 * 256;
            int actual = MyImage.Convertir_Endian_To_Int(tab);
            Assert.AreEqual(expected, actual);
        }
    }
}
