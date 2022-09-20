using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private FuelUI fuelUI;
    [SerializeField] private JumpUI jumpUI;
    [SerializeField] private GrenadeUI grenadeUI;
    [SerializeField] private PowerUI powerUI;
    [SerializeField] private HpUI hpUI;
    [SerializeField] private TimerUI timerUI;
    [SerializeField] private GameObject playerOverlay;
    [SerializeField] private GameObject roundOverlay;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private Button firstSelected;
    [SerializeField] private int menuSceneInt = 0;
    private bool _paused = false;

    public bool paused
    {
        get
        {
            return _paused;
        }
    }

    private Player player
    {
        get
        {
            return GameManager.instance.playerManager.currentPlayer;
        }
    }

    private void Update()
    {

    }

    private void LateUpdate()
    {
        fuelUI.FuelUIUpdate(player.fuel, player.maxFuel);
        grenadeUI.GrenadeUIUpdate(player.grenades);
        jumpUI.JumpUIUpdate(player.jumps > 0);
        powerUI.PowerUIUpdate(player.power, GameManager.instance.characterController.maxPower);
        hpUI.HpUIUpdate(player.health);
        timerUI.TimerUIUpdate(GameManager.instance.turnManager.timeRemaining);

        if (GameManager.instance.playerInputController.pause)
        {
            Pause();
        }
        GameManager.instance.playerInputController.pause = false;
    }

    public void RoundOverlay(int round = -1)
    {
        if (round == -1)
        {
            roundOverlay.SetActive(false);
        }
        else
        {
            roundOverlay.GetComponentInChildren<TMP_Text>().text = "Round " + round;
            roundOverlay.SetActive(true);
        }
    }

    public void PlayerOverlay(bool active)
    {
        playerOverlay.SetActive(active);
    }

    public void Pause()
    {
        if (_paused)
        {
            roundOverlay.SetActive(GameManager.instance.turnManager.roundTransition);
            playerOverlay.SetActive(!GameManager.instance.turnManager.roundTransition);
            pauseMenu.SetActive(false);
            _paused = false;
            Time.timeScale = 1;
            GameManager.instance.playerInputController.pause = false;
            GameManager.instance.characterController.enabled = true;
        }
        else
        {
            roundOverlay.SetActive(false);
            playerOverlay.SetActive(false);
            firstSelected.Select();
            pauseMenu.SetActive(true);
            _paused = true;
            Time.timeScale = 0;
            GameManager.instance.characterController.enabled = false;
        }
    }
    public void Menu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(menuSceneInt);
    }
    public void Quit()
    {
        Application.Quit();
        Debug.Log("Quit Button Pressed");
    }
}
