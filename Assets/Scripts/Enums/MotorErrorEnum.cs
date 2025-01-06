using System;

namespace Enums
{
    [Flags]
    public enum MotorErrorEnum : ushort
    {
        None = 0,
        MotionAbort = 7,
        Skipped = 8

    }
}