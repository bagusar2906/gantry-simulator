using System.Globalization;
using TMPro;
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
    public GameObject tubeAdapter1_5;
    public GameObject tube1_5;
    public ContextMenuType menuType;
    public GameObject peristalticPump;
    public GameObject MenuOwner { private get; set; }
    
    public GameObject ContextMenu { get; set; }

    private string _selectedOption;

    private void Start()
    {
        switch (menuType)
        {
            case ContextMenuType.VolumeSetter:
            {
                var optionSelector = volumeInput.GetComponentInChildren<OptionSelector>();
                optionSelector.minFloat = 0.1f;
                optionSelector.maxFloat = MenuOwner.name.Contains("50") ? 50.0f : 15.0f;
                optionSelector.valueIndex = 0;

                var liquidControl = MenuOwner.GetComponentInChildren<LiquidControl>();
                var volume = volumeInput.GetComponentInChildren<InputField>();
                volume.text = liquidControl.volume.ToString(CultureInfo.CurrentCulture);
                break;
            }
            case ContextMenuType.AdapterSelection:
            {
                adapterSelection.onValueChanged.AddListener(OnTMPDropdownValueChanged);

                // Optionally, get the initial value
                var currentIndex = adapterSelection.value;
                _selectedOption = adapterSelection.options[currentIndex].text;
                Debug.Log($"Initial Value: {_selectedOption}");
                break;
            }
        }
    }

    private void OnTMPDropdownValueChanged(int index)
    {
        _selectedOption = adapterSelection.options[index].text;
        Debug.Log($"Selected: {_selectedOption}");
    }

    public void OnClick()
    {
        Debug.Log(name + " was clicked.");

        Text caption;
        switch (menuType)
        {
            case ContextMenuType.ChipClamp:
                caption = GetComponentInChildren<Text>();
                var controller = chipClamp.GetComponent<ChipClampController>();
                if (controller.CurrentState == GripState.Opened)
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
            case ContextMenuType.PeristalticPump:
                caption = GetComponentInChildren<Text>();
                var pumpController = peristalticPump.GetComponentInChildren<PeristalticPump>();
                if (!pumpController.IsPumping)
                {
                    pumpController.MoveVel(5,1,10, true);
                    caption.text = "Pump OFF";
                }
                else
                {
                    pumpController.AbortMotor();
                    caption.text = "Pump ON";
                }

                break;
            case ContextMenuType.AdapterSelection:
            {
                Debug.Log($"Tube: {_selectedOption} ml");
                GameObject tubeAdapter;
                GameObject tube;
                var x = MenuOwner.name.Contains("Left") ? 1.45f : -1.47f;
                if (_selectedOption.Contains("50"))
                {
                    tube = Instantiate(tube50);
                    tube.transform.position = new Vector3(x, 19.401f, 2.09f);
                    tubeAdapter = Instantiate(tubeAdapter50, tube.transform, true);
                    tubeAdapter.transform.localPosition = new Vector3(0, -0.0754f, 0f);
                    
                }
                else if (_selectedOption.Contains("15"))
                {
                    tube = Instantiate(tube15);
                    tube.transform.position = new Vector3(x, 18.46f, 2.04f);
                    tubeAdapter = Instantiate(tubeAdapter15, tube.transform, true);
                    tubeAdapter.transform.localPosition = new Vector3(0f, -0.073f, 0.0023f);

                }
                else 
                {
                    tube = Instantiate(tube1_5);
                    tube.transform.position = new Vector3(-0.01f, 18.92f, 2.01f);
                    tubeAdapter = Instantiate(tubeAdapter1_5, tube.transform, true);
                    tubeAdapter.transform.localPosition = new Vector3(0f, -0.073f, 0f);

                }

                Destroy(ContextMenu);
                break;
            }
            case ContextMenuType.VolumeSetter:
            {
                var volume = volumeInput.GetComponentInChildren<InputField>();
                var tube = MenuOwner.GetComponentInChildren<Tube>();
                tube.Fill(LiquidType.Sample, float.Parse(volume.text), true);
                Debug.Log($"Set Volume: {volume.text}");
                Destroy(ContextMenu);
                break;
            }
            
    }
}

}