using System;
using System.Collections.Generic;
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
            add => _motorController.MotorMoveDone += value;
            remove => _motorController.MotorMoveDone -= value;
        }
        public event EventHandler<MotorHomeDoneEventArgs> MotorHomeDone
        {
            add => _motorController.MotorHomeDone += value;
            remove => _motorController.MotorHomeDone -= value;
        }
        
        public event EventHandler<MotorErrorOccuredEventArgs> MotorErrorOccured
        {
            add => _motorController.MotorErrorOccured += value;
            remove => _motorController.MotorErrorOccured -= value;
        }
        
        public event EventHandler<ChipClampStateChangedEventArgs> OnChipClampStateChanged
        {
            add => _chipClampController.OnChipClampStateChanged += value;
            remove => _chipClampController.OnChipClampStateChanged -= value;
        }
        
        private PeristalticPump _pumpController;
        private ChipClampController _chipClampController;
        private MotorController _motorController;

        private IDictionary<VolumeSensorEnum, VolumeSensor> _volumeSensors;
        
        public bool MotionAbortEnabled
        {
            get => _motorController.MotionAbortEnabled;
            set => _motorController.MotionAbortEnabled = value ; 
        }

        // Start is called before the first frame update
        void Start()
        {
          
            _pumpController = peristalticPump.GetComponentInChildren<PeristalticPump>();
            _chipClampController = chipClamp.GetComponent<ChipClampController>();
            _motorController = lowVolumeLoadCell.GetComponentInChildren<MotorController>();
          
            _volumeSensors = new Dictionary<VolumeSensorEnum, VolumeSensor>();
            _volumeSensors[VolumeSensorEnum.Left] = leftLoadCell.GetComponentInChildren<VolumeSensor>();
            _volumeSensors[VolumeSensorEnum.LowVolume] = lowVolumeLoadCell.GetComponentInChildren<VolumeSensor>();
            _volumeSensors[VolumeSensorEnum.Right] = rightLoadCell.GetComponentInChildren<VolumeSensor>();
        }
        

        public void MoveAbs(short motorId, double velocity, double targetPosition)
        {
            _motorController.MoveAbs(busId, motorId, velocity, targetPosition);
        }

        public void ClampChip(short clampState)
        {
            if (_chipClampController.gripState is GripState.Opening or GripState.Closing) 
                return;
            _chipClampController.gripState = clampState == 1 ?
                 GripState.Closing
                : GripState.Opening;
        }

        public void Home(short motorId, float velocity)
        {
            _motorController.Home(busId, motorId, velocity);
        }

        public void UpdateVolumeSensor(short sensorId, double volume)
        {
            if (_volumeSensors.TryGetValue((VolumeSensorEnum)sensorId, out var volumeSensor))
                volumeSensor.UpdateWeight((float)volume);
        }

        public void StopMove(short motorId)
        {
            _motorController.StopMove();
        }

        public void MoveVel(short motorId, double velocity, bool isForward)
        {
            _pumpController.MoveVel(busId, motorId, velocity, isForward);
        }

        public void ClearMotorFault(short motorId)
        {
            _motorController.ClearFault();
        }

        public void MoveSlider(short state)
        {
           
        }
    }
}
