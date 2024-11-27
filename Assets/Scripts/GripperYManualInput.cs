using UnityEngine;

public class GripperYManualInput : MonoBehaviour
{
    public GameObject hand;


    void Update()
    {
        float input = Input.GetAxis("Vertical");
        var moveState = MoveStateForInput(input);
        var controller = hand.GetComponent<GripperYController>();
        controller.moveState = moveState;
    }

    BigHandState MoveStateForInput(float input)
    {
        if (input > 0)
        {
            return BigHandState.MovingUp;
        }
        else if (input < 0)
        {
            return BigHandState.MovingDown;
        }
        else
        {
            return BigHandState.Fixed;
        }
    }
}
