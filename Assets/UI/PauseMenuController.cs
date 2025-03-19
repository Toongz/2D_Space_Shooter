using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenuController : MonoBehaviour
{
    public GameObject pauseMenuPanel; 
    public Slider volumeSlider;       
    private bool isGamePaused = false; 

    void Start()
    {
        float savedVolume = PlayerPrefs.GetFloat("Volume", 1f);
        volumeSlider.value = savedVolume;
        AudioListener.volume = savedVolume;

        volumeSlider.onValueChanged.AddListener(SetVolume);

        pauseMenuPanel.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isGamePaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        isGamePaused = true;
        pauseMenuPanel.SetActive(true);
        Time.timeScale = 0f; 
        AudioListener.pause = true; 
    }

    public void ResumeGame()
    {
        isGamePaused = false;
        pauseMenuPanel.SetActive(false);
        Time.timeScale = 1f; 
        AudioListener.pause = false; 
    }

    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
        PlayerPrefs.SetFloat("Volume", volume); 
        PlayerPrefs.Save(); 
    }

    public void QuitToMainMenu()
    {
        Time.timeScale = 1f; 
        SceneManager.LoadScene("MainMenu");
    }
}
