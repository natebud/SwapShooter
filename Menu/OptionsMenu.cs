using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{

    public AudioMixer audioMixer;

    public Dropdown resolutionDropdown;

    Resolution[] resolutions;

    public static float currVolume;

    private CamMouseLook currSens;

    void Start()
    {
        if (SceneManager.GetActiveScene().name != "MainMenu")
        {
            currSens = GameObject.Find("Player").transform.Find("Main Camera").GetComponent<CamMouseLook>();
            GameObject.Find("Sensitivity Slider").GetComponent<Slider>().value = PlayerPrefs.GetFloat("sensitivity");
        }

        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        int currResIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " X " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currResIndex = i;
            }
        }

        if (PlayerPrefs.GetInt("isFS") == 1)
        {
            GameObject.Find("Fullscreen Toggle").GetComponent<Toggle>().isOn = true;
            Screen.fullScreen = true;
        }
        else if (PlayerPrefs.GetInt("isFS") == -1)
        {
            GameObject.Find("Fullscreen Toggle").GetComponent<Toggle>().isOn = false;
            Screen.fullScreen = false;
        }

        resolutionDropdown.AddOptions(options);
        if (PlayerPrefs.GetInt("resIndex") == 0)
        {
            resolutionDropdown.value = currResIndex;
        }
        else
        {
            resolutionDropdown.value = PlayerPrefs.GetInt("resIndex");
        }
        resolutionDropdown.RefreshShownValue();

        GameObject.Find("Volume Slider").GetComponent<Slider>().value = PlayerPrefs.GetFloat("volume");

    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        PlayerPrefs.SetInt("resIndex", resolutionIndex);
        PlayerPrefs.Save();
    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("volume", volume);
        currVolume = volume;
        PlayerPrefs.SetFloat("volume", currVolume);
        PlayerPrefs.Save();
    }

    public void SetSensitivity(float sensitivity)
    {
        currSens.sensitivity = sensitivity;
        PlayerPrefs.SetFloat("sensitivity", sensitivity);
        PlayerPrefs.Save();
    }

    public void SetFullScreen(bool isFullScreen)
    {
        if (isFullScreen)
        {
            Screen.fullScreen = isFullScreen;
            PlayerPrefs.SetInt("isFS", 1);
            PlayerPrefs.Save();
        }
        else if (!isFullScreen)
        {
            Screen.fullScreen = isFullScreen;
            PlayerPrefs.SetInt("isFS", -1);
            PlayerPrefs.Save();
        }
    }
}