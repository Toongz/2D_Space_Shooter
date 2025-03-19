using System.Collections;
using UnityEngine;
using static PowerUp;

public class PlayerController : MonoBehaviour
{
    public Transform projectileSpawnPoint;
    public float moveSpeed = 5f;
    public float fireRate = 13f;
    public int maxWeaponLevel = 3;
    public float powerUpDuration = 30f;
    public Animator playAnim { get; private set; }

    public GameObject playerFireEffectPrefab;
    private GameObject activePlayerFireEffect;

    private PowerUpType currentProjectileType = PowerUpType.ProjectileUpgrade;
    private IShipState currentState;
    private float nextFireTime;
    private int currentWeaponLevel = 0;
    private int additionalDamage = 0;
    private Coroutine powerUpTimerCoroutine;
    private Camera mainCamera;
    private Vector2 screenBounds;
    private float playerWidth;
    private float playerHeight;
    [SerializeField] private SpriteRenderer shipSprite;

    private void Start()
    {
        playAnim = GetComponent<Animator>();
        SetState(new IdleState());
        mainCamera = Camera.main;
        screenBounds = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, mainCamera.transform.position.z));
        if (shipSprite != null)
        {
            playerWidth = shipSprite.bounds.size.x / 2;
            playerHeight = shipSprite.bounds.size.y / 2;
        }

    }

    public void SetState(IShipState newState)
    {
        currentState = newState;
    }

    private void UpdatePlayerVisualEffect()
    {
        bool shouldShowFireEffect = (currentProjectileType == PowerUpType.Fire);

        if (shouldShowFireEffect && (activePlayerFireEffect == null || !activePlayerFireEffect.activeInHierarchy))
        {
            if (activePlayerFireEffect == null && playerFireEffectPrefab != null)
            {
                activePlayerFireEffect = Instantiate(playerFireEffectPrefab, transform);
                activePlayerFireEffect.transform.localPosition = Vector3.zero;
            }

            if (activePlayerFireEffect != null)
            {
                activePlayerFireEffect.SetActive(true);
            }
        }
        else if (!shouldShowFireEffect && activePlayerFireEffect != null && activePlayerFireEffect.activeInHierarchy)
        {
            activePlayerFireEffect.SetActive(false);
        }
    }

    public void UpgradeProjectile()
    {
        if (currentWeaponLevel < maxWeaponLevel)
        {
            currentWeaponLevel++;
        }
        else
        {
            additionalDamage += 2;
        }
    }

    private void Update()
    {
        HandleMovement();
        ClampPlayerPosition();
        currentState.HandleInput(this);
        currentState.UpdateState(this);
    }
    private void ClampPlayerPosition()
    {
        Vector3 viewPos = transform.position;

        // Giới hạn player trong phạm vi camera
        viewPos.x = Mathf.Clamp(viewPos.x, -screenBounds.x + playerWidth, screenBounds.x - playerWidth);
        viewPos.y = Mathf.Clamp(viewPos.y, -screenBounds.y + playerHeight, screenBounds.y - playerHeight);

        transform.position = viewPos;
    }
    private void HandleMovement()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, moveVertical, 0);
        transform.position += movement * moveSpeed * Time.deltaTime;
    }

    public void ApplyPowerUp(PowerUpType type)
    {
        if (powerUpTimerCoroutine != null)
        {
            StopCoroutine(powerUpTimerCoroutine);
        }

        switch (type)
        {
            case PowerUpType.Fire:
                currentProjectileType = PowerUpType.Fire;
                UpdatePlayerVisualEffect();
                powerUpTimerCoroutine = StartCoroutine(PowerUpTimer(PowerUpType.Fire));
                //Debug.Log("Fire - Active for 30 seconds");
                break;
            case PowerUpType.Ice:
                currentProjectileType = PowerUpType.Ice;
                UpdatePlayerVisualEffect();
                powerUpTimerCoroutine = StartCoroutine(PowerUpTimer(PowerUpType.Ice));
                //Debug.Log("Ice - Active for 30 seconds");
                break;
            case PowerUpType.ProjectileUpgrade:
                UpgradeProjectile();
                break;
        }
    }

    private IEnumerator PowerUpTimer(PowerUpType activeType)
    {
        yield return new WaitForSeconds(powerUpDuration);

        if (currentProjectileType == activeType)
        {
            currentProjectileType = PowerUpType.ProjectileUpgrade;
            UpdatePlayerVisualEffect();
            //Debug.Log($"{activeType} power-up has expired");
        }

        powerUpTimerCoroutine = null;
    }

    public void Shoot()
    {
        if (Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + fireRate;
            int projectileTypeIndex = Mathf.Min(currentWeaponLevel, maxWeaponLevel);
            GameObject projectile = ObjectPool.Instance.GetProjectile(projectileTypeIndex);

            if (projectile != null)
            {
                Projectile projectileComponent = projectile.GetComponent<Projectile>();
                if (projectileComponent != null)
                {
                    projectileComponent.damage = 1 + (currentWeaponLevel * 2) + additionalDamage;
                    projectileComponent.SetProjectileType(currentProjectileType);
                }
                projectile.transform.position = projectileSpawnPoint.position;
                projectile.SetActive(true);
            }

            AudioManager.Instance.PlayShootSound();
        }
    }
}

