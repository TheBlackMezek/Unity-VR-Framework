using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TeleportManager : MonoBehaviour
{
    
    [Header("Universal Settings")]
    
    public TeleportType teleportType;
    public TeleportTransitionType transitionType;
    public LayerMask teleportMask;
    public Color validColor;
    public Color invalidColor;

    [Header("Point Teleport Settings")]

    public float maxDist;

    [Header("Arc Teleport Settings")]

    public float velocity;
    public float gravity;
    public float segmentLength;
    public int maxSegments;

    [Header("Fade Transition Settings")]

    public float fadeInLength;
    public float fadeOutLength;
    public Color fadeColor;

    [Header("Lerp Transition Settings")]

    public float lerpLength;

    [Header("Links")]
    
    public Transform playerObject;
    public Transform cameraObject;
    public Image fadeImage;
    
    private Vector3 targetPos;
    private Vector3 initialPos;

    private float fadeInClock;
    private float fadeOutClock;
    private float lerpClock;



    private void Start()
    {
        fadeImage.color = Color.clear;
    }

    private void Update()
    {
        if (fadeInClock > 0f)
        {
            fadeInClock -= Time.deltaTime;
            if (fadeInClock <= 0f)
            {
                fadeInClock = 0f;
                playerObject.position = targetPos;
            }

            fadeImage.color = Color.Lerp(Color.clear, fadeColor, Mathf.InverseLerp(fadeInLength, 0f, fadeInClock));
        }
        else if (fadeOutClock > 0f)
        {
            fadeOutClock -= Time.deltaTime;
            if (fadeOutClock < 0f)
                fadeOutClock = 0f;

            fadeImage.color = Color.Lerp(fadeColor, Color.clear, Mathf.InverseLerp(fadeOutLength, 0f, fadeOutClock));
        }

        if (lerpClock > 0f)
        {
            lerpClock -= Time.deltaTime;
            if (lerpClock <= 0f)
            {
                lerpClock = 0f;
            }

            playerObject.position = Vector3.Lerp(initialPos, targetPos, Mathf.InverseLerp(lerpLength, 0f, lerpClock));
        }
    }

    public bool SelectionUpdate(LineRenderer lineRenderer, MeshRenderer teleportEnd)
    {
        lineRenderer.SetPosition(0, lineRenderer.transform.position);

        switch (teleportType)
        {
            case TeleportType.POINT:
                return PointTeleportCheck(lineRenderer, teleportEnd);
            case TeleportType.ARC:
                return ArcTeleportCheck(lineRenderer, teleportEnd);
            default:
                return false;
        }
    }

    private bool PointTeleportCheck(LineRenderer lineRenderer, MeshRenderer teleportEnd)
    {
        lineRenderer.positionCount = 2;

        RaycastHit hit;
        if (Physics.Raycast(lineRenderer.transform.position, lineRenderer.transform.forward, out hit, maxDist, teleportMask))
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

            return true;
        }
        else
        {
            lineRenderer.SetPosition(1, lineRenderer.transform.position + lineRenderer.transform.forward * maxDist);
            lineRenderer.startColor = invalidColor;
            lineRenderer.endColor = invalidColor;

            teleportEnd.gameObject.SetActive(false);

            return false;
        }
    }

    private bool ArcTeleportCheck(LineRenderer lineRenderer, MeshRenderer teleportEnd)
    {
        Vector3 currentVelocity = lineRenderer.transform.forward * velocity;
        Vector3 currentPoint = lineRenderer.transform.position;
        List<Vector3> arcPoints = new List<Vector3>();
        arcPoints.Add(currentPoint);

        RaycastHit hit;
        bool validTeleport = false;

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

        if (validTeleport)
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

        return validTeleport;
    }

    public void Teleport(Vector3 endPoint)
    {
        switch (transitionType)
        {
            case TeleportTransitionType.FADE:
                fadeInClock = fadeInLength;
                fadeOutClock = fadeOutLength;
                targetPos = endPoint;
                targetPos.x -= cameraObject.localPosition.x;
                targetPos.z -= cameraObject.localPosition.z;
                break;
            case TeleportTransitionType.LERP:
                lerpClock = lerpLength;
                targetPos = endPoint;
                targetPos.x -= cameraObject.localPosition.x;
                targetPos.z -= cameraObject.localPosition.z;
                initialPos = playerObject.position;
                break;
            default:
                Vector3 finalPos = endPoint;
                finalPos.x -= cameraObject.localPosition.x;
                finalPos.z -= cameraObject.localPosition.z;
                playerObject.position = finalPos;
                break;
        }
    }

}
