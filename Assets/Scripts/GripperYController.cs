﻿using UnityEngine;

public class GripperYController : MonoBehaviour
{

    public MotorState moveState = MotorState.Fixed;
    public float speed = 1.0f;

    private void FixedUpdate()
    {
        if (moveState != MotorState.Fixed)
        {
            ArticulationBody articulation = GetComponent<ArticulationBody>();

            //get jointPosition along y axis
            float xDrivePosition = articulation.jointPosition[0];
            Debug.Log(xDrivePosition);

            //increment this y position
            float targetPosition = xDrivePosition + -(float)moveState * Time.fixedDeltaTime * speed;

            //set joint Drive to new position
            var drive = articulation.yDrive;
            drive.target = targetPosition;
            articulation.yDrive = drive;
        }
    }
}
