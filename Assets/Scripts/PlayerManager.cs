using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{

    private PlayerInputController playerInputController;
    [SerializeField] private float _maxFuel;
    [SerializeField] private int maxGrenades;
    [SerializeField] private int maxJumps;
    [SerializeField] private int numPlayers = 1;
    [SerializeField] private GameObject player;
    private List<Player> players = new List<Player>();
    private int activePlayer;
    public Player currentPlayer
    {
        get
        {
            return players[activePlayer];
        }
    }

    public float maxFuel
    {
        get
        {
            return _maxFuel;
        }
    }


    private void Start()
    {
        playerInputController = gameObject.GetComponent<PlayerInputController>();
        for (int i = 0; i < numPlayers; i++)
        {
            // Debug.LogError(i);
            Vector3 position = transform.position;
            position.x += Random.Range(-10f, 10f);
            position.z += Random.Range(-10f, 10f);

            GameObject newPlayer = Instantiate(player, position, transform.rotation, transform);
            newPlayer.name = "Player " + (i + 1);
            players.Add(newPlayer.GetComponent<Player>());
        }
        activePlayer = 0;
        ActivatePlayer(currentPlayer);
    }

    private void Update()
    {
        SwitchPlayer();
    }

    private void SwitchPlayer()
    {
        if (playerInputController.swap)
        {
            DeactivatePlayer(currentPlayer);
            if (activePlayer >= players.Count - 1)
            {
                activePlayer = 0;
            }
            else
            {
                activePlayer++;
            }
            ActivatePlayer(currentPlayer);
        }
        playerInputController.swap = false;
    }

    private void ActivatePlayer(Player player)
    {
        player.cinemachineCamera.enabled = true;
        player.cameraFollow.transform.rotation = Quaternion.Euler(playerInputController.cameraTargetPitch);
        player.fuel = maxFuel;
        player.grenades = maxGrenades;
        player.jumps = maxJumps;
    }
    private void DeactivatePlayer(Player player)
    {
        player.cinemachineCamera.enabled = false;
    }

}
