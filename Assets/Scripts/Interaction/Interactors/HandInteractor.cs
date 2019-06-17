﻿using System.Collections;
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

    private List<HapticMaterial> touchingMaterials = new List<HapticMaterial>();



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

        for(int i = 0; i < touchingMaterials.Count; ++i)
        {
            if (holding.Contains(touchingMaterials[i].GetComponent<Interactable>()))
            {
                float swingAmp = touchingMaterials[i].GetSwingUpdateAmplitude(Time.deltaTime);
                if(swingAmp > 0f)
                    HapticHandler.Blip(node, swingAmp, false);
                continue;
            }
            else if(HapticHandler.DoingPulse(node))
            {
                continue;
            }

            float amp = touchingMaterials[i].GetStayTouchAmplitude(body.velocity);
            if (amp > 0f)
                HapticHandler.Blip(node, amp, false);
        }
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        HapticMaterial mat = other.GetComponent<HapticMaterial>();

        if (mat != null)
        {
            touchingMaterials.Add(mat);
            HapticHandler.DoPulseData(node, mat.GetBeginTouchPulse(body.velocity));
        }
    }

    protected override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);

        HapticMaterial mat = other.GetComponent<HapticMaterial>();

        if (mat != null)
        {
            touchingMaterials.Remove(mat);
            HapticHandler.DoPulseData(node, mat.GetEndTouchPulse(body.velocity));
        }
    }

    public override void Hold(Interactable interactable)
    {
        base.Hold(interactable);

        HapticMaterial mat = interactable.GetComponent<HapticMaterial>();

        if (mat != null)
        {
            HapticHandler.DoPulseData(node, mat.GetBeginTouchPulse(body.velocity));
            mat.SetStartHoldPos();
            mat.onCollisionNode = node;
        }
    }

    public override void Release(Interactable interactable)
    {
        base.Release(interactable);

        HapticMaterial mat = interactable.GetComponent<HapticMaterial>();

        if (mat != null)
        {
            HapticHandler.DoPulseData(node, mat.GetBeginTouchPulse(body.velocity));
            mat.onCollisionNode = XRNode.LeftEye;
        }
    }

    public override bool GetNode(out XRNode node)
    {
        node = this.node;
        return true;
    }

}
