using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Teleporter : MonoBehaviour
{

    [Header("Settings")]

    [SerializeField] private Handedness hand;
    [SerializeField] private float maxDist;
    [SerializeField] private LayerMask teleportMask;

    [Header("Links")]

    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private Transform playerObject;

    private bool validTeleport;



    private void Start()
    {
        if(hand == Handedness.LEFT)
        {
            VRInputManager.OnLeftPadClickDown.AddListener(SelectionStart);
            VRInputManager.OnLeftPadClick.AddListener(SelectionUpdate);
            VRInputManager.OnLeftPadClickUp.AddListener(Teleport);
        }
        else
        {
            VRInputManager.OnRightPadClickDown.AddListener(SelectionStart);
            VRInputManager.OnRightPadClick.AddListener(SelectionUpdate);
            VRInputManager.OnRightPadClickUp.AddListener(Teleport);
        }

        lineRenderer.enabled = false;
        lineRenderer.positionCount = 2;
    }

    private void SelectionStart()
    {
        lineRenderer.enabled = true;
    }

    private void SelectionUpdate()
    {
        lineRenderer.SetPosition(0, transform.position);

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, maxDist, teleportMask))
        {
            lineRenderer.SetPosition(1, hit.point);
            validTeleport = true;
        }
        else
        {
            lineRenderer.SetPosition(1, transform.position + transform.forward * maxDist);
            validTeleport = false;
        }
    }

    private void Teleport()
    {
        lineRenderer.enabled = false;

        if(validTeleport)
            playerObject.position = lineRenderer.GetPosition(1);
    }

}
