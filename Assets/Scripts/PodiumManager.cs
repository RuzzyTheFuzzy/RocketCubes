using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PodiumManager : MonoBehaviour
{

    [SerializeField] private MeshRenderer[] winners;
    [SerializeField] private float podiumTime;
    private float timer = 0;
    private Color[] colors;

    private void Awake()
    {
        colors = GameManager.instance.playerManager.winningColors;
        for (int i = 0; i < winners.Length; i++)
        {
            if (colors[i] != new Color())
            {
                winners[i].material.color = colors[i];
            }
            else
            {
                Destroy(winners[i].gameObject);
            }
        }
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= podiumTime)
        {
            GameManager.instance.gameState = GameManager.GameState.Menu;
            GameManager.instance.EndGame();
            timer = float.NegativeInfinity;
        }
    }


}
