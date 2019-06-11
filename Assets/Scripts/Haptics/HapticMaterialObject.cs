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
        Debug.Log(relativeVelocity + " " + stayTouchVelocityCurve.Evaluate(relativeVelocity));
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

}
