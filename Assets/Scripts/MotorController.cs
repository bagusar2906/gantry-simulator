using System;
using Enums;
using EventArgs;
using UnityEngine;

public class MotorController : MonoBehaviour
{
    private short _busId;
    private short _motorId;
    public MotorState moveState = MotorState.Fixed;
    public float speed = 1.0f;
    public GameObject target; // Target object to touch
    public float maxTravelLimit;
    private double _targetPosition;
    private bool _isHoming ;
    public bool MotionAbortEnabled { get; set; }

    public event EventHandler<MotorHomeDoneEventArgs> MotorHomeDone;
    public event EventHandler<MotorMoveStartedEventArgs> MotorMoveStarted;
    public event EventHandler<MotorMoveDoneEventArgs> MotorMoveDone;
    public event EventHandler<MotorErrorOccuredEventArgs> MotorErrorOccured;
    public float CurrentPos { get; private set; }
    
    private void FixedUpdate()
    {
        if (moveState == MotorState.Fixed) return;
        
        var articulation = GetComponent<ArticulationBody>();

        //get jointPosition along y axis
        CurrentPos = articulation.jointPosition[0];
        Debug.Log(CurrentPos);

        //increment this y position
        var targetPosition = CurrentPos + -(float)moveState * Time.fixedDeltaTime * speed;
        targetPosition = Mathf.Clamp(targetPosition, 0f, (float)maxTravelLimit);
        if (targetPosition >= _targetPosition && moveState == MotorState.MovingUp )
        {
            moveState = MotorState.Fixed;
            MotorMoveDone?.Invoke(this, new MotorMoveDoneEventArgs()
            {
                BusId = _busId,
                MotorID = _motorId,
                Position = CurrentPos,
                Status = 0
            });
            return;
        }

        if (targetPosition <= _targetPosition && moveState == MotorState.MovingDown)
        {
            moveState = MotorState.Fixed;
            if (_isHoming)
            {
                MotorHomeDone?.Invoke(this, new MotorHomeDoneEventArgs()
                {
                    BusId = _busId,
                    MotorID = _motorId,
                    Position = CurrentPos  
                });
                _isHoming = false;
                return;
            }
            MotorMoveDone?.Invoke(this, new MotorMoveDoneEventArgs()
            {
                BusId = _busId,
                MotorID = _motorId,
                Position = CurrentPos,
                Status = 0
            });
            return;
        }

        //set joint Drive to new position
        var drive = articulation.yDrive;
        drive.target = targetPosition;
        articulation.yDrive = drive;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if ( !MotionAbortEnabled) 
            return;
        // Check if we've touched the target
        if (collision.gameObject == target )
        {
            moveState = MotorState.Fixed;
            MotorErrorOccured?.Invoke(this, new MotorErrorOccuredEventArgs()
            {
                BustId = _busId,
                MotorID = _motorId,
                Position = CurrentPos,
                MotorErrorCode = (ushort)MotorErrorEnum.MotionAbort
            });
            Debug.Log("Target touched!");
        }
    }

    private void OnCollisionExit(Collision other)
    {
        // Check if we've touched the target
        if (other.gameObject == target)
        {
          
            Debug.Log("Target exit touched!");
        }
    }

    public void MoveAbs(short busId, short motorId, double velocity, double targetPosition)
    {
        _targetPosition = Mathf.Clamp((float)targetPosition, 0f, maxTravelLimit);
        speed = (float)velocity/100;
        _busId = busId;
        _motorId = motorId;
        if (CurrentPos > _targetPosition)
        {
            moveState = MotorState.MovingDown;
            MotorMoveStarted?.Invoke(this, new MotorMoveStartedEventArgs()
            {
                BusId = _busId,
                MotorID = _motorId
            });
        }
        else if (CurrentPos < _targetPosition)
        {
            moveState = MotorState.MovingUp;
            MotorMoveStarted?.Invoke(this, new MotorMoveStartedEventArgs()
            {
                MotorID = motorId
            });
        }
        else
        {
            moveState = MotorState.Fixed;
        }
    }
    
    

    public void ClearFault(short motorId)
    {
        MotionAbortEnabled = false;
    }
   
    public void StopMove(short motorId)
    {
        moveState = MotorState.Fixed;
    }

    public void Home(short busId, short motorId, float homeVelocity)
    {
        _isHoming = true;
        MoveAbs(busId, motorId, homeVelocity, 0f);
    }
}