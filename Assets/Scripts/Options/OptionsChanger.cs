using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsChanger : MonoBehaviour
{
    // SettingsManager follows into gameplay, so i need something in the menu scene to change the values.
    public void ChangeMaxFuel(float value)
    {
        Options.MaxFuel.value = value;
    }
    public void ChangePlayers(float value)
    {
        Options.Players.value = value;
    }
    public void ChangeMaxGrenades(float value)
    {
        Options.Grenades.value = value;
    }
    public void ChangeSpeed(float value)
    {
        Options.Speed.value = value;
    }
    public void ChangeTurnLength(float value)
    {
        Options.TurnLength.value = value;
    }

}
