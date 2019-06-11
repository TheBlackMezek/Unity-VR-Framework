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
        for(int i = 0; i < NODE_COUNT; ++i)
        {
            switch (pulseData[i].type)
            {
                case HapticPulseType.FLAT:
                    pulseData[i].clock += dt;
                    Blip((XRNode)i, pulseData[i].amplitude1);
                    if (pulseData[i].clock >= pulseData[i].length)
                        pulseData[i].type = HapticPulseType.NONE;
                    break;
            }
        }
    }

    public static void Blip(XRNode node, float amplitude = 1f)
    {
        if (devices[(int)node].name != "")
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

}
