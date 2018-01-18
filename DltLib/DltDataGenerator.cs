using DltLib.Constants;
using DltLib.Data;
using DltLib.Util;

namespace DltLib
{
    public class DltDataGenerator
    {
        private readonly ChecksumCalculator _checksumCalculator;

        public DltDataGenerator()
        {
            _checksumCalculator = new ChecksumCalculator();
        }

        public byte[] GenerateDltData(DltPacket dltPacket)
        {
            var dltData = new byte[12 + dltPacket.DataBlock.Length];

            dltData[0] = DltConstants.FrameStartByte;
            AssignAddress(dltData, dltPacket.Address);
            dltData[7] = DltConstants.FrameStartByte;
            dltData[8] = dltPacket.ControlByte.Byte;
            AssignDataBlock(dltData, dltPacket.DataBlock);

            var checksum = _checksumCalculator.CalculateChecksum(dltData, 0, dltData.Length - 3);
            dltData[dltData.Length - 2] = checksum;
            dltData[dltData.Length - 1] = DltConstants.FrameStopByte;

            return dltData;
        }

        private static void AssignAddress(byte[] dltData, long address)
        {
            for (var i = 1; i <= 6; ++i)
            {
                var twoDigit = (byte) (address % 100);
                var hexDigit = ByteUtility.ToHexDigit(twoDigit);
                dltData[i] = hexDigit;
                address /= 100;
            }
        }

        private static void AssignDataBlock(byte[] dltData, byte[] dataBlock)
        {
            dltData[9] = (byte) dataBlock.Length;
            for (int i = 0; i < dataBlock.Length; i++)
            {
                dltData[10 + i] = (byte) (dataBlock[i] + 0x33);
            }
        }
    }
}