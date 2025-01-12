using Enums;

namespace EventArgs
{
    public class ChipClampStateChangedEventArgs : System.EventArgs
    {
        public ClampState State { get; set; }
        public short BusId { get; set; }
    }
}