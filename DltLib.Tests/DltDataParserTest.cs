using DltLib.Data;
using DltLib.Exceptions;
using DltLib.Tests.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DltLib.Tests
{
    [TestClass]
    public class DltDataParserTest
    {
        private DltDataParser _dltDataParser;

        [TestInitialize]
        public void Setup()
        {
            _dltDataParser = new DltDataParser();
        }

        [TestMethod]
        public void TestParseValidDltData()
        {
            AssertParseValidDltData(
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
                },
                new DltPacket
                {
                    Address = 687423401927,
                    ControlByte = new ControlByte(0x11),
                    DataBlock = new byte[] {0x01, 0x02, 0x90, 0x00}
                });
            AssertParseValidDltData(
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
                },
                new DltPacket
                {
                    Address = 129078563412,
                    ControlByte = new ControlByte(0x91),
                    DataBlock = new byte[] {0x01, 0x02, 0x90, 0x00, 0x29, 0x09, 0x00, 0x00}
                });
        }

        private void AssertParseValidDltData(byte[] dltData, DltPacket expectedDltPacket)
        {
            var actualDltPacket = _dltDataParser.ParseDltData(dltData);
            AssertEqualDltPacket(expectedDltPacket, actualDltPacket);
        }

        private void AssertEqualDltPacket(DltPacket expectedDltPacket, DltPacket actualDltPacket)
        {
            if (ReferenceEquals(expectedDltPacket, actualDltPacket)) return;

            Assert.AreEqual(expectedDltPacket.Address, actualDltPacket.Address);
            Assert.AreEqual(expectedDltPacket.ControlByte, actualDltPacket.ControlByte);
            CollectionAssert.AreEqual(expectedDltPacket.DataBlock, actualDltPacket.DataBlock);
        }

        [TestMethod]
        public void TestCouldntParseInvalidDltData()
        {
            DltLibAssert.AssertException<InvalidFrameStartByteException>(() =>
                _dltDataParser.ParseDltData(
                    new byte[]
                    {
                        0x12,
                        0x27, 0x19, 0x40, 0x23, 0x74, 0x68,
                        0x68,
                        0x11,
                        0x04,
                        0x34, 0x35, 0xC3, 0x33,
                        0xC3,
                        0x16
                    }), ex =>
                ex.InvalidByte == 0x12 && ex.Message == "Invalid frame start byte 0x12");
            DltLibAssert.AssertException<InvalidFrameStartByteException>(() =>
                _dltDataParser.ParseDltData(
                    new byte[]
                    {
                        0x68,
                        0x27, 0x19, 0x40, 0x23, 0x74, 0x68,
                        0x84,
                        0x11,
                        0x04,
                        0x34, 0x35, 0xC3, 0x33,
                        0xC3,
                        0x16
                    }), ex =>
                ex.InvalidByte == 0x84 && ex.Message == "Invalid frame start byte 0x84");
            DltLibAssert.AssertException<FrameChecksumMismatchedException>(() =>
                _dltDataParser.ParseDltData(
                    new byte[]
                    {
                        0x68,
                        0x27, 0x19, 0x40, 0x23, 0x74, 0x68,
                        0x68,
                        0x11,
                        0x04,
                        0x34, 0x35, 0xC3, 0x33,
                        0x4A,
                        0x16
                    }), ex =>
                ex.ExpectedChecksum == 0xC3
                && ex.ActualChecksum == 0x4A
                && ex.Message == "Frame checksum doesn't matched expected checksum: 0xC3, actual checksum: 0x4A");
            DltLibAssert.AssertException<InvalidFrameStopByteException>(() =>
                _dltDataParser.ParseDltData(
                    new byte[]
                    {
                        0x68,
                        0x27, 0x19, 0x40, 0x23, 0x74, 0x68,
                        0x68,
                        0x11,
                        0x04,
                        0x34, 0x35, 0xC3, 0x33,
                        0xC3,
                        0x74
                    }), ex =>
                ex.InvalidByte == 0x74 && ex.Message == "Invalid frame stop byte 0x74");
        }
    }
}