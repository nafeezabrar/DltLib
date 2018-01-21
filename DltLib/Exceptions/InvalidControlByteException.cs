namespace DltLib.Exceptions
{
    public class InvalidControlByteException : InvalidDltDataException
    {
        public byte InvalidControlByte { get; }

        public InvalidControlByteException(byte invalidControlByte)
            : base($"Invalid control byte 0x{invalidControlByte:X2}")
        {
            InvalidControlByte = invalidControlByte;
        }
    }
}