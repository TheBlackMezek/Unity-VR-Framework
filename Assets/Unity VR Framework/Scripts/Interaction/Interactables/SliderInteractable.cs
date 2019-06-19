using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderInteractable : Interactable
{

    [Header("Slider Settings")]

    [SerializeField] private Transform lowEnd;
    [SerializeField] private Transform highEnd;
    [SerializeField] private float lowVal;
    [SerializeField] private float highVal;
    [SerializeField] private float initialVal;
    [SerializeField] private HapticPulseData lowHitPulse;
    [SerializeField] private HapticPulseData highHitPulse;
    [Tooltip("Set to 0 for no increment")]
    [SerializeField] private float increment;

    public float Value { get; private set; }

    private Vector3 interactorOffset;



    protected override void Awake()
    {
        base.Awake();

        SetCurrentValue(initialVal);
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
        interactorOffset = holder.transform.position - transform.position;
    }

    public override void HoldUpdate(Transform interactor, Vector3 velocity, Vector3 angularVelocity)
    {
        base.HoldUpdate(interactor, velocity, angularVelocity);

        Vector3 pos = lowEnd.position - (interactor.position - interactorOffset);
        Vector3 dir = lowEnd.position - highEnd.position;

        pos = Vector3.Project(pos, dir.normalized);
        if (pos.normalized == -dir.normalized)
            pos = Vector3.zero;
        float lerp = pos.magnitude / dir.magnitude;
        lerp = Mathf.Clamp(lerp, 0f, 1f);

        SetCurrentValue(Mathf.Lerp(lowVal, highVal, lerp));
    }

    protected void SetCurrentValue(float val)
    {
        if(increment > 0f)
        {
            val = val - (val % increment);
        }
        transform.position = Vector3.Lerp(lowEnd.position, highEnd.position, Mathf.InverseLerp(lowVal, highVal, val));
        Value = val;
    }

}
