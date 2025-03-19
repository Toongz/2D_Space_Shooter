using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAsteroid : Asteroid
{
    public GameObject smallAsteroidPrefab;
    public int smallAsteroidCount;
    public int lastHealth;
    private bool isMovingDown = true;
    private float verticalMovementSpeed = 1.0f;

    [Header("Boss Power-Up Settings")]
    [Range(0, 100)]
    public float bossUpgradeDropChance = 100f; 

    private void Start()
    {
        ResetBossVelocity();
    }

    private void Update()
    { 
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
        if (isMovingDown && transform.position.y <= 3.0f)
        {
            isMovingDown = false;
            ResetBossVelocity();
        }
    }

    private void ResetBossVelocity()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            if (isMovingDown)
            {
                rb.velocity = Vector2.down * speed * 0.5f;
            }
            else
            {
                float horizontalDirection = Random.Range(-1f, 1f);
                rb.velocity = new Vector2(horizontalDirection * speed, -verticalMovementSpeed);
            }
        }
    }

    public override void TakeDamage(int damage)
    {
        lastHealth = health; 
        base.TakeDamage(damage);
        while (lastHealth > health) 
        {
            if (lastHealth % 15 == 0)
            {
                SpawnSmallAsteroids();
            }
            lastHealth--; 
        }
        if (health <= 0)
        {
            DropProjectileUpgrade();

            SpawnSmallAsteroids();


            GameEvents.OnBossDefeated?.Invoke();
        }
    }

    public override void OnEnable()
    {
        health += difficultyManager.GetBossHealth();
        isMovingDown = true;
        ResetBossVelocity();
    }
    private void DropProjectileUpgrade()
    {
        if (Random.Range(0f, 100f) <= bossUpgradeDropChance)
        {
            GameObject powerUp = ObjectPool.Instance.GetPowerUp();
            if (powerUp != null)
            {
                powerUp.transform.position = transform.position;

                PowerUp powerUpComponent = powerUp.GetComponent<PowerUp>();
                if (powerUpComponent != null)
                {
                    powerUpComponent.type = PowerUp.PowerUpType.ProjectileUpgrade;
                }

                powerUp.SetActive(true);
            }
        }
    }
    private void SpawnSmallAsteroids()
    {
        int smallAsteroidIndex = System.Array.IndexOf(ObjectPool.Instance.asteroidPrefabs, smallAsteroidPrefab);

        for (int i = 0; i < smallAsteroidCount; i++)
        {
            Vector3 spawnPosition = transform.position + Random.insideUnitSphere * 0.5f;

            GameObject smallAsteroid = ObjectPool.Instance.GetAsteroid(smallAsteroidIndex);

            if (smallAsteroid != null)
            {
                Asteroid asteroidComponent = smallAsteroid.GetComponent<Asteroid>();
                if (asteroidComponent != null)
                {
                    asteroidComponent.ResetAsteroid();
                }

                smallAsteroid.transform.position = spawnPosition;
                smallAsteroid.transform.rotation = Quaternion.identity;
                smallAsteroid.SetActive(true);
            }
        }
    }
}
