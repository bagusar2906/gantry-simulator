namespace EventArgs
{
    public class WeightChangedEventArgs : System.EventArgs
    {
        public double Weight { get; set; }
        public LiquidType LiquidType { get; set; }
    }
}