using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(LineRenderer))]
public class Teleporter : MonoBehaviour
{

    [Header("Settings")]

    [SerializeField] private Handedness hand;

    [Header("Links")]

    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private MeshRenderer teleportEnd;
    [SerializeField] private TeleportManager teleportManager;

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
        teleportEnd.gameObject.SetActive(false);
    }

    private void SelectionStart()
    {
        lineRenderer.enabled = true;
    }

    private void SelectionUpdate()
    {
        validTeleport = teleportManager.SelectionUpdate(lineRenderer, teleportEnd);
    }

    private void Teleport()
    {
        lineRenderer.enabled = false;
        teleportEnd.gameObject.SetActive(false);

        if (validTeleport)
            teleportManager.Teleport(lineRenderer.GetPosition(lineRenderer.positionCount - 1));
    }

}
