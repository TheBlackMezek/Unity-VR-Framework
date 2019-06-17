using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManagerTester : MonoBehaviour
{

    [Header("Colors")]

    [SerializeField] private Color neuralColor;
    [SerializeField] private Color downColor;
    [SerializeField] private Color heldColor;
    [SerializeField] private Color upColor;

    [Header("Left")]

    [SerializeField] private MeshRenderer leftTrigger;
    [SerializeField] private MeshRenderer leftGrip;
    [SerializeField] private MeshRenderer leftPadTouch;
    [SerializeField] private MeshRenderer leftPadClick;
    [SerializeField] private MeshRenderer leftMenu;

    [Header("Right")]

    [SerializeField] private MeshRenderer rightTrigger;
    [SerializeField] private MeshRenderer rightGrip;
    [SerializeField] private MeshRenderer rightPadTouch;
    [SerializeField] private MeshRenderer rightPadClick;
    [SerializeField] private MeshRenderer rightMenu;



    private void Update()
    {
        MaterialPropertyBlock block = new MaterialPropertyBlock();

        #region Left
        if (VRInputManager.LeftTriggerDown)
            block.SetColor("_Color", downColor);
        else if (VRInputManager.LeftTriggerUp)
            block.SetColor("_Color", upColor);
        else if (VRInputManager.LeftTrigger)
            block.SetColor("_Color", heldColor);
        else
            block.SetColor("_Color", neuralColor);

        leftTrigger.SetPropertyBlock(block);

        if (VRInputManager.LeftGripDown)
            block.SetColor("_Color", downColor);
        else if (VRInputManager.LeftGripUp)
            block.SetColor("_Color", upColor);
        else if (VRInputManager.LeftGrip)
            block.SetColor("_Color", heldColor);
        else
            block.SetColor("_Color", neuralColor);

        leftGrip.SetPropertyBlock(block);

        if (VRInputManager.LeftPadTouchDown)
            block.SetColor("_Color", downColor);
        else if (VRInputManager.LeftPadTouchUp)
            block.SetColor("_Color", upColor);
        else if (VRInputManager.LeftPadTouch)
            block.SetColor("_Color", heldColor);
        else
            block.SetColor("_Color", neuralColor);

        leftPadTouch.SetPropertyBlock(block);

        if (VRInputManager.LeftPadClickDown)
            block.SetColor("_Color", downColor);
        else if (VRInputManager.LeftPadClickUp)
            block.SetColor("_Color", upColor);
        else if (VRInputManager.LeftPadClick)
            block.SetColor("_Color", heldColor);
        else
            block.SetColor("_Color", neuralColor);

        leftPadClick.SetPropertyBlock(block);

        if (VRInputManager.LeftMenuDown)
            block.SetColor("_Color", downColor);
        else if (VRInputManager.LeftMenuUp)
            block.SetColor("_Color", upColor);
        else if (VRInputManager.LeftMenu)
            block.SetColor("_Color", heldColor);
        else
            block.SetColor("_Color", neuralColor);

        leftMenu.SetPropertyBlock(block);
        #endregion

        #region Right
        if (VRInputManager.RightTriggerDown)
            block.SetColor("_Color", downColor);
        else if (VRInputManager.RightTriggerUp)
            block.SetColor("_Color", upColor);
        else if (VRInputManager.RightTrigger)
            block.SetColor("_Color", heldColor);
        else
            block.SetColor("_Color", neuralColor);

        rightTrigger.SetPropertyBlock(block);

        if (VRInputManager.RightGripDown)
            block.SetColor("_Color", downColor);
        else if (VRInputManager.RightGripUp)
            block.SetColor("_Color", upColor);
        else if (VRInputManager.RightGrip)
            block.SetColor("_Color", heldColor);
        else
            block.SetColor("_Color", neuralColor);

        rightGrip.SetPropertyBlock(block);

        if (VRInputManager.RightPadTouchDown)
            block.SetColor("_Color", downColor);
        else if (VRInputManager.RightPadTouchUp)
            block.SetColor("_Color", upColor);
        else if (VRInputManager.RightPadTouch)
            block.SetColor("_Color", heldColor);
        else
            block.SetColor("_Color", neuralColor);

        rightPadTouch.SetPropertyBlock(block);

        if (VRInputManager.RightPadClickDown)
            block.SetColor("_Color", downColor);
        else if (VRInputManager.RightPadClickUp)
            block.SetColor("_Color", upColor);
        else if (VRInputManager.RightPadClick)
            block.SetColor("_Color", heldColor);
        else
            block.SetColor("_Color", neuralColor);

        rightPadClick.SetPropertyBlock(block);

        if (VRInputManager.RightMenuDown)
            block.SetColor("_Color", downColor);
        else if (VRInputManager.RightMenuUp)
            block.SetColor("_Color", upColor);
        else if (VRInputManager.RightMenu)
            block.SetColor("_Color", heldColor);
        else
            block.SetColor("_Color", neuralColor);

        rightMenu.SetPropertyBlock(block);
        #endregion
    }

}
