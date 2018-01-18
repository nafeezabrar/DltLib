namespace DltLib.Exceptions
{
    public class FrameChecksumMismatchedException : InvalidDltDataException
    {
        public byte ExpectedChecksum { get; }
        public byte ActualChecksum { get; }

        public FrameChecksumMismatchedException(byte expectedChecksum, byte actualChecksum)
            : base(
                $"Frame checksum doesn't matched expected checksum: 0x{expectedChecksum:X2}, actual checksum: 0x{actualChecksum:X2}")
        {
            ExpectedChecksum = expectedChecksum;
            ActualChecksum = actualChecksum;
        }
    }
}