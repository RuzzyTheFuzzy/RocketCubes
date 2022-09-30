using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public enum GameState
{
    Menu,
    Game,
    Win,
    Transition
}

public class GameManager : MonoBehaviour
{

    public static GameManager Instance { get; private set; }
    public static PlayerManager PlayerManager { get; private set; }
    public static WeaponManager WeaponManager { get; private set; }
    public static UIManager UIManager { get; private set; }
    public static PlayerInputController PlayerInputController { get; private set; }
    public static CharacterController CharacterController { get; private set; }
    public static TurnManager TurnManager { get; private set; }
    public static GameState gameState;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        Instance = this;
        gameState = GameState.Menu;


        PlayerManager = GetComponentInChildren<PlayerManager>();
        TurnManager = GetComponentInChildren<TurnManager>();
        WeaponManager = GetComponentInChildren<WeaponManager>();
        UIManager = GetComponentInChildren<UIManager>();
        PlayerInputController = GetComponentInChildren<PlayerInputController>();
        CharacterController = GetComponentInChildren<CharacterController>();

        DontDestroyOnLoad(Instance.gameObject);
    }

    public void StartGame()
    {
        gameState = GameState.Game;
        PlayerManager.NewGame();
        TurnManager.NewGame();
        WeaponManager.NewGame();
        UIManager.NewGame();
        PlayerManager.enabled = true;
        TurnManager.enabled = true;
        UIManager.enabled = true;
        CharacterController.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void EndGame()
    {
        PlayerManager.StopGame();
        TurnManager.StopGame();
        WeaponManager.StopGame();
        UIManager.StopGame();
        PlayerManager.enabled = false;
        TurnManager.enabled = false;
        UIManager.enabled = false;
        CharacterController.enabled = false;
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
        PlayerManager.Win();
        Debug.Log(PlayerManager.CurrentPlayer.name + " WINS!!!");
        EndGame();
    }

}
