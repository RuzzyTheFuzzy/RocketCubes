using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Which scene to go to
    [SerializeField] private int scene = 1;

    public void PlayGame()
    {
        SceneManager.LoadScene(scene);
    }

    public void QuitGame()
    {
        // Quit dosent work in editor, so lovely log tells us when it happends
        Debug.Log("Quit Button Pressed");
        Application.Quit();
    }

}
