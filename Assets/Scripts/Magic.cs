using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Magic : MonoBehaviour
{
    [SerializeField] private GameObject[] magicProj;
    [SerializeField] private float searchRange;
    [SerializeField] private LayerMask enemy;

    [SerializeField] private float fireballCooldown;
    [SerializeField] private float iceSpearsCooldown;
    [SerializeField] private Image[] cdImage;

    private float fireballTimeToActive;
    private float iceSpearsTimeToActive;
    private Vector3 enemyPos;
    private Vector2 difference;
    private float rotZ;
    private float offset = -180;
    private void Start()
    {
        fireballTimeToActive = 0f;
    }
    public void CastFireball()
    {
        if (fireballTimeToActive <= 0)
        {
            enemyPos = FindNearestEnemy();
            if (enemyPos != new Vector3(0f, 0f, 0f))
            {
                difference = enemyPos - transform.position;
                rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg - offset;
                Instantiate(magicProj[0], transform.position, Quaternion.Euler(0f, 0f, rotZ));
                fireballTimeToActive = fireballCooldown;
                StartCoroutine(FireballCD());
            }
        }
    }
    public void CastIceSpears()
    {
        if (iceSpearsTimeToActive <= 0)
        {
            enemyPos = FindNearestEnemy();
            if (enemyPos != new Vector3(0f, 0f, 0f))
            {
                difference = enemyPos - transform.position;
                rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg - offset;
                Instantiate(magicProj[1], transform.position, Quaternion.Euler(0f, 0f, rotZ));
                Instantiate(magicProj[1], transform.position, Quaternion.Euler(0f, 0f, rotZ + 10));
                Instantiate(magicProj[1], transform.position, Quaternion.Euler(0f, 0f, rotZ - 10));
                Instantiate(magicProj[1], transform.position, Quaternion.Euler(0f, 0f, rotZ + 20));
                Instantiate(magicProj[1], transform.position, Quaternion.Euler(0f, 0f, rotZ - 20));
                iceSpearsTimeToActive = iceSpearsCooldown;
                StartCoroutine(IceSpearsCD());
            }
        }
    }
    private Vector3 FindNearestEnemy()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, searchRange, enemy);
        float distanceToEnemy = Mathf.Infinity;
        int nearestEnemy = 0;
        if (enemies.Length > 0)
        {
            for (int i = 0; i < enemies.Length; i++)
            {
                if (Vector2.Distance(transform.position, enemies[i].transform.position) < distanceToEnemy)
                {
                    distanceToEnemy = Vector2.Distance(transform.position, enemies[i].transform.position);
                    nearestEnemy = i;
                }
            }
            return enemies[nearestEnemy].transform.position;
        }
        else
        {
            return new Vector3(0f, 0f, 0f);
        }
    }

    IEnumerator FireballCD()
    {
        while (fireballTimeToActive > 0)
        {
            fireballTimeToActive -= 0.1f;
            yield return new WaitForSeconds(0.1f);
            cdImage[0].fillAmount = fireballTimeToActive / fireballCooldown;
        } 
    }

    IEnumerator IceSpearsCD()
    {
        while (iceSpearsTimeToActive > 0)
        {
            iceSpearsTimeToActive -= 0.1f;
            yield return new WaitForSeconds(0.1f);
            cdImage[1].fillAmount = iceSpearsTimeToActive / iceSpearsCooldown;
        }

    }
}
