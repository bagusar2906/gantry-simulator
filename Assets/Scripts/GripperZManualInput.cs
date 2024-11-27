using UnityEngine;

public class GripperZManualInput : MonoBehaviour
{
    public GameObject hand;


    void Update()
    {
        var leftRight = Input.GetAxis("BackwardForward");

        var xController = hand.GetComponent<GripperZController>();
        
        xController.moveState = MoveStateForInput(leftRight);
    }

    ZAxisState MoveStateForInput(float input)
    {
        return input switch
        {
            > 0 => ZAxisState.MovingBackward,
            < 0 => ZAxisState.MovingForward,
            _ => ZAxisState.Fixed
        };
    }
}
