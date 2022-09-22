using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class TurnManager : MonoBehaviour
{

    [SerializeField] private float turnLenght;
    [SerializeField] private float roundTransitionTime = 10f;
    [SerializeField] private float cameraOrbitSpeed = 0.01f;
    [SerializeField] private float roundScaler;
    [SerializeField] private float turnAdd;
    private CinemachineOrbitalTransposer cameraOrbitTransposer;
    private float transitionTimer;
    private CinemachineVirtualCamera roundCamera;
    private Transform level;
    private float turnTime;
    private Vector3 size;
    private int round;
    private int turn;
    private float timer;
    private bool roundTransition = false;
    private bool startTransition = false;
    public float turnTransitionTime { get; private set; } = 2f;

    public float timeRemaining
    {
        get
        {
            return turnLenght - turnTime;
        }
    }

    public bool newTurn
    {
        get
        {
            return turnTime >= turnLenght;
        }
    }

    public void NewGame()
    {
        level = LevelManager.instance.level;
        roundCamera = LevelManager.instance.roundCamera;
        cameraOrbitTransposer = roundCamera.GetCinemachineComponent<CinemachineOrbitalTransposer>();
        turn = 0;
        round = 1;
        timer = 0;
        startTransition = true;
        GameManager.instance.gameState = GameManager.GameState.Transition;
    }

    public void StopGame()
    {
        level = null;
        roundCamera = null;
        cameraOrbitTransposer = null;
        turn = 0;
        round = 0;
        timer = 0;
    }

    private void Update()
    {
        if (startTransition)
        {
            StartTransition();
        }
        else if (roundTransition)
        {
            RoundTransition();
        }
        else
        {
            turnTime += Time.deltaTime;
            if (newTurn || GameManager.instance.playerInputController.swap)
            {
                GameManager.instance.playerManager.SwitchPlayer();
                GameManager.instance.playerInputController.swap = false;
            }
        }
        if (GameManager.instance.playerManager.playersLeft <= 1)
        {
            GameManager.instance.Win();
        }
    }

    public void NextTurn()
    {
        turn++;
        turnTime = 0;
    }

    public void NextRound()
    {
        round++;
        timer = 0;
        size = level.localScale;
        roundTransition = true;
        roundCamera.enabled = true;
        GameManager.instance.characterController.enabled = false;
        GameManager.instance.uIManager.PlayerOverlay(false);
        GameManager.instance.uIManager.RoundOverlay(round);
        GameManager.instance.gameState = GameManager.GameState.Transition;
    }

    private void RoundTransition()
    {
        timer += Time.deltaTime;

        if (timer >= roundTransitionTime)
        {
            GameManager.instance.characterController.enabled = true;
            roundCamera.enabled = false;
            roundTransition = false;
            GameManager.instance.uIManager.PlayerOverlay(true);
            GameManager.instance.uIManager.RoundOverlay();
            GameManager.instance.playerInputController.swap = false;
            GameManager.instance.gameState = GameManager.GameState.Game;
            NextTurn();
        }
        else
        {
            cameraOrbitTransposer.m_XAxis.m_InputAxisValue = cameraOrbitSpeed;
            float boatScale = Mathf.Lerp(1f, roundScaler, timer / roundTransitionTime);
            if (boatScale > 0.01)
            {
                level.localScale = new Vector3(size.x * boatScale, size.y, size.z * boatScale);
            }
            else
            {
                level.gameObject.SetActive(false);
            }
        }
    }

    private void StartTransition()
    {
        if (timer <= 0)
        {
            roundCamera.enabled = true;
            GameManager.instance.characterController.enabled = false;
            GameManager.instance.uIManager.PlayerOverlay(false);
            GameManager.instance.uIManager.RoundOverlay(round);
        }
        timer += Time.deltaTime;

        if (timer >= roundTransitionTime)
        {
            startTransition = false;
            GameManager.instance.characterController.enabled = true;
            roundCamera.enabled = false;
            GameManager.instance.uIManager.PlayerOverlay(true);
            GameManager.instance.uIManager.RoundOverlay();
            GameManager.instance.playerInputController.swap = false;
            Debug.Log(turn);
            GameManager.instance.gameState = GameManager.GameState.Game;
            NextTurn();
        }
        else
        {
            cameraOrbitTransposer.m_XAxis.m_InputAxisValue = cameraOrbitSpeed;
        }
    }
}
