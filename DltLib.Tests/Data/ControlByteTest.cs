using System.ComponentModel;
using DltLib.Data;
using DltLib.Exceptions;
using DltLib.Tests.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DltLib.Tests.Data
{
    [TestClass]
    public class ControlByteTest
    {
        [TestMethod]
        public void CanReadAndWriteFrameTypeCorrectly()
        {
            var cmdControlByte = new ControlByte(0b01111111);
            Assert.AreEqual(FrameType.Cmd, cmdControlByte.FrameType);
            var ackControlByte = new ControlByte(0b11111111);
            Assert.AreEqual(FrameType.Ack, ackControlByte.FrameType);

            var controlByte = new ControlByte(0b01110011);
            controlByte.FrameType = FrameType.Ack;
            Assert.AreEqual(0b11110011, controlByte.Byte);
            controlByte.FrameType = FrameType.Cmd;
            Assert.AreEqual(0b01110011, controlByte.Byte);
        }


        [TestMethod]
        public void CanReadAndWriteHasErrorCorrectly()
        {
            var errorAckControlByte = new ControlByte(0b10111111);
            Assert.IsFalse(errorAckControlByte.HasError);
            var nonErrorAckControlByte = new ControlByte(0b11111111);
            Assert.IsTrue(nonErrorAckControlByte.HasError);

            var controlByte = new ControlByte(0b10110011);
            controlByte.HasError = true;
            Assert.AreEqual(0b11110011, controlByte.Byte);
            controlByte.HasError = false;
            Assert.AreEqual(0b10110011, controlByte.Byte);
        }

        [TestMethod]
        public void CanReadAndWriteHasNextFrameCorrectly()
        {
            var errorAckControlByte = new ControlByte(0b00111111);
            Assert.IsTrue(errorAckControlByte.HasNextFrame);
            var nonNextFrameAckControlByte = new ControlByte(0b00011111);
            Assert.IsFalse(nonNextFrameAckControlByte.HasNextFrame);

            var controlByte = new ControlByte(0b10010011);
            controlByte.HasNextFrame = true;
            Assert.AreEqual(0b10110011, controlByte.Byte);
            controlByte.HasNextFrame = false;
            Assert.AreEqual(0b10010011, controlByte.Byte);
        }

        [TestMethod]
        public void CanReadAndWriteValidFunctionCodeCorrectly()
        {
            AssertValidFunctionCode(0b00000, FunctionCode.Reserved);
            AssertValidFunctionCode(0b01000, FunctionCode.BroadcastTimeCalibration);
            AssertValidFunctionCode(0b10001, FunctionCode.DataRead);
            AssertValidFunctionCode(0b10010, FunctionCode.SuccessiveDataRead);
            AssertValidFunctionCode(0b10011, FunctionCode.CommunicationAddressRead);
            AssertValidFunctionCode(0b10100, FunctionCode.DataWrite);
            AssertValidFunctionCode(0b10101, FunctionCode.CommunicationAddressWrite);
            AssertValidFunctionCode(0b10110, FunctionCode.FreezeCommand);
            AssertValidFunctionCode(0b10111, FunctionCode.BaudRateChange);
            AssertValidFunctionCode(0b11000, FunctionCode.CodeChange);
            AssertValidFunctionCode(0b11001, FunctionCode.MaximumDataClear);
            AssertValidFunctionCode(0b11010, FunctionCode.MeterDataClear);
            AssertValidFunctionCode(0b11011, FunctionCode.EventClear);
        }

        [TestMethod]
        public void CanReadAndWriteInvalidFunctionCodeCorrectly()
        {
            AssertInvalidFunctionCode(0b00001);
            AssertInvalidFunctionCode(0b00010);
            AssertInvalidFunctionCode(0b00010);
            AssertInvalidFunctionCode(0b00010);
            AssertInvalidFunctionCode(0b00010);
            AssertInvalidFunctionCode(0b00010);
            AssertInvalidFunctionCode(0b00010);
            AssertInvalidFunctionCode(0b00010);
            AssertInvalidFunctionCode(0b00010);
            AssertInvalidFunctionCode(0b00010);
            AssertInvalidFunctionCode(0b00010);
            AssertInvalidFunctionCode(0b00010);
            AssertInvalidFunctionCode(0b00010);
            AssertInvalidFunctionCode(0b00010);
            AssertInvalidFunctionCode(0b00010);
            AssertInvalidFunctionCode(0b00010);
            AssertInvalidFunctionCode(0b00010);
            AssertInvalidFunctionCode(0b00010);
            AssertInvalidFunctionCode(0b00010);
        }

        private static void AssertValidFunctionCode(byte functionCodeValue, FunctionCode functionCode)
        {
            for (byte i = 0; i < 8; i++)
            {
                var uppperThreeBit = (byte) (i << 5);
                var controlByteValue = (byte) (uppperThreeBit | functionCodeValue);
                AssertReadValidFunctionCode(functionCode, new ControlByte(controlByteValue));
                AssertWriteValidFunction(functionCode, controlByteValue, uppperThreeBit);
            }
        }

        private static void AssertReadValidFunctionCode(FunctionCode functionCode, ControlByte controlByte)
        {
            Assert.AreEqual(functionCode, controlByte.FunctionCode);
        }

        private static void AssertWriteValidFunction(FunctionCode functionCode, byte controlByteValue,
            byte uppperThreeBit)
        {
            var controlByte = new ControlByte(uppperThreeBit);
            controlByte.FunctionCode = functionCode;
            Assert.AreEqual(controlByteValue, controlByte.Byte);
        }

        private static void AssertInvalidFunctionCode(byte functionCodeValue)
        {
            for (byte i = 0; i < 8; i++)
            {
                var uppperThreeBit = (byte) (i << 5);
                var controlByteValue = (byte) (uppperThreeBit | functionCodeValue);

                AssertReadInvalidFunctionCode(new ControlByte(controlByteValue));
                AssertWriteInvalidFunctionCode(functionCodeValue, uppperThreeBit);
            }
        }

        private static void AssertReadInvalidFunctionCode(ControlByte controlByte)
        {
            void AccessFunctionCode()
            {
                var _ = controlByte.FunctionCode;
            }

            DltLibAssert.AssertException<InvalidControlByteException>(AccessFunctionCode,
                ex => ex.InvalidControlByte == controlByte.Byte);
        }

        private static void AssertWriteInvalidFunctionCode(byte functionCodeValue, byte uppperThreeBit)
        {
            var controlByte = new ControlByte(uppperThreeBit);

            void AssignFunctionCode()
            {
                controlByte.FunctionCode = (FunctionCode) functionCodeValue;
            }

            DltLibAssert.AssertException<InvalidEnumArgumentException>(AssignFunctionCode);
        }
    }
}