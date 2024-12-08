using UnityEngine;

public class LVMotorController : MonoBehaviour
{
    public MotorState moveState = MotorState.Fixed;
    public float speed = 1.0f;
    private void FixedUpdate()
    {
        if (moveState != MotorState.Fixed)
        {
            ArticulationBody articulation = GetComponent<ArticulationBody>();

            //get jointPosition along y axis
            float drivePosition = articulation.jointPosition[0];
            Debug.Log(drivePosition);

            //increment this y position
            var targetPosition = drivePosition + -(float)moveState * Time.fixedDeltaTime * speed;

            //set joint Drive to new position
            var drive = articulation.yDrive;
            drive.target = targetPosition;
            articulation.yDrive = drive;
        }
    }
}