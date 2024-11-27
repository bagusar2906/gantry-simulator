using UnityEngine;

public enum BigHandState { Fixed = 0, MovingUp = -1, MovingDown = 1 };

public class GripperYController : MonoBehaviour
{

    public BigHandState moveState = BigHandState.Fixed;
    public float speed = 1.0f;

    private void FixedUpdate()
    {
        if (moveState != BigHandState.Fixed)
        {
            ArticulationBody articulation = GetComponent<ArticulationBody>();

            //get jointPosition along y axis
            float xDrivePosition = articulation.jointPosition[0];
            Debug.Log(xDrivePosition);

            //increment this y position
            float targetPosition = xDrivePosition + -(float)moveState * Time.fixedDeltaTime * speed;

            //set joint Drive to new position
            var drive = articulation.xDrive;
            drive.target = targetPosition;
            articulation.xDrive = drive;
        }
    }
}
