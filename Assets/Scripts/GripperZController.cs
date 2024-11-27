using UnityEngine;

public enum ZAxisState { Fixed = 0, MovingBackward = -1, MovingForward = 1 };

public class GripperZController : MonoBehaviour
{

    public ZAxisState moveState = ZAxisState.Fixed;
    public float speed = 1.0f;

    private void FixedUpdate()
    {
        if (moveState != ZAxisState.Fixed)
        {
            ArticulationBody articulation = GetComponent<ArticulationBody>();

            //get jointPosition along y axis
            float drivePosition = articulation.jointPosition[0];
            Debug.Log(drivePosition);

            //increment this x position
            float targetPosition = drivePosition + -(float)moveState * Time.fixedDeltaTime * speed;

            //set joint Drive to new position
            var drive = articulation.zDrive;
            drive.target = targetPosition;
            articulation.zDrive = drive;
        }
    }
}
