using DltLib.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DltLib.Tests.Util
{
    [TestClass]
    public class ChecksumCalculatorTest
    {
        private ChecksumCalculator _checksumCalculator;

        [TestInitialize]
        public void Setup()
        {
            _checksumCalculator = new ChecksumCalculator();
        }

        [TestMethod]
        public void CalculateChecksumCorrectly()
        {
            Assert.AreEqual(0xC3, _checksumCalculator.CalculateChecksum(new byte[]
            {
                0x68, 0x27, 0x19, 0x40, 0x23, 0x74, 0x68, 0x68, 0x11, 0x04, 0x34, 0x35, 0xC3, 0x33, 0xC3, 0x16
            }, 0, 13));
            Assert.AreEqual(0x7C, _checksumCalculator.CalculateChecksum(new byte[]
            {
                0x68, 0x12, 0x34, 0x56, 0x78, 0x90, 0x12, 0x68, 0x91, 0x08, 0x34, 0x35, 0xC3, 0x33, 0x5C, 0x3C, 0x33,
                0x33, 0x7C, 0x16
            }, 0, 17));
        }
    }
}