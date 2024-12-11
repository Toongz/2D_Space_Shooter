using UnityEngine;
using System.Collections.Generic;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance { get; private set; }

    public GameObject[] asteroidPrefabs;
    public GameObject[] projectilePrefabs;
    public GameObject powerUpPrefab;
    public ParticleSystem explosionParticlePrefab; 

    private List<ParticleSystem> pooledExplosions;
    private List<GameObject>[] asteroidPools;
    private List<GameObject>[] projectilePools;
    private List<GameObject> powerUpPool;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        InitializePools();
    }

    private void InitializePools()
    {
        asteroidPools = new List<GameObject>[asteroidPrefabs.Length];
        for (int i = 0; i < asteroidPrefabs.Length; i++)
        {
            asteroidPools[i] = new List<GameObject>();
        }

        projectilePools = new List<GameObject>[projectilePrefabs.Length];
        for (int i = 0; i < projectilePrefabs.Length; i++)
        {
            projectilePools[i] = new List<GameObject>();
        }

        powerUpPool = new List<GameObject>();

        pooledExplosions = new List<ParticleSystem>();
    }

    public GameObject GetAsteroid(int typeIndex)
    {
        if (typeIndex >= asteroidPools.Length)
            return null;

        foreach (var asteroid in asteroidPools[typeIndex])
        {
            if (!asteroid.activeInHierarchy)
                return asteroid;
        }

        GameObject newAsteroid = Instantiate(asteroidPrefabs[typeIndex]);
        asteroidPools[typeIndex].Add(newAsteroid);
        return newAsteroid;
    }

    public GameObject GetProjectile(int typeIndex = 0)
    {
        if (typeIndex >= projectilePools.Length) 
            typeIndex = 2; 

        foreach (var projectile in projectilePools[typeIndex])
        {
            if (!projectile.activeInHierarchy)
                return projectile;
        }

        GameObject newProjectile = Instantiate(projectilePrefabs[typeIndex]);
        projectilePools[typeIndex].Add(newProjectile);
        return newProjectile;
    }
    public GameObject GetPowerUp()
    {
        foreach (var powerUp in powerUpPool)
        {
            if (!powerUp.activeInHierarchy)
                return powerUp;
        }

        GameObject newPowerUp = Instantiate(powerUpPrefab);
        powerUpPool.Add(newPowerUp);
        return newPowerUp;
    }

    public ParticleSystem GetPooledObject(GameObject prefab)
    {
        foreach (ParticleSystem particle in pooledExplosions)
        {
            if (!particle.gameObject.activeInHierarchy)
            {
                particle.gameObject.SetActive(true);
              
                return particle;
            }
        }

        ParticleSystem newParticle = Instantiate(prefab).GetComponent<ParticleSystem>();
        pooledExplosions.Add(newParticle);
        return newParticle;
    }
}

