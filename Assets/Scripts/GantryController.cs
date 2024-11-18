using UnityEngine;

public class GantryController : MonoBehaviour
{
    public Transform xAxis;  // Reference to X axis
    public Transform yAxis;  // Reference to Y axis
    public Transform zAxis;  // Reference to Z axis

    public float xSpeed = 2.0f; // Speed of X movement
    public float ySpeed = 2.0f; // Speed of Y movement
    public float zSpeed = 2.0f; // Speed of Z movement
   
    // Update is called once per frame
    void Update()
    {
        // Control X axis movement
        float xMovement = Input.GetAxis("Horizontal") * xSpeed * Time.deltaTime;
        xAxis.Translate(xMovement, 0, 0);

        // Control Y axis movement
        float yMovement = Input.GetAxis("Vertical") * ySpeed * Time.deltaTime;
        yAxis.Translate(0, yMovement, 0);

        // Control Z axis movement with custom keys (e.g., "Q" and "E")
        if (Input.GetKey(KeyCode.Q))
        {
            zAxis.Translate(0, 0, zSpeed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.E))
        {
            zAxis.Translate(0, 0, -zSpeed * Time.deltaTime);
        }
    }
}