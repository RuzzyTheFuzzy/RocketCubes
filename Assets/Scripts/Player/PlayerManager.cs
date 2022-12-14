using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private List<Color> playerColors;
    private Transform spawns;
    private Transform[] spawnPoints;
    private List<Player> players = new List<Player>();
    private int activePlayer;
    private int maxGrenades;
    public Player CurrentPlayer { get; private set; }
    public Color[] WinningColors { get; private set; }

    public float MaxFuel { get; private set; }
    public int NumPlayers { get; private set; }


    public void NewGame()
    {
        List<Color> colors = new List<Color>(playerColors);

        // Yoink values from options
        MaxFuel = Options.MaxFuel.value;
        NumPlayers = (int)Options.Players.value;
        maxGrenades = (int)Options.Grenades.value;

        WinningColors = new Color[4];

        spawns = LevelManager.Instance.Spawns;
        spawnPoints = spawns.GetComponentsInChildren<Transform>();

        for (int i = 0; i < NumPlayers; i++)
        {
            GameObject newPlayer = Instantiate(player, RandomSpawnpoint(), transform.rotation);
            newPlayer.name = "Player " + (i + 1);
            Player newPlayerComponent = newPlayer.GetComponent<Player>();
            newPlayerComponent.id = i;
            newPlayerComponent.speed = Options.Speed.value;
            Material newMaterial = newPlayer.GetComponentInChildren<MeshRenderer>().material;
            newMaterial.color = colors[0];
            colors.RemoveAt(0);
            players.Add(newPlayerComponent);
        }
        activePlayer = 0;
        CurrentPlayer = players[activePlayer];
        ActivatePlayer(CurrentPlayer);
    }
    public void StopGame()
    {
        spawns = null;
        players = new List<Player>();
        CurrentPlayer = null;
    }

    private void Update()
    {
        if (players.Count <= 1)
        {
            GameManager.Instance.Win();
        }

        if (GameManager.PlayerInputController.randomKill)
        {
            // Debug command for fast games
            RandomKill();
        }
        GameManager.PlayerInputController.randomKill = false;
    }

    public void SwitchPlayer()
    {
        DeactivatePlayer(CurrentPlayer);

        bool newRound = false;

        if (activePlayer >= players.Count - 1)
        {
            activePlayer = 0;
            newRound = true; // If we wrapped around, new round
        }
        else
        {
            activePlayer++;
        }

        CurrentPlayer = players[activePlayer];
        ActivatePlayer(CurrentPlayer);

        if (newRound)
        {
            GameManager.TurnManager.NextRound();
        }
        else
        {
            GameManager.TurnManager.NextTurn();
        }
    }

    private void ActivatePlayer(Player player)
    {
        player.cinemachineCamera.enabled = true;
        player.cameraFollow.transform.rotation =
        Quaternion.Euler(GameManager.PlayerInputController.cameraTargetPitch); // Set camera rotation for smooth transitions

        player.fuel = player.MaxFuel;
        player.grenades = maxGrenades;
    }
    private void DeactivatePlayer(Player player)
    {
        player.cinemachineCamera.enabled = false;
    }

    public void Death(int id)
    {
        WinningColors[players.Count - 1] = players[id].GetComponentInChildren<MeshRenderer>().material.color;
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
            // Someone else died so who cares, just update the activePlayer
            activePlayer = CurrentPlayer.id;
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
        WinningColors[0] = players[0].GetComponentInChildren<MeshRenderer>().material.color;
    }

    private void RandomKill()
    {
        Player randomPlayer = players[Random.Range(0, players.Count)];
        randomPlayer.Hit();
    }
}
