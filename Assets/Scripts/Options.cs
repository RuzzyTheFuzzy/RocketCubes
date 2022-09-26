using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Options : MonoBehaviour
{

    public void ChangeMaxFuel(float value)
    {
        OptionsManager.instance.maxFuel.value = value;
    }
    public void ChangePlayers(float value)
    {
        OptionsManager.instance.players.value = value;
    }
    public void ChangeMaxGrenades(float value)
    {
        OptionsManager.instance.grenades.value = value;
    }
    public void ChangeSpeed(float value)
    {
        OptionsManager.instance.speed.value = value;
    }
    public void ChangeTurnLength(float value)
    {
        OptionsManager.instance.turnLength.value = value;
    }

}
