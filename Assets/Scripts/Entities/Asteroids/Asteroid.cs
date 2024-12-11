using System.Collections;
using UnityEngine;
using static PowerUp;

public class Asteroid : MonoBehaviour
{
    public float speed;
    public int health;
    public float rotationSpeed = 50f;
    protected DifficultyManager difficultyManager;
    private Rigidbody2D rb;
    public ParticleSystem explodePartical;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        difficultyManager = FindObjectOfType<DifficultyManager>();
    }
    private void Start()
    {
        rb.velocity = Vector2.down * speed;
        rb.gravityScale = 1; 
        rb.mass = 1;
        rb.drag = 2; 
        rb.angularVelocity = Random.Range(-100f, 100f);
    }
    public void Initialize(int initialHealth, float initialSpeed)
    {
        health = initialHealth;
        speed = initialSpeed;
    }

    private void Update()
    {
        BehaviourAsteroid();
        BoundAsteroid();

    }

    public virtual void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            GameManager.Instance.AddScore(10);
            Explode();
        }
    }
    public virtual void OnEnable()
    {
        health += difficultyManager.GetAsteroidHealth();
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

                StartCoroutine(DisableParticleAfterPlay(explosionInstance));
            }
        }
        else
        {
            Debug.LogWarning("Explode is not assigned!");
        }

        gameObject.SetActive(false); 
    }
    public void BehaviourAsteroid()
    {
        transform.Translate(Vector3.down * speed * Time.deltaTime);

        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
    }
    public void BoundAsteroid()
    {
        if (transform.position.y < -6f)
        {
            gameObject.SetActive(false);
        }
    }
    private IEnumerator DisableParticleAfterPlay(ParticleSystem particleSystem)
    {
        if (particleSystem == null) yield break;

        while (particleSystem.isPlaying)
        {
            yield return null; 
        }

        particleSystem.Clear();
        particleSystem.gameObject.SetActive(false);

        Debug.Log("Particle System Disabled: " + particleSystem.gameObject.name);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            UIManager.Instance.ShowGameOverScreen();
        }
    }
}
