using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewHapticMaterial", menuName = "Unity VR Framework/HapticMaterialObject")]
public class HapticMaterialObject : ScriptableObject
{

    [Header("When Touched")]
    
    public HapticPulseData beginTouchPulse;
    public HapticPulseData endTouchPulse;
    public AnimationCurve beginTouchVelocityCurve;
    public AnimationCurve endTouchVelocityCurve;
    public AnimationCurve stayTouchVelocityCurve;
    
    [Header("When Held")]

    public HapticPulseData beginHoldPulse;
    public HapticPulseData endHoldPulse;
    public HapticPulseData beginHitPulse;
    public HapticPulseData endHitPulse;
    public AnimationCurve beginHitVelocityCurve;
    public AnimationCurve endHitVelocityCurve;
    public AnimationCurve swingVelocityCurve;



    public HapticPulseData GetBeginTouchPulse(float relativeVelocity)
    {
        HapticPulseData dat = beginTouchPulse;
        dat.amplitude1 *= beginTouchVelocityCurve.Evaluate(relativeVelocity);
        dat.amplitude2 *= beginTouchVelocityCurve.Evaluate(relativeVelocity);
        return dat;
    }

    public HapticPulseData GetEndTouchPulse(float relativeVelocity)
    {
        HapticPulseData dat = endTouchPulse;
        dat.amplitude1 *= endTouchVelocityCurve.Evaluate(relativeVelocity);
        dat.amplitude2 *= endTouchVelocityCurve.Evaluate(relativeVelocity);
        return dat;
    }

    public float GetStayTouchAmplitude(float relativeVelocity)
    {
        return stayTouchVelocityCurve.Evaluate(relativeVelocity);
    }

    public HapticPulseData GetBeginHoldPulse()
    {
        return beginHoldPulse;
    }

    public HapticPulseData GetEndHoldPulse()
    {
        return endHoldPulse;
    }

    public HapticPulseData GetBeginHitPulse(float relativeVelocity)
    {
        HapticPulseData dat = beginHitPulse;
        dat.amplitude1 *= beginHitVelocityCurve.Evaluate(relativeVelocity);
        dat.amplitude2 *= beginHitVelocityCurve.Evaluate(relativeVelocity);
        return dat;
    }

    public HapticPulseData GetEndHitPulse(float relativeVelocity)
    {
        HapticPulseData dat = endHitPulse;
        dat.amplitude1 *= endHitVelocityCurve.Evaluate(relativeVelocity);
        dat.amplitude2 *= endHitVelocityCurve.Evaluate(relativeVelocity);
        return dat;
    }

    public float GetSwingAmplitude(float velocity)
    {
        return swingVelocityCurve.Evaluate(velocity);
    }

}
