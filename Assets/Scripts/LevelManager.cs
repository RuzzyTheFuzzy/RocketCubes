using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance { get; private set; }
    [SerializeField] private Transform _spawns;
    [SerializeField] private Transform _level;
    [SerializeField] private CinemachineVirtualCamera _roundCamera;
    public Transform spawns => _spawns;
    public Transform level => _level;
    public CinemachineVirtualCamera roundCamera => _roundCamera;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
            return;
        }

        instance = this;
    }

    private void Start()
    {
        if (GameManager.instance != null)
            GameManager.instance.StartGame();
    }
}
