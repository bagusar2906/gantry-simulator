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
    public GameObject MenuOwner { private get; set; }
    
    public GameObject ContextMenu { get; set; }

    private string _selectedOption;

    private void Start()
    {
        if (menuType == ContextMenuType.VolumeSetter)
        {
            var optionSelector = volumeInput.GetComponentInChildren<OptionSelector>();
            optionSelector.minFloat = 0.1f;
            optionSelector.maxFloat = MenuOwner.name.Contains("50") ? 50.0f : 15.0f;
            optionSelector.valueIndex = 0;

            var liquidControl = MenuOwner.GetComponentInChildren<LiquidControl>();
            var volume = volumeInput.GetComponentInChildren<InputField>();
            volume.text = liquidControl.volume.ToString(CultureInfo.CurrentCulture);
            
        }
        else if (menuType == ContextMenuType.AdapterSelection)
        {
            adapterSelection.onValueChanged.AddListener(OnTMPDropdownValueChanged);

            // Optionally, get the initial value
            var currentIndex = adapterSelection.value;
            _selectedOption = adapterSelection.options[currentIndex].text;
            Debug.Log($"Initial Value: {_selectedOption}");
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

        switch (menuType)
        {
            case ContextMenuType.ChipClamp:
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