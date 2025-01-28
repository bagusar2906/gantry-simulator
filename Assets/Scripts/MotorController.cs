using System;
using Enums;
using EventArgs;
using Extensions;
using UnityEngine;

public class MotorController : MonoBehaviour, IMotorSim
{
    private short _busId;
    private short _motorId;
    public MotorState moveState = MotorState.Fixed;
    public float speed = 1.0f;
    public float scale = 1;
    public float offset = 0;
    public float maxTravelLimit;
    public double targetPosition;
    private bool _isHoming ;
    public bool MotionAbortEnabled { get; set; }
    private bool _isMotionAbortTriggered;

    public event EventHandler<MotorHomeDoneEventArgs> MotorHomeDone;
    public event EventHandler<MotorMoveStartedEventArgs> MotorMoveStarted;
    public event EventHandler<MotorMoveDoneEventArgs> MotorMoveDone;
    public event EventHandler<MotorErrorOccuredEventArgs> MotorErrorOccured;

    public float CurrentPos => _currentPos.ToEng(scale, offset);
    private float _currentPos;
    
    private void Start()
    {
        var articulation = GetComponent<ArticulationBody>();

        //get jointPosition along y axis
        _currentPos = articulation.jointPosition[0];
    }
    private void FixedUpdate()
    {
        if (moveState == MotorState.Fixed) return;
        
        var articulation = GetComponent<ArticulationBody>();

        //get jointPosition along y axis
        _currentPos = articulation.jointPosition[0];
       
        //increment this y position
        var target = _currentPos + (float)moveState * Time.fixedDeltaTime * speed;
        /*target = moveState == MotorState.MovingUp
            ? Mathf.Clamp(target, _currentPos, (float)targetPosition)
            : Mathf.Clamp(target, (float)targetPosition, _currentPos);*/

        if (_currentPos >= targetPosition && moveState == MotorState.MovingUp && _isMotionAbortTriggered)
        {
            moveState = MotorState.Fixed;
            MotorErrorOccured?.Invoke(this, new MotorErrorOccuredEventArgs()
            {
                BustId = _busId,
                MotorID = _motorId,
                Position = CurrentPos,
                MotorErrorCode = (ushort)MotorErrorEnum.MotionAbort
            });
            Debug.Log($"Motor stop due to collision, pos: {CurrentPos}");
        }
        
        if (target >= targetPosition && moveState == MotorState.MovingUp )
        {
            moveState = MotorState.Fixed;
            MotorMoveDone?.Invoke(this, new MotorMoveDoneEventArgs()
            {
                BusId = _busId,
                MotorID = _motorId,
                Position = CurrentPos,
                Status = 0
            });
            Debug.Log($"Move Done:id:{_motorId}, pos:{CurrentPos}");
            return;
        }

        if (target <= targetPosition && moveState == MotorState.MovingDown)
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
                Debug.Log($"Home Done: id:{_motorId}, pos:{CurrentPos}");
                return;
            }
            MotorMoveDone?.Invoke(this, new MotorMoveDoneEventArgs()
            {
                BusId = _busId,
                MotorID = _motorId,
                Position = CurrentPos,
                Status = 0
            });
            Debug.Log($"Move Done: id:{_motorId}, pos:{CurrentPos}");
            
        }
        
        //set joint Drive to new position
        var drive = articulation.yDrive;
        drive.target = target;
        articulation.yDrive = drive;


       
    }
    

    
    private void OnCollisionEnter(Collision collision)
    {
        if (!MotionAbortEnabled)
            return;
        _isMotionAbortTriggered = true;
        // Check if we've touched the target
        
       //not immediate stop to simulate real movement
       speed *= 0.2f;
       const float overshoot = 1f;
       targetPosition = _currentPos + overshoot.ToNative(scale, offset);

       var targetPosEng = ((float)targetPosition).ToEng(scale, offset);
       Debug.Log($"Collision occured at {CurrentPos}.  it will stop at pos: {targetPosEng}");

    }

    private void OnCollisionExit(Collision other)
    {
        
          
            Debug.Log("Target exit touched!");
        
    }

    public void MoveAbs(short busId, short motorId, double velocity, double destination)
    {
     
        if (moveState != MotorState.Fixed) return;
      
        targetPosition = Mathf.Clamp(((float)destination).ToNative(scale,  offset) , 0f, maxTravelLimit);
        speed = (float)(0.015 * velocity);
        _busId = busId;
        _motorId = motorId;
        _isHoming = false;
        if (_currentPos > targetPosition)
        {
            moveState = MotorState.MovingDown;
            MotorMoveStarted?.Invoke(this, new MotorMoveStartedEventArgs()
            {
                BusId = _busId,
                MotorID = _motorId
            });
            Debug.Log($"Move Started: id:{_motorId}, pos:{CurrentPos}");
        }
        else if (_currentPos < targetPosition)
        {
            moveState = MotorState.MovingUp;
            MotorMoveStarted?.Invoke(this, new MotorMoveStartedEventArgs()
            {
                MotorID = motorId
            });
            Debug.Log($"Move Started: id:{_motorId}, pos:{CurrentPos}");
        }
        else
        {
            moveState = MotorState.Fixed;
        }
    }

    public ushort MoveVel(short busId, short motorId, double vel, bool forward)
    {
        return 0;
    }

    public void AbortMotor()
    {
        
    }


    public void ClearFault()
    {
        MotionAbortEnabled = false;
        _isMotionAbortTriggered = false;
    }
   
    public void StopMove()
    {
        moveState = MotorState.Fixed;
    }

    public void Home(short busId, short motorId, float homeVelocity)
    {
        if (moveState != MotorState.Fixed) return;
        Debug.Log($"Homing Started: id:{_motorId}");
        _isMotionAbortTriggered = false;
        targetPosition = 0;
        speed = homeVelocity / 100;
        _busId = busId;
        _motorId = motorId;
        _isHoming = true;
        moveState = MotorState.MovingDown;
    }
}