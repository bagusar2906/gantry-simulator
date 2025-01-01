using System;

namespace DTOs
{
    [Serializable]
    public class MoveGripperDoneDto: MotorBaseDto
    {
        public double position;
    }
}