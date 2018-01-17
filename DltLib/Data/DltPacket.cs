namespace DltLib.Data
{
    public class DltPacket
    {
        public long Address { get; set; }
        public byte ControlByte { get; set; }
        public byte[] DataBlock { get; set; }
    }
}