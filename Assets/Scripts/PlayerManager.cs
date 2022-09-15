using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{

    private PlayerInputController playerInputController;
    [SerializeField] private int numPlayers = 1;
    [SerializeField] private GameObject player;
    private List<CharacterController> players = new List<CharacterController>();
    private int activePlayer;


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
            players.Add(newPlayer.GetComponent<CharacterController>());
        }

        foreach (CharacterController player in players)
        {
            player.enabled = false;
        }
        players[0].enabled = true;
        activePlayer = 0;
    }

    private void Update()
    {
        SwitchPlayer();
    }

    private void SwitchPlayer()
    {
        if (playerInputController.swap)
        {
            players[activePlayer].enabled = false;
            if (activePlayer >= players.Count - 1)
            {
                activePlayer = 0;
            }
            else
            {
                activePlayer++;
            }
            players[activePlayer].enabled = true;
        }
        playerInputController.swap = false;
    }

}
