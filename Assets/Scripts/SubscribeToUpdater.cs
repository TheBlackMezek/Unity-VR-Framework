using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubscribeToUpdater : MonoBehaviour
{
    public static event System.Action UpdateEvent;

    void Update()
    {
        if (UpdateEvent != null)
            UpdateEvent();
    }

}
