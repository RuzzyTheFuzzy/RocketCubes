using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }
    public PlayerManager playerManager { get; private set; }
    public WeaponManager weaponManager { get; private set; }
    public UIManager uIManager { get; private set; }
    public PlayerInputController playerInputController { get; private set; }
    public CharacterController characterController { get; private set; }
    public TurnManager turnManager { get; private set; }
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
            return;
        }

        instance = this;
        uIManager = GetComponentInChildren<UIManager>();
        playerManager = GetComponentInChildren<PlayerManager>();
        weaponManager = GetComponentInChildren<WeaponManager>();
        playerInputController = GetComponentInChildren<PlayerInputController>();
        characterController = GetComponentInChildren<CharacterController>();
        turnManager = GetComponentInChildren<TurnManager>();
    }
}
