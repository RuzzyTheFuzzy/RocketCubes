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
    [SerializeField] private Transform bowl;
    [SerializeField] private CinemachineVirtualCamera roundCamera;
    private float turnTime;
    private Vector3 size;
    private int round = 1;
    private int turn = 1;
    public bool roundTransition { get; private set; } = false;
    private float timer = 0;
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

    private void Update()
    {
        if (roundTransition)
        {
            RoundTransition();
        }
        else
        {
            turnTime += Time.deltaTime;
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
        size = bowl.localScale;
        roundTransition = true;
        roundCamera.enabled = true;
        GameManager.instance.characterController.enabled = false;
        GameManager.instance.uIManager.PlayerOverlay(false);
        GameManager.instance.uIManager.RoundOverlay(round);
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
            NextTurn();
        }
        else
        {
            roundCamera.GetCinemachineComponent<CinemachineOrbitalTransposer>().m_XAxis.m_InputAxisValue = cameraOrbitSpeed;
            float boatScale = Mathf.Lerp(1f, roundScaler, timer / roundTransitionTime);
            if (boatScale > 0.01)
            {
                bowl.localScale = new Vector3(size.x * boatScale, size.y, size.z * boatScale);
            }
            else
            {
                bowl.gameObject.SetActive(false);
            }
        }
    }
}
