using UnityEngine;

public enum XAxisState { Fixed = 0, MovingLeft = 1, MovingRight = -1 };

public class GripperXController : MonoBehaviour
{

    public XAxisState moveState = XAxisState.Fixed;
    public float speed = 1.0f;

    private void FixedUpdate()
    {
        if (moveState != XAxisState.Fixed)
        {
            ArticulationBody articulation = GetComponent<ArticulationBody>();

            //get jointPosition along y axis
            float drivePosition = articulation.jointPosition[0];
            Debug.Log(drivePosition);

            //increment this x position
            float targetPosition = drivePosition + -(float)moveState * Time.fixedDeltaTime * speed;

            //set joint Drive to new position
            var drive = articulation.xDrive;
            drive.target = targetPosition;
            articulation.xDrive = drive;
        }
    }
}
