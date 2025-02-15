using UnityEngine;
using UnityEngine.UIElements;

public class PickUpObject : MonoBehaviour
{
    private Camera mainCamera; // Reference to the main camera
    private GameObject pickedObject; // The currently picked up object
    
    [Header("Pickup Settings")]
    public Vector3 pickUpDistance = new Vector3(0,20f,0); // Distance from the camera to hold the object

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

    private float _distanceFromObject;
    void TryPickUpObject()
    {
        // Cast a ray from the camera to the mouse position
        var ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out var hit))
        {
            // Check if the object has a Rigidbody and is movable
            var rb = hit.collider.GetComponent<Rigidbody>();
            if (rb != null)
            {
                pickedObject = hit.collider.gameObject;
                var position = pickedObject.transform.position;
                Debug.Log("Pickup position: " + position);
                //move away toward z from object to avoid collision
                _distanceFromObject = position.z + pickUpDistance.z;
                rb.useGravity = false; // Disable gravity while picked up
                foreach (var child in pickedObject.GetComponentsInChildren<Rigidbody>())
                {
                   
                    child.isKinematic = true;
                }
            }
        }
    }

    void MovePickedObject()
    {
        // Calculate the new position of the object
        var mousePosition = Input.mousePosition;
        mousePosition.z = pickUpDistance.y; // Set the distance from the camera
        var targetPosition = mainCamera.ScreenToWorldPoint(mousePosition);
       
        var position = pickedObject.transform.position;
        targetPosition.y = position.y ;
        targetPosition.z = _distanceFromObject;
        
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
            foreach (var child in pickedObject.GetComponentsInChildren<Rigidbody>())
            {
                //child.useGravity = true;
                child.isKinematic = false;
            }
        }
        pickedObject = null;
    }
}