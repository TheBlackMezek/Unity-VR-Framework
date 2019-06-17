using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Interactor : MonoBehaviour
{

    [SerializeField] protected Rigidbody body;

    protected List<Interactable> touching = new List<Interactable>();
    protected List<Interactable> holding = new List<Interactable>();



    protected virtual void OnTriggerEnter(Collider other)
    {
        Interactable interactable = other.GetComponent<Interactable>();

        if(interactable != null)
        {
            touching.Add(interactable);
            interactable.TouchBegin(transform);
        }
    }

    protected virtual void Update()
    {
        for (int i = 0; i < touching.Count; ++i)
            touching[i].TouchUpdate(transform);

        for (int i = 0; i < holding.Count; ++i)
            holding[i].HoldUpdate(transform, body.velocity, body.angularVelocity);
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        Interactable interactable = other.GetComponent<Interactable>();

        if(interactable != null)
        {
            touching.Remove(interactable);
            interactable.TouchEnd(transform);
        }
    }

    public virtual void InteractBegin()
    {
        for(int i = 0; i < touching.Count; ++i)
        {
            touching[i].InteractBegin(this);
        }
    }

    public virtual void InteractEnd()
    {
        for (int i = 0; i < holding.Count; ++i)
            holding[i].InteractEnd(transform);
    }

    public virtual void ReleaseHoldInput()
    {
        for (int i = 0; i < holding.Count; ++i)
            holding[i].NoncontinuousHoldRelease();
    }

    public virtual void Hold(Interactable interactable)
    {
        holding.Add(interactable);
    }

    public virtual void Release(Interactable interactable)
    {
        holding.Remove(interactable);
    }

    public virtual bool GetNode(out UnityEngine.XR.XRNode node)
    {
        node = UnityEngine.XR.XRNode.LeftEye;
        return false;
    }

}
