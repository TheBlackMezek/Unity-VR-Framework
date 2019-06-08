using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{

    [Header("Interactable Settings")]

    [Tooltip("If false, OnTouchBegin will only be called for the first concurrent Interactor to touch it, and OnTouchEnd only when all Interactors have stopped touching it")]
    public bool touchForEachInput;
    [Tooltip("If true, ")]
    public bool continuousHold = true;

    [Header("Interactable Events")]

    public UnityEvent OnTouchBegin;
    public UnityEvent OnTouchUpdate;
    public UnityEvent OnTouchEnd;
    public UnityEvent OnHoldBegin;
    public UnityEvent OnHoldUpdate;
    public UnityEvent OnHoldEnd;

    protected int touchCount;
    protected bool held;
    protected Interactor holder;



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

    public virtual void HoldBegin(Interactor holder)
    {
        if (held)
            holder.Release(this);
        else
            held = true;

        this.holder = holder;
        OnHoldBegin.Invoke();
    }

    public virtual void HoldUpdate(Transform interactor)
    {
        OnHoldUpdate.Invoke();
    }

    public virtual void HoldEnd(Transform interactor)
    {
        held = false;
        OnHoldEnd.Invoke();
    }

}
