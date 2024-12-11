using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAsteroid : Asteroid
{
    public GameObject smallAsteroidPrefab;
    public int smallAsteroidCount = 3;

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        if (health <= 0)
        {
            SpawnSmallAsteroids();
        }
    }
    public override void OnEnable()
    {
        health += difficultyManager.GetBossHealth();

    }


    //private void SpawnSmallAsteroids()
    //{
    //    for (int i = 0; i < smallAsteroidCount; i++)
    //    {
    //        Vector3 spawnPosition = transform.position + Random.insideUnitSphere * 0.5f;
    //        GameObject smallAsteroid = ObjectPool.Instance.GetAsteroid(0);
    //        if (smallAsteroid != null)
    //        {
    //            smallAsteroid.transform.position = spawnPosition;
    //            smallAsteroid.transform.rotation = Quaternion.identity;
    //            smallAsteroid.SetActive(true);
    //        }
    //    }
    //}
    private void SpawnSmallAsteroids()
    {
        // Tìm index của smallAsteroidPrefab trong mảng asteroidPrefabs
        int smallAsteroidIndex = System.Array.IndexOf(ObjectPool.Instance.asteroidPrefabs, smallAsteroidPrefab);

        for (int i = 0; i < smallAsteroidCount; i++)
        {
            Vector3 spawnPosition = transform.position + Random.insideUnitSphere * 0.5f;

            // Sử dụng index tìm được thay vì hardcode 0
            GameObject smallAsteroid = ObjectPool.Instance.GetAsteroid(smallAsteroidIndex);

            if (smallAsteroid != null)
            {
                smallAsteroid.transform.position = spawnPosition;
                smallAsteroid.transform.rotation = Quaternion.identity;
                smallAsteroid.SetActive(true);
            }
        }
    }
}
