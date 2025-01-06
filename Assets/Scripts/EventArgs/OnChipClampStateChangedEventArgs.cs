using Enums;

namespace EventArgs
{
    public class OnChipClampStateChangedEventArgs : System.EventArgs
    {
        public ClampState State { get; set; }
    }
}