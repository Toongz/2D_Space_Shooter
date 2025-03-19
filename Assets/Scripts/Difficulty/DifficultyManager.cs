using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    public static DifficultyManager Instance { get; private set; }

    public DifficultySettings difficultySettings;

    private int difficultyLevel = 0;

    public int GetAsteroidHealth()
    {
        return difficultySettings.asteroidHealthIncrease * difficultyLevel;
    }

    public int GetBossHealth()
    {
        return  difficultySettings.bossHealthIncrease * difficultyLevel;
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    public void GetDifficultyLevel()
    {
        difficultyLevel = LevelManager.Instance.currentLevelIndex;
    }
    private void Update()
    {
        GetDifficultyLevel();
    }
}
