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
    private bool _paused = false;

    public bool paused
    {
        get
        {
            return _paused;
        }
    }


    public void Pause()
    {
        if (_paused)
        {
            pauseMenu.SetActive(false);
            _paused = false;
            Time.timeScale = 1;
        }
        else
        {
            firstSelected.Select();
            pauseMenu.SetActive(true);
            _paused = true;
            Time.timeScale = 0;
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
