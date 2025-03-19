using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    public GameObject optionsPanel;
    public GameObject settingsPanel;
    public GameObject tutorialPanel;
    public Slider volumeSlider;     

    private const string VolumeKey = "Volume"; 


    void Start()
    {
        float savedVolume = PlayerPrefs.GetFloat(VolumeKey, 1f);
        volumeSlider.value = savedVolume;
        AudioListener.volume = savedVolume;

        volumeSlider.onValueChanged.AddListener(SetVolume);
    }
    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
        PlayerPrefs.SetFloat(VolumeKey, volume);
        PlayerPrefs.Save();
    }
    public void StartGame()
    {

        SceneManager.LoadScene("GamePlay");
    }
    public void OpenOptions()
    {
        optionsPanel.SetActive(true);
        gameObject.SetActive(false);
    }

    public void CloseOptions()
    {
        optionsPanel.SetActive(false);
        gameObject.SetActive(true);

 
    }

    public void OpenSettingsTab()
    {
        settingsPanel.SetActive(true);
        tutorialPanel.SetActive(false);
    }

    public void OpenTutorialTab()
    {
        settingsPanel.SetActive(false);
        tutorialPanel.SetActive(true);
    }

 
    public void QuitGame()
    {
        Application.Quit();
    }
}
