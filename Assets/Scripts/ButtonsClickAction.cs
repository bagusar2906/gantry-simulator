using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ButtonsClickAction : MonoBehaviour
{
    public TMP_Dropdown adapterSelection;
    public GameObject volumeInput;
    public GameObject chipClamp;
 
    public GameObject ContextMenu { get; set; }

    private string _selectedOption;

    private void Start()
    {
        if (adapterSelection == null)
            return;
        
        adapterSelection.onValueChanged.AddListener(OnTMPDropdownValueChanged);

        // Optionally, get the initial value
        var currentIndex = adapterSelection.value;
        _selectedOption = adapterSelection.options[currentIndex].text;
        Debug.Log($"Initial Value: {_selectedOption}");
    }

    private void OnTMPDropdownValueChanged(int index)
    {
         _selectedOption = adapterSelection.options[index].text;
        Debug.Log($"Selected: {_selectedOption}");
    }

    public void OnClick()
    {
        Debug.Log(name + " was clicked.");
        switch (name)
        {
            case "ClampButton":
                var caption = GetComponentInChildren<Text>();
                var controller = chipClamp.GetComponent<ChipClampController>();
                if (caption.text == "Clamp")
                {
                    controller.gripState = GripState.Closing;
                    caption.text = "UnClamp";
                }
                else
                {
                    controller.gripState = GripState.Opening;
                    caption.text = "Clamp";
                }
              
                break;
            default:
                if (adapterSelection != null)
                {
                    Debug.Log($"Tube: {_selectedOption} ml");
                }
                else if (volumeInput != null)
                {
                    var volume = volumeInput.GetComponentInChildren<InputField>();
                    Debug.Log($"Set Volume: {volume.text}");
                }

                Destroy(ContextMenu);
                break;
        }
    }

}