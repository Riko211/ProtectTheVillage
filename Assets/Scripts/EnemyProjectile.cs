using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    [Header("Bullet options")]
    [SerializeField] private float projectileSpeed;
    [SerializeField] private float projectileLifetime;
    [SerializeField] private float projectileDamage;
    [SerializeField] private float damagePerLevel;

    [SerializeField] private GameObject destroyEffect;

    private void Start()
    {
        StartCoroutine(DestoyProj());
        projectileDamage += WaveManager._instance.GetCurrentWave() * damagePerLevel;
    }
    void FixedUpdate()
    {
        transform.Translate(Vector2.left * projectileSpeed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            collision.collider.GetComponent<Player>().TakeDamage(projectileDamage);
            Instantiate(destroyEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
        if (collision.collider.CompareTag("Wall"))
        {
            Instantiate(destroyEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
    IEnumerator DestoyProj()
    {
        yield return new WaitForSeconds(projectileLifetime);
        Destroy(gameObject);
    }
}
