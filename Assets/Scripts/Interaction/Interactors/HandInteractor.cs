using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class HandInteractor : Interactor
{

    [Header("HandInteractor Settingss")]

    [SerializeField] private Handedness hand;
    [SerializeField] private DeviceFollower velocityTracker;
    
    private XRNode node;
    private float clock = 0f;



    private void Start()
    {
        if(hand == Handedness.LEFT)
        {
            VRInputManager.OnLeftTriggerDown.AddListener(InteractBegin);
            VRInputManager.OnLeftTriggerUp.AddListener(InteractEnd);
            VRInputManager.OnLeftGripDown.AddListener(ReleaseHoldInput);
            node = XRNode.LeftHand;
        }
        else
        {
            VRInputManager.OnRightTriggerDown.AddListener(InteractBegin);
            VRInputManager.OnRightTriggerUp.AddListener(InteractEnd);
            VRInputManager.OnRightGripDown.AddListener(ReleaseHoldInput);
            node = XRNode.RightHand;
        }
    }

    override protected void Update()
    {
        body.velocity = velocityTracker.Velocity;
        body.angularVelocity = velocityTracker.AngularVelocity;
        base.Update();

        if (clock > 0f)
            clock -= Time.deltaTime;
        else
        {
            clock = 1f;
            //HapticHandler.Blip(node, 0.1f);
            HapticHandler.FlatPulse(node, 0.5f, 0.5f);
        }
    }

}
