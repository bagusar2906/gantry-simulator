using System;
using UnityEngine;

public class PhysicsGantryController : MonoBehaviour
{
    public Rigidbody xAxisRb;  // Reference to X axis Rigidbody
    public Rigidbody yAxisRb;  // Reference to Y axis Rigidbody
    public Rigidbody zAxisRb;  // Reference to Z axis Rigidbody

    public float forceAmount = 100.0f;  // Amount of force applied to each axis

    void Update()
    {
        // Apply force to X axis using horizontal input
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            xAxisRb.AddForce(Vector3.left * forceAmount);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            xAxisRb.AddForce(Vector3.right * forceAmount);
        }

        // Apply force to Y axis using vertical input
        if (Input.GetKey(KeyCode.UpArrow))
        {
            yAxisRb.AddForce(Vector3.up * forceAmount);
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            yAxisRb.AddForce(Vector3.down * forceAmount);
        }

        // Apply force to Z axis using custom keys (e.g., "Q" and "E")
        if (Input.GetKey(KeyCode.Q))
        {
            zAxisRb.AddForce(Vector3.forward * forceAmount);
        }
        else if (Input.GetKey(KeyCode.E))
        {
            zAxisRb.AddForce(Vector3.back * forceAmount);
        }
    }

    private void Start()
    {
        yAxisRb.AddForce(Vector3.up * forceAmount);
    }
}