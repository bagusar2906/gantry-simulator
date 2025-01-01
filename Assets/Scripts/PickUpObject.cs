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
        if (Input.GetMouseButtonDown((int)MouseButton.LeftMouse)) // Left mouse button
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
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            // Check if the object has a Rigidbody and is movable
            Rigidbody rb = hit.collider.GetComponent<Rigidbody>();
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
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = pickUpDistance; // Set the distance from the camera
        Vector3 targetPosition = mainCamera.ScreenToWorldPoint(mousePosition);

        // Smoothly move the object to the target position
        pickedObject.transform.position = Vector3.Lerp(pickedObject.transform.position, targetPosition, Time.deltaTime * 10f);
    }

    void DropObject()
    {
        // Drop the object
        Rigidbody rb = pickedObject.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.useGravity = true; // Re-enable gravity
        }
        pickedObject = null;
    }
}