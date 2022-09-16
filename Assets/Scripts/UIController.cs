using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{

    [SerializeField] private PauseMenu pauseMenu;
    [SerializeField] private PlayerInputController playerInputController;
    [SerializeField] private PlayerManager playerManager;
    [SerializeField] private FuelUI fuelUI;
    [SerializeField] private JumpUI jumpUI;
    [SerializeField] private GrenadeUI grenadeUI;
    private Player player
    {
        get
        {
            return playerManager.currentPlayer;
        }
    }

    private void Update()
    {
        if (playerInputController.pause)
        {
            pauseMenu.Pause();
        }
        playerInputController.pause = false;
    }

    private void LateUpdate()
    {
        fuelUI.FuelUIUpdate(player.fuel, playerManager.maxFuel);
        grenadeUI.GrenadeUIUpdate(player.grenades);
        jumpUI.JumpUIUpdate(player.jumps > 0);
    }

}
