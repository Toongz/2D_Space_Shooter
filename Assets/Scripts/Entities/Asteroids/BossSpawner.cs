//using UnityEngine;

//public class BossSpawner : MonoBehaviour
//{
//    public AsteroidType[] bossTypes; 
//    public Transform spawnPoint;
//    private bool bossSpawned = false;

    
//    public void SpawnBoss(int level)
//    {
//        Debug.Log("Boss is comming");
//        if (bossSpawned) return;

//        int typeIndex = Mathf.Min(level, bossTypes.Length - 1);
//        AsteroidType selectedBoss = bossTypes[typeIndex];

//        GameObject boss = Instantiate(selectedBoss.prefab, spawnPoint.position, Quaternion.identity);
//        boss.GetComponent<Asteroid>().Initialize(
//            selectedBoss.baseHealth + DifficultyManager.Instance.GetBossHealth(),
//            selectedBoss.speed
//        );

//        bossSpawned = true;
//    }

//}
using UnityEngine;

public class BossSpawner : MonoBehaviour
{
    public AsteroidType[] bossTypes;
    public Transform spawnPoint;

    public void SpawnBoss(LevelConfig levelConfig)
    {
        if (levelConfig.bossPrefab != null)
        {
            foreach (var item in bossTypes)
            {
                if(item.prefab == levelConfig.bossPrefab)
                {
                    AsteroidType selectedBoss = item;
                    GameObject boss = Instantiate(levelConfig.bossPrefab, spawnPoint.position, Quaternion.identity);
                    boss.GetComponent<BossAsteroid>().Initialize(
                        selectedBoss.baseHealth + DifficultyManager.Instance.GetBossHealth(),
                        selectedBoss.speed
                        );
                }
            }
        }
    }
}
