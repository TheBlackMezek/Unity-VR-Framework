using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Interactor : MonoBehaviour
{

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
            holding[i].HoldUpdate(transform);
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

    public virtual void HoldBegin()
    {
        for(int i = 0; i < touching.Count; ++i)
        {
            holding.Add(touching[i]);
            touching[i].HoldBegin(this);
        }
    }

    public virtual void HoldEnd()
    {
        for (int i = 0; i < holding.Count; ++i)
            holding[i].HoldEnd(transform);

        holding.Clear();
    }

    public virtual void Release(Interactable interactable)
    {
        holding.Remove(interactable);
    }

}
