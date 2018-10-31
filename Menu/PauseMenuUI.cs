using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuUI : MonoBehaviour {

    public static bool isGamePaused = false;
    public GameObject pauseMenuUI;
    public GameObject optionsMenu;
    private float pauseTime;
    public GameObject goScreen;
    public GameObject winScreen;

    public GameObject hud;
	
	// Update is called once per frame
	void Update () {
        if (!SceneManager.GetSceneByName("MainMenu").isLoaded)
        {
            if (Input.GetKeyDown(KeyCode.Escape) && pauseMenuUI.activeSelf == false && goScreen.activeSelf == false && winScreen.activeSelf == false && optionsMenu.activeSelf == false)
            {
                if (isGamePaused /*&& optionsMenu.activeSelf == false*/)
                {
                    Resume();
                }
                else
                {
                    Pause();
                }
            }
        }
	}

    public void Resume()
    {
        isGamePaused = false;
        pauseMenuUI.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1f;
        hud.SetActive(true);
    }

    public void Pause()
    {
        Cursor.visible = true;
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isGamePaused = true;
        hud.SetActive(false);
    }

    /*public void LoadOptions()
    {
        pauseMenuUI.SetActive(false);
        optionsMenu.SetActive(true);
    }*/

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;        //temp fix
        pauseMenuUI.SetActive(false);   //temp fix
        isGamePaused = false;
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
