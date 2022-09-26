using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private int maxJumps;
    [SerializeField] private GameObject player;
    private Transform spawns;
    private Transform[] spawnPoints;
    private List<Player> players = new List<Player>();
    private int activePlayer;
    public Player currentPlayer { get; private set; }
    public Color[] winningColors { get; private set; }

    public float maxFuel { get; private set; }
    private int maxGrenades;
    public int numPlayers { get; private set; }


    public void NewGame()
    {
        maxFuel = OptionsManager.instance.maxFuel.value;
        numPlayers = (int)OptionsManager.instance.players.value;
        maxGrenades = (int)OptionsManager.instance.grenades.value;
        winningColors = new Color[4];
        spawns = LevelManager.instance.spawns;
        spawnPoints = spawns.GetComponentsInChildren<Transform>();
        for (int i = 0; i < numPlayers; i++)
        {
            GameObject newPlayer = Instantiate(player, RandomSpawnpoint(), transform.rotation);
            newPlayer.name = "Player " + (i + 1);
            Player newPlayerComponent = newPlayer.GetComponent<Player>();
            newPlayerComponent.id = i;
            newPlayerComponent.speed = OptionsManager.instance.speed.value;
            Material newMaterial = newPlayer.GetComponentInChildren<MeshRenderer>().material;
            newMaterial.color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1f);
            players.Add(newPlayerComponent);
        }
        activePlayer = 0;
        currentPlayer = players[activePlayer];
        ActivatePlayer(currentPlayer);
    }
    public void StopGame()
    {
        spawns = null;
        players = new List<Player>();
        currentPlayer = null;
    }

    private void Update()
    {
        if (players.Count <= 1)
        {
            GameManager.instance.Win();
        }

        if (GameManager.instance.playerInputController.randomKill)
        {
            RandomKill();
        }
        GameManager.instance.playerInputController.randomKill = false;
    }

    public void SwitchPlayer()
    {
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

    private void ActivatePlayer(Player player)
    {
        player.cinemachineCamera.enabled = true;
        player.cameraFollow.transform.rotation = Quaternion.Euler(GameManager.instance.playerInputController.cameraTargetPitch);
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
        winningColors[players.Count - 1] = players[id].GetComponentInChildren<MeshRenderer>().material.color;
        Destroy(players[id].gameObject);
        players.RemoveAt(id);
        UpdatePlayers();

        if (id == activePlayer)
        {
            // The list collapses so the next player is the same as switching from the previous, even if its outside of players array
            activePlayer--;
            SwitchPlayer();
        }
        else
        {
            // Someone else died so who cares just update the activePlayer
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
        // The parent empty will also be included at index 0, so just skip it
        int index = Random.Range(1, spawnPoints.Length);
        return spawnPoints[index].position;
    }

    public void AllChangeMaxFuel(float fuel)
    {
        foreach (Player player in players)
        {
            player.ChangeMaxFuel(fuel);
        }
    }

    public void Win()
    {
        winningColors[0] = players[0].GetComponentInChildren<MeshRenderer>().material.color;
    }

    private void RandomKill()
    {
        Player randomPlayer = players[Random.Range(0, players.Count)];
        randomPlayer.Hit();
    }
}
