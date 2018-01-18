using DltLib.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DltLib.Tests
{
    [TestClass]
    public class DltDataGeneratorTest
    {
        private DltDataGenerator _dltDataGenerator;

        [TestInitialize]
        public void Setup()
        {
            _dltDataGenerator = new DltDataGenerator();
        }

        [TestMethod]
        public void TestGenerateValidDltData()
        {
            AssertGenerateValidDltData(
                new DltPacket
                {
                    Address = 687423401927,
                    ControlByte = new ControlByte(0x11),
                    DataBlock = new byte[] {0x01, 0x02, 0x90, 0x00}
                },
                new byte[]
                {
                    0x68,
                    0x27, 0x19, 0x40, 0x23, 0x74, 0x68,
                    0x68,
                    0x11,
                    0x04,
                    0x34, 0x35, 0xC3, 0x33,
                    0xC3,
                    0x16
                });
            AssertGenerateValidDltData(
                new DltPacket
                {
                    Address = 129078563412,
                    ControlByte = new ControlByte(0x91),
                    DataBlock = new byte[] {0x01, 0x02, 0x90, 0x00, 0x29, 0x09, 0x00, 0x00}
                },
                new byte[]
                {
                    0x68,
                    0x12, 0x34, 0x56, 0x78, 0x90, 0x12,
                    0x68,
                    0x91,
                    0x08,
                    0x34, 0x35, 0xC3, 0x33, 0x5C, 0x3C, 0x33, 0x33,
                    0x7C,
                    0x16
                });
        }

        private void AssertGenerateValidDltData(DltPacket dltPacket, byte[] expectedDltData)
        {
            var actualDltData = _dltDataGenerator.GenerateDltData(dltPacket);
            CollectionAssert.AreEqual(expectedDltData, actualDltData);
        }
    }
}