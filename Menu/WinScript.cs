using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinScript : MonoBehaviour
{

    public bool win = true;
    public GameObject winScreen;
    public GameObject boss;
    private EnemyHealth bossHealth;
    public GameObject[] Enemies;
    //public GameObject bigbad;
    private AudioSource[] source;
    private TextMeshProUGUI enemyCounter;
    private int counter = 0;
    private GameObject hudObject;
    private float timer;
    private TextMeshProUGUI finishText;

    private void Start()
    {
        //bigbad = GameObject.FindGameObjectWithTag("Boss");

        source = GameObject.Find("UICanvas").transform.Find("WinScreen").gameObject.GetComponents<AudioSource>();

        enemyCounter = GameObject.Find("HUD").transform.Find("Enemy Counter").GetComponent<TextMeshProUGUI>();
        hudObject = GameObject.Find("HUD");

        finishText = GameObject.Find("UICanvas").transform.Find("WinScreen").transform.Find("FinishTime").GetComponent<TextMeshProUGUI>();

        timer = Time.time;

        if (boss!=null)
        {
            bossHealth = boss.GetComponent<EnemyHealth>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (win)
        {
            Enemies = GameObject.FindGameObjectsWithTag("Enemy");
            enemyCounter.SetText("Enemies Left: " + (Enemies.Length - 1));

            if (Enemies.Length == 1)
            {
                Time.timeScale = 0f;
                int temp = (int)Time.time - (int)timer;
                int minutes = temp / 60;
                int seconds = temp % 60;
                float fraction = timer * 1000;
                fraction = (fraction % 1000);
                string finishTime = "Finish Time: " + minutes + "m:" + seconds + "s:" + fraction.ToString("0") + "ms";
                finishText.SetText(finishTime);
                dispWinScreen();
            }
            else if (boss != null && boss.GetComponent<EnemyHealth>().Health() <= 20)
            {
                Time.timeScale = 0f;
                int temp = (int)Time.time - (int)timer;
                int minutes = temp / 60;
                int seconds = temp % 60;
                float fraction = timer * 1000;
                fraction = (fraction % 1000);
                string finishTime = "Finish Time: " + minutes + "m:" + seconds + "s:" + fraction.ToString("0") + "ms";
                finishText.SetText(finishTime);
                dispWinScreen();
            }
        }
    }

    public void dispWinScreen()
    {
        hudObject.SetActive(false);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        winScreen.SetActive(true);
        GameObject.Find("Player").GetComponent<CharacterController>().enabled = false;
        if (counter == 0)
        {
            source[1].Play();
            counter++;
        }
    }
}
