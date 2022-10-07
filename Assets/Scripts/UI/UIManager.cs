using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private FuelUI fuelUI;
    [SerializeField] private GrenadeUI grenadeUI;
    [SerializeField] private HpUI hpUI;
    [SerializeField] private TimerUI timerUI;
    [SerializeField] private DeathMessages deathMessages;
    [SerializeField] private Canvas playerOverlay;
    [SerializeField] private Canvas roundOverlay;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private Button firstSelected;
    public bool Paused { get; private set; } = false;


    private Player Player
    {
        get
        {
            return GameManager.PlayerManager.CurrentPlayer;
        }
    }

    public void NewGame()
    {
        pauseMenu.SetActive(false);
    }

    public void StopGame()
    {
        pauseMenu.SetActive(false);
        playerOverlay.enabled = false;
        roundOverlay.enabled = false;
    }

    private void LateUpdate()
    {
        // Dont wanna run them during the menu
        if (GameManager.gameState != GameState.Menu)
        {
            fuelUI.FuelUIUpdate(Player.fuel, Player.MaxFuel);
            grenadeUI.GrenadeUIUpdate(Player.grenades, Player.Anti);
            hpUI.HpUIUpdate(Player.Health);
            timerUI.TimerUIUpdate(GameManager.TurnManager.timeRemaining);

            if (GameManager.PlayerInputController.pause)
            {
                Pause();
            }
        }
        GameManager.PlayerInputController.pause = false;
    }

    public void OverlayText(string text = "")
    {
        if (text == "") // If its empty, no need to show it
        {
            roundOverlay.enabled = false;
        }
        else
        {
            roundOverlay.GetComponentInChildren<TMP_Text>().text = text;
            roundOverlay.enabled = true;
        }
    }

    public void PlayerOverlay(bool active)
    {
        playerOverlay.enabled = active;
    }

    public void Pause()
    {
        if (Paused)
        {
            roundOverlay.enabled = (GameManager.gameState == GameState.Transition);
            playerOverlay.enabled = (GameManager.gameState == GameState.Game);
            pauseMenu.SetActive(false);
            Paused = false;
            Time.timeScale = 1;
            GameManager.PlayerInputController.pause = false;
            GameManager.CharacterController.enabled = true;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            roundOverlay.enabled = false;
            playerOverlay.enabled = false;
            firstSelected.Select();
            pauseMenu.SetActive(true);
            Paused = true;
            Time.timeScale = 0;
            GameManager.CharacterController.enabled = false;

            // Let the mouse leave the screen when we are paused (ESC always unlocks the mouse while in the editor)
            Cursor.lockState = CursorLockMode.None;
        }
    }

    public void Menu()
    {
        Pause();
        GameManager.Instance.EndGame();
    }

    public void Quit()
    {
        Application.Quit();
        Debug.Log("Quit Button Pressed");
    }

    public void DeathMessage(Player player)
    {
        deathMessages.NewMessage(player);
    }
}
