using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class ChipClampManualControl : MonoBehaviour
{
     public GameObject chipClamp;


    void Update()
    {
        float input = Input.GetAxis("ChipClamp");
        var moveState = MoveStateForInput(input);
        var controller = chipClamp.GetComponent<ChipClampController>();
       // controller.gripState = moveState;
    }

    public void OnClampButtonClick()
    {
        
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