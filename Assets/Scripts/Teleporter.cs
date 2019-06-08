using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(LineRenderer))]
public class Teleporter : MonoBehaviour
{

    public enum TeleportType
    {
        POINT,
        ARC
    }

    public enum TeleportTransitionType
    {
        NONE,
        FADE
    }



    [Header("Universal Settings")]

    [SerializeField] private Handedness hand;
    [SerializeField] private TeleportType teleportType;
    [SerializeField] private TeleportTransitionType transitionType;
    [SerializeField] private LayerMask teleportMask;
    [SerializeField] private Color validColor;
    [SerializeField] private Color invalidColor;

    [Header("Point Teleport Settings")]

    [SerializeField] private float maxDist;

    [Header("Arc Teleport Settings")]

    [SerializeField] private float velocity;
    [SerializeField] private float gravity;
    [SerializeField] private float segmentLength;
    [SerializeField] private int maxSegments;

    [Header("Fade Transition Settings")]

    [SerializeField] private float fadeInLength;
    [SerializeField] private float fadeOutLength;
    [SerializeField] private Color fadeColor;

    [Header("Links")]

    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private Transform playerObject;
    [SerializeField] private Transform cameraObject;
    [SerializeField] private MeshRenderer teleportEnd;
    [SerializeField] private Image fadeImage;

    private bool validTeleport;
    private static Vector3 targetPos;

    private static float fadeInClock;
    private static float fadeOutClock;



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
        fadeImage.color = Color.clear;
    }

    private void Update()
    {
        if(fadeInClock > 0f)
        {
            fadeInClock -= Time.deltaTime;
            if (fadeInClock <= 0f)
            {
                fadeInClock = 0f;
                playerObject.position = targetPos;
            }

            fadeImage.color = Color.Lerp(Color.clear, fadeColor, Mathf.InverseLerp(fadeInLength, 0f, fadeInClock));
        }
        else if(fadeOutClock > 0f)
        {
            fadeOutClock -= Time.deltaTime;
            if (fadeOutClock < 0f)
                fadeOutClock = 0f;

            fadeImage.color = Color.Lerp(fadeColor, Color.clear, Mathf.InverseLerp(fadeOutLength, 0f, fadeOutClock));
        }
    }

    private void SelectionStart()
    {
        lineRenderer.enabled = true;
    }

    private void SelectionUpdate()
    {
        lineRenderer.SetPosition(0, transform.position);

        switch (teleportType)
        {
            case TeleportType.POINT:
                PointTeleportCheck();
                break;
            case TeleportType.ARC:
                ArcTeleportCheck();
                break;
        }
    }

    private void PointTeleportCheck()
    {
        lineRenderer.positionCount = 2;

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

    private void ArcTeleportCheck()
    {
        Vector3 currentVelocity = transform.forward * velocity;
        Vector3 currentPoint = transform.position;
        List<Vector3> arcPoints = new List<Vector3>();
        arcPoints.Add(currentPoint);

        RaycastHit hit;
        validTeleport = false;

        while (!validTeleport && arcPoints.Count < maxSegments)
        {
            if (Physics.Raycast(currentPoint, currentVelocity.normalized, out hit, segmentLength, teleportMask))
            {
                arcPoints.Add(hit.point);
                currentPoint = hit.point;
                validTeleport = true;
            }
            else
            {
                currentPoint = currentPoint + (currentVelocity.normalized * segmentLength);
                arcPoints.Add(currentPoint);
                currentVelocity.y -= gravity;
            }
        }

        lineRenderer.positionCount = arcPoints.Count;
        lineRenderer.SetPositions(arcPoints.ToArray());

        if(validTeleport)
        {
            lineRenderer.startColor = validColor;
            lineRenderer.endColor = validColor;

            teleportEnd.gameObject.SetActive(true);
            MaterialPropertyBlock block = new MaterialPropertyBlock();
            block.SetColor("_Color", validColor);
            teleportEnd.SetPropertyBlock(block);
            teleportEnd.transform.position = arcPoints[arcPoints.Count - 1];
            teleportEnd.transform.rotation = Quaternion.identity;
        }
        else
        {
            lineRenderer.startColor = invalidColor;
            lineRenderer.endColor = invalidColor;

            teleportEnd.gameObject.SetActive(false);
        }
    }

    private void Teleport()
    {
        lineRenderer.enabled = false;
        teleportEnd.gameObject.SetActive(false);

        if (validTeleport)
        {
            switch(transitionType)
            {
                case TeleportTransitionType.FADE:
                    fadeInClock = fadeInLength;
                    fadeOutClock = fadeOutLength;
                    targetPos = lineRenderer.GetPosition(lineRenderer.positionCount - 1);
                    targetPos.x -= cameraObject.localPosition.x;
                    targetPos.z -= cameraObject.localPosition.z;
                    break;
                default:
                    Vector3 finalPos = lineRenderer.GetPosition(lineRenderer.positionCount - 1);
                    finalPos.x -= cameraObject.localPosition.x;
                    finalPos.z -= cameraObject.localPosition.z;
                    playerObject.position = finalPos;
                    break;
            }
        }
    }

}
