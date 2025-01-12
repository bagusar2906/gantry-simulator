namespace EventArgs
{
    public class ChipClampErrorEventArgs
    {
        public short Operation { get; set; }
        public short State { get; set; }
        public short Error { get; set; }
    }
}