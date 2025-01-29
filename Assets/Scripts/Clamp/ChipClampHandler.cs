using Enums;
using UnityEngine;

namespace Clamp
{
    public class ChipClampHandler : MonoBehaviour
    {
        public GameObject chipClamp;
        private ArticulationBody _articulationBody;
        private ChipClampController _chipClampController;

        void Start()
        {
            // Get the ArticulationBody component
            _articulationBody = GetComponent<ArticulationBody>();
             _chipClampController = chipClamp.GetComponent<ChipClampController>();
        }

        void Update()
        {
            // Example: Rotate the hinge to a specific target position using drive
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SetTargetRotation(45f); // Set hinge to 45 degrees
            }
            else if (Input.GetKeyDown(KeyCode.R))
            {
                SetTargetRotation(0f); // Reset hinge to 0 degrees
            }
        }

        void SetTargetRotation(float targetAngle)
        {
            // Configure the drive for rotational movement
            var drive = _articulationBody.xDrive;
            drive.target = targetAngle; // Set the target rotation
            _articulationBody.xDrive = drive;
        }

        public void ToggleAction()
        {
            var drive = _articulationBody.xDrive;
            var clampState = drive.target > 1f ? ClampState.UnClamp : ClampState.Clamp;
            _chipClampController.ClampChip(clampState);
            SetTargetRotation(drive.target > 1f ? 0f : 45f);
           
        }
    }
}
