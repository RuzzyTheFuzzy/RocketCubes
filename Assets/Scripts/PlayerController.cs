using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private PlayerInputController playerInputController;
    [SerializeField] private Player player1;
    [SerializeField] private Player player2;
    [SerializeField] private Player player3;
    [SerializeField] private Player player4;
    private Player[] players;
    private int activePlayer;


    private void Start()
    {
        playerInputController = gameObject.GetComponent<PlayerInputController>();
        players = new Player[] { player1, player2, player3, player4 };
        foreach (Player player in players)
        {
            player.enabled = false;
        }
        player1.enabled = true;
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
            if (activePlayer >= players.Length - 1)
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
