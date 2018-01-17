namespace DltLib.Util
{
    public class ChecksumCalculator
    {
        public byte CalculateChecksum(byte[] bytes, int start, int end)
        {
            byte checksum = 0;
            for (var i = start; i <= end; i++)
                checksum += bytes[i];

            return checksum;
        }
    }
}