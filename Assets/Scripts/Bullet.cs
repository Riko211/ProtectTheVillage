using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Weapon Id")]
    public int weaponId;
    [Header("Bullet options")]
    public float bulletSpeed;
    public float bulletLifetime;
    public float bulletDistance;
    public float bulletPiercesCount;
    public float bulletDamage;

    [Header("Interract with layers")]
    public LayerMask whatIsSolid;

    private void Start()
    {
        StartCoroutine(BulletDestroy());
        bulletDamage = FindObjectOfType<Shop>().GetBulletDamage(weaponId);

        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, 0.2f, whatIsSolid);
        for (int i = 0; i < enemies.Length; i++)
        {
            enemies[i].GetComponent<Enemy>().TakeDamage(bulletDamage);
            bulletPiercesCount--;
        }
    }
    void Update()
    {
        transform.Translate(Vector2.left * bulletSpeed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Enemy"))
        {
            collision.collider.GetComponent<Enemy>().TakeDamage(bulletDamage);
            if (bulletPiercesCount > 0)
            {
                bulletPiercesCount--;
            }
            else
            {
                Destroy(gameObject);
            }
        }
        if (collision.collider.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
    IEnumerator BulletDestroy()
    {
        yield return new WaitForSeconds(bulletLifetime + Random.Range(-0.1f, 0.1f));
        Destroy(gameObject);
    }
}
