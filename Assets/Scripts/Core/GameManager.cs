using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public int Score { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    private void Start()
    {
        Time.timeScale = 1f;
    }
    public void AddScore(int value)
    {
        Score += value;
        UIManager.Instance.UpdateScore(Score);
    }

    public void GameOver()
    {
        UIManager.Instance.ShowGameOverScreen();
        Time.timeScale = 0;
    }
}
