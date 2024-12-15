using UnityEngine;

public class LVManualControl : MonoBehaviour
{
    public GameObject hand;


    void Update()
    {
        float input = Input.GetAxis("Vertical");
        var moveState = MoveStateForInput(input);
        var controller = hand.GetComponent<LVMotorController>();
        controller.moveState = moveState;
    }

    MotorState MoveStateForInput(float input)
    {
        if (input > 0)
        {
            return MotorState.MovingUp;
        }
        else if (input < 0)
        {
            return MotorState.MovingDown;
        }
        else
        {
            return MotorState.Fixed;
        }
    }
}