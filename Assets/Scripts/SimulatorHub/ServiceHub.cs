using System;
using DTOs;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SimulatorHub
{
    public class ServiceHub : MonoBehaviour
    {
        private SignalR _signalR;
        public TMP_Text statusDisplay;

        public GameObject chipClamp;
        public GameObject peristalticPump;
        private PeristalticPump _pumpController;
        private ChipClampController _chipClampController;


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

            _pumpController = GetComponentInChildren<PeristalticPump>();
            _chipClampController = GetComponentInChildren<ChipClampController>();
          

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
            throw new NotImplementedException();
        }

        private void MoveSliderAction(string obj)
        {
            throw new NotImplementedException();
        }

        private void UpdateVolumeAction(string obj)
        {
            throw new NotImplementedException();
        }

        private void HomeAction(string obj)
        {
            throw new NotImplementedException();
        }

        private void StopMoveAction(string obj)
        {
            throw new NotImplementedException();
        }

        private void SetMotionAbortAction(string obj)
        {
            throw new NotImplementedException();
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
            _pumpController.MoveVel(dto.velocity, true);
        }

        private void MoveAbsAction(string request)
        {
            
        }

        private void PublishEvents()
        {
           
        }

        private void UpdateStatus(string status)
        {
            Debug.Log(status);
            if (statusDisplay != null) statusDisplay.text = status;
        }

      
    }
}
