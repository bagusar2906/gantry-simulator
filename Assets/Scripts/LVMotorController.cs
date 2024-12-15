using UnityEngine;

public class LVMotorController : MonoBehaviour
{
    public MotorState moveState = MotorState.Fixed;
    public float speed = 1.0f;
    public GameObject target; // Target object to touch
    private bool _isMoving = true;
    private void FixedUpdate()
    {
        if (moveState != MotorState.Fixed)
        {
            if (moveState == MotorState.MovingUp && !_isMoving)
            {
               // isMoving = true;
                return;
            }
            var articulation = GetComponent<ArticulationBody>();

            //get jointPosition along y axis
            var drivePosition = articulation.jointPosition[0];
            Debug.Log(drivePosition);

            //increment this y position
            var targetPosition = drivePosition + -(float)moveState * Time.fixedDeltaTime * speed;

            //set joint Drive to new position
            var drive = articulation.yDrive;
            drive.target = targetPosition;
            articulation.yDrive = drive;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Check if we've touched the target
        if (collision.gameObject == target && _isMoving)
        {
            _isMoving = false; // Stop moving
            Debug.Log("Target touched!");
        }
    }

    private void OnCollisionExit(Collision other)
    {
        // Check if we've touched the target
        if (other.gameObject == target && !_isMoving)
        {
            _isMoving = true; //  moving
            Debug.Log("Target exit touched!");
        }
    }
}