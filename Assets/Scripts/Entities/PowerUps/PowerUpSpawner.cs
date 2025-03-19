using UnityEngine;

public class PowerUpSpawner : MonoBehaviour
{
    public Transform[] spawnPoints; 
    public float spawnInterval = 5f;

    private float nextSpawnTime;

    private void Update()
    {
        if (Time.time >= nextSpawnTime)
        {
            SpawnPowerUp();
            nextSpawnTime = Time.time + spawnInterval;
        }
    }

    private void SpawnPowerUp()
    {
        if (spawnPoints.Length == 0) return;

        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject powerUp = ObjectPool.Instance.GetPowerUpUpgrade();
        if (powerUp != null)
        {
            powerUp.transform.position = spawnPoint.position;
            powerUp.transform.rotation = Quaternion.identity;
            powerUp.SetActive(true);
        }
    }
}



