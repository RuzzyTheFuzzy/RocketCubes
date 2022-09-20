using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{

    public PlayerInputController playerInputController { get; private set; }
    private float transitionTimer;
    [SerializeField] private float _maxFuel;
    [SerializeField] private int maxGrenades;
    [SerializeField] private int maxJumps;
    [SerializeField] private int numPlayers = 1;
    [SerializeField] private Transform spawns;
    [SerializeField] private GameObject player;
    private Transform[] spawnPoints;
    private List<Player> players = new List<Player>();
    private int activePlayer;
    public Player currentPlayer { get; private set; }

    public float maxFuel
    {
        get
        {
            return _maxFuel;
        }
    }


    private void Start()
    {
        spawnPoints = spawns.GetComponentsInChildren<Transform>();
        playerInputController = gameObject.GetComponent<PlayerInputController>();
        for (int i = 0; i < numPlayers; i++)
        {
            GameObject newPlayer = Instantiate(player, RandomSpawnpoint(), transform.rotation, transform);
            newPlayer.name = "Player " + (i + 1);
            Player newPlayerComponent = newPlayer.GetComponent<Player>();
            newPlayerComponent.id = i;
            Material newMaterial = newPlayer.GetComponentInChildren<MeshRenderer>().material;
            newMaterial.color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1f);
            players.Add(newPlayerComponent);
        }
        activePlayer = 0;
        currentPlayer = players[activePlayer];
        ActivatePlayer(currentPlayer);
    }

    private void Update()
    {
        if (transitionTimer > 0)
        {
            transitionTimer -= Time.deltaTime;
            if ((transitionTimer) <= 0)
            {
                SwitchPlayer();
            }
        }
        else
        {
            transitionTimer = ShouldSwitch();
        }
    }

    private float ShouldSwitch()
    {
        if (GameManager.instance.turnManager.newTurn)
            return GameManager.instance.turnManager.turnTransitionTime;
        if (playerInputController.swap)
        {
            playerInputController.swap = false;
            return 0.1f;
        }
        return 0f;
    }

    private void SwitchPlayer()
    {
        if (players.Count <= 1)
        {
            Win();
        }
        DeactivatePlayer(currentPlayer);
        if (activePlayer >= players.Count - 1)
        {
            activePlayer = 0;
            GameManager.instance.turnManager.NextRound();
        }
        else
        {
            activePlayer++;
            GameManager.instance.turnManager.NextTurn();
        }
        currentPlayer = players[activePlayer];
        ActivatePlayer(currentPlayer);
    }

    private void Win()
    {
        Debug.Log(players[0].name + " WINS!!!");
    }

    private void ActivatePlayer(Player player)
    {
        player.cinemachineCamera.enabled = true;
        player.cameraFollow.transform.rotation = Quaternion.Euler(playerInputController.cameraTargetPitch);
        player.fuel = player.maxFuel;
        player.grenades = maxGrenades;
        player.jumps = maxJumps;
    }
    private void DeactivatePlayer(Player player)
    {
        player.cinemachineCamera.enabled = false;
    }

    public void Death(int id)
    {
        Destroy(players[id].gameObject);
        players.RemoveAt(id);
        UpdatePlayers();

        if (id == activePlayer)
        {
            activePlayer--;
            SwitchPlayer();
        }
        else
        {
            activePlayer = currentPlayer.id;
        }
    }

    private void UpdatePlayers()
    {
        for (int i = 0; i < players.Count; i++)
        {
            players[i].id = i;
        }
    }

    public Vector3 RandomSpawnpoint()
    {
        int index = Random.Range(0, spawnPoints.Length);
        return spawnPoints[index].position;
    }

}
