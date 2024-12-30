using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ButtonsClickAction : MonoBehaviour
{
    public TMP_Dropdown adapterSelection;
    public GameObject volumeInput;
    public GameObject chipClamp;
    public GameObject tubeAdapter50;
    public GameObject tube50;
    public GameObject tubeAdapter15;
    public GameObject tube15;
    public GameObject MenuOwner { private get; set; }

 
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
                    GameObject tubeAdapter;
                    GameObject tube;
                    var x = MenuOwner.name.Contains("Left") ? 1.45f : -1.47f;
                    if (_selectedOption.Contains("50"))
                    { 
                        tubeAdapter = Instantiate(tubeAdapter50);
                       
                        tubeAdapter.transform.position = new Vector3(x, 15.34f, 2.09f);
                        tube = Instantiate(tube50);
                        tube.transform.position = new Vector3(x, 18.501f, 2.09f);
                    }
                    else if (_selectedOption.Contains("15"))
                    {
                        tubeAdapter = Instantiate(tubeAdapter15);
                        tubeAdapter.transform.position = new Vector3(x, 15.29f, 2.09f);
                        tube = Instantiate(tube15);
                        tube.transform.position = new Vector3(x, 18.46f, 2.04f);
                    }
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