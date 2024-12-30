using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

public class OutlineSelection : MonoBehaviour
{
    public GameObject contextMenuAdapterPrefab;
    public GameObject contextMenuSetVolumePrefab;
    private GameObject _currentContextMenu;
    private Transform _highLight;

    private Transform _selection;

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

        if (!EventSystem.current.IsPointerOverGameObject() && Physics.Raycast(ray, out  _raycastHit))
        {
            _highLight = _raycastHit.transform;
            if (_highLight.CompareTag("Selectable") )
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
        
        if (!Input.GetMouseButtonDown((int)MouseButton.RightMouse)) return;
        // Destroy existing context menu
        if (_currentContextMenu != null)
        {
            Destroy(_currentContextMenu);
            return;
        }

        if (_highLight)
        {
            /*if (_highLight.parent != null)
            {
                var clamp = _highLight.parent.gameObject.GetComponent<ChipClampController>();
                if (clamp != null)
                {

                    clamp.gripState = clamp.CurrentState != GripState.Opening ? GripState.Closing : GripState.Opening;
                }
            }*/
            // if (_selection != null)
            // {
            //   //  _selection.gameObject.GetComponent<Outline>().enabled = false;
            // }
            
            // Instantiate the context menu
            var mousePosition = Input.mousePosition;
            mousePosition.y += 50;
            
            _currentContextMenu =
                Instantiate(
                    _highLight.transform.gameObject.name.Contains("LoadCell")
                        ? contextMenuAdapterPrefab
                        : contextMenuSetVolumePrefab, mousePosition, Quaternion.identity, FindObjectOfType<Canvas>().transform);

            var clickButton = _currentContextMenu.GetComponentInChildren<ButtonsClickAction>();
            clickButton.ContextMenu = _currentContextMenu;
            clickButton.MenuOwner = _highLight.transform.gameObject;
            // Adjust menu position to avoid going out of screen bounds
            var rect = _currentContextMenu.GetComponent<RectTransform>();
            Vector2 screenBounds = new Vector2(Screen.width, Screen.height);

            if (mousePosition.x + rect.sizeDelta.x > screenBounds.x)
                mousePosition.x -= rect.sizeDelta.x;

            if (mousePosition.y - rect.sizeDelta.y < 0)
                mousePosition.y += rect.sizeDelta.y;

            _currentContextMenu.transform.position = mousePosition;
            

            _selection = _raycastHit.transform;
          //  _selection.gameObject.GetComponent<Outline>().enabled = true;
           // _highLight = null;
        }
        else
        {
            if (!_selection) return;
            _selection.gameObject.GetComponent<Outline>().enabled = false;
            _selection = null;
        }
    }
}
