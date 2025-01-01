using System;

namespace DTOs
{
    [Serializable]
    public class MoveGripperDto : MotorBaseDto
    {
        public double position;
        public double velocity;

    }
}