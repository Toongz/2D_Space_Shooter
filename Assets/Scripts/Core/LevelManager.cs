using System;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    public LevelConfig[] levels;
    public AsteroidSpawner asteroidSpawner;
    public BossSpawner bossSpawner; 

    public static event Action<bool> OnBossStateChanged;

    public int currentLevelIndex = 0;
    private bool bossSpawned = false;
    private bool bossDefeated = false;
    private float levelStartTime;
    public float timeBetweenLevels = 60f;
    private float bossDefeathCheckInterval = 2f;
    private float lastBossCheckTime = 0f;

    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        GameEvents.OnBossDefeated += HandleBossDefeated;
    }

    private void OnDisable()
    {
        GameEvents.OnBossDefeated -= HandleBossDefeated;
    }

    private void Start()
    {
        levelStartTime = Time.time;
        StartLevel();
    }

    private void Update()
    {
        LevelConfig currentLevel = GetCurrentLevel();

        if (bossSpawned)
        {
            if (bossDefeated || (Time.time - lastBossCheckTime > bossDefeathCheckInterval && !IsBossActive()))
            {
                bossDefeated = true;
                AdvanceToNextLevel();
                return;
            }

            if (Time.time - lastBossCheckTime > bossDefeathCheckInterval)
            {
                lastBossCheckTime = Time.time;
            }
        }

        if (Time.time - levelStartTime > timeBetweenLevels && !currentLevel.hasBoss)
        {
            AdvanceToNextLevel();
        }
        else if (currentLevel.hasBoss)
        {
            if (!bossSpawned && AreAllAsteroidsDestroyed())
            {
                Debug.Log("All asteroids destroyed! Spawning boss...");
                bossSpawner.SpawnBoss(currentLevel);
                bossSpawned = true;
                lastBossCheckTime = Time.time;
                OnBossStateChanged?.Invoke(true);

            }
        }
        else
        {
            asteroidSpawner.SpawnAsteroids(currentLevel);
        }
    }

    private void HandleBossDefeated()
    {
        bossDefeated = true;
    }

    private bool IsBossActive()
    {
        BossAsteroid boss = FindObjectOfType<BossAsteroid>();
        return boss != null && boss.gameObject.activeSelf;
    }


    private void StartLevel()
    {
        bossSpawned = false;
        bossDefeated = false;
        levelStartTime = Time.time;
        LevelConfig currentLevel = GetCurrentLevel();

        if (currentLevel.hasBoss)
        {
            //Debug.Log($"Starting Level {currentLevelIndex + 1} - Boss Level");
            GameEvents.OnBossLevelStarted?.Invoke();
        }
    }

    private void AdvanceToNextLevel()
    {
        if (bossSpawned)
            OnBossStateChanged?.Invoke(false);

        BossAsteroid existingBoss = FindObjectOfType<BossAsteroid>();
        if (existingBoss != null)
            existingBoss.gameObject.SetActive(false);
        currentLevelIndex++;
        if (currentLevelIndex < levels.Length)
        {
            //Debug.Log($"Next Level {currentLevelIndex + 1}");
            StartLevel();
        }
        else
        {
            Asteroid existingAsteroid = FindObjectOfType<Asteroid>();
            if (existingAsteroid == null)
            {
                UIManager.Instance.ShowCongratulationScreen();
                Debug.Log("Complete all levels!");
            }
            
        }
    }

    private bool AreAllAsteroidsDestroyed()
    {
        return FindObjectsOfType<Asteroid>().Length == 0;
    }

    private LevelConfig GetCurrentLevel()
    {
        return levels[Mathf.Min(currentLevelIndex, levels.Length - 1)];
    }
}
