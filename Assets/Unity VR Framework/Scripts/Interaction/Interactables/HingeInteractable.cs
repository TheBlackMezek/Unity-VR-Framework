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
    [SerializeField] private HapticPulseData onMaxReachedPulse;
    [SerializeField] private HapticPulseData onMaxLeftPulse;
    [SerializeField] private HapticPulseData onMinReachedPulse;
    [SerializeField] private HapticPulseData onMinLeftPulse;

    [Header("Hinge Momentum Settings")]

    [SerializeField] private bool doPostHoldMomentum;
    [SerializeField] private float throwVelocityThreshold;
    [SerializeField] private float drag;
    [SerializeField] private bool bounce;
    [SerializeField] private float bounceVelocityDivisor;

    private float hingeDist;
    private Vector3 alignmentAxis;

    private Vector3 originalDir;
    private Vector3 alignedDir;
    private Quaternion originalRot;
    private Vector3 interactorOffset;
    private float currentAngle;

    private float angularVel;
    private Queue<float> momentum = new Queue<float>();



    protected override void Awake()
    {
        base.Awake();

        currentAngle = initialRotation;

        hingeDist = Vector3.Distance(hingePoint.position, transform.position);
        originalDir = (transform.position - hingePoint.position).normalized;
        alignedDir = originalDir;
        originalRot = transform.rotation;

        switch (axis)
        {
            case Axis.X:
                alignmentAxis = hingePoint.right;
                alignedDir.x = 0f;
                alignedDir.Normalize();
                break;
            case Axis.Y:
                alignmentAxis = hingePoint.up;
                alignedDir.y = 0f;
                alignedDir.Normalize();
                break;
            case Axis.Z:
                alignmentAxis = hingePoint.forward;
                alignedDir.z = 0f;
                alignedDir.Normalize();
                break;
        }
    }

    protected override void HoldBegin()
    {
        base.HoldBegin();
        SetInitialTransformData();
        SubscribeToUpdater.UpdateEvent -= MomentumUpdate;
    }

    protected override void HoldSwap()
    {
        base.HoldSwap();
        SetInitialTransformData();
    }

    protected override void HoldEnd(Vector3 velocity, Vector3 angularVelocity)
    {
        base.HoldEnd(velocity, angularVelocity);
        if (doPostHoldMomentum && Mathf.Abs(angularVel) > throwVelocityThreshold)
        {
            int count = momentum.Count;
            float sum = 0f;
            while (momentum.Count > 0)
                sum += momentum.Dequeue();

            angularVel = sum / count;
            SubscribeToUpdater.UpdateEvent += MomentumUpdate;
        }
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
        
        float prevAngle = currentAngle;
        SetCurrentAngle(Vector3.SignedAngle(alignedDir, interactorPos.normalized, angleAxis) + initialRotation);
        angularVel = (currentAngle - prevAngle) / Time.deltaTime;
        momentum.Enqueue(angularVel);
        if (momentum.Count > 10)
            momentum.Dequeue();
    }

    protected void MomentumUpdate()
    {
        float dt = Time.deltaTime;

        SetCurrentAngle(currentAngle + angularVel * dt);

        if(currentAngle == 0f || currentAngle == rotationRange)
        {
            if(bounce)
            {
                angularVel /= bounceVelocityDivisor;
                angularVel = -angularVel;
            }
            else
            {
                angularVel = 0f;
            }
        }
        else
        {
            angularVel -= angularVel * dt * drag;
        }
        
        if (Mathf.Abs(angularVel) < 0.001f)
        {
            angularVel = 0f;
            SubscribeToUpdater.UpdateEvent -= MomentumUpdate;
        }
    }

    protected void SetCurrentAngle(float angle)
    {
        if (rotationRange > 0f)
        {
            if (angle > rotationRange)
                angle = rotationRange;
            else if (angle < 0f)
                angle = 0f;

            UnityEngine.XR.XRNode node;
            if (held && holder.GetNode(out node) && angle != currentAngle)
            {
                if (angle == 0f)
                    HapticHandler.DoPulseData(node, onMinReachedPulse);
                else if (angle == rotationRange)
                    HapticHandler.DoPulseData(node, onMaxReachedPulse);
                else if (currentAngle == 0f)
                    HapticHandler.DoPulseData(node, onMinLeftPulse);
                else if (currentAngle == rotationRange)
                    HapticHandler.DoPulseData(node, onMaxLeftPulse);
            }
        }

        currentAngle = angle;

        Quaternion rotMod = Quaternion.AngleAxis(angle - initialRotation, alignmentAxis);
        transform.rotation = originalRot * rotMod;
        transform.position = hingePoint.TransformPoint(rotMod * originalDir * hingeDist);
    }

}
