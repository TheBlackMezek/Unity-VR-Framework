using UnityEngine;

[System.Serializable]
public struct HapticPulseData
{
    public HapticPulseType type;
    [HideInInspector]
    public float clock;
    public float length;
    public float amplitude1;
    public float amplitude2;
    public float cycles;
}
