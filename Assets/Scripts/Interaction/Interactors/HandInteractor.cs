using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandInteractor : Interactor
{

    [Header("HandInteractor Settingss")]

    [SerializeField] private Handedness hand;
    [SerializeField] private DeviceFollower velocityTracker;



    private void Start()
    {
        if(hand == Handedness.LEFT)
        {
            VRInputManager.OnLeftTriggerDown.AddListener(InteractBegin);
            VRInputManager.OnLeftTriggerUp.AddListener(InteractEnd);
            VRInputManager.OnLeftGripDown.AddListener(ReleaseHoldInput);
        }
        else
        {
            VRInputManager.OnRightTriggerDown.AddListener(InteractBegin);
            VRInputManager.OnRightTriggerUp.AddListener(InteractEnd);
            VRInputManager.OnRightGripDown.AddListener(ReleaseHoldInput);
        }
    }

    override protected void Update()
    {
        body.velocity = velocityTracker.Velocity;
        body.angularVelocity = velocityTracker.AngularVelocity;
        base.Update();
    }

}
