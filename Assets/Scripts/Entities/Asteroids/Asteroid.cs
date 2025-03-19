using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField]protected float speed;
    [SerializeField] protected int health;
    [SerializeField] protected float rotationSpeed = 50f;

    [Header("Power-Up Settings")]
    [Range(0, 100)]
    public float powerUpDropChance = 15f; 

    protected DifficultyManager difficultyManager;
    private Rigidbody2D rb;
    public ParticleSystem explodePartical;

    private Camera mainCamera;
    private Vector2 screenBounds;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        difficultyManager = FindObjectOfType<DifficultyManager>();
        mainCamera = Camera.main;
        CalculateScreenBounds();
    }

    public void Initialize(int initialHealth, float initialSpeed)
    {
        health = initialHealth;
        speed = initialSpeed;
        ResetVelocity();
    }

    private void Update()
    {
        RotationTransform();
    }
    private void RotationTransform()
    {
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
    }
    private void FixedUpdate()
    {
        HandleScreenBoundsPhysics();
    }

    private void ResetVelocity()
    {
        if (rb != null)
        {
            Vector2 initialDirection = (Vector2.down + Random.insideUnitCircle * 0.5f).normalized;
            rb.velocity = initialDirection * speed;
        }
    }

    private void CalculateScreenBounds()
    {
        if (mainCamera == null) return;

        screenBounds = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, mainCamera.transform.position.z));
    }
    private void HandleScreenBoundsPhysics()
    {
        Vector2 currentPosition = rb.position;
        bool bounced = false;

        // Horizontal bounce
        if (Mathf.Abs(currentPosition.x) > screenBounds.x)
        {
            rb.velocity = new Vector2(-rb.velocity.x, rb.velocity.y); // Không nhân với bounceForce
            bounced = true;
        }

        // Vertical bounce (cả trên và dưới)
        if (Mathf.Abs(currentPosition.y) > screenBounds.y)
        {
            rb.velocity = new Vector2(rb.velocity.x, -rb.velocity.y); // Không nhân với bounceForce
            bounced = true;
        }

        // Random slight velocity adjustment (có thể tắt nếu không muốn dao động)
        if (bounced)
        {
            rb.velocity += Random.insideUnitCircle * 0.3f; // Giảm nhỏ giá trị để không ảnh hưởng nhiều
        }

        // Giới hạn tốc độ tối đa
        float maxSpeed = speed; 
        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }
    }


    public void ApplyStatusEffectDamage(int damage)
    {
        health -= damage;
        CheckHealth();
    }

    public virtual void TakeDamage(int damage)
    {
        health -= damage;
        CheckHealth();
    }

    private void CheckHealth()
    {
        if (health <= 0)
        {
            TryDropPowerUp();

            Explode();
            var effects = GetComponents<StatusEffect>();
            foreach (var effect in effects)
            {
                effect.RemoveEffect(this);
                Destroy(effect);
            }

            GameManager.Instance.AddScore(10);
           
        }
    }
    private void TryDropPowerUp()
    {
        if (Random.Range(0f, 100f) <= powerUpDropChance)
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
    public virtual void OnEnable()
    {
        ResetAsteroid();
    }
    private void Explode()
    {
        if (explodePartical != null)
        {
            ParticleSystem explosionInstance = ObjectPool.Instance.GetPooledObject(explodePartical.gameObject)?.GetComponent<ParticleSystem>();

            if (explosionInstance != null)
            {
                explosionInstance.Clear();
                explosionInstance.Stop();

                explosionInstance.transform.position = transform.position;
                explosionInstance.gameObject.SetActive(true);
                explosionInstance.Play();
                AudioManager.Instance.PlayExplosionSound();

                StartCoroutine(DisableParticleAndAsteroid(explosionInstance));
            }
        }
        else
        {
            Debug.LogWarning("Explode particle is not assigned!");
        }
    }

    private System.Collections.IEnumerator DisableParticleAndAsteroid(ParticleSystem particleSystem)
    {
        if (particleSystem == null) yield break;

        while (particleSystem.isPlaying)
        {
            yield return null;
        }

        particleSystem.Clear();
        particleSystem.gameObject.SetActive(false);

        gameObject.SetActive(false);
    }
    public void ResetAsteroid()
    {
        health = difficultyManager.GetAsteroidHealth(); 
        ResetVelocity();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            UIManager.Instance.ShowGameOverScreen();
        }
    }
}
