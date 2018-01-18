using DltLib.Constants;
using DltLib.Data;
using DltLib.Exceptions;
using DltLib.Util;

namespace DltLib
{
    public class DltDataParser
    {
        private readonly ChecksumCalculator _checksumCalculator;

        public DltDataParser()
        {
            _checksumCalculator = new ChecksumCalculator();
        }

        public DltPacket ParseDltData(byte[] dltData)
        {
            if (dltData[0] != DltConstants.FrameStartByte)
                throw new InvalidFrameStartByteException(dltData[0]);
            if (dltData[7] != DltConstants.FrameStartByte)
                throw new InvalidFrameStartByteException(dltData[7]);

            var expectedChecksum = _checksumCalculator.CalculateChecksum(dltData, 0, dltData.Length - 3);
            var actualChecksum = dltData[dltData.Length - 2];
            if (expectedChecksum != actualChecksum)
                throw new FrameChecksumMismatchedException(expectedChecksum, actualChecksum);


            if (dltData[dltData.Length - 1] != DltConstants.FrameStopByte)
                throw new InvalidFrameStopByteException(dltData[dltData.Length - 1]);

            return new DltPacket
            {
                Address = ParseAddress(dltData),
                ControlByte = new ControlByte(dltData[8]),
                DataBlock = ParseDataBlock(dltData)
            };
        }

        private static long ParseAddress(byte[] dltData)
        {
            var address = 0L;
            for (var i = 6; i >= 1; --i)
            {
                var twoDigit = ByteUtility.FromHexDigit(dltData[i]);
                address *= 100;
                address += twoDigit;
            }

            return address;
        }

        private static byte[] ParseDataBlock(byte[] dltData)
        {
            var dataBlock = new byte[dltData[9]];
            for (int i = 0; i < dataBlock.Length; i++)
            {
                dataBlock[i] = (byte) (dltData[10 + i] - 0x33);
            }

            return dataBlock;
        }
    }
}