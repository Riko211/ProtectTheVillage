using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public Joystick joystick;

    public int weaponID;
    public float offset;
    public GameObject bullet;
    public Transform shotPoint;
    public Transform playerDir;
    public float reloadTime;
    public float spread;
    public int projectileCount;

    public float searchRange;
    public LayerMask enemy;


    private float timeToShot;
    private Vector2 difference;
    private float rotZ;
    private Vector3 Scaler;
    private float ScaleY, ScaleX;
    private bool isPressed;

    private Vector3 viewPoint;

    private void Start()
    {
        Scaler = transform.localScale;
        ScaleY = transform.localScale.y;
        ScaleX = transform.localScale.x;
        transform.rotation = Quaternion.Euler(0f, 0f,offset);

    }
    void FixedUpdate()
    {
        //viewPoint = FindNearestEnemy();
        viewPoint = new Vector2(transform.position.x + joystick.Horizontal, transform.position.y + joystick.Vertical);
        if (joystick.Horizontal != 0 || joystick.Vertical != 0)
        {
            difference = viewPoint - transform.position;
            rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, rotZ + offset);
        }
        else
        {
            if (playerDir.localScale.x > 0)
            {
                rotZ = 180;
                transform.rotation = Quaternion.Euler(0f, 0f, rotZ + offset);
            }
            else
            {
                rotZ = 0;
                transform.rotation = Quaternion.Euler(0f, 0f, rotZ + offset);
            }
        }
        
        
        if (rotZ> -90 && rotZ < 90)
        {
            Scaler.y = -ScaleY;
            transform.localScale = Scaler;
        }
        else
        {
            Scaler.y = ScaleY;
            transform.localScale = Scaler;
        }

        
        if (playerDir.localScale.x < 0)
        {
            Scaler.x = -ScaleX;
            transform.localScale = Scaler;
        }
        else
        {
            Scaler.x = ScaleX;
            transform.localScale = Scaler;
        }
        

        /*
        touchPosition = Input.mousePosition;
        if (Input.GetMouseButtonDown(0))
        {
            isPressed = true;
        }
        */

        if (timeToShot <= 0 && (joystick.Horizontal != 0 || joystick.Vertical != 0))
        {
            Shoot(projectileCount);
            timeToShot = reloadTime;
            isPressed = false;
            
        }
        else
        {
            timeToShot -= Time.deltaTime;
        }
    }

    private void Shoot(int projectiles)
    {
        for (int i = 0; i < projectileCount; i++)
        {
            Instantiate(bullet, shotPoint.position, Quaternion.Euler(0f, 0f, rotZ + offset + Random.Range(-spread, spread)));
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
            return new Vector3(0f ,0f, 0f);
        }
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, searchRange);

        Gizmos.DrawWireSphere(shotPoint.transform.position, 0.1f);
    }
}
