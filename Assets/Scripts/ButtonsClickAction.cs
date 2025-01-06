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
                LiquidControl liquidControl;
                var x = MenuOwner.name.Contains("Left") ? 1.45f : -1.47f;
                if (_selectedOption.Contains("50"))
                {
                    tubeAdapter = Instantiate(tubeAdapter50);

                    tubeAdapter.transform.position = new Vector3(x, 15.34f, 2.09f);
                    tube = Instantiate(tube50);
                    tube.transform.position = new Vector3(x, 18.501f, 2.09f);
                    liquidControl = tube.GetComponentInChildren<LiquidControl>();
                    liquidControl.SetVolume(LiquidType.Sample, 0f);
                }
                else if (_selectedOption.Contains("15"))
                {
                    tubeAdapter = Instantiate(tubeAdapter15);
                    tubeAdapter.transform.position = new Vector3(x, 15.29f, 2.09f);
                    tube = Instantiate(tube15);
                    tube.transform.position = new Vector3(x, 18.46f, 2.04f);
                    liquidControl = tube.GetComponentInChildren<LiquidControl>();
                    liquidControl.SetVolume(LiquidType.Sample, 0f);
                }

                Destroy(ContextMenu);
                break;
            }
            case ContextMenuType.VolumeSetter:
            {
                var volume = volumeInput.GetComponentInChildren<InputField>();
                var liquid = MenuOwner.GetComponentInChildren<LiquidControl>();
                liquid.SetVolume(LiquidType.Sample, float.Parse(volume.text));
                Debug.Log($"Set Volume: {volume.text}");
                Destroy(ContextMenu);
                break;
            }
            
    }
}

}