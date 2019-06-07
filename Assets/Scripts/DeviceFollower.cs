using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

//This is to be placed on the objects which represent the hands or head to provide highest-accuracy physical device data during gameplay

public class DeviceFollower : MonoBehaviour
{

    [SerializeField]
    private DeviceEnum deviceEnum;

    private InputDevice device;



    private void Awake()
    {
        PreUpdater.PreUpdate += PreUpdate;

        GetDevice();
    }

    private void PreUpdate()
    {
        Vector3 pos;
        Quaternion rot;

        device.TryGetFeatureValue(CommonUsages.devicePosition, out pos);
        device.TryGetFeatureValue(CommonUsages.deviceRotation, out rot);

        transform.localPosition = pos;
        transform.localRotation = rot;
    }

    private void GetDevice()
    {
        switch (deviceEnum)
        {
            case DeviceEnum.HEAD:
                device = InputDevices.GetDeviceAtXRNode(XRNode.Head);
                break;
            case DeviceEnum.LEFT_HAND:
                device = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
                break;
            case DeviceEnum.RIGHT_HAND:
                device = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
                break;
        }
        //The device might not have been recognized yet so this check is necessary
        if (device.name == "")
            Invoke("GetDevice", 0.1f);
    }

}
