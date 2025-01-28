using System;

namespace DTOs
{
    [Serializable]
    public class SetMotionAbortDto: MotorBaseDto
    {
        public uint enableMask;
    }
}