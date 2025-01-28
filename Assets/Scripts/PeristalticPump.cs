using System;
using EventArgs;
using UnityEngine;

public class PeristalticPump :  MonoBehaviour, IMotorSim
{
    public float rotationSpeed = 100f;

    public Vector3 rotationAxis = Vector3.up;
    public ParticleSystem liquidFlow;

    private readonly double _updatePositionIntervalInSec = 0.5;
    private float _rotationSpeed = 0;
    private double _totalTime;

    public bool IsPumping => _rotationSpeed > 0;
    // Start is called before the first frame update
    void Start()
    {
        liquidFlow.Stop();
    }

    public event EventHandler<MotorMoveDoneEventArgs> MotorMoveDone;
    public event EventHandler<MotorHomeDoneEventArgs> MotorHomeDone;
    public event EventHandler<MotorMoveStartedEventArgs> MotorMoveStarted;
    public event EventHandler<MotorErrorOccuredEventArgs> MotorErrorOccured;
    public bool MotionAbortEnabled { get; set; }

    public void MoveAbs(short busId, short motorId, double velocity, double destination)
    {
        
    }

    public ushort MoveVel(short busId, short motorId, double vel, bool forward)
    {
        _rotationSpeed = rotationSpeed;
        if (!(rotationSpeed > 0)) return 0;
        if (liquidFlow != null)
            liquidFlow.Play();
        _totalTime = 0;
        
        return 0;
    }

    public void AbortMotor()
    {
        _rotationSpeed = 0;
        liquidFlow.Stop();
    }

    public void StopMove()
    {
        _rotationSpeed = 0;
        liquidFlow.Stop();
    }

    public void ClearFault()
    {
    }

    public void Home(short busId, short motorId, float homeVelocity)
    {
    }


    // Update is called once per frame
    void Update()
    {
        if (_rotationSpeed == 0)
            return;
        
        transform.Rotate(rotationAxis, _rotationSpeed * Time.deltaTime);
        _totalTime += Time.deltaTime;
        if (!(_totalTime > _updatePositionIntervalInSec)) return;
        _totalTime = 0;
    }
}
