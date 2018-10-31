using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScript : MonoBehaviour {

    public GameObject goScreen;
    public IDamageable playHealth;
    public GameObject player;
    private AudioSource[] source;
    private int counter = 0;
    private GameObject hudObject;

    private void Start()
    {
        player = GameObject.Find("Player");
        playHealth = player.GetComponent<IDamageable>();

        source = GameObject.Find("UICanvas").transform.Find("GameOverScreen").gameObject.GetComponents<AudioSource>();
        hudObject = GameObject.Find("HUD");
    }

    // Update is called once per frame
    void Update () {

        if (playHealth.Health() <= 0)
        {
            Time.timeScale = 0f;
            dispGoScreen();
        }
    }

    public void dispGoScreen()
    {
        hudObject.SetActive(false);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        goScreen.SetActive(true);
        GameObject.Find("Player").GetComponent<CharacterController>().enabled = false;
        if (counter == 0)
        {
            source[1].Play();
            counter++;
        }
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
