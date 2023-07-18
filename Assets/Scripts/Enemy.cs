using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [Header("Enemy options")]
    public float basicHealth;
    public float basicDamage;
    public float moveSpeed;
    public float reloadTime;
    public float attackRange;
    public float basicGoldReward;
    public float basicExpReward;
    public bool isRange;
    public bool isBoss;

    [Header("Level dependent options")]
    public float healthPerLevelScale;
    public float damagePerLevelScale;

    [Header("Level dependent reward options")]
    public float goldPerLevelScale;
    public float expPerLevelScale;

    [Header("Enemy stats and rewards")]
    [SerializeField] private int enemyLevel;
    [SerializeField] private float maxHealth;
    [SerializeField] private float goldReward;
    [SerializeField] private float expReward;
    [SerializeField] private float attackDamage;

    [Header("Other")]
    public int weaponOffset;
    public GameObject projectile;
    public Transform attackPos;
    public GameObject Bar;
    public Image healthBar;

    public Text goldRewardText;
    public GameObject UIObj;
    [SerializeField] private LayerMask layerPlayer;

    private Vector2 moveDirection;
    private Player player;
    private Money playerMoney;
    private Rigidbody2D rb;
    private Animator anim;
    private float curHealth;
    private float timeToAttack;
    private float defaultFreezeTime = 2f;

    private bool run = false;
    
    private bool isAttacking = false;
    private bool isFrozen = false;
    private bool barActive = false;
    private bool alive = true;
    private bool facingRight;
    

    private void Start()
    {
        enemyLevel = WaveManager._instance.GetCurrentWave();
        goldReward = basicGoldReward + goldPerLevelScale * enemyLevel;
        expReward = basicExpReward + expPerLevelScale * enemyLevel;
        maxHealth = basicHealth + healthPerLevelScale * enemyLevel;
        attackDamage = basicDamage + damagePerLevelScale * enemyLevel;
        curHealth = maxHealth;
        timeToAttack = 0f;
        player = FindObjectOfType<Player>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        playerMoney = player.GetComponent<Money>();
        goldRewardText.text = Mathf.Round(goldReward).ToString();

        if (isRange) StartCoroutine(RangeEnemy());
    }

    private void FixedUpdate()
    {
        if (!isRange)
        {
            if (timeToAttack <= 0 && alive && !isFrozen)
            {
                moveDirection = (player.transform.position - transform.position).normalized;
                rb.MovePosition(rb.position + moveDirection * moveSpeed * Time.fixedDeltaTime);
                anim.SetBool("isRunning", true);
            }
            else if (alive && !isFrozen)
            {
                rb.velocity = new Vector2(0f, 0f);
                anim.SetBool("isRunning", false);
            }
            else if (isFrozen) rb.velocity = new Vector2(0f, 0f);
        }
        else if (isRange)
        {
            if (run && alive && !isFrozen)
            {
                moveDirection = (player.transform.position - transform.position).normalized;
                rb.MovePosition(rb.position + moveDirection * moveSpeed * Time.fixedDeltaTime);
                anim.SetBool("isRunning", true);
            }
            else if (alive && !isFrozen)
            {
                rb.velocity = new Vector2(0f, 0f);
                anim.SetBool("isRunning", false);
            }
            else if (isFrozen) rb.velocity = new Vector2(0f, 0f);
        }

        if (!facingRight && moveDirection.x > 0 && alive && !isFrozen)
        {
            Flip();
        }
        else if (facingRight && moveDirection.x < 0 && alive && !isFrozen)
        {
            Flip();
        }

        if (timeToAttack > 0) timeToAttack -= Time.deltaTime;
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isRange && collision.gameObject.tag == "Player" && alive && !isFrozen) StartCoroutine(Attack());
    }
    public void OnCollisionExit2D(Collision2D collision)
    {
        if (!isRange && collision.gameObject.tag == "Player" && alive) StopAllCoroutines();
    }

    IEnumerator Attack()
    {
        while (true)
        {
            if (!isAttacking)
            {
                timeToAttack = reloadTime;
                anim.SetTrigger("attack");
                isAttacking = true;
            }
            yield return new WaitForSeconds(reloadTime);
        }
    }
    public void AttackOver()
    {
        isAttacking = false; 
    }

    IEnumerator RangeEnemy()
    {
        while (true)
        {
            if (Vector2.Distance(transform.position, player.transform.position) <= attackRange)
            {
                run = false;
                if (!isAttacking)
                {
                    anim.SetTrigger("attack");
                    isAttacking = true;
                }
            }
            else if (Vector2.Distance(transform.position, player.transform.position) > attackRange)
            {
                run = true;               
            }
            yield return new WaitForSeconds(reloadTime);
        }
        
    }
    private void SpawnProjectile()
    {
        Vector2 difference = player.transform.position - attackPos.position;
        difference.y += 0.3f;
        float rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg - weaponOffset;
        Instantiate(projectile, transform.position, Quaternion.Euler(0f, 0f, rotZ));
    }

    public void DamagePlayer()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(attackPos.position, attackRange, layerPlayer);
        if (enemies.Length > 0)
        {
            player.TakeDamage(attackDamage);
        }         
    }

    public void TakeDamage(float damage)
    {
        if (!barActive)
        {
            Bar.SetActive(true);
            barActive = true;
        }
        curHealth -= damage;
        healthBar.fillAmount = curHealth / maxHealth;

        if (curHealth <= 0 && alive)
        {
            playerMoney.AddGold(goldReward);
            playerMoney.AddExp(expReward);
            if (isBoss) playerMoney.AddGems(1);
            alive = false;
            anim.SetTrigger("death");
        }
    }
    public void Freeze(float duration)
    {
        float animSpeed = defaultFreezeTime / duration;
        anim.speed = animSpeed;
        anim.SetTrigger("freeze");
        isFrozen = true;
    }
    private void UnFreeze()
    {
        isFrozen = false;
        anim.speed = 1;

        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, 3f, layerPlayer);
        if (enemies.Length > 0)
        {
            StartCoroutine(Attack());
        }
    }
    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 UIScaler = UIObj.transform.localScale;
        Vector3 Scaler = transform.localScale;
        Scaler.x *= -1;
        UIScaler.x *= -1;
        transform.localScale = Scaler;
        UIObj.transform.localScale = UIScaler;
    }

    private void Destroy()
    {
        GlobalEventManager.SendEnemyKilled();
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }
}
