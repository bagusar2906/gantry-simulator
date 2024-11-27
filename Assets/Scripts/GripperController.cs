using UnityEngine;

public class GripperController : MonoBehaviour
{
    private HingeJoint _finger;

    // Start is called before the first frame update
    void Start()
    {
        _finger = GetComponentInChildren<HingeJoint>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.K))
        {
            _finger.useMotor = true;
        }
        else if (Input.GetKey(KeyCode.L))
        {
            _finger.useMotor = false;
        }
    }
}
