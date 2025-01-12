using TMPro;
using UnityEngine;

public class Tube : MonoBehaviour
{
    [SerializeField] public TubeType tubeType;
    public TextMeshPro volumeLabel;
    public double Volume { get; private set; }

    private void Start()
    {
        Fill(LiquidType.Sample, 0f);
    }

    public void Fill(double volume)
    {
     //   var volumeLabel = GetComponentInChildren<TextMeshPro>();
        volumeLabel.text = $"{volume:F2} mL";
        var liquidVolume = GetComponentInChildren<LiquidControl>();
        liquidVolume.SetVolume((float)volume);
        Volume = volume;
    }
    
    public void Fill(LiquidType liquidType, double volume)
    {
      //  var volumeLabel = GetComponentInChildren<TextMeshPro>();
        volumeLabel.text = $"{volume:F2} mL";
        var liquidVolume = GetComponentInChildren<LiquidControl>();
        liquidVolume.SetVolume(liquidType, (float)volume);
        Volume = volume;
    }
 
}
