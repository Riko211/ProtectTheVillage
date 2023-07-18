using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private Money playerMoney;

    public GameObject[] buyButton;
    public GameObject[] equipButton;
    public Text[] equipText;
    public GameObject[] upgradeButton;

    public Text[] weaponStatsText;
    public Text[] buyButtonText;

    public string[] weaponNames;
    public string[] weaponLevelNames;

    public float[] buyPrice;
    public int[] weaponLevel;
    public float[] defaultDamage;
    public float[] damagePerLevel;
    public float[] upgradePriceScale;
    public float[] defaultUpgradePrice;


    private void Start()
    {
        player = FindObjectOfType<Player>();
        playerMoney = player.GetComponent<Money>();
        LoadPurchasedWeapons();
        LoadWeaponStats();
    }

    public void BuyWeapon(int weaponId)
    {
        if (playerMoney.GetGold() >= buyPrice[weaponId])
        {
            playerMoney.AddGold(-buyPrice[weaponId]);
            buyButton[weaponId].SetActive(false);
            equipButton[weaponId].SetActive(true);
            upgradeButton[weaponId].SetActive(true);
            PlayerPrefs.SetInt(weaponNames[weaponId], 1);
        }
    }
    public void EquipWeapon(int weaponId)
    {
        int eqWeapon = PlayerPrefs.GetInt("EquipedWeaponID");
        equipButton[eqWeapon].GetComponent<Image>().color = Color.white;
        equipText[eqWeapon].text = "Equip";
        PlayerPrefs.SetInt("EquipedWeaponID", weaponId);
        equipButton[weaponId].GetComponent<Image>().color = Color.grey;
        equipText[weaponId].text = "Equipped";
        player.UseWeapon();
    }

    public void UpgradeWeapon(int weaponId)
    {
        float upgradeCost = Mathf.Round(defaultUpgradePrice[weaponId] * upgradePriceScale[weaponId] * weaponLevel[weaponId]);
        Debug.Log(upgradeCost);
        if (playerMoney.GetGold() >= upgradeCost)
        {
            Debug.Log("Upgrade" + " " + weaponId.ToString() + " " + upgradeCost);
            playerMoney.AddGold(-upgradeCost);
            weaponLevel[weaponId] += 1;
            PlayerPrefs.SetInt(weaponLevelNames[weaponId], weaponLevel[weaponId]);
            GlobalEventManager.SendWeaponUpgraded();
            RefreshWeaponStat(weaponId);
        }
    }

    public void RefreshWeaponStat(int weaponId)
    {
        if (weaponId == 0)
        {
            weaponStatsText[weaponId].text = "Damage: " + Mathf.Round(defaultDamage[weaponId] + weaponLevel[weaponId] * damagePerLevel[weaponId]).ToString() +
                                      "\nWeapon level: " + weaponLevel[weaponId] +
                                      "\nUpgrade cost: " + Mathf.Round(defaultUpgradePrice[weaponId] * upgradePriceScale[weaponId] * weaponLevel[weaponId]).ToString();
        }
        else
        {
            weaponStatsText[weaponId].text = "Projectile damage: " + Mathf.Round(defaultDamage[weaponId] + weaponLevel[weaponId] * damagePerLevel[weaponId]).ToString() +
                                      "\nWeapon level: " + weaponLevel[weaponId] +
                                      "\nUpgrade cost: " + Mathf.Round(defaultUpgradePrice[weaponId] * upgradePriceScale[weaponId] * weaponLevel[weaponId]).ToString();
        }
    }

    public float GetBulletDamage(int weaponId)
    {
        float bulletDamage = defaultDamage[weaponId] + weaponLevel[weaponId] * damagePerLevel[weaponId];
        return (bulletDamage);
    }

    private void LoadWeaponStats()
    {
        for (int i = 0; i < weaponLevelNames.Length; i++)
        {
            if (PlayerPrefs.HasKey(weaponLevelNames[i]))
            {
                weaponLevel[i] = PlayerPrefs.GetInt(weaponLevelNames[i]);
            }
            else
            {
                weaponLevel[i] = 1;
            }
        }

        for (int i = 0; i < weaponStatsText.Length; i++)  //урон, уровень, цена улучшения оружия
        {
            if (i == 0)
            {
                weaponStatsText[i].text = "Damage: " + Mathf.Round(defaultDamage[i] + weaponLevel[i] * damagePerLevel[i]).ToString() +
                                          "\nWeapon level: " + weaponLevel[i] +
                                          "\nUpgrade cost: " + Mathf.Round(defaultUpgradePrice[i] * upgradePriceScale[i] * weaponLevel[i]).ToString();
            }
            else
            {
                weaponStatsText[i].text = "Projectile damage: " + Mathf.Round(defaultDamage[i] + weaponLevel[i] * damagePerLevel[i]).ToString() +
                                          "\nWeapon level: " + weaponLevel[i] +
                                          "\nUpgrade cost: " + Mathf.Round(defaultUpgradePrice[i] * upgradePriceScale[i] * weaponLevel[i]).ToString();
            }
        }
    }
    private void LoadPurchasedWeapons()
    {
        for (int i = 0; i < buyButton.Length; i++) //кнопки покупки оружия
        {
            if (PlayerPrefs.HasKey(weaponNames[i]))
            {
                if (PlayerPrefs.GetInt(weaponNames[i]) == 1)
                {
                    buyButton[i].SetActive(false);
                    equipButton[i].SetActive(true);
                    upgradeButton[i].SetActive(true);
                }
            }
            else
            {
                PlayerPrefs.SetInt(weaponNames[i], 0);
            }
        }
        for (int i = 0; i < buyButton.Length; i++) //цены покупки оружия
        {
            buyButtonText[i].text = "Buy\n" + buyPrice[i].ToString() + "g";
        }

        int eqWeapon = PlayerPrefs.GetInt("EquipedWeaponID");
        equipText[eqWeapon].text = "Equiped";
        equipButton[eqWeapon].GetComponent<Image>().color = Color.grey;
    }
}
