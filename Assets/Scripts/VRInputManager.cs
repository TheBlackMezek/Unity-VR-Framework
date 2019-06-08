using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.Events;

public class VRInputManager : MonoBehaviour
{

    public static UnityEvent OnLeftTriggerDown;
    public static UnityEvent OnLeftTrigger;
    public static UnityEvent OnLeftTriggerUp;
    public static UnityEvent OnLeftGripDown;
    public static UnityEvent OnLeftGrip;
    public static UnityEvent OnLeftGripUp;
    public static UnityEvent OnLeftMenuDown;
    public static UnityEvent OnLeftMenu;
    public static UnityEvent OnLeftMenuUp;
    public static UnityEvent OnLeftPadTouchDown;
    public static UnityEvent OnLeftPadTouch;
    public static UnityEvent OnLeftPadTouchUp;
    public static UnityEvent OnLeftPadClickDown;
    public static UnityEvent OnLeftPadClick;
    public static UnityEvent OnLeftPadClickUp;

    public static UnityEvent OnRightTriggerDown;
    public static UnityEvent OnRightTrigger;
    public static UnityEvent OnRightTriggerUp;
    public static UnityEvent OnRightGripDown;
    public static UnityEvent OnRightGrip;
    public static UnityEvent OnRightGripUp;
    public static UnityEvent OnRightMenuDown;
    public static UnityEvent OnRightMenu;
    public static UnityEvent OnRightMenuUp;
    public static UnityEvent OnRightPadTouchDown;
    public static UnityEvent OnRightPadTouch;
    public static UnityEvent OnRightPadTouchUp;
    public static UnityEvent OnRightPadClickDown;
    public static UnityEvent OnRightPadClick;
    public static UnityEvent OnRightPadClickUp;

    public static bool LeftTriggerDown { get; private set; }
    public static bool LeftTrigger { get; private set; }
    public static bool LeftTriggerUp { get; private set; }
    public static float LeftTriggerVal { get; private set; }
    public static bool LeftGripDown { get; private set; }
    public static bool LeftGrip { get; private set; }
    public static bool LeftGripUp { get; private set; }
    public static float LeftGripVal { get; private set; }
    public static bool LeftMenuDown { get; private set; }
    public static bool LeftMenu { get; private set; }
    public static bool LeftMenuUp { get; private set; }
    public static bool LeftPadTouchDown { get; private set; }
    public static bool LeftPadTouch { get; private set; }
    public static bool LeftPadTouchUp { get; private set; }
    public static bool LeftPadClickDown { get; private set; }
    public static bool LeftPadClick { get; private set; }
    public static bool LeftPadClickUp { get; private set; }
    public static float LeftHandBattery { get; private set; }

    public static bool RightTriggerDown { get; private set; }
    public static bool RightTrigger { get; private set; }
    public static bool RightTriggerUp { get; private set; }
    public static float RightTriggerVal { get; private set; }
    public static bool RightGripDown { get; private set; }
    public static bool RightGrip { get; private set; }
    public static bool RightGripUp { get; private set; }
    public static float RightGripVal { get; private set; }
    public static bool RightMenuDown { get; private set; }
    public static bool RightMenu { get; private set; }
    public static bool RightMenuUp { get; private set; }
    public static bool RightPadTouchDown { get; private set; }
    public static bool RightPadTouch { get; private set; }
    public static bool RightPadTouchUp { get; private set; }
    public static bool RightPadClickDown { get; private set; }
    public static bool RightPadClick { get; private set; }
    public static bool RightPadClickUp { get; private set; }
    public static float RightHandBattery { get; private set; }

    public static Vector3 HeadPos { get; private set; }
    public static Quaternion HeadRot { get; private set; }

    public static Vector3 LeftHandPos { get; private set; }
    public static Quaternion LeftHandRot { get; private set; }

    public static Vector3 RightHandPos { get; private set; }
    public static Quaternion RightHandRot { get; private set; }

    private InputDevice head;
    private InputDevice leftHand;
    private InputDevice rightHand;



    private void Awake()
    {
        PreUpdater.PreUpdate += PreUpdate;

        OnLeftTriggerDown = new UnityEvent();
        OnLeftTrigger = new UnityEvent();
        OnLeftTriggerUp = new UnityEvent();
        OnLeftGripDown = new UnityEvent();
        OnLeftGrip = new UnityEvent();
        OnLeftGripUp = new UnityEvent();
        OnLeftMenuDown = new UnityEvent();
        OnLeftMenu = new UnityEvent();
        OnLeftMenuUp = new UnityEvent();
        OnLeftPadTouchDown = new UnityEvent();
        OnLeftPadTouch = new UnityEvent();
        OnLeftPadTouchUp = new UnityEvent();
        OnLeftPadClickDown = new UnityEvent();
        OnLeftPadClick = new UnityEvent();
        OnLeftPadClickUp = new UnityEvent();

        OnRightTriggerDown = new UnityEvent();
        OnRightTrigger = new UnityEvent();
        OnRightTriggerUp = new UnityEvent();
        OnRightGripDown = new UnityEvent();
        OnRightGrip = new UnityEvent();
        OnRightGripUp = new UnityEvent();
        OnRightMenuDown = new UnityEvent();
        OnRightMenu = new UnityEvent();
        OnRightMenuUp = new UnityEvent();
        OnRightPadTouchDown = new UnityEvent();
        OnRightPadTouch = new UnityEvent();
        OnRightPadTouchUp = new UnityEvent();
        OnRightPadClickDown = new UnityEvent();
        OnRightPadClick = new UnityEvent();
        OnRightPadClickUp = new UnityEvent();
    }

    private void Start()
    {
        //These are in Start because the hardware is more likely to have been recognized by here than in Awake
        GetHead();
        GetLeftHand();
        GetRightHand();
    }

    private void PreUpdate()
    {
        #region Variable creation and value getting
        Vector3 headPos;
        Quaternion headRot;

        float leftHandBattery;
        bool leftTrigger;
        float leftTriggerVal;
        bool leftGrip;
        float leftGripVal;
        bool leftPadTouch;
        bool leftPadClick;
        bool leftMenu;
        Vector3 leftPos;
        Quaternion leftRot;

        float rightHandBattery;
        bool rightTrigger;
        float rightTriggerVal;
        bool rightGrip;
        float rightGripVal;
        bool rightPadTouch;
        bool rightPadClick;
        bool rightMenu;
        Vector3 rightPos;
        Quaternion rightRot;

        head.TryGetFeatureValue(CommonUsages.devicePosition, out headPos);
        head.TryGetFeatureValue(CommonUsages.deviceRotation, out headRot);

        leftHand.TryGetFeatureValue(CommonUsages.batteryLevel, out leftHandBattery);
        leftHand.TryGetFeatureValue(CommonUsages.triggerButton, out leftTrigger);
        leftHand.TryGetFeatureValue(CommonUsages.trigger, out leftTriggerVal);
        leftHand.TryGetFeatureValue(CommonUsages.gripButton, out leftGrip);
        leftHand.TryGetFeatureValue(CommonUsages.grip, out leftGripVal);
        leftHand.TryGetFeatureValue(CommonUsages.primary2DAxisTouch, out leftPadTouch);
        leftHand.TryGetFeatureValue(CommonUsages.primary2DAxisClick, out leftPadClick);
        leftHand.TryGetFeatureValue(CommonUsages.menuButton, out leftMenu);
        leftHand.TryGetFeatureValue(CommonUsages.devicePosition, out leftPos);
        leftHand.TryGetFeatureValue(CommonUsages.deviceRotation, out leftRot);

        rightHand.TryGetFeatureValue(CommonUsages.batteryLevel, out rightHandBattery);
        rightHand.TryGetFeatureValue(CommonUsages.triggerButton, out rightTrigger);
        rightHand.TryGetFeatureValue(CommonUsages.trigger, out rightTriggerVal);
        rightHand.TryGetFeatureValue(CommonUsages.gripButton, out rightGrip);
        rightHand.TryGetFeatureValue(CommonUsages.grip, out rightGripVal);
        rightHand.TryGetFeatureValue(CommonUsages.primary2DAxisTouch, out rightPadTouch);
        rightHand.TryGetFeatureValue(CommonUsages.primary2DAxisClick, out rightPadClick);
        rightHand.TryGetFeatureValue(CommonUsages.menuButton, out rightMenu);
        rightHand.TryGetFeatureValue(CommonUsages.devicePosition, out rightPos);
        rightHand.TryGetFeatureValue(CommonUsages.deviceRotation, out rightRot);
        #endregion

        #region Property Setting
        HeadPos = headPos;
        HeadRot = headRot;

        LeftHandBattery = leftHandBattery;
        LeftTriggerDown = (leftTrigger && !LeftTrigger);
        LeftTriggerUp = (!leftTrigger && LeftTrigger);
        LeftTrigger = leftTrigger;
        LeftGripDown = (leftGrip && !LeftGrip);
        LeftGripUp = (!leftGrip && LeftGrip);
        LeftGrip = leftGrip;
        LeftPadTouchDown = (leftPadTouch && !LeftPadTouch);
        LeftPadTouchUp = (!leftPadTouch && LeftPadTouch);
        LeftPadTouch = leftPadTouch;
        LeftPadClickDown = (leftPadClick && !LeftPadClick);
        LeftPadClickUp = (!leftPadClick && LeftPadClick);
        LeftPadClick = leftPadClick;
        LeftMenuDown = (leftMenu && !LeftMenu);
        LeftMenuUp = (!leftMenu && LeftMenu);
        LeftMenu = leftMenu;
        LeftHandPos = leftPos;
        LeftHandRot = leftRot;

        RightHandBattery = rightHandBattery;
        RightTriggerDown = (rightTrigger && !RightTrigger);
        RightTriggerUp = (!rightTrigger && RightTrigger);
        RightTrigger = rightTrigger;
        RightGripDown = (rightGrip && !RightGrip);
        RightGripUp = (!rightGrip && RightGrip);
        RightGrip = rightGrip;
        RightPadTouchDown = (rightPadTouch && !RightPadTouch);
        RightPadTouchUp = (!rightPadTouch && RightPadTouch);
        RightPadTouch = rightPadTouch;
        RightPadClickDown = (rightPadClick && !RightPadClick);
        RightPadClickUp = (!rightPadClick && RightPadClick);
        RightPadClick = rightPadClick;
        RightMenuDown = (rightMenu && !RightMenu);
        RightMenuUp = (!rightMenu && RightMenu);
        RightMenu = rightMenu;
        RightHandPos = rightPos;
        RightHandRot = rightRot;
        #endregion

        #region Event Invokes
        if (LeftTriggerDown)
            OnLeftTriggerDown.Invoke();
        if (LeftTrigger)
            OnLeftTrigger.Invoke();
        if (LeftTriggerUp)
            OnLeftTriggerUp.Invoke();
        if (LeftGripDown)
            OnLeftGripDown.Invoke();
        if (LeftGrip)
            OnLeftGrip.Invoke();
        if (LeftGripUp)
            OnLeftGripUp.Invoke();
        if (LeftPadTouchDown)
            OnLeftPadTouchDown.Invoke();
        if (LeftPadTouch)
            OnLeftPadTouch.Invoke();
        if (LeftPadTouchUp)
            OnLeftPadTouchUp.Invoke();
        if (LeftPadClickDown)
            OnLeftPadClickDown.Invoke();
        if (LeftPadClick)
            OnLeftPadClick.Invoke();
        if (LeftPadClickUp)
            OnLeftPadClickUp.Invoke();
        if (LeftMenuDown)
            OnLeftMenuDown.Invoke();
        if (LeftMenu)
            OnLeftMenu.Invoke();
        if (LeftMenuUp)
            OnLeftMenuUp.Invoke();

        if (RightTriggerDown)
            OnRightTriggerDown.Invoke();
        if (RightTrigger)
            OnRightTrigger.Invoke();
        if (RightTriggerUp)
            OnRightTriggerUp.Invoke();
        if (RightGripDown)
            OnRightGripDown.Invoke();
        if (RightGrip)
            OnRightGrip.Invoke();
        if (RightGripUp)
            OnRightGripUp.Invoke();
        if (RightPadTouchDown)
            OnRightPadTouchDown.Invoke();
        if (RightPadTouch)
            OnRightPadTouch.Invoke();
        if (RightPadTouchUp)
            OnRightPadTouchUp.Invoke();
        if (RightPadClickDown)
            OnRightPadClickDown.Invoke();
        if (RightPadClick)
            OnRightPadClick.Invoke();
        if (RightPadClickUp)
            OnRightPadClickUp.Invoke();
        if (RightMenuDown)
            OnRightMenuDown.Invoke();
        if (RightMenu)
            OnRightMenu.Invoke();
        if (RightMenuUp)
            OnRightMenuUp.Invoke();
        #endregion
    }

    private void GetHead()
    {
        head = InputDevices.GetDeviceAtXRNode(XRNode.Head);
        //The device might not have been recognized yet so this check is necessary
        if (head.name == "")
            Invoke("GetHead", 0.1f);
    }

    private void GetLeftHand()
    {
        leftHand = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
        //The device might not have been recognized yet so this check is necessary
        if (leftHand.name == "")
            Invoke("GetLeftHand", 0.1f);
    }

    private void GetRightHand()
    {
        rightHand = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
        //The device might not have been recognized yet so this check is necessary
        if (rightHand.name == "")
            Invoke("GetRightHand", 0.1f);
    }
    
}
