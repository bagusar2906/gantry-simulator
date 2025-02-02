using System;
using Enums;
using EventArgs;
using UnityEngine;

public class ClampOnTouch : MonoBehaviour
{
    
    public event EventHandler<ChipClampStateChangedEventArgs> OnChipClampStateChanged;
    private Renderer _objectRenderer;

    // Start is called before the first frame update
    void Start()
    {
        _objectRenderer = GetComponent<Renderer>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        _objectRenderer.material.color = collision.gameObject.name.StartsWith("Chip") ?
            Color.red : Color.green;
        OnChipClampStateChanged?.Invoke(this, new ChipClampStateChangedEventArgs()
        {
            State = ClampState.Clamp
        });
    }

    private void OnCollisionExit(Collision other)
    {
        
        OnChipClampStateChanged?.Invoke(this, new ChipClampStateChangedEventArgs()
        {
            State = ClampState.UnClamp
        });
    }
}
