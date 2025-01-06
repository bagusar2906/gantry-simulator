using System;
using UnityEngine;

public class LVManualControl : MonoBehaviour
{
    public GameObject hand;


    void Update()
    {
        var input = Input.GetAxis("Vertical");
        var moveState = MoveStateForInput(input);
        var controller = hand.GetComponent<MotorController>();
        controller.moveState = moveState;
        switch (controller.moveState)
        {
            case MotorState.MovingUp:
                break;
            case MotorState.MovingDown:
                break;
            case MotorState.Fixed:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    MotorState MoveStateForInput(float input)
    {
        return input switch
        {
            > 0 => MotorState.MovingUp,
            < 0 => MotorState.MovingDown,
            _ => MotorState.Fixed
        };
    }
}