﻿using System;
using Clamp;
using Enums;
using EventArgs;
using UnityEngine;


public class ChipClampController : MonoBehaviour
{
    public GameObject fingerA;
    public GameObject fingerB;
    
    public event EventHandler<ChipClampStateChangedEventArgs> OnChipClampStateChanged
    {
        add => _clampOnTouch.OnChipClampStateChanged += value;
        remove => _clampOnTouch.OnChipClampStateChanged -= value;
    }

    PincherFingerController _fingerAController;
    PincherFingerController _fingerBController;
    private ClampOnTouch _clampOnTouch;

    // Grip - the extent to which the pincher is closed. 0: fully open, 1: fully closed.
    public float grip;
    public float gripSpeed = 3.0f;
    public GripState gripState = GripState.Fixed;

    public GripState CurrentState { get; private set; }

    public void ClampChip(ClampState clampState)
    {
        if (CurrentState is GripState.Opening or GripState.Closing) 
            return;
        gripState = clampState == ClampState.Clamp ?
            GripState.Closing
            : GripState.Opening;
    }
    void Start()
    {
        _fingerAController = fingerA.GetComponent<PincherFingerController>();
        _fingerBController = fingerB.GetComponent<PincherFingerController>();
        _clampOnTouch = GetComponentInChildren<ClampOnTouch>();
        CurrentState = GripState.Opened;
    }

    void FixedUpdate()
    {
        UpdateGrip();
        UpdateFingersForGrip();
    }


    // READ

    public float CurrentGrip()
    {
        // TODO - we can't really assume the fingers agree, need to think about that
        float meanGrip = (_fingerAController.CurrentGrip() + _fingerBController.CurrentGrip()) / 2.0f;
        return meanGrip;
    }


    public Vector3 CurrentGraspCenter()
    {
        /* Gets the point directly between the middle of the pincher fingers,
         * in the global coordinate system.      
         */
        Vector3 localCenterPoint = (_fingerAController.GetOpenPosition() + _fingerBController.GetOpenPosition()) / 2.0f;
        Vector3 globalCenterPoint = transform.TransformPoint(localCenterPoint);
        return globalCenterPoint;
    }


    // CONTROL

    public void ResetGripToOpen()
    {
        grip = 0.0f;
        _fingerAController.ForceOpen(transform);
        _fingerBController.ForceOpen(transform);
        gripState = GripState.Fixed;
    }

    // GRIP HELPERS

    void UpdateGrip()
    {
        if (gripState == GripState.Fixed) return;
        var gripChange = (float)gripState * gripSpeed * Time.fixedDeltaTime;
        var gripGoal = CurrentGrip() + gripChange;
        grip = Mathf.Clamp01(gripGoal);
        CurrentState = gripState == GripState.Opening ? GripState.Opened:GripState.Closed;
    }

    void UpdateFingersForGrip()
    {
        _fingerAController.UpdateGrip(grip);
        _fingerBController.UpdateGrip(grip);
    }





}
