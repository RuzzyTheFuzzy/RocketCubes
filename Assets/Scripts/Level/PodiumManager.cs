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
        colors = GameManager.PlayerManager.WinningColors;
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
            GameManager.gameState = GameState.Menu;
            GameManager.Instance.EndGame();
            // Negative infinity will probably not be reached before we switch scenes
            timer = float.NegativeInfinity;
        }
    }


}
