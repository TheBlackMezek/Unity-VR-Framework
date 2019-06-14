using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{

    [Header("Interactable Settings")]

    [Tooltip("If false, OnTouchBegin will only be called for the first concurrent Interactor to touch it, and OnTouchEnd only when all Interactors have stopped touching it")]
    public bool touchForEachInput;
    [Tooltip("If true, will continue being held until interaction stops" +
        "If false, will continue being held until secondary input. This also separates use from hold")]
    public bool continuousHold = true;
    [SerializeField] protected Rigidbody body;

    [Header("Interactable Events")]

    public UnityEvent OnTouchBegin;
    public UnityEvent OnTouchUpdate;
    public UnityEvent OnTouchEnd;
    public UnityEvent OnHoldBegin;
    public UnityEvent OnHoldUpdate;
    public UnityEvent OnHoldEnd;
    public UnityEvent OnUseBegin;
    public UnityEvent OnUseUpdate;
    public UnityEvent OnUseEnd;

    protected int touchCount;
    protected bool held;
    protected bool justStartedHold;
    protected Interactor holder;
    protected CollisionDetectionMode originalDetectionMode;



    protected virtual void Awake()
    {
        if (OnTouchBegin == null)
            OnTouchBegin = new UnityEvent();
        if (OnTouchUpdate == null)
            OnTouchUpdate = new UnityEvent();
        if (OnTouchEnd == null)
            OnTouchEnd = new UnityEvent();
        if (OnHoldBegin == null)
            OnHoldBegin = new UnityEvent();
        if (OnHoldUpdate == null)
            OnHoldUpdate = new UnityEvent();
        if (OnHoldEnd == null)
            OnHoldEnd = new UnityEvent();
        if (OnUseBegin == null)
            OnUseBegin = new UnityEvent();
        if (OnUseUpdate == null)
            OnUseUpdate = new UnityEvent();
        if (OnUseEnd == null)
            OnUseEnd = new UnityEvent();

        if(body != null)
            originalDetectionMode = body.collisionDetectionMode;
    }

    public virtual void TouchBegin(Transform interactor)
    {
        ++touchCount;
        if(touchForEachInput || touchCount == 1)
            OnTouchBegin.Invoke();
    }

    public virtual void TouchUpdate(Transform interactor)
    {
        OnTouchUpdate.Invoke();
    }

    public virtual void TouchEnd(Transform interactor)
    {
        --touchCount;
        if(touchForEachInput || touchCount == 0)
            OnTouchEnd.Invoke();
    }

    public virtual void InteractBegin(Interactor interactor)
    {
        if(!held)
        {
            held = true;
            holder = interactor;
            HoldBegin();
            OnHoldBegin.Invoke();

            if(continuousHold)
                OnUseBegin.Invoke();
            else
                justStartedHold = true;
        }
        else if(held && holder != interactor)
        {
            holder.Release(this);
            holder = interactor;
            HoldSwap();
        }
        else
        {
            OnUseBegin.Invoke();
        }
    }

    public virtual void InteractUpdate(Transform interactor)
    {
        if(!justStartedHold)
            OnUseUpdate.Invoke();
    }

    public virtual void InteractEnd(Transform interactor)
    {
        if(continuousHold)
        {
            held = false;
            HoldEnd(body.velocity, body.angularVelocity);
            OnHoldEnd.Invoke();
        }

        if (justStartedHold)
            justStartedHold = false;
        else
            OnUseEnd.Invoke();
    }

    public virtual void HoldUpdate(Transform interactor, Vector3 velocity, Vector3 angularVelocity)
    {
        OnHoldUpdate.Invoke();
    }

    public virtual void NoncontinuousHoldRelease()
    {
        if(!continuousHold)
        {
            held = false;
            holder.Release(this);
            HoldEnd(body.velocity, body.angularVelocity);
            OnHoldEnd.Invoke();
        }
    }

    protected virtual void HoldBegin()
    {
        holder.Hold(this);
        body.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
    }

    protected virtual void HoldSwap()
    {
        holder.Hold(this);
    }

    protected virtual void HoldEnd(Vector3 velocity, Vector3 angularVelocity)
    {
        holder.Release(this);
        body.collisionDetectionMode = originalDetectionMode;
    }

}
