using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandInteractor : Interactor
{

    [Header("HandInteractor Settingss")]

    [SerializeField] private Handedness hand;



    private void Start()
    {
        if(hand == Handedness.LEFT)
        {
            VRInputManager.OnLeftTriggerDown.AddListener(HoldBegin);
            VRInputManager.OnLeftTriggerUp.AddListener(HoldEnd);
        }
        else
        {
            VRInputManager.OnRightTriggerDown.AddListener(HoldBegin);
            VRInputManager.OnRightTriggerUp.AddListener(HoldEnd);
        }
    }

}
