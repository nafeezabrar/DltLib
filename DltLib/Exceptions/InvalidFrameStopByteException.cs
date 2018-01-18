namespace DltLib.Exceptions
{
    public class InvalidFrameStopByteException : InvalidDltDataException
    {
        public byte InvalidByte { get; }

        public InvalidFrameStopByteException(byte invalidByte)
            : base($"Invalid frame stop byte 0x{invalidByte:X2}")
        {
            InvalidByte = invalidByte;
        }
    }
}