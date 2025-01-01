using System;

namespace DTOs
{
    [Serializable]
    public class MoveAbsDto : MotorBaseDto
    {
        public double position;
        public double velocity;

    }
}