using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.XR;

[InitializeOnLoad]
public class AutoSettingsConfig
{

    private const string PACKAGE_MANIFEST_PATH = "Packages/manifest.json";

    static AutoSettingsConfig()
    {
        PlayerSettings.virtualRealitySupported = true;
    }

}
