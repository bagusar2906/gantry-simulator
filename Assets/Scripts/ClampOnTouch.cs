using System;
using Enums;
using EventArgs;
using UnityEngine;

public class ClampOnTouch : MonoBehaviour
{
    
    public event EventHandler<OnChipClampStateChangedEventArgs> OnChipClampStateChanged;
    private Renderer _objectRenderer;

    // Start is called before the first frame update
    void Start()
    {
        _objectRenderer = GetComponent<Renderer>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        _objectRenderer.material.color = Color.red;
        OnChipClampStateChanged?.Invoke(this, new OnChipClampStateChangedEventArgs()
        {
            State = ClampState.Clamp
        });
    }

    private void OnCollisionExit(Collision other)
    {
        _objectRenderer.material.color = Color.green;
        OnChipClampStateChanged?.Invoke(this, new OnChipClampStateChangedEventArgs()
        {
            State = ClampState.UnClamp
        });
    }
}
