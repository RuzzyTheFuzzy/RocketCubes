using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        Menu,
        Game,
        Win,
        Transition
    }

    public static GameManager instance { get; private set; }
    public PlayerManager playerManager { get; private set; }
    public WeaponManager weaponManager { get; private set; }
    public UIManager uIManager { get; private set; }
    public PlayerInputController playerInputController { get; private set; }
    public CharacterController characterController { get; private set; }
    public TurnManager turnManager { get; private set; }
    public GameState gameState;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
            return;
        }

        instance = this;
        gameState = GameState.Menu;


        playerManager = GetComponentInChildren<PlayerManager>();
        turnManager = GetComponentInChildren<TurnManager>();
        weaponManager = GetComponentInChildren<WeaponManager>();
        uIManager = GetComponentInChildren<UIManager>();
        playerInputController = GetComponentInChildren<PlayerInputController>();
        characterController = GetComponentInChildren<CharacterController>();

        DontDestroyOnLoad(instance.gameObject);
    }

    public void StartGame()
    {
        gameState = GameState.Game;
        playerManager.NewGame();
        turnManager.NewGame();
        weaponManager.NewGame();
        uIManager.NewGame();
        playerManager.enabled = true;
        turnManager.enabled = true;
        uIManager.enabled = true;
        characterController.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void EndGame()
    {
        playerManager.StopGame();
        turnManager.StopGame();
        weaponManager.StopGame();
        uIManager.StopGame();
        playerManager.enabled = false;
        turnManager.enabled = false;
        uIManager.enabled = false;
        characterController.enabled = false;
        Cursor.lockState = CursorLockMode.None;
        if (gameState == GameState.Win)
        {
            SceneManager.LoadScene("Win");
        }
        else
        {
            gameState = GameState.Menu;
            SceneManager.LoadScene("Main Menu");
        }
    }

    public void Win()
    {
        gameState = GameState.Win;
        playerManager.Win();
        Debug.Log(playerManager.currentPlayer.name + " WINS!!!");
        EndGame();
    }

}
