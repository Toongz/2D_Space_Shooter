using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Transform projectileSpawnPoint;
    public float moveSpeed = 5f;
    public float fireRate = 0.3f;
    public int maxWeaponLevel = 3;
    public Animator playAnim { get; private set; }

    private IShipState currentState;
    private float nextFireTime;
    private int currentWeaponLevel = 0;
    private int additionalDamage = 0;

    private void Start()
    {

        playAnim = GetComponent<Animator>();
        SetState(new IdleState());
    }
    public void SetState(IShipState newState) { currentState = newState; }
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
        currentState.HandleInput(this);
        currentState.UpdateState(this);
    }

    private void HandleMovement()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, moveVertical, 0);
        transform.position += movement * moveSpeed * Time.deltaTime;
    }

    //private void HandleShooting()
    //{
    //    if (Input.GetMouseButton(0) && Time.time >= nextFireTime)
    //    {
    //        nextFireTime = Time.time + fireRate;
    //        Shoot();
    //        playAnim.SetBool("isFiring", true);
    //    }
    //    else 
    //    { 
    //        playAnim.SetBool("isFiring", false); 
    //    }

    //}


    public void Shoot()
    {
        if (Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + fireRate; int projectileTypeIndex = Mathf.Min(currentWeaponLevel, maxWeaponLevel); 
            GameObject projectile = ObjectPool.Instance.GetProjectile(projectileTypeIndex); 
            if (projectile != null)
            { 
            Projectile projectileComponent = projectile.GetComponent<Projectile>(); 
            if (projectileComponent != null) 
            { 
                projectileComponent.damage = 1 + (currentWeaponLevel * 2) + additionalDamage; 
            } 
            projectile.transform.position = projectileSpawnPoint.position;
            projectile.transform.rotation = Quaternion.identity; 
            projectile.SetActive(true); 
            }
            AudioManager.Instance.PlayShootSound();
        }
    }
   




}
