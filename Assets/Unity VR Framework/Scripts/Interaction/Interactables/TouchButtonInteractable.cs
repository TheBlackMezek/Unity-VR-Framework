using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TouchButtonInteractable : Interactable
{

    [Header("Touch Button Settings")]

    [SerializeField] private bool startsOn;
    [SerializeField] private bool callEventsOnStart;
    [SerializeField] private float lerpLength;
    [Tooltip("If false, state is changed on lerp end instead")]
    [SerializeField] private bool changeStateOnLerpStart;
    [SerializeField] private bool changeColor;
    [SerializeField] private bool lerpColors;
    [SerializeField] private Color onColor;
    [SerializeField] private Color offColor;
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private bool changeTransform;
    [SerializeField] private bool lerpTransforms;
    [SerializeField] private Transform onTransform;
    [SerializeField] private Transform offTransform;
    [SerializeField] private TMPro.TextMeshProUGUI textObject;
    [SerializeField] private string onText;
    [SerializeField] private string offText;

    [Header("Button Events")]

    public UnityEvent OnTurnedOn;
    public UnityEvent OnTurnedOff;
    public UnityEvent OnLerp;

    public bool IsOn { get; private set; }
    public float LerpClock { get; private set; }



    private void Reset()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    protected override void Awake()
    {
        base.Awake();

        IsOn = startsOn;
        LerpClock = IsOn ? 1f : 0f;

        if(changeColor)
        {
            MaterialPropertyBlock block = new MaterialPropertyBlock();
            block.SetColor("_Color", IsOn ? onColor : offColor);
            meshRenderer.SetPropertyBlock(block);
        }
        if(changeTransform)
        {
            transform.position = IsOn ? onTransform.position : offTransform.position;
            transform.rotation = IsOn ? onTransform.rotation : offTransform.rotation;
        }
        if (textObject != null)
        {
            textObject.text = IsOn ? onText : offText;
        }
    }

    private void Start()
    {
        if(callEventsOnStart)
        {
            if (IsOn)
                OnTurnedOn.Invoke();
            else
                OnTurnedOff.Invoke();
        }
    }

    public override void TouchBegin(Transform interactor)
    {
        base.TouchBegin(interactor);
        
        if (LerpClock != 0f && LerpClock != 1f)
            return;
        
        if(lerpTransforms || lerpColors)
        {
            SubscribeToUpdater.UpdateEvent += SubscribedUpdate;
            if(changeStateOnLerpStart)
            {
                IsOn = !IsOn;
                if (IsOn)
                    OnTurnedOn.Invoke();
                else
                    OnTurnedOff.Invoke();

                if (textObject != null)
                    textObject.text = IsOn ? onText : offText;
            }

            if(!lerpColors && changeColor)
            {
                MaterialPropertyBlock block = new MaterialPropertyBlock();
                block.SetColor("_Color", IsOn ? onColor : offColor);
                meshRenderer.SetPropertyBlock(block);
            }
            if(!lerpTransforms && changeTransform)
            {
                transform.position = IsOn ? onTransform.position : offTransform.position;
                transform.rotation = IsOn ? onTransform.rotation : offTransform.rotation;
            }
        }
        else
        {
            IsOn = !IsOn;

            if (changeColor)
            {
                MaterialPropertyBlock block = new MaterialPropertyBlock();
                block.SetColor("_Color", IsOn ? onColor : offColor);
                meshRenderer.SetPropertyBlock(block);
            }
            if (changeTransform)
            {
                transform.position = IsOn ? onTransform.position : offTransform.position;
                transform.rotation = IsOn ? onTransform.rotation : offTransform.rotation;
            }
            if (textObject != null)
            {
                textObject.text = IsOn ? onText : offText;
            }
        }
    }

    private void SubscribedUpdate()
    {
        OnLerp.Invoke();
        
        if ((changeStateOnLerpStart && !IsOn) || (!changeStateOnLerpStart && IsOn))
        {
            LerpClock -= Time.deltaTime;
            if(LerpClock <= 0f)
            {
                LerpClock = 0f;
                if(!changeStateOnLerpStart)
                {
                    IsOn = false;
                    OnTurnedOff.Invoke();
                    if (textObject != null)
                        textObject.text = IsOn ? onText : offText;
                }
                SubscribeToUpdater.UpdateEvent -= SubscribedUpdate;
            }
        }
        else
        {
            LerpClock += Time.deltaTime;
            if (LerpClock >= 1f)
            {
                LerpClock = 1f;
                if (!changeStateOnLerpStart)
                {
                    IsOn = true;
                    OnTurnedOn.Invoke();
                    if (textObject != null)
                        textObject.text = IsOn ? onText : offText;
                }
                SubscribeToUpdater.UpdateEvent -= SubscribedUpdate;
            }
        }

        float lerpVal = Mathf.InverseLerp(0f, lerpLength, LerpClock);
        if (lerpColors)
        {
            MaterialPropertyBlock block = new MaterialPropertyBlock();
            block.SetColor("_Color", Color.Lerp(offColor, onColor, lerpVal));
            meshRenderer.SetPropertyBlock(block);
        }
        if (lerpTransforms)
        {
            transform.position = Vector3.Lerp(offTransform.position, onTransform.position, lerpVal);
            transform.rotation = Quaternion.Lerp(offTransform.rotation, onTransform.rotation, lerpVal);
        }
    }

}
