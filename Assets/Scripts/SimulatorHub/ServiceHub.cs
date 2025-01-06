using System;
using System.Collections.Generic;
using DTOs;
using Sensors;
using TMPro;
using UnityEngine;

namespace SimulatorHub
{
    public class ServiceHub : MonoBehaviour
    {
        private SignalR _signalR;
        public TMP_Text statusDisplay;

        public short busId;
        public short stationId;
        public GameObject chipClamp;
        public GameObject peristalticPump;
        public GameObject leftLoadCell;
        public GameObject lowVolumeLoadCell;
        public GameObject rightLoadCell;
        private PeristalticPump _pumpController;
        private ChipClampController _chipClampController;
        private MotorController _motorController;

        private IDictionary<VolumeSensorEnum, VolumeSensor> _volumeSensors;
        private ClampOnTouch _chipClampOnTouch;

        // Start is called before the first frame update
        void Start()
        {
            _signalR = new SignalR();
            _signalR.OnError += (sender, message) =>
            {
                UpdateStatus(message);
                _signalR.Stop();
            };
            _signalR.ConnectionStarted += (sender, args) =>
            {
                UpdateStatus($"Connection started");
                PublishEvents();
                RegisterServices();
            };
            _signalR.ConnectionClosed += (sender, args) =>
            {
                UpdateStatus($"Connection closed");
            };
            var url = "pulse-hub";
            _signalR.Init(url);
            try
            {
                UpdateStatus($"Connecting to.{url}");
                _signalR.Connect();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                UpdateStatus($"Error:{e.Message}");
            }

            _pumpController = peristalticPump.GetComponentInChildren<PeristalticPump>();
            _chipClampController = chipClamp.GetComponent<ChipClampController>();
            _chipClampOnTouch = chipClamp.GetComponentInChildren<ClampOnTouch>();
            _motorController = lowVolumeLoadCell.GetComponentInChildren<MotorController>();
            _chipClampOnTouch.OnChipClampStateChanged += (sender, args) =>
            {
                var dto = new StateChangedDto()
                {
                    busId = 1,
                    state = (short)args.State,
                    stationId = 1
                };
                var json = JsonUtility.ToJson(dto);
                _signalR.Invoke(EventHandlers.OnChipClampStateChanged, json);
            };

            _volumeSensors = new Dictionary<VolumeSensorEnum, VolumeSensor>();
            _volumeSensors[VolumeSensorEnum.Left] = leftLoadCell.GetComponentInChildren<VolumeSensor>();
            _volumeSensors[VolumeSensorEnum.LowVolume] = lowVolumeLoadCell.GetComponentInChildren<VolumeSensor>();
            _volumeSensors[VolumeSensorEnum.Right] = rightLoadCell.GetComponentInChildren<VolumeSensor>();
        }

        private void RegisterServices()
        {
           
            _signalR.On<string>(FunctionHandlers.MoveAbs, MoveAbsAction);
            _signalR.On<string>(FunctionHandlers.SetMotionAbort, SetMotionAbortAction);
            _signalR.On<string>(FunctionHandlers.StopMove, StopMoveAction);
            _signalR.On<string>(FunctionHandlers.Home, HomeAction);
          
            _signalR.On<string>(FunctionHandlers.MoveVel, MoveVelAction);
            _signalR.On<string>(FunctionHandlers.UpdateVolume, UpdateVolumeAction);
            _signalR.On<string>(FunctionHandlers.SliderAction, MoveSliderAction);
            _signalR.On<string>(FunctionHandlers.ChipClampAction, ClampChipAction);
            _signalR.On<string>(FunctionHandlers.ClearMotorFault, ClearMotorFaultAction);
        }

        private void ClearMotorFaultAction(string obj)
        {
        }

        private void MoveSliderAction(string obj)
        {
            throw new NotImplementedException();
        }

        private void UpdateVolumeAction(string request)
        {
            var dto = JsonUtility.FromJson<VolumeSensorDto>(request);
            if (_volumeSensors.TryGetValue((VolumeSensorEnum)dto.id, out var volumeSensor))
                volumeSensor.UpdateVolume((float)dto.weight);
        }

        private void HomeAction(string request)
        {
            var dto = JsonUtility.FromJson<HomeDto>(request);
            _motorController.Home(dto.busId, dto.motorId, 800f);
        }

        private void StopMoveAction(string obj)
        {
          
        }

        private void SetMotionAbortAction(string request)
        {
            var dto = JsonUtility.FromJson<SetMotionAbortDto>(request);
            _motorController.MotionAbortEnabled = dto.enableMask > 0;
        }

        private void ClampChipAction(string request)
        {
            var dto = JsonUtility.FromJson<ClampChipDto>(request);
            _chipClampController.gripState = _chipClampController.CurrentState == GripState.Opened
                ? GripState.Closing
                : GripState.Opening;
        }

        private void MoveVelAction(string request)
        {
           var dto = JsonUtility.FromJson<MoveVelDto>(request);
            _pumpController.MoveVel(dto.busId, dto.motorId, dto.velocity, true);
        }

        private void MoveAbsAction(string request)
        {
            var dto = JsonUtility.FromJson<MoveAbsDto>(request);

            _motorController.MoveAbs(dto.busId, dto.motorId, dto.velocity, dto.position);

        }

        private void PublishEvents()
        {

            _motorController.MotorMoveDone += (sender, args) =>
            {
                var dto = new MoveDoneDto()
                {
                    busId = args.BusId,
                    motorId = args.MotorID,
                    status = args.Status,
                    position = args.Position,
                    stationId = 1
                };
                var json = JsonUtility.ToJson(dto);
                _signalR.Invoke(EventHandlers.MoveDone, json);
            };

            _motorController.MotorHomeDone += (sender, args) =>
            {
                var dto = new HomeDoneDto()
                {
                    busId = args.BusId,
                    motorId = args.MotorID,
                    position = args.Position,
                    stationId = 1
                };
                var json = JsonUtility.ToJson(dto);
                _signalR.Invoke(EventHandlers.HomeDone, json);
            };

            _motorController.MotorErrorOccured += (sender, args) =>
            {
                var dto = new MotorErrorDto()
                {
                    busId = args.BustId,
                    stationId = stationId,
                    motorId = args.MotorID,
                    errorCode = args.MotorErrorCode
                };
                var json = JsonUtility.ToJson(dto);
                _signalR.Invoke(EventHandlers.MotorErrorOccured, json);
            };
        }

        private void UpdateStatus(string status)
        {
            Debug.Log(status);
            if (statusDisplay != null) statusDisplay.text = status;
        }

      
    }
}
