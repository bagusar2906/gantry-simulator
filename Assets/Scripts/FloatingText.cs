using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    private Transform mainCam;

    private Transform unit;

    private Transform worldSpaceCanvas;

    public Vector3 offset;
    // Start is called before the first frame update
    void Start()
    {
        mainCam = Camera.main.transform;
        unit = transform.parent;
        worldSpaceCanvas = GameObject.FindObjectOfType<Canvas>().transform;
        transform.SetParent(worldSpaceCanvas);
    }

    // Update is called once per frame
    void Update()
    {
        Transform transform1;
        (transform1 = transform).rotation = Quaternion.LookRotation(transform.position - mainCam.transform.position);
        transform1.position = unit.position + offset;
    }
}
