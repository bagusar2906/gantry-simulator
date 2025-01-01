using System;

namespace DTOs
{
    [Serializable]
    public class LoadCellValueDto:StationBaseDto
    {
        public short id;
        public double weight;
    }
}