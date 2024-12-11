using UnityEngine;

public class BossSpawner : MonoBehaviour
{
    public AsteroidType[] bossTypes; 
    public Transform spawnPoint;
    private bool bossSpawned = false;

    
    public void SpawnBoss(int level)
    {
        if (bossSpawned) return;

        int typeIndex = Mathf.Min(level, bossTypes.Length - 1);
        AsteroidType selectedBoss = bossTypes[typeIndex];

        GameObject boss = Instantiate(selectedBoss.prefab, spawnPoint.position, Quaternion.identity);
        boss.GetComponent<Asteroid>().Initialize(
            selectedBoss.baseHealth + DifficultyManager.Instance.GetBossHealth(),
            selectedBoss.speed
        );

        bossSpawned = true;
    }

}
