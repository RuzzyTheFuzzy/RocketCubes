using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Setting
{
    maxFuel,
    players,
    grenades,
    turnLength
}

public class OptionsManager : MonoBehaviour
{

    public static OptionsManager instance { get; private set; }
    public SettingsValue maxFuel;
    public SettingsValue players;
    public SettingsValue grenades;
    public SettingsValue speed;
    public SettingsValue turnLength;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
            return;
        }

        instance = this;

    }
}
