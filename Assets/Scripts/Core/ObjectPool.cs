
using UnityEngine;
using System.Collections.Generic;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance { get; private set; }

    public GameObject[] asteroidPrefabs;
    public GameObject[] projectilePrefabs;
    public GameObject powerUpFirePrefab;
    public ParticleSystem explosionParticlePrefab;
    public GameObject fireEffectVisualPrefab;
    public GameObject powerUpPrefab;

    private List<ParticleSystem> pooledExplosions;
    private List<GameObject>[] asteroidPools;
    private List<GameObject>[] projectilePools;
    private List<GameObject> powerUpFirePool;
    private List<GameObject> fireEffectVisualPool;
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

        powerUpFirePool = new List<GameObject>();
        pooledExplosions = new List<ParticleSystem>();
        fireEffectVisualPool = new List<GameObject>();
        powerUpPool = new List<GameObject>();
    }
    public GameObject GetProjectile(int typeIndex = 0)
    {
        if (typeIndex >= projectilePools.Length)
            typeIndex = 2;

        GameObject projectile = null;

        foreach (var proj in projectilePools[typeIndex])
        {
            if (!proj.activeInHierarchy)
            {
                projectile = proj;
                break;
            }
        }

        if (projectile == null)
        {
            projectile = Instantiate(projectilePrefabs[typeIndex]);
            projectilePools[typeIndex].Add(projectile);
        }
        projectile.SetActive(true);
        return projectile;
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

    public GameObject GetPowerUpUpgrade()
    {
        foreach (var powerUp in powerUpFirePool)
        {
            if (!powerUp.activeInHierarchy)
                return powerUp;
        }

        GameObject newPowerUp = Instantiate(powerUpFirePrefab);
        powerUpFirePool.Add(newPowerUp);
        return newPowerUp;
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
    public GameObject GetFireEffectVisual()
    {
        foreach (var effect in fireEffectVisualPool)
        {
            if (!effect.activeInHierarchy)
                return effect;
        }

        GameObject newEffect = Instantiate(fireEffectVisualPrefab);
        fireEffectVisualPool.Add(newEffect);
        return newEffect;
    }

}