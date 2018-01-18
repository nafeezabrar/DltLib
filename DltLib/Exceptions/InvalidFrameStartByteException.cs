namespace DltLib.Exceptions
{
    public class InvalidFrameStartByteException : InvalidDltDataException
    {
        public byte InvalidByte { get; }

        public InvalidFrameStartByteException(byte invalidByte)
            : base($"Invalid frame start byte 0x{invalidByte:X2}")
        {
            InvalidByte = invalidByte;
        }
    }
}