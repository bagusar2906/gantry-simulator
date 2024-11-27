using UnityEngine;

public class GripperXManualInput : MonoBehaviour
{
    public GameObject hand;


    void Update()
    {
        var leftRight = Input.GetAxis("LeftRight");

        var xController = hand.GetComponent<GripperXController>();
        
        xController.moveState = MoveStateForInput(leftRight);
    }

    XAxisState MoveStateForInput(float input)
    {
        return input switch
        {
            > 0 => XAxisState.MovingLeft,
            < 0 => XAxisState.MovingRight,
            _ => XAxisState.Fixed
        };
    }
}
