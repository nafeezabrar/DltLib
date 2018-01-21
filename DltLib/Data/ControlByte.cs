using System.ComponentModel;
using DltLib.Exceptions;
using DltLib.Util;

namespace DltLib.Data
{
    public class ControlByte
    {
        public byte Byte { get; private set; }

        public FrameType FrameType
        {
            get => ByteUtility.GetBit(Byte, 7) == 0 ? FrameType.Cmd : FrameType.Ack;
            set
            {
                var bit = value == FrameType.Cmd ? 0 : 1;
                Byte = ByteUtility.SetBit(Byte, 7, bit);
            }
        }

        public bool HasError
        {
            get => ByteUtility.GetBit(Byte, 6) == 1;
            set
            {
                var bit = value ? 1 : 0;
                Byte = ByteUtility.SetBit(Byte, 6, bit);
            }
        }

        public bool HasNextFrame
        {
            get => ByteUtility.GetBit(Byte, 5) == 1;
            set
            {
                var bit = value ? 1 : 0;
                Byte = ByteUtility.SetBit(Byte, 5, bit);
            }
        }

        public FunctionCode FunctionCode
        {
            get
            {
                var functionCode = GetLastFiveBit();
                switch (functionCode)
                {
                    case 0b00000:
                        return FunctionCode.Reserved;
                    case 0b01000:
                        return FunctionCode.BroadcastTimeCalibration;
                    case 0b10001:
                        return FunctionCode.DataRead;
                    case 0b10010:
                        return FunctionCode.SuccessiveDataRead;
                    case 0b10011:
                        return FunctionCode.CommunicationAddressRead;
                    case 0b10100:
                        return FunctionCode.DataWrite;
                    case 0b10101:
                        return FunctionCode.CommunicationAddressWrite;
                    case 0b10110:
                        return FunctionCode.FreezeCommand;
                    case 0b10111:
                        return FunctionCode.BaudRateChange;
                    case 0b11000:
                        return FunctionCode.CodeChange;
                    case 0b11001:
                        return FunctionCode.MaximumDataClear;
                    case 0b11010:
                        return FunctionCode.MeterDataClear;
                    case 0b11011:
                        return FunctionCode.EventClear;
                    default:
                        throw new InvalidControlByteException(Byte);
                }
            }
            set
            {
                byte functionCode;

                switch (value)
                {
                    case FunctionCode.Reserved:
                        functionCode = 0b00000;
                        break;
                    case FunctionCode.BroadcastTimeCalibration:
                        functionCode = 0b01000;
                        break;
                    case FunctionCode.DataRead:
                        functionCode = 0b10001;
                        break;
                    case FunctionCode.SuccessiveDataRead:
                        functionCode = 0b10010;
                        break;
                    case FunctionCode.CommunicationAddressRead:
                        functionCode = 0b10011;
                        break;
                    case FunctionCode.DataWrite:
                        functionCode = 0b10100;
                        break;
                    case FunctionCode.CommunicationAddressWrite:
                        functionCode = 0b10101;
                        break;
                    case FunctionCode.FreezeCommand:
                        functionCode = 0b10110;
                        break;
                    case FunctionCode.BaudRateChange:
                        functionCode = 0b10111;
                        break;
                    case FunctionCode.CodeChange:
                        functionCode = 0b11000;
                        break;
                    case FunctionCode.MaximumDataClear:
                        functionCode = 0b11001;
                        break;
                    case FunctionCode.MeterDataClear:
                        functionCode = 0b11010;
                        break;
                    case FunctionCode.EventClear:
                        functionCode = 0b11011;
                        break;
                    default:
                        throw new InvalidEnumArgumentException(nameof(value), (int) value, typeof(FunctionCode));
                }

                SetLastFiveBit(functionCode);
            }
        }

        private byte GetLastFiveBit()
        {
            return (byte) (Byte & 0b00011111);
        }

        private void SetLastFiveBit(byte value)
        {
            Byte = (byte) ((Byte & 0b11100000) | (value & 0b00011111));
        }

        public ControlByte(byte @byte)
        {
            Byte = @byte;
        }

        public ControlByte(FrameType frameType, bool hasError, bool hasNextFrame, FunctionCode functionCode)
        {
            FrameType = frameType;
            HasError = hasError;
            HasNextFrame = hasNextFrame;
            FunctionCode = functionCode;
        }

        protected bool Equals(ControlByte that)
        {
            return this.Byte == that.Byte;
        }

        public override bool Equals(object that)
        {
            if (ReferenceEquals(null, that)) return false;
            if (ReferenceEquals(this, that)) return true;
            if (this.GetType() != that.GetType()) return false;
            return Equals((ControlByte) that);
        }

        public override int GetHashCode()
        {
            return Byte.GetHashCode();
        }
    }

    public enum FrameType
    {
        Cmd,
        Ack
    }

    public enum FunctionCode
    {
        Reserved = 0b00000,
        BroadcastTimeCalibration = 0b01000,
        DataRead = 0b10001,
        SuccessiveDataRead = 0b10010,
        CommunicationAddressRead = 0b10011,
        DataWrite = 0b10100,
        CommunicationAddressWrite = 0b10101,
        FreezeCommand = 0b10110,
        BaudRateChange = 0b10111,
        CodeChange = 0b11000,
        MaximumDataClear = 0b11001,
        MeterDataClear = 0b11010,
        EventClear = 0b11011
    }
}