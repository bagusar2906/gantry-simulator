using System;

namespace DTOs
{
    [Serializable]
    public class MotorErrorDto : MotorBaseDto
    {
        public ushort errorCode;
        public double position;
    }
}