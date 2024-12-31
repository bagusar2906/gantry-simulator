using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    
    private Renderer _objectRenderer;

    // Start is called before the first frame update
    void Start()
    {
        _objectRenderer = GetComponent<Renderer>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        _objectRenderer.material.color = Color.red;
    }

    private void OnCollisionExit(Collision other)
    {
        _objectRenderer.material.color = Color.green;
    }
}
