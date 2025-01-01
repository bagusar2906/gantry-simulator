using System;

namespace DTOs
{
    [Serializable]
    public class CurrentChangedDto : MotorBaseDto
    {
        public double holdingCurrent;
    }
}