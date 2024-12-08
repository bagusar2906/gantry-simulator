using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class OutlineSelection : MonoBehaviour
{
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
                if (_highLight.gameObject.GetComponent<Outline>() != null)
                {
                    _highLight.GetComponent<Outline>().enabled = true;
                }
                else
                {
                    var outline = _highLight.gameObject.AddComponent<Outline>();
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
        
        //Selection

        if (!Input.GetMouseButtonDown((int)MouseButton.RightMouse)) return;
        if (_highLight)
        {
            if (_selection != null)
            {
                _selection.gameObject.GetComponent<Outline>().enabled = false;
            }

            _selection = _raycastHit.transform;
            _selection.gameObject.GetComponent<Outline>().enabled = true;
            _highLight = null;
        }
        else
        {
            if (!_selection) return;
            _selection.gameObject.GetComponent<Outline>().enabled = false;
            _selection = null;
        }
    }
}
