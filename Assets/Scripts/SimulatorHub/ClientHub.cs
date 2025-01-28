using System;
using System.Collections.Generic;
using System.Linq;
using DTOs;
using Sensors;
using Station;
using TMPro;
using UnityEngine;

namespace SimulatorHub
{
    public class ClientHub : MonoBehaviour
    {
        public List<GameObject> stations;
        public TMP_Text statusDisplay;


        private readonly IDictionary<int, PulseStation> _stationsMap = new Dictionary<int, PulseStation>();
        private SignalR _signalR;
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

            foreach (var pulseStation in stations.Select(station => station.GetComponent<PulseStation>()))
            {
                _stationsMap[pulseStation.stationId] = pulseStation;
            }
            
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

       


        private void PublishEvents()
        {

            foreach (var pulseStation in _stationsMap.Values)
            {
                pulseStation.InitialWeightChanged += (sender, args) =>
                {
                    var dto = new LoadCellValueDto()
                    {
                        id = args.Id,
                        busId = pulseStation.busId,
                        weight = args.Weight,
                        stationId = pulseStation.stationId
                    };
                    var json = JsonUtility.ToJson(dto);
                    _signalR.Invoke(EventHandlers.OnWeightChanged, json);
                };
                
                pulseStation.MotorMoveDone += (sender, args) =>
                {
                    var dto = new MoveDoneDto()
                    {
                        busId = pulseStation.busId,
                        motorId = args.MotorID,
                        status = args.Status,
                        position = args.Position,
                        stationId = pulseStation.stationId
                    };
                    var json = JsonUtility.ToJson(dto);
                    _signalR.Invoke(EventHandlers.MoveDone, json);
                };

                pulseStation.MotorHomeDone += (sender, args) =>
                {
                    var dto = new HomeDoneDto()
                    {
                        busId = pulseStation.busId,
                        motorId = args.MotorID,
                        position = args.Position,
                        stationId = pulseStation.stationId
                    };
                    var json = JsonUtility.ToJson(dto);
                    _signalR.Invoke(EventHandlers.HomeDone, json);
                };

                pulseStation.MotorErrorOccured += (sender, args) =>
                {
                    var dto = new MotorErrorDto()
                    {
                        busId = pulseStation.busId,
                        stationId = pulseStation.stationId,
                        motorId = args.MotorID,
                        errorCode = args.MotorErrorCode
                    };
                    var json = JsonUtility.ToJson(dto);
                    _signalR.Invoke(EventHandlers.MotorErrorOccured, json);
                };

                pulseStation.OnChipClampStateChanged += (sender, args) =>
                {
                    var dto = new StateChangedDto()
                    {
                        busId = pulseStation.busId,
                        state = (short)args.State,
                        stationId = pulseStation.stationId
                    };
                    var json = JsonUtility.ToJson(dto);
                    _signalR.Invoke(EventHandlers.OnChipClampStateChanged, json);
                };

            }
            
        }

        private void SetMotionAbortAction(string request)
        {
            var dto = JsonUtility.FromJson<SetMotionAbortDto>(request);
            _stationsMap[dto.stationId].SetMotionAbort(dto.motorId, dto.enableMask > 0) ;
        }

        private void ClampChipAction(string request)
        {
            var dto = JsonUtility.FromJson<ClampChipDto>(request);
            _stationsMap[dto.stationId].ClampChip(dto.state);
        }

        private void HomeAction(string request)
        {
            var dto = JsonUtility.FromJson<HomeDto>(request);
            _stationsMap[dto.stationId].Home( dto.motorId, 800f);
        }

        private void StopMoveAction(string request)
        {
            var dto = JsonUtility.FromJson<StopMoveDto>(request);
            _stationsMap[dto.stationId].StopMove(dto.motorId);
        }

        private void UpdateVolumeAction(string request)
        {
            var dto = JsonUtility.FromJson<VolumeSensorDto>(request);
            _stationsMap[dto.stationId].UpdateVolumeSensor(dto.id, dto.weight);
        }
        private void MoveAbsAction(string request)
        {
            var dto = JsonUtility.FromJson<MoveAbsDto>(request);

            _stationsMap[dto.stationId].MoveAbs(dto.motorId, dto.velocity, dto.position);
        }
        
        private void MoveVelAction(string request)
        {
            var dto = JsonUtility.FromJson<MoveVelDto>(request);
            _stationsMap[dto.stationId].MoveVel(dto.motorId, dto.velocity, true);
        }
        
        private void ClearMotorFaultAction(string request)
        {
            var dto = JsonUtility.FromJson<ClearMotorFaultDto>(request);
            _stationsMap[dto.stationId].ClearMotorFault(dto.motorId);
        }

        private void MoveSliderAction(string request)
        {
            var dto = JsonUtility.FromJson<MoveSliderDto>(request);
            _stationsMap[dto.stationId].MoveSlider(dto.state);
        }
        private void UpdateStatus(string status)
        {
            Debug.Log(status);
            if (statusDisplay != null) statusDisplay.text = status;
        }
    }
}
