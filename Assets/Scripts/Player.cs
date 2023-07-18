using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [Header("Player stats")]
    public float basicHealth;
    public float curHealth;
    public float moveSpeed;
    public float basicHealthRegen;

    [Header("Character upgrades")]
    [SerializeField] private string[] characterUpgradeName;
    [SerializeField] private int[] characterUpgradeLevel;
    [SerializeField] private float[] characterStatPerLevel;
    [SerializeField] private float healthRegen;

    public enum ControlType {WASD, Joystick};
    public ControlType controlType;
    public Joystick joystick;
    public GameObject[] Weapons;
    public GameObject shield;
    public GameObject shopPanel;
    public GameObject magicShopPanel;
    public GameObject DeathPanel;
    public Transform rangeWeaponPos;
    public Image healthBar;
    public Text hpText;

    private float maxHealth;
    private float effectiveDamage;

    private int equipedRangeWeaponId;
    private int selectedWeapon;
    private Rigidbody2D rb;
    private Animator anim;
    private Vector2 spawnPoint;
    private Vector2 moveInput;
    private Vector2 moveVelocity;
    private bool facingRight;
    private bool alive = true;
    void Start()
    {
        LoadWeapon();
        LoadCharacterUpgrades();
        //if (!Application.isEditor)
        //{
            Application.targetFrameRate = 60;
        //}   
        spawnPoint = transform.position;
        curHealth = maxHealth;
        hpText.text = curHealth.ToString() + " / " + maxHealth.ToString();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        StartCoroutine(RegenerateHealth());
        GlobalEventManager.OnBattleEnd.AddListener(RestoreHealth);    
    }

    void Update()
    {

        if (Application.isEditor)
        {
            moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        }
        else if (!Application.isEditor)
        {
            moveInput = new Vector2(joystick.Horizontal, joystick.Vertical);
        }
        moveVelocity = moveInput.normalized * moveSpeed;



        if (moveInput == new Vector2(0, 0)) 
        {
            anim.SetBool("isRunning", false);
        }
        else
        {
            anim.SetBool("isRunning", true);
        }  
    }
    private void FixedUpdate()
    {
        if (alive) rb.MovePosition(rb.position + moveVelocity * Time.fixedDeltaTime);
        if (!facingRight && moveInput.x > 0)
        {
            Flip();
        }
        else if (facingRight && moveInput.x < 0)
        {
            Flip();
        }       
    }
    void Flip() //поворот персонажа
    {
        facingRight = !facingRight;
        Vector3 Scaler = transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler;
    }
    
    public void UseWeapon()
    {
        equipedRangeWeaponId = PlayerPrefs.GetInt("EquipedWeaponID");
        Weapons[selectedWeapon].SetActive(false);
        shield.SetActive(false);
        Weapons[equipedRangeWeaponId].SetActive(true);
        selectedWeapon = equipedRangeWeaponId;
        
    }
    public void SwitchWeapon() //смена меча не пушку
    {
        if (selectedWeapon == 0)
        {
            Weapons[selectedWeapon].SetActive(false);
            shield.SetActive(false);
            Weapons[equipedRangeWeaponId].SetActive(true);
            selectedWeapon = equipedRangeWeaponId;
            PlayerPrefs.SetInt("SelectedWeapon", equipedRangeWeaponId);
        }
        else
        {
            Weapons[selectedWeapon].SetActive(false);
            Weapons[0].SetActive(true);
            shield.SetActive(true);
            selectedWeapon = 0;
            PlayerPrefs.SetInt("SelectedWeapon", 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Shop")) shopPanel.SetActive(true);
        else if (collision.CompareTag("MagicShop")) magicShopPanel.SetActive(true);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Shop")) shopPanel.SetActive(false);
        else if (collision.CompareTag("MagicShop")) magicShopPanel.SetActive(false);       
    }
    public void TakeDamage(float damage)
    {
        if (selectedWeapon == 0)
        {
            effectiveDamage = damage * 0.3f;
        }
        else
        {
            effectiveDamage = damage;
        }
        curHealth -= effectiveDamage;
        if (curHealth <= 0f && alive)
        {
            alive = false;
            GlobalEventManager.SendPlayerDied();
            curHealth = 0f;
            DeathPanel.SetActive(true);
        }       
        RedrawHealthBar();
    }
    public void HealHealth(float heal)
    {
        if (alive)
        {
            if (curHealth + heal >= maxHealth)
            {
                curHealth = maxHealth;
                RedrawHealthBar();
            }
            else
            {
                curHealth += heal;
                RedrawHealthBar();
            }
        }
    }
    public void RestoreHealth()
    {
        curHealth = maxHealth;
        RedrawHealthBar();
    }
    public void ReturnToTown()
    {
        transform.position = spawnPoint;
        alive = true;
        RestoreHealth();
    }
    IEnumerator RegenerateHealth()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            HealHealth(healthRegen);
        }
    }
    private void RedrawHealthBar()
    {
        healthBar.fillAmount = curHealth / maxHealth;
        hpText.text = Mathf.Round(curHealth).ToString() + " / " + maxHealth.ToString();
    }
    private void LoadWeapon()
    {
        if (PlayerPrefs.HasKey("EquipedWeaponID"))
        {
            selectedWeapon = PlayerPrefs.GetInt("EquipedWeaponID");
            equipedRangeWeaponId = PlayerPrefs.GetInt("EquipedWeaponID");
            Weapons[selectedWeapon].SetActive(true);
        }
        else
        {
            PlayerPrefs.SetInt("SelectedWeapon", 0);
            selectedWeapon = 0;
            equipedRangeWeaponId = 1;
            Weapons[selectedWeapon].SetActive(true);
            shield.SetActive(true);

        }
    }
    public void LoadCharacterUpgrades()
    {
        for (int i = 0; i < characterUpgradeName.Length; i++)
        {
            if (PlayerPrefs.HasKey(characterUpgradeName[i])){
                characterUpgradeLevel[i] = PlayerPrefs.GetInt(characterUpgradeName[i]);
            }
            else
            {
                characterUpgradeLevel[i] = 0;
                PlayerPrefs.SetInt(characterUpgradeName[i], characterUpgradeLevel[i]);
            }
        }
        maxHealth = basicHealth + characterStatPerLevel[0] * characterUpgradeLevel[0];
        RedrawHealthBar();
        RestoreHealth();
        healthRegen = basicHealthRegen + characterStatPerLevel[1] * characterUpgradeLevel[1];
    }
}
