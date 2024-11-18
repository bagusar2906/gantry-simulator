using UnityEngine;

public class ZAxisJointController : MonoBehaviour
{
    public float targetVelocity = 2f;  // Set desired sliding speed
    public float positionSpring = 100f; // Spring to keep joint in position
    public float positionDamper = 10000f;  // Damper to slow down movement
    
    private ConfigurableJoint _configurableJoint;

    // Start is called before the first frame update
    void Start()
    {
        _configurableJoint = GetComponent<ConfigurableJoint>();

        // Configure X Drive (or Y/Z Drive based on your axis)
        var drive = new JointDrive
        {
            positionSpring = positionSpring,
            positionDamper = positionDamper,
            maximumForce = Mathf.Infinity
        };

        // Apply drive to the desired axis
        _configurableJoint.zDrive = drive;
        
    }

    // Update is called once per frame
    void Update()
    {
        // Control Target Position with Input
        if (Input.GetKeyDown(KeyCode.W))
        {
            var newTarget = _configurableJoint.targetPosition;
            newTarget.z = -newTarget.z;  // Reverse direction
            _configurableJoint.targetPosition = newTarget;
            // Control Target Velocity with Input
            var velocity = _configurableJoint.targetVelocity;
            velocity.z = targetVelocity;
            _configurableJoint.targetVelocity = velocity;
        }

       
    }
}