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
    [SerializeField] private GameObject playerOverlay;
    [SerializeField] private GameObject roundOverlay;
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
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }

    }

    public void StopGame()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }

    }

    private void LateUpdate()
    {
        // Only want to run them during Gameplay
        if (GameManager.gameState == GameState.Game)
        {
            fuelUI.FuelUIUpdate(Player.fuel, Player.maxFuel);
            grenadeUI.GrenadeUIUpdate(Player.grenades, Player.anti);
            hpUI.HpUIUpdate(Player.health);
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
        if (text == "")
        {
            roundOverlay.SetActive(false);
        }
        else
        {
            roundOverlay.GetComponentInChildren<TMP_Text>().text = text;
            roundOverlay.SetActive(true);
        }
    }

    public void PlayerOverlay(bool active)
    {
        playerOverlay.SetActive(active);
    }

    public void Pause()
    {
        if (Paused)
        {
            roundOverlay.SetActive(GameManager.gameState == GameState.Transition);
            playerOverlay.SetActive(GameManager.gameState == GameState.Game);
            pauseMenu.SetActive(false);
            Paused = false;
            Time.timeScale = 1;
            GameManager.PlayerInputController.pause = false;
            GameManager.CharacterController.enabled = true;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            roundOverlay.SetActive(false);
            playerOverlay.SetActive(false);
            firstSelected.Select();
            pauseMenu.SetActive(true);
            Paused = true;
            Time.timeScale = 0;
            GameManager.CharacterController.enabled = false;
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
}
