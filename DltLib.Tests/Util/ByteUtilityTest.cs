using System;
using DltLib.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DltLib.Tests.Util
{
    [TestClass]
    public class ByteUtilityTest
    {
        [TestMethod]
        public void TestFromHexDigit()
        {
            Assert.AreEqual((byte) 68, ByteUtility.FromHexDigit(0x68));
            Assert.AreEqual((byte) 27, ByteUtility.FromHexDigit(0x27));
            Assert.AreEqual((byte) 93, ByteUtility.FromHexDigit(0x93));
            Assert.AreEqual((byte) 40, ByteUtility.FromHexDigit(0x40));
            Assert.AreEqual((byte) 08, ByteUtility.FromHexDigit(0x08));
        }

        [TestMethod]
        public void TestToHexDigit()
        {
            Assert.AreEqual((byte) 0x68, ByteUtility.ToHexDigit(68));
            Assert.AreEqual((byte) 0x27, ByteUtility.ToHexDigit(27));
            Assert.AreEqual((byte) 0x93, ByteUtility.ToHexDigit(93));
            Assert.AreEqual((byte) 0x40, ByteUtility.ToHexDigit(40));
            Assert.AreEqual((byte) 0x08, ByteUtility.ToHexDigit(08));
        }

        [TestMethod]
        public void TestGetBitValidInput()
        {
            Assert.AreEqual(0, ByteUtility.GetBit(0b01111101, 1));
            Assert.AreEqual(1, ByteUtility.GetBit(0b01111101, 4));
            Assert.AreEqual(1, ByteUtility.GetBit(0b00101001, 5));
            Assert.AreEqual(0, ByteUtility.GetBit(0b00101001, 7));
            Assert.AreEqual(1, ByteUtility.GetBit(0b10010011, 7));
        }

        [TestMethod]
        public void TestGetBitInvalidInput()
        {
            DltLibAssert.AssertException<ArgumentOutOfRangeException>(() => ByteUtility.GetBit(0b00101101, -11),
                ex => ex.ParamName == "position");
            DltLibAssert.AssertException<ArgumentOutOfRangeException>(() => ByteUtility.GetBit(0b01011001, 12),
                ex => ex.ParamName == "position");
            DltLibAssert.AssertException<ArgumentOutOfRangeException>(() => ByteUtility.GetBit(0b01111101, 8),
                ex => ex.ParamName == "position");
        }

        [TestMethod]
        public void TestSetBitValidInput()
        {
            Assert.AreEqual(0b01111111, ByteUtility.SetBit(0b01111101, 1, 1));
            Assert.AreEqual(0b01111101, ByteUtility.SetBit(0b01111101, 7, 0));
            Assert.AreEqual(0b00101001, ByteUtility.SetBit(0b00101001, 2, 0));
            Assert.AreEqual(0b00001001, ByteUtility.SetBit(0b00101001, 5, 0));
            Assert.AreEqual(0b10000011, ByteUtility.SetBit(0b10010011, 4, 0));
        }

        [TestMethod]
        public void TestSetBitInvalidInput()
        {
            DltLibAssert.AssertException<ArgumentOutOfRangeException>(() => ByteUtility.SetBit(0b00101101, -11, 0),
                ex => ex.ParamName == "position");
            DltLibAssert.AssertException<ArgumentOutOfRangeException>(() => ByteUtility.SetBit(0b01011001, 12, 1),
                ex => ex.ParamName == "position");
            DltLibAssert.AssertException<ArgumentOutOfRangeException>(() => ByteUtility.SetBit(0b01111101, 8, 0),
                ex => ex.ParamName == "position");
            DltLibAssert.AssertException<ArgumentOutOfRangeException>(() => ByteUtility.SetBit(0b00101101, 5, 2),
                ex => ex.ParamName == "bit");
            DltLibAssert.AssertException<ArgumentOutOfRangeException>(() => ByteUtility.SetBit(0b01011001, 3, -1),
                ex => ex.ParamName == "bit");
            DltLibAssert.AssertException<ArgumentOutOfRangeException>(() => ByteUtility.SetBit(0b01111101, 2, 3),
                ex => ex.ParamName == "bit");
        }
    }
}