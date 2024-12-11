using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed;
    public int damage;

    private void Update()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime);

        if (transform.position.y > 6f)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Asteroid"))
        {
            collision.GetComponent<Asteroid>().TakeDamage(damage);
           gameObject.SetActive(false);
        }
        if (collision.CompareTag("Boss"))
        {
            collision.GetComponent<BossAsteroid>().TakeDamage(damage);
            gameObject.SetActive(false);
        }
    }
}
