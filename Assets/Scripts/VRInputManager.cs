using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.Events;

public class VRInputManager : MonoBehaviour
{

    public static VRInputManager Instance { get; private set; }

    public UnityEvent OnLeftTriggerDown;
    public UnityEvent OnLeftTrigger;
    public UnityEvent OnLeftTriggerUp;
    public UnityEvent OnRightTriggerDown;
    public UnityEvent OnRightTrigger;
    public UnityEvent OnRightTriggerUp;

    private InputDevice leftHand;
    private InputDevice rightHand;

    private bool leftTriggerThisFrame;
    private bool leftTriggerLastFrame;
    private bool rightTriggerThisFrame;
    private bool rightTriggerLastFrame;



    private void Awake()
    {
        Instance = this;

        OnLeftTriggerDown = new UnityEvent();
        OnLeftTrigger = new UnityEvent();
        OnLeftTriggerUp = new UnityEvent();
        OnRightTriggerDown = new UnityEvent();
        OnRightTrigger = new UnityEvent();
        OnRightTriggerUp = new UnityEvent();
    }

    private void Start()
    {
        GetLeftHand();
        GetRightHand();
    }

    private void Update()
    {
        leftTriggerThisFrame = LeftTrigger();
        rightTriggerThisFrame = RightTrigger();

        if (leftTriggerThisFrame && !leftTriggerLastFrame)
            OnLeftTriggerDown.Invoke();
        else if (leftTriggerThisFrame)
            OnLeftTrigger.Invoke();
        else if (!leftTriggerThisFrame && leftTriggerLastFrame)
            OnLeftTriggerUp.Invoke();

        if (rightTriggerThisFrame && !rightTriggerLastFrame)
            OnRightTriggerDown.Invoke();
        else if (rightTriggerThisFrame)
            OnRightTrigger.Invoke();
        else if (!rightTriggerThisFrame && rightTriggerLastFrame)
            OnRightTriggerUp.Invoke();

        leftTriggerLastFrame = LeftTrigger();
        rightTriggerLastFrame = RightTrigger();
    }

    #region Device Retrievers
    private void GetLeftHand()
    {
        leftHand = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
        if (leftHand.name == "")
            Invoke("GetLeftHand", 0.1f);
    }

    private void GetRightHand()
    {
        rightHand = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
        if (rightHand.name == "")
            Invoke("GetRightHand", 0.1f);
    }
    #endregion

    #region Left Hand Input
    public float LeftTriggerVal()
    {
        float ret;
        leftHand.TryGetFeatureValue(CommonUsages.trigger, out ret);
        return ret;
    }

    public bool LeftTriggerDown()
    {
        return !leftTriggerLastFrame && LeftTrigger();
    }

    public bool LeftTrigger()
    {
        bool ret;
        leftHand.TryGetFeatureValue(CommonUsages.triggerButton, out ret);
        return ret;
    }

    public bool LeftTriggerUp()
    {
        return leftTriggerLastFrame && !LeftTrigger();
    }

    public Vector3 LeftHandPos()
    {
        Vector3 ret;
        leftHand.TryGetFeatureValue(CommonUsages.devicePosition, out ret);
        return ret;
    }

    public float LeftHandBattery()
    {
        float ret;
        leftHand.TryGetFeatureValue(CommonUsages.batteryLevel, out ret);
        return ret;
    }
    #endregion

    #region Right Hand Input
    public float RightTriggerVal()
    {
        float ret;
        rightHand.TryGetFeatureValue(CommonUsages.trigger, out ret);
        return ret;
    }

    public bool RightTriggerDown()
    {
        return !rightTriggerLastFrame && RightTrigger();
    }

    public bool RightTrigger()
    {
        bool ret;
        rightHand.TryGetFeatureValue(CommonUsages.triggerButton, out ret);
        return ret;
    }

    public bool RightTriggerUp()
    {
        return rightTriggerLastFrame && !RightTrigger();
    }

    public Vector3 RightHandPos()
    {
        Vector3 ret;
        rightHand.TryGetFeatureValue(CommonUsages.devicePosition, out ret);
        return ret;
    }

    public float RightHandBattery()
    {
        float ret;
        rightHand.TryGetFeatureValue(CommonUsages.batteryLevel, out ret);
        return ret;
    }
    #endregion

}
