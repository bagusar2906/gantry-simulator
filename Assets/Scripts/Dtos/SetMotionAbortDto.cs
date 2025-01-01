using System;

namespace DTOs
{
    [Serializable]
    public class SetMotionAbortDto: StationBaseDto
    {
        public uint enableMask;
    }
}