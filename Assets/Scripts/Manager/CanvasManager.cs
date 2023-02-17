using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{




    public GameObject PauseMenu;

    public Character C;


    public void PlayGame()
    {
        SceneManager.LoadScene("level");
    }

    public void QuitGame()
    {
        Debug.Log("Quit!");
        Application.Quit();
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("main menu");
    }

    public void Win()
    {
        SceneManager.LoadScene("Winner");
    }

    public void GameOver()
    {
        SceneManager.LoadScene("Gameover");
    }

    

    public void unPause()
    {
        PauseMenu.SetActive(false);
        Time.timeScale = 1.0f;
        C.enabled = true;
    }
}
