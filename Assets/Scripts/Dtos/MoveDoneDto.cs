using System;

namespace DTOs
{
    [Serializable]
    public class MoveDoneDto: MotorBaseDto
    {
        public ushort status;
        public double position;
    }
}