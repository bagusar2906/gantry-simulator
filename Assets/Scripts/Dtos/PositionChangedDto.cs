using System;

namespace DTOs
{
    [Serializable]
    public class PositionChangedDto: MotorBaseDto
    {
        public double position;
    }
}