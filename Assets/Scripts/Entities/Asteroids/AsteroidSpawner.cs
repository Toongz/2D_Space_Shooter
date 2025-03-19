using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    public Transform[] spawnPoints;
    public bool canSpawnAsteroids = true;

    private float nextSpawnTime;
    private void OnEnable()
    {
        LevelManager.OnBossStateChanged += HandleBossStateChanged;
    }
    private void HandleBossStateChanged(bool isBossActive)
    {
        canSpawnAsteroids = !isBossActive;
        //Debug.Log(isBossActive ? "Boss spawned. Stopping asteroid spawn." : "Boss defeated. Resuming asteroid spawn.");
    }

    public void SpawnAsteroids(LevelConfig levelConfig)
    {
        if (Time.time < nextSpawnTime) return;

        int randomValue = Random.Range(1, 101); 
        int accumulatedRate = 0;

        foreach (var spawnRate in levelConfig.spawnRates)
        {
            accumulatedRate += spawnRate.percentage;
            if (randomValue <= accumulatedRate)
            {
                GameObject asteroid = ObjectPool.Instance.GetAsteroid(spawnRate.asteroidType);
                if (asteroid != null)
                {
                    Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
                    asteroid.transform.position = spawnPoint.position;
                    asteroid.transform.rotation = Quaternion.identity;
                    asteroid.SetActive(true);
                }
                break;
            }
        }

        nextSpawnTime = Time.time + levelConfig.spawnInterval;
    }
}



