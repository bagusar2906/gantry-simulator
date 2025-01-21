using System;
using EventArgs;
using TMPro;
using UnityEngine;

public class Tube : MonoBehaviour
{
    [SerializeField] public TubeType tubeType;
    public TextMeshPro volumeLabel;
    public double Weight { get; private set; }
    public event EventHandler<WeightChangedEventArgs> WeightChanged;
    public double Volume { get; private set; }

    public double InitialWeight { get; private set; }

    private void Start()
    {
        Fill(LiquidType.Sample, 0f, false);
        var rgb = GetComponent<Rigidbody>();
        InitialWeight = rgb.mass;
        Weight = InitialWeight;
    }
    
    public void Fill(double volume)
    {
     
        volumeLabel.text = $"{volume:F2} mL";
        var liquidVolume = GetComponentInChildren<LiquidControl>();
        liquidVolume.SetVolume((float)volume);
        Volume = volume;
        Weight = InitialWeight + Volume;

    }
    
    public void Fill(LiquidType liquidType, double volume, bool triggerWeightChanged )
    {
     
        volumeLabel.text = $"{volume:F2} mL";
        var liquidVolume = GetComponentInChildren<LiquidControl>();
        liquidVolume.SetVolume(liquidType, (float)volume);
        Volume = volume;
        Weight = InitialWeight + Volume;
       if (triggerWeightChanged)
           WeightChanged?.Invoke(this, new WeightChangedEventArgs()
           {
               LiquidType = liquidType,
               Weight = Weight
           });   
    }
    
}
