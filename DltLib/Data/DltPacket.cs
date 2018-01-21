namespace DltLib.Data
{
    public class DltPacket
    {
        public long Address { get; set; }
        public ControlByte ControlByte { get; set; }
        public byte[] DataBlock { get; set; }
    }
}