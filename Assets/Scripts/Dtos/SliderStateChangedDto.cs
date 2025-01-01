using System;

namespace DTOs
{
    [Serializable]
    public class StateChangedDto: StationBaseDto
    {
        public short state;
    }
}