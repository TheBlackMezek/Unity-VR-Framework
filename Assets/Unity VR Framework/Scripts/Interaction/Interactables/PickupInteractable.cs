using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupInteractable : Interactable
{

    [Tooltip("If true, object will go to the center of the Interactor which is grabbing it")]
    [SerializeField] private bool snapToHand;

    private Transform originalParent;
    private Vector3 localPos;
    private Quaternion localRot;



    protected override void Awake()
    {
        base.Awake();
        originalParent = transform.parent;
    }

    protected override void HoldBegin()
    {
        base.HoldBegin();
        SetInitialTransformData();
    }

    protected override void HoldSwap()
    {
        base.HoldSwap();
        SetInitialTransformData();
    }

    protected void SetInitialTransformData()
    {
        transform.parent = holder.transform;
        if (snapToHand)
        {
            localPos = Vector3.zero;
            localRot = Quaternion.identity;
        }
        else
        {
            localPos = transform.localPosition;
            localRot = transform.localRotation;
        }
    }

    public override void HoldUpdate(Transform interactor, Vector3 velocity, Vector3 angularVelocity)
    {
        body.velocity = velocity;
        body.angularVelocity = angularVelocity;

        transform.localPosition = localPos;
        transform.localRotation = localRot;

        base.HoldUpdate(interactor, velocity, angularVelocity);

    }

    protected override void HoldEnd(Vector3 velocity, Vector3 angularVelocity)
    {
        body.velocity = velocity;
        body.angularVelocity = angularVelocity;

        transform.parent = originalParent;

        base.HoldEnd(velocity, angularVelocity);
    }

}
