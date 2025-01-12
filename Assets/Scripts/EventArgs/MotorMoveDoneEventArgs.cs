namespace EventArgs
{
  public class MotorMoveDoneEventArgs : System.EventArgs
  {
    public short MotorID { get; set; }
    public ushort Status { get; set; }
    public double Position { get; set; }
    public short BusId { get; set; }

  }
}