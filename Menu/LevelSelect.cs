using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelect : MonoBehaviour {

	public void LoadRooftops()
    {
        SceneManager.LoadScene("Rooftops");
        Cursor.visible = false;
        Time.timeScale = 1f;
    }

    public void LoadJail()
    {
        SceneManager.LoadScene("Prison");
        Cursor.visible = false;
        Time.timeScale = 1f;
    }
}
