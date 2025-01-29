using System;
using System.Collections.Generic;
using Enums;
using EventArgs;
using Sensors;
using SimulatorHub;
using UnityEngine;

namespace Station
{
    public class PulseStation : MonoBehaviour
    {
        private SignalR _signalR;
       
        public short busId;
        public short stationId;
        public GameObject chipClamp;
        public GameObject peristalticPump;
        public GameObject leftLoadCell;
        public GameObject lowVolumeLoadCell;
        public GameObject rightLoadCell;
        
        public event EventHandler<LoadCellValueChangedEventArgs> InitialWeightChanged
        {
            add
            {
                _volumeSensors[VolumeSensorEnum.Left].OnInitialWeightChanged += value;
                _volumeSensors[VolumeSensorEnum.LowVolume].OnInitialWeightChanged += value;
                _volumeSensors[VolumeSensorEnum.Right].OnInitialWeightChanged += value;
            }
            remove
            {
                _volumeSensors[VolumeSensorEnum.Left].OnInitialWeightChanged -= value;
                _volumeSensors[VolumeSensorEnum.LowVolume].OnInitialWeightChanged -= value;
                _volumeSensors[VolumeSensorEnum.Right].OnInitialWeightChanged -= value;
            }
        }

        public event EventHandler<MotorMoveDoneEventArgs> MotorMoveDone
        {
            add
            {
                _motorController[0].MotorMoveDone += value;
                _motorController[1].MotorMoveDone += value;
            }
            remove
            {
                _motorController[0].MotorMoveDone -= value;
                _motorController[1].MotorMoveDone -= value;
            }
        }
        public event EventHandler<MotorHomeDoneEventArgs> MotorHomeDone
        {
            add
            {
                _motorController[0].MotorHomeDone += value;
                _motorController[1].MotorHomeDone += value;
            }
            remove
            {
                _motorController[0].MotorHomeDone -= value;
                _motorController[1].MotorHomeDone -= value;
            }
        }
        
        public event EventHandler<MotorErrorOccuredEventArgs> MotorErrorOccured
        {
            add
            {
                _motorController[0].MotorErrorOccured += value;
                _motorController[1].MotorErrorOccured += value;
            }
            remove
            {
                _motorController[0].MotorErrorOccured -= value;
                _motorController[1].MotorErrorOccured -= value;
            }
        }
        
        public event EventHandler<ChipClampStateChangedEventArgs> OnChipClampStateChanged
        {
            add => _chipClampController.OnChipClampStateChanged += value;
            remove => _chipClampController.OnChipClampStateChanged -= value;
        }
        
        
        private ChipClampController _chipClampController;
      

        private IDictionary<VolumeSensorEnum, VolumeSensor> _volumeSensors;
        private IDictionary<short, IMotorSim> _motorController;

        public void SetMotionAbort(short motorId, bool state)
        {
            if (_motorController.TryGetValue(motorId, out var motor))
                motor.MotionAbortEnabled = state;
        }

        public bool GetMotionAbort(short motorId)
        {
            return _motorController.TryGetValue(motorId, out var motor) 
                   && motor.MotionAbortEnabled;
        }
        // Start is called before the first frame update
        void Start()
        {
          
            var pumpMotor = peristalticPump.GetComponentInChildren<PeristalticPump>();
            _chipClampController = chipClamp.GetComponent<ChipClampController>();
            var lvMotor = lowVolumeLoadCell.GetComponentInChildren<MotorController>();
          
            _volumeSensors = new Dictionary<VolumeSensorEnum, VolumeSensor>();
            _volumeSensors[VolumeSensorEnum.Left] = leftLoadCell.GetComponentInChildren<VolumeSensor>();
            _volumeSensors[VolumeSensorEnum.LowVolume] = lowVolumeLoadCell.GetComponentInChildren<VolumeSensor>();
            _volumeSensors[VolumeSensorEnum.Right] = rightLoadCell.GetComponentInChildren<VolumeSensor>();

            _motorController = new Dictionary<short, IMotorSim>()
            {
                { 0, pumpMotor },
                { 1, lvMotor }
            };
        }
        

        public void MoveAbs(short motorId, double velocity, double targetPosition)
        {
            if (_motorController.TryGetValue(motorId, out var motor))
             motor.MoveAbs(busId, motorId, velocity, targetPosition);
        }

        public void ClampChip(short clampState)
        {
            _chipClampController.ClampChip(clampState == 1 ? ClampState.Clamp 
                : ClampState.UnClamp);
        }

        public void Home(short motorId, float velocity)
        {
            if (_motorController.TryGetValue(motorId, out var motor))
                motor.Home(busId, motorId, velocity);
        }

        public void UpdateVolumeSensor(short sensorId, double volume)
        {
            if (_volumeSensors.TryGetValue((VolumeSensorEnum)sensorId, out var volumeSensor))
                volumeSensor.UpdateWeight((float)volume);
        }

        public void StopMove(short motorId)
        {
            if (_motorController.TryGetValue(motorId, out var motor))
                motor.StopMove();
        }

        public void MoveVel(short motorId, double velocity, bool isForward)
        {
            if (_motorController.TryGetValue(motorId, out var motor))
                motor.MoveVel(busId, motorId, velocity, isForward);
        }

        public void ClearMotorFault(short motorId)
        {
            if (_motorController.TryGetValue(motorId, out var motor))
                motor.ClearFault();
        }

        public void MoveSlider(short state)
        {
           
        }
    }
}
