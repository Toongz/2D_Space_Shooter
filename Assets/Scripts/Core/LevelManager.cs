using System;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    public LevelConfig[] levels;
    public AsteroidSpawner asteroidSpawner;
 

    public static event Action<bool> OnBossStateChanged;

    public int currentLevelIndex = 0;
    private bool bossSpawned = false;
    private float levelStartTime;
    public float timeBetweenLevels = 60f;
    private float bossDuration = 15f;
    private float bossSpawnTime;

    private void Awake()
    {
        Instance = this;
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
            if (Time.time - bossSpawnTime > bossDuration)
            {
                AdvanceToNextLevel();
                return;
            }
            return;
        }

        if (Time.time - levelStartTime > timeBetweenLevels)
        {
            AdvanceToNextLevel();
        }

        if (!currentLevel.hasBoss)
        {
            asteroidSpawner.SpawnAsteroids(currentLevel);
        }
    }

    private void StartLevel()
    {
        bossSpawned = false;
        levelStartTime = Time.time;
        LevelConfig currentLevel = GetCurrentLevel();

        if (currentLevel.hasBoss)
        {

            Debug.Log($"Bắt đầu Level {currentLevelIndex + 1 } - Spawn Boss");
            GameEvents.OnBossLevelStarted?.Invoke();
            SpawnBoss(currentLevel);
        }
    }

    private void SpawnBoss(LevelConfig levelConfig)
    {
        if (levelConfig.bossPrefab != null)
        {
            GameObject boss = Instantiate(levelConfig.bossPrefab);
            boss.transform.position = new Vector3(0, 9f, 0);
            boss.tag = "Boss";
            bossSpawned = true;
            bossSpawnTime = Time.time;

           
            OnBossStateChanged?.Invoke(true);

        }
    }

    private void AdvanceToNextLevel()
    {
        if (bossSpawned)
            OnBossStateChanged?.Invoke(false);

        GameObject existingBoss = GameObject.FindWithTag("Boss");
        if (existingBoss != null)
            Destroy(existingBoss);

        currentLevelIndex++;
        if (currentLevelIndex < levels.Length)
        {
            Debug.Log($"Next Level {currentLevelIndex + 1}");
            StartLevel();
        }
        else
        {
            Debug.Log("Complete all levels!");
            UIManager.Instance.ShowCongratulationScreen();
        }
    }

    private LevelConfig GetCurrentLevel()
    {
        return levels[Mathf.Min(currentLevelIndex, levels.Length - 1)];
    }
}



