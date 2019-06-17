using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class HapticMaterial : MonoBehaviour
{

    [SerializeField] private HapticMaterialObject material;
    [SerializeField] private Rigidbody body;
    [SerializeField] private Transform swingTrackingPosition;

    [HideInInspector] public XRNode onCollisionNode;

    private Vector3 lastHoldPos;



    private void Reset()
    {
        body = GetComponent<Rigidbody>();
        swingTrackingPosition = transform;
    }

    public HapticPulseData GetBeginTouchPulse(Vector3 velocity)
    {
        float relativeVelocity;
        if (body == null)
            relativeVelocity = velocity.magnitude;
        else
            relativeVelocity = (velocity - body.velocity).magnitude;

        return material.GetBeginTouchPulse(relativeVelocity);
    }

    public HapticPulseData GetEndTouchPulse(Vector3 velocity)
    {
        float relativeVelocity;
        if (body == null)
            relativeVelocity = velocity.magnitude;
        else
            relativeVelocity = (velocity - body.velocity).magnitude;

        return material.GetEndTouchPulse(relativeVelocity);
    }

    public float GetStayTouchAmplitude(Vector3 velocity)
    {
        float relativeVelocity;
        if (body == null)
            relativeVelocity = velocity.magnitude;
        else
            relativeVelocity = (velocity - body.velocity).magnitude;

        return material.GetStayTouchAmplitude(relativeVelocity);
    }

    public HapticPulseData GetBeginHoldPulse()
    {
        return GetBeginHoldPulse();
    }

    public HapticPulseData GetEndHoldPulse()
    {
        return GetEndHoldPulse();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(onCollisionNode != XRNode.LeftEye)
        {
            HapticPulseData dat = material.GetBeginHitPulse(collision.relativeVelocity.magnitude);

            if (dat.amplitude1 > 0f || dat.amplitude2 > 0f)
                HapticHandler.DoPulseData(onCollisionNode, dat);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (onCollisionNode != XRNode.LeftEye)
        {
            HapticPulseData dat = material.GetEndHitPulse(collision.relativeVelocity.magnitude);

            if (dat.amplitude1 > 0f || dat.amplitude2 > 0f)
                HapticHandler.DoPulseData(onCollisionNode, dat);
        }
    }

    public void SetStartHoldPos()
    {
        lastHoldPos = swingTrackingPosition.position;
    }

    public float GetSwingUpdateAmplitude(float dt)
    {
        float ret = material.GetSwingAmplitude((lastHoldPos - swingTrackingPosition.position).magnitude / dt);
        lastHoldPos = swingTrackingPosition.position;
        return ret;
    }

}
