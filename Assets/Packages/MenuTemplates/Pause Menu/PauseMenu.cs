using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private Button firstSelected;
    [SerializeField] private int menuSceneInt = 0;
    private bool paused = false;

    public void Pause()
    {
        if (paused)
        {
            firstSelected.Select();
            pauseMenu.SetActive(true);
            paused = true;
            Time.timeScale = 0;
        }
        else
        {
            pauseMenu.SetActive(false);
            paused = false;
            Time.timeScale = 1;
        }
    }
    public void Menu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(menuSceneInt);
    }
    public void Quit()
    {
        Application.Quit();
        Debug.Log("Quit Button Pressed");
    }

}
