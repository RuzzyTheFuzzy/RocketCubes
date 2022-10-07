using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Options : MonoBehaviour
{
    // Singleton for extra easy access.
    public static Options Instance { get; private set; }
    [SerializeField] private OptionsValue maxFuel;
    public static OptionsValue MaxFuel => Instance.maxFuel;
    // Read only for the object, the value instide is still public
    [SerializeField] private OptionsValue players;
    public static OptionsValue Players => Instance.players;
    [SerializeField] private OptionsValue grenades;
    public static OptionsValue Grenades => Instance.grenades;
    [SerializeField] private OptionsValue speed;
    public static OptionsValue Speed => Instance.speed;
    [SerializeField] private OptionsValue turnLength;
    public static OptionsValue TurnLength => Instance.turnLength;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        Instance = this;
    }
}
