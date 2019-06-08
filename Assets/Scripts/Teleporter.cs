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
    [SerializeField] private Color validColor;
    [SerializeField] private Color invalidColor;

    [Header("Links")]

    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private Transform playerObject;
    [SerializeField] private Transform cameraObject;
    [SerializeField] private MeshRenderer teleportEnd;

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
        lineRenderer.SetPosition(0, transform.position);

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, maxDist, teleportMask))
        {
            lineRenderer.SetPosition(1, hit.point);
            lineRenderer.startColor = validColor;
            lineRenderer.endColor = validColor;

            teleportEnd.gameObject.SetActive(true);
            MaterialPropertyBlock block = new MaterialPropertyBlock();
            block.SetColor("_Color", validColor);
            teleportEnd.SetPropertyBlock(block);
            teleportEnd.transform.position = hit.point;
            teleportEnd.transform.rotation = Quaternion.identity;

            validTeleport = true;
        }
        else
        {
            lineRenderer.SetPosition(1, transform.position + transform.forward * maxDist);
            lineRenderer.startColor = invalidColor;
            lineRenderer.endColor = invalidColor;

            teleportEnd.gameObject.SetActive(false);

            validTeleport = false;
        }
    }

    private void Teleport()
    {
        lineRenderer.enabled = false;
        teleportEnd.gameObject.SetActive(false);

        if (validTeleport)
        {
            Vector3 finalPos = lineRenderer.GetPosition(1);
            finalPos.x -= cameraObject.localPosition.x;
            finalPos.z -= cameraObject.localPosition.z;
            playerObject.position = finalPos;
        }
    }

}
