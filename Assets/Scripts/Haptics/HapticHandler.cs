using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class HapticHandler : MonoBehaviour
{
    //This is 9 because that's how many node types are in the XRNode enum.
    //I don't know how to dynamically get that number in case it changes.
    private const int NODE_COUNT = 9;
    private static InputDevice[] devices = new InputDevice[NODE_COUNT];
    private static HapticPulseData[] pulseData = new HapticPulseData[NODE_COUNT];

    [SerializeField] private int maxDeviceGetTries;

    private static int tries;



    private void Start()
    {
        for (int i = 0; i < NODE_COUNT; ++i)
            pulseData[i] = new HapticPulseData();

        GetDevices();
    }

    private void GetDevices()
    {
        bool finished = true;
        ++tries;

        for(int i = 0; i < NODE_COUNT; ++i)
        {
            if(devices[i] == null || devices[i].name == "")
            {
                devices[i] = InputDevices.GetDeviceAtXRNode((XRNode)i);
                
                if (devices[i].name == "")
                    finished = false;
            }
        }

        if (!finished && tries < maxDeviceGetTries)
            Invoke("GetDevices", 0.1f);
    }

    private void Update()
    {
        float dt = Time.deltaTime;
        float amp;
        for (int i = 0; i < NODE_COUNT; ++i)
        {
            switch (pulseData[i].type)
            {
                case HapticPulseType.BLIP:
                    Blip((XRNode)i, pulseData[i].amplitude1);
                    pulseData[i].type = HapticPulseType.NONE;

                    break;

                case HapticPulseType.FLAT:
                    pulseData[i].clock += dt;
                    Blip((XRNode)i, pulseData[i].amplitude1);
                    if (pulseData[i].clock >= pulseData[i].length)
                        pulseData[i].type = HapticPulseType.NONE;

                    break;

                case HapticPulseType.SINE:
                    pulseData[i].clock += dt;
                    amp = Mathf.Lerp(0f, Mathf.PI * 2f * pulseData[i].cycles,
                            Mathf.InverseLerp(0f, pulseData[i].length, pulseData[i].clock));
                    amp = Mathf.Lerp(pulseData[i].amplitude1, pulseData[i].amplitude2,
                            (Mathf.Sin(amp) + 1f) / 2f);

                    Blip((XRNode)i, amp);
                    if (pulseData[i].clock >= pulseData[i].length)
                        pulseData[i].type = HapticPulseType.NONE;

                    break;

                case HapticPulseType.COSINE:
                    pulseData[i].clock += dt;
                    amp = Mathf.Lerp(0f, Mathf.PI * 2f * pulseData[i].cycles,
                            Mathf.InverseLerp(0f, pulseData[i].length, pulseData[i].clock));
                    amp = Mathf.Lerp(pulseData[i].amplitude1, pulseData[i].amplitude2,
                            (Mathf.Cos(amp) + 1f) / 2f);

                    Blip((XRNode)i, amp);
                    if (pulseData[i].clock >= pulseData[i].length)
                        pulseData[i].type = HapticPulseType.NONE;

                    break;

                case HapticPulseType.LERP:
                    pulseData[i].clock += dt;
                    Blip((XRNode)i, Mathf.Lerp(pulseData[i].amplitude1, pulseData[i].amplitude2,
                                               Mathf.InverseLerp(0f, pulseData[i].length, pulseData[i].clock)));
                    if (pulseData[i].clock >= pulseData[i].length)
                        pulseData[i].type = HapticPulseType.NONE;

                    break;
            }
        }
    }

    public static bool DoingPulse(XRNode node)
    {
        return pulseData[(int)node].type != HapticPulseType.NONE;
    }

    public static void DoPulseData(XRNode node, HapticPulseData dat)
    {
        if (dat.type != HapticPulseType.NONE)
            pulseData[(int)node] = dat;
    }

    public static void Blip(XRNode node, float amplitude = 1f)
    {
        if (devices[(int)node].name != "" && pulseData[(int)node].type == HapticPulseType.NONE)
            devices[(int)node].SendHapticImpulse(0, amplitude);
        else
            Debug.LogWarning("Blip called for device which has not been found yet");
    }

    public static void FlatPulse(XRNode node, float amplitude, float length)
    {
        HapticPulseData dat = new HapticPulseData();
        dat.type = HapticPulseType.FLAT;
        dat.length = length;
        dat.amplitude1 = amplitude;
        pulseData[(int)node] = dat;
    }

    public static void SinePulse(XRNode node, float troughAmplitude, float crestAmplitude, float cycles, float length)
    {
        HapticPulseData dat = new HapticPulseData();
        dat.type = HapticPulseType.SINE;
        dat.amplitude1 = troughAmplitude;
        dat.amplitude2 = crestAmplitude;
        dat.cycles = cycles;
        dat.length = length;
        pulseData[(int)node] = dat;
    }

    public static void CosinePulse(XRNode node, float troughAmplitude, float crestAmplitude, float cycles, float length)
    {
        HapticPulseData dat = new HapticPulseData();
        dat.type = HapticPulseType.COSINE;
        dat.amplitude1 = troughAmplitude;
        dat.amplitude2 = crestAmplitude;
        dat.cycles = cycles;
        dat.length = length;
        pulseData[(int)node] = dat;
    }

    public static void LerpPulse(XRNode node, float startAmplitude, float endAmplitude, float length)
    {
        HapticPulseData dat = new HapticPulseData();
        dat.type = HapticPulseType.LERP;
        dat.amplitude1 = startAmplitude;
        dat.amplitude2 = endAmplitude;
        dat.length = length;
        pulseData[(int)node] = dat;
    }

}
