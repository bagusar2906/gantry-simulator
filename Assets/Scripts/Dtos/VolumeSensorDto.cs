using System;

namespace DTOs
{
    [Serializable]
    public class VolumeSensorDto: StationBaseDto
    {
        public short id;
        public double weight;
        public double flowRate;
    }
}