using System.Collections.Generic;
using Clamp;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using MouseButton = UnityEngine.UIElements.MouseButton;

public class OutlineSelection : MonoBehaviour
{
    public GameObject contextMenuAdapterPrefab;
    public GameObject contextMenuSetVolumePrefab;
    public GameObject chipClamp;
    private GameObject _currentContextMenu;
    private Transform _highLight;

    private RaycastHit _raycastHit;
    // Start is called before the first frame update
    
    // Update is called once per frame
    void Update()
    {
        //Highlight

        if (_highLight != null)
        {
            _highLight.gameObject.GetComponent<Outline>().enabled = false;
            _highLight = null;
        }

        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (!EventSystem.current.IsPointerOverGameObject() && Physics.Raycast(ray, out _raycastHit))
        {
            _highLight = _raycastHit.transform;
            if (_highLight.CompareTag("Selectable"))
            {
                var outline = _highLight.gameObject.GetComponent<Outline>();
                if (outline != null)
                {
                    outline.enabled = true;
                }
                else
                {
                    outline = _highLight.gameObject.AddComponent<Outline>();
                    outline.enabled = true;
                    outline.OutlineColor = Color.cyan;
                    outline.OutlineWidth = 2.0f;
                }
            }
            else
            {
                _highLight = null;
            }
        }

        GameObject highLightObject;
        if (Input.GetMouseButtonDown((int)MouseButton.LeftMouse))
        {
            if (!_highLight) return;
            highLightObject = _highLight.transform.gameObject;

            if (highLightObject.name.Contains("ChipClamp") 
                || highLightObject.name.Contains("Lever"))
            {
                var chipClampHandler = chipClamp.GetComponentInChildren<ChipClampHandler>();
                chipClampHandler.ToggleAction();
                return;
            }
        }
        if (!Input.GetMouseButtonDown((int)MouseButton.RightMouse)) return;

        // Destroy existing context menu
        if (_currentContextMenu != null)
        {
            Destroy(_currentContextMenu);
            return;
        }

        if (!_highLight) return;
        // Instantiate the context menu

        var mousePosition = Camera.main.WorldToScreenPoint(_highLight.transform.position);
        mousePosition.y += 50;

        highLightObject = _highLight.transform.gameObject;

        if (highLightObject.name.Contains("LVLoadCell"))
        {
            _currentContextMenu = CreateMenuAdapter(mousePosition, new[] { "1.5 mL" });
        }
        else if (highLightObject.name.Contains("LoadCell"))
        {
            _currentContextMenu = CreateMenuAdapter(mousePosition, new[] { "15 mL", "50 mL" });
        }
        else if (highLightObject.name.Contains("ChipClamp"))
        {
        }
        else
        {
            _currentContextMenu =
                Instantiate(contextMenuSetVolumePrefab, mousePosition, Quaternion.identity,
                    FindObjectOfType<Canvas>().transform);
        }

        var clickButton = _currentContextMenu.GetComponentInChildren<ButtonsClickAction>();
        clickButton.ContextMenu = _currentContextMenu;
        clickButton.MenuOwner = _highLight.transform.gameObject;


        // Adjust menu position to avoid going out of screen bounds
        var rect = _currentContextMenu.GetComponent<RectTransform>();
        var screenBounds = new Vector2(Screen.width, Screen.height);

        if (mousePosition.x + rect.sizeDelta.x > screenBounds.x)
            mousePosition.x -= rect.sizeDelta.x;

        if (mousePosition.y - rect.sizeDelta.y < 0)
            mousePosition.y += rect.sizeDelta.y;

        _currentContextMenu.transform.position = mousePosition;
    }

    private GameObject CreateMenuAdapter(Vector3 mousePosition, IEnumerable<string> adapters)
    {
        var menu = Instantiate(contextMenuAdapterPrefab, mousePosition, Quaternion.identity,
            FindObjectOfType<Canvas>().transform);
        var dropdown = menu.GetComponentInChildren<TMP_Dropdown>();
        if (dropdown != null)
        {
            // Clear existing options (optional)
            dropdown.ClearOptions();


            // Add the options to the dropdown
            foreach (var adapter in adapters)
            {
                var option = new TMP_Dropdown.OptionData(adapter);
                dropdown.options.Add(option);
            }

            // Optionally, refresh the dropdown
            dropdown.RefreshShownValue();
        }

        return menu;
    }
}
