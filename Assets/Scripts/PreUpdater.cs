using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreUpdater : MonoBehaviour
{
    public static event System.Action PreUpdate;
    
    void Update()
    {
        if (PreUpdate != null)
            PreUpdate();
    }

}
