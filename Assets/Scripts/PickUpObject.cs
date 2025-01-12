using UnityEngine;
using UnityEngine.UIElements;

public class PickUpObject : MonoBehaviour
{
    private Camera mainCamera; // Reference to the main camera
    private GameObject pickedObject; // The currently picked up object
    
    [Header("Pickup Settings")]
    public float pickUpDistance = 10.0f; // Distance from the camera to hold the object

    void Start()
    {
        mainCamera = Camera.main; // Get the main camera
    }

    void Update()
    {
        // Handle mouse click
        if (Input.GetKeyDown(KeyCode.P)) // Left mouse button
        {
            if (pickedObject == null)
            {
                TryPickUpObject();
            }
            else
            {
                DropObject();
            }
        }

        // Move the picked-up object with the mouse
        if (pickedObject != null)
        {
            MovePickedObject();
        }
    }

    void TryPickUpObject()
    {
        // Cast a ray from the camera to the mouse position
        var ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            // Check if the object has a Rigidbody and is movable
            var rb = hit.collider.GetComponent<Rigidbody>();
            if (rb != null)
            {
                pickedObject = hit.collider.gameObject;
                rb.useGravity = false; // Disable gravity while picked up
              
            }
        }
    }

    void MovePickedObject()
    {
        // Calculate the new position of the object
        var mousePosition = Input.mousePosition;
        mousePosition.z = pickUpDistance; // Set the distance from the camera
        var targetPosition = mainCamera.ScreenToWorldPoint(mousePosition);
        var position = pickedObject.transform.position;
        targetPosition.z = position.z;
        // Smoothly move the object to the target position
        position = Vector3.Lerp(position, targetPosition, Time.deltaTime * 10f);
        pickedObject.transform.position = position;
    }

    void DropObject()
    {
        // Drop the object
        var rb = pickedObject.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.useGravity = true; // Re-enable gravity
        }
        pickedObject = null;
    }
}