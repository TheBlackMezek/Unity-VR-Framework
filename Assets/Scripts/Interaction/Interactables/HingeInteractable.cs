using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HingeInteractable : Interactable
{

    [Header("Hinge Settings")]

    [SerializeField] private Transform hingePoint;
    [SerializeField] private Axis axis;
    [SerializeField] private float rotationRange;
    [SerializeField] private float initialRotation;

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
        interactorOffset = transform.InverseTransformPoint(holder.transform.position);
    }

    public override void HoldUpdate(Transform interactor, Vector3 velocity, Vector3 angularVelocity)
    {
        base.HoldUpdate(interactor, velocity, angularVelocity);

        Vector3 interactorPos = interactor.position;
        interactorPos = hingePoint.InverseTransformPoint(transform.TransformPoint(transform.InverseTransformPoint(interactorPos) - interactorOffset));

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

        Vector3 dir = interactorPos.normalized;
        float newAngle = Vector3.SignedAngle(originalDir, dir, angleAxis);

        if (rotationRange > 0f)
        {
            float rangeAngle = newAngle + initialRotation;
            if (rangeAngle > rotationRange)
                rangeAngle = rotationRange;
            else if (rangeAngle < 0f)
                rangeAngle = 0f;
            newAngle = rangeAngle - initialRotation;
        }

        Quaternion rotMod = Quaternion.AngleAxis(newAngle, alignmentAxis);
        transform.rotation = originalRot * rotMod;
        dir = rotMod * originalDir;
        transform.position = hingePoint.TransformPoint(dir * hingeDist);
    }

}
