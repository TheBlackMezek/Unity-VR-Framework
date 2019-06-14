using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HingeInteractable : Interactable
{

    [SerializeField] private Transform hingePoint;
    [SerializeField] private Axis axis;

    private float hingeDist;
    private Vector3 alignmentAxis;

    private Vector3 originalDir;
    private Quaternion originalRot;
    private Vector3 interactorOffset;



    protected override void Awake()
    {
        base.Awake();

        hingeDist = Vector3.Distance(hingePoint.position, transform.position);
        originalDir = (transform.position - hingePoint.position).normalized;
        originalRot = transform.rotation;

        switch (axis)
        {
            case Axis.X:
                alignmentAxis = hingePoint.right;
                break;
            case Axis.Y:
                alignmentAxis = hingePoint.up;
                break;
            case Axis.Z:
                alignmentAxis = hingePoint.forward;
                break;
        }
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
        interactorOffset = transform.InverseTransformPoint(holder.transform.position - transform.position);
    }

    public override void HoldUpdate(Transform interactor, Vector3 velocity, Vector3 angularVelocity)
    {
        base.HoldUpdate(interactor, velocity, angularVelocity);

        Vector3 interactorPos = interactor.position - transform.TransformPoint(interactorOffset);
        interactorPos = hingePoint.InverseTransformPoint(interactorPos);

        Vector3 angleAxis = Vector3.zero;
        
        switch(axis)
        {
            case Axis.X:
                interactorPos.x = 0f;
                angleAxis = Vector3.right;
                break;
            case Axis.Y:
                interactorPos.y = 0f;
                angleAxis = Vector3.up;
                break;
            case Axis.Z:
                interactorPos.z = 0f;
                angleAxis = Vector3.forward;
                break;
        }
        
        //Using the interactor position in hinge space as a direction
        Vector3 newPos = interactorPos.normalized * hingeDist;
        transform.position = hingePoint.TransformPoint(newPos);

        Vector3 dir = (transform.position - hingePoint.position).normalized;
        Debug.Log(Vector3.SignedAngle(originalDir, dir, angleAxis));
        transform.rotation = originalRot * Quaternion.AngleAxis(Vector3.SignedAngle(originalDir, dir, angleAxis), alignmentAxis);
    }

}
