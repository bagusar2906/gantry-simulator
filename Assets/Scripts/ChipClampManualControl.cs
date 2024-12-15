using UnityEngine;

public class ChipClampManualControl : MonoBehaviour
{
    public GameObject hand;


    void Update()
    {
        float input = Input.GetAxis("ChipClamp");
        var moveState = MoveStateForInput(input);
        var controller = hand.GetComponent<ChipClampController>();
        controller.gripState = moveState;
    }

    GripState MoveStateForInput(float input)
    {
        if (input > 0)
        {
            return GripState.Opening;
        }
        else if (input < 0)
        {
            return GripState.Closing;
        }
        else
        {
            return GripState.Fixed;
        }
    }
}