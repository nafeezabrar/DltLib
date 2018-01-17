using System;

namespace DltLib.Util
{
    public static class ByteUtility
    {
        public static byte FromHexDigit(byte hexByte)
        {
            return (byte) (hexByte - hexByte / 16 * 6);
        }

        public static byte ToHexDigit(byte twoDigit)
        {
            return (byte) (twoDigit + twoDigit / 10 * 6);
        }

        public static int GetBit(byte @byte, int position)
        {
            if (0 > position || position > 7)
                throw new ArgumentOutOfRangeException(nameof(position));
            return (@byte & 1 << position) == 0 ? 0 : 1;
        }

        public static byte SetBit(byte @byte, int position, int bit)
        {
            if (0 > position || position > 7)
                throw new ArgumentOutOfRangeException(nameof(position));

            switch (bit)
            {
                case 1:
                    return (byte) (@byte | (1 << position));
                case 0:
                    return (byte) (@byte & ~(1 << position));
                default:
                    throw new ArgumentOutOfRangeException(nameof(bit));
            }
        }
    }
}