using System;
using EventArgs;

public interface IMotorSim
{
    event EventHandler<MotorMoveDoneEventArgs> MotorMoveDone;
    event EventHandler<MotorHomeDoneEventArgs> MotorHomeDone;
    event EventHandler<MotorMoveStartedEventArgs> MotorMoveStarted;
    event EventHandler<MotorErrorOccuredEventArgs> MotorErrorOccured;
    bool MotionAbortEnabled { get; set; }
    void MoveAbs(short busId, short motorId, double velocity, double destination);
    ushort MoveVel(short busId, short motorId, double vel, bool forward);
    void AbortMotor();
    void StopMove();
    void ClearFault();
    void Home(short busId, short motorId, float homeVelocity);
   
}