using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class LevelManager : MonoBehaviour
{
    // Storing all the important level objects somewhere accessible.
    public static LevelManager Instance { get; private set; }
    [SerializeField] private Transform spawns;
    [SerializeField] private Transform level;
    [SerializeField] private CinemachineVirtualCamera roundCamera;
    public Transform Spawns => spawns;
    public Transform Level => level;
    public CinemachineVirtualCamera RoundCamera => roundCamera;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        // Loads in when we switch into the level, so we start the game.
        if (GameManager.Instance != null)
        {
            GameManager.Instance.StartGame();
        }
    }
}
