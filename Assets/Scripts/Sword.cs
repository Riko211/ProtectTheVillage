using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    public Joystick joystick;

    public float basicSwordDamage;
    public float damagePerLevel;
    public float reloadTime;
    public Transform attackPos;
    public float swordRange;
    public float swordDamage;
    public Animator anim;
    public LayerMask enemy;

    private int swordLevel;
    private float timeToShot;
    private bool isPressed;
    private int selectedWeapon;


    private void Start()
    {
        LoadSwordDamage();
        GlobalEventManager.OnWeaponUpgrade.AddListener(UpdateSwordDamage);
        isPressed = false;
        timeToShot = 0f;
    }
    void Update()
    {
        SwordAttack();
        if (timeToShot > 0) timeToShot -= Time.deltaTime;
    }
    public void SwordAttack()
    {
        if (timeToShot <= 0 && isPressed)
        {
            anim.SetTrigger("attack");
            Collider2D[] enemies = Physics2D.OverlapCircleAll(attackPos.position, swordRange, enemy);
            for (int i = 0; i < enemies.Length; i++)
            {
                enemies[i].GetComponent<Enemy>().TakeDamage(swordDamage);
            }
            timeToShot = reloadTime;
        }
    }
    public void TouchDown()
    {
        isPressed = true;
    }
    public void TouchUp()
    {
        isPressed = false;
    }
    public void UpdateSwordDamage()
    {
        swordDamage = FindObjectOfType<Shop>().GetBulletDamage(0);
    }
    public void LoadSwordDamage()
    {
        if (PlayerPrefs.HasKey("swordlevel"))
        {
            swordLevel = PlayerPrefs.GetInt("swordlevel");
        }
        else
        {
            swordLevel = 1;
        }

        swordDamage = basicSwordDamage + swordLevel * damagePerLevel;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, swordRange);
    }
}
