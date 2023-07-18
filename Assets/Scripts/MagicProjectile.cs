using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicProjectile : MonoBehaviour
{
    [SerializeField] private string projectileLevelName;
    [Header("Stats")]
    [SerializeField] private int level;
    [SerializeField] private float baseDamage;
    [SerializeField] private float damagePerLevel;
    [SerializeField] private float baseEffect;
    [SerializeField] private float effectPerLevel;

    [SerializeField] private int projectileId;
    [SerializeField] private float projectileLifetime;
    [SerializeField] private float projectileSpeed;
    [SerializeField] private float projectileDamage;
    [SerializeField] private float projectileEffect;
    [SerializeField] private GameObject destroyEffect;
    [SerializeField] private bool hasDeathEffect;

    [SerializeField] private LayerMask enemy;
    private void Start()
    {
        LoadStats();

        StartCoroutine(BulletDestroy());
    }
    void FixedUpdate()
    {
        transform.Translate(Vector2.left * projectileSpeed * Time.deltaTime);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Enemy"))
        {
            if (projectileId == 0)
            {
                Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, projectileEffect, enemy);
                for (int i = 0; i < enemies.Length; i++)
                {
                    enemies[i].GetComponent<Enemy>().TakeDamage(projectileDamage);
                }
                DestroyObj();
            }
            else if (projectileId == 1)
            {
                collision.collider.GetComponent<Enemy>().TakeDamage(projectileDamage);
                collision.collider.GetComponent<Enemy>().Freeze(projectileEffect);
            }
        }
        if (collision.collider.CompareTag("Wall"))
        {
            DestroyObj();
        }
    }
    IEnumerator BulletDestroy()
    {
        yield return new WaitForSeconds(projectileLifetime);
        DestroyObj();
    }

    private void LoadStats()
    {
        if (PlayerPrefs.HasKey(projectileLevelName)) level = PlayerPrefs.GetInt(projectileLevelName);
        projectileDamage = baseDamage + damagePerLevel * level;
        projectileEffect = baseEffect + effectPerLevel * level;

    }
    private void DestroyObj()
    {
        if (hasDeathEffect) Instantiate(destroyEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
