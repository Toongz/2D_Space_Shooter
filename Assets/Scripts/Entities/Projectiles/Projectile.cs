using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PowerUp;

public class Projectile : MonoBehaviour
{
    public float speed;
    public int damage;
    public PowerUpType projectileType = PowerUpType.ProjectileUpgrade;

    public ParticleSystem trailParticleSystem;

    public void SetProjectileType(PowerUpType type)
    {
        projectileType = type;
        UpdateTrailParticleSystem();
    }

    private void Start()
    {
        UpdateTrailParticleSystem();
    }
    private void Update()
    {
        MoveProjectile();
        CheckOutOfBounds();
    }
    public void MoveProjectile()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime);
    }
    public void CheckOutOfBounds()
    {
        if (transform.position.y > 6f)
        {
            gameObject.SetActive(false);
        }
    }
    public void UpdateTrailParticleSystem()
    {
        if (trailParticleSystem != null)
        {
            if (projectileType == PowerUpType.Fire)
            {
                trailParticleSystem.gameObject.SetActive(true);
                if (!trailParticleSystem.isPlaying)
                {
                    trailParticleSystem.Clear();
                    trailParticleSystem.Play();
                }
            }
            else
            {
                trailParticleSystem.Stop();
                trailParticleSystem.gameObject.SetActive(false);
            }
        }
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Asteroid") || collision.CompareTag("Boss"))
        {
            var asteroid = collision.GetComponent<Asteroid>();

            if (projectileType != PowerUpType.ProjectileUpgrade)
            {
                ApplyStatusEffect(asteroid);
            }

            asteroid.TakeDamage(damage);
            gameObject.SetActive(false);
        }
    }


    private void ApplyStatusEffect(Asteroid asteroid)
    {
        StatusEffect existingEffect = null;

        switch (projectileType)
        {
            case PowerUpType.Fire:
                existingEffect = asteroid.GetComponent<BurnEffect>();
                if (existingEffect == null)
                {
                    existingEffect = asteroid.gameObject.AddComponent<BurnEffect>();
                    existingEffect.Initialize(duration: 5f, tickRate: 1, value: 2f);
                    existingEffect.StartEffect(asteroid);
                }
                else
                {
                    existingEffect.duration = 5f;
                }
                break;
            case PowerUpType.Ice:
                existingEffect = asteroid.GetComponent<FreezeEffect>();
                if (existingEffect == null)
                {
                    existingEffect = asteroid.gameObject.AddComponent<FreezeEffect>();
                    existingEffect.Initialize(duration: 5f, tickRate: 1, value: 0.9f);
                    existingEffect.StartEffect(asteroid);
                }
                else
                {
                    existingEffect.duration = 5f;
                }
                break;
        }
    }
}