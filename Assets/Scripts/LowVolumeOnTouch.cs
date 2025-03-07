using System;
using Enums;
using EventArgs;
using UnityEngine;

public class LowVolumeOnTouch : MonoBehaviour
{
    
    private Renderer _objectRenderer;
    private Color _loadCellColor;

    // Start is called before the first frame update
    void Start()
    {
        _objectRenderer = GetComponent<Renderer>();
        _loadCellColor = _objectRenderer.material.color;
    }

    private void OnCollisionEnter(Collision collision)
    {
        _objectRenderer.material.color = Color.red;
    }

    private void OnCollisionExit(Collision other)
    {
        _objectRenderer.material.color = _loadCellColor;
    }
}
