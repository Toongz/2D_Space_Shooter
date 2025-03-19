using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public Text bossLevelText; 
    public Text  scoreText;
    public Text scoreOverText;
    public Text scoreAgainText;
    public GameObject gameOverScreen;
    public GameObject gameCongratulationScreen;
    public GameObject pauseMenuPanel;
    public float displayTime = 3f;
    public Slider volumeSlider;       
   
    private bool isPause = false;


    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    private void Start()
    {
        Time.timeScale = 1f;
       
        float savedVolume = PlayerPrefs.GetFloat("Volume", 1f);
        volumeSlider.value = savedVolume;
        AudioListener.volume = savedVolume;

        volumeSlider.onValueChanged.AddListener(SetVolume);
        pauseMenuPanel.SetActive(false);
        
    }
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (isPause)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }

    }
    public void SetVolume(float volume)
    {
        AudioListener.volume = volume; 
        PlayerPrefs.SetFloat("Volume", volume); 
        PlayerPrefs.Save(); 
    }
    public void PauseGame()
    {
        isPause = true;
        pauseMenuPanel.SetActive(true);
        Time.timeScale = 0f; 
    }

    public void ResumeGame()
    {
        isPause = false;
        pauseMenuPanel.SetActive(false);
        Time.timeScale = 1f;
    }
    public void PlayAgain()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("GamePlay");
    }
    public void Exit()
    {
        isPause = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
    public void UpdateScore(int score)
    {
        scoreText.text = "Score: " + score;
    }
    public void ShowCongratulationScreen()
    {
        
        isPause = true;
        Time.timeScale = 0f;
        scoreAgainText.text = GameManager.Instance.Score + " score";
        gameCongratulationScreen.SetActive(true);
    }
    public void ShowGameOverScreen()
    {
        isPause = true;
        Time.timeScale = 0f;
        scoreOverText.text = GameManager.Instance.Score + " score";
        gameOverScreen.SetActive(true);
    }
    private void OnEnable()
    {
        GameEvents.OnBossLevelStarted += ShowBossLevelNotification;
    }

    private void OnDisable()
    {
        GameEvents.OnBossLevelStarted -= ShowBossLevelNotification;
    }

    private void ShowBossLevelNotification()
    {
        AudioManager.Instance.PlayWarningBossSound();
        StopAllCoroutines();
        StartCoroutine(DisplayBossNotification());
    }

    private IEnumerator DisplayBossNotification()
    {
        bossLevelText.gameObject.SetActive(true);
        yield return new WaitForSeconds(displayTime);
        bossLevelText.gameObject.SetActive(false);
    }
}
