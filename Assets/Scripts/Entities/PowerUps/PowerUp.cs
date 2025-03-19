using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public enum PowerUpType { ProjectileUpgrade, Heal, Ice, Fire }
    public PowerUpType type;
    public float speed = 3f;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController playerController = collision.GetComponent<PlayerController>();

            switch (type)
            {
                case PowerUpType.ProjectileUpgrade:
                    playerController.UpgradeProjectile();
                    break;
                case PowerUpType.Fire:
                    playerController.ApplyPowerUp(type);
                    break;
                //case PowerUpType.Ice:
                //    playerController.ApplyPowerUp(type);
                //    break;
                //case PowerUpType.Heal:
                //    break;
            }

            gameObject.SetActive(false);
        }
    }
    private void Update()
    {
        transform.Translate(Vector3.down * speed * Time.deltaTime);

        if (transform.position.y < -6f)
        {
            gameObject.SetActive(false);
        }
    }

  

}
