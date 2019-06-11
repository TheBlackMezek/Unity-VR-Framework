using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HapticMaterial : MonoBehaviour
{

    [SerializeField] private HapticMaterialObject material;
    [SerializeField] private Rigidbody body;



    private void Reset()
    {
        body = GetComponent<Rigidbody>();
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

}
