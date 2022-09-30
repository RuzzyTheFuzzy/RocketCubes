using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class TurnManager : MonoBehaviour
{

    [SerializeField] private float roundTransitionTime = 10f;
    [SerializeField] private float cameraOrbitSpeed = 0.01f;
    [SerializeField] private float roundScaler;
    [SerializeField] private float turnAdd;
    private CinemachineOrbitalTransposer cameraOrbitTransposer;
    private float transitionTimer;
    private CinemachineVirtualCamera roundCamera;
    private Transform level;
    private float turnLenght;
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
        turnLenght = Options.TurnLength.value;
        level = LevelManager.Instance.Level;
        roundCamera = LevelManager.Instance.RoundCamera;
        cameraOrbitTransposer = roundCamera.GetCinemachineComponent<CinemachineOrbitalTransposer>();
        turn = 0;
        round = 1;
        timer = 0;
        startTransition = true;
        GameManager.gameState = GameState.Transition;
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
            if (newTurn || GameManager.PlayerInputController.swap)
            {
                GameManager.PlayerManager.SwitchPlayer();
                GameManager.PlayerInputController.swap = false;
            }
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
        GameManager.CharacterController.enabled = false;
        GameManager.UIManager.PlayerOverlay(false);
        GameManager.UIManager.RoundOverlay(round);
        GameManager.gameState = GameState.Transition;
    }

    private void RoundTransition()
    {
        timer += Time.deltaTime;

        if (timer >= roundTransitionTime)
        {
            GameManager.CharacterController.enabled = true;
            roundCamera.enabled = false;
            roundTransition = false;
            GameManager.UIManager.PlayerOverlay(true);
            GameManager.UIManager.RoundOverlay();
            GameManager.PlayerInputController.swap = false;
            GameManager.gameState = GameState.Game;
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
            GameManager.CharacterController.enabled = false;
            GameManager.UIManager.PlayerOverlay(false);
            GameManager.UIManager.RoundOverlay(round);
        }
        timer += Time.deltaTime;

        if (timer >= roundTransitionTime)
        {
            startTransition = false;
            GameManager.CharacterController.enabled = true;
            roundCamera.enabled = false;
            GameManager.UIManager.PlayerOverlay(true);
            GameManager.UIManager.RoundOverlay();
            GameManager.PlayerInputController.swap = false;
            GameManager.gameState = GameState.Game;
            NextTurn();
        }
        else
        {
            cameraOrbitTransposer.m_XAxis.m_InputAxisValue = cameraOrbitSpeed;
        }
    }
}
