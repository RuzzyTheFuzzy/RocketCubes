using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class OptionsValue : ScriptableObject
{

    // Store them settings in Scriptable Objects so i only need one Settings Slider script instead of like 100 million.
    public float value;
    public float defaultValue;

    // Make the vaule reset between sessions.
    // Awake dosent work as they are never initiallized.
    private void OnEnable()
    {
        value = defaultValue;
    }

}
