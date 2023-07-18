using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MagicShop : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private Money playerMoney;

    [Header("Names")]
    [SerializeField] private string[] magicNames;
    [SerializeField] private string[] magicLevelNames;

    [Header("Buttons")]
    [SerializeField] private GameObject[] skillButton;
    [SerializeField] private GameObject[] buyButton;
    [SerializeField] private Text[] buyButtonText;
    [SerializeField] private GameObject[] upgradeButton;
    [SerializeField] private Text[] upgradeButtonText;

    [Header("Info")]
    [SerializeField] private Text[] magicInfoText; 
    [SerializeField] private string[] magicEffect;
    [SerializeField] private string[] magicName;
    
    [Header("Stats")]
    [SerializeField] private float[] baseDamage;
    [SerializeField] private float[] damagePerLevel;
    [SerializeField] private float[] baseEffect;
    [SerializeField] private float[] effectPerLevel;

    [SerializeField] private int[] magicLevel;
    [SerializeField] private int[] buyPrice;
    void Start()
    {
        player = FindObjectOfType<Player>();
        playerMoney = player.GetComponent<Money>();

        LoadPurchasedMagic();
        LoadMagicLevels();
    }

    public void BuyMagic(int magicId)
    {
        if (playerMoney.GetGems() >= buyPrice[magicId])
        {
            playerMoney.AddGems(-buyPrice[magicId]);
            buyButton[magicId].SetActive(false);
            upgradeButton[magicId].SetActive(true);
            skillButton[magicId].SetActive(true);
            PlayerPrefs.SetInt(magicNames[magicId], 1);
        }
    }

    public void UpgradeMagic(int magicId)
    {
        int upgradeCost = 1 + Mathf.RoundToInt(magicLevel[magicId] / 100);
        if (playerMoney.GetGems() >= upgradeCost)
        {
            Debug.Log("Upgrade magic " + magicId.ToString() + " " + upgradeCost);
            playerMoney.AddGems(-upgradeCost);
            magicLevel[magicId] += 1;
            PlayerPrefs.SetInt(magicLevelNames[magicId], magicLevel[magicId]);
            upgradeButtonText[magicId].text = "Upgrade\n" + upgradeCost.ToString() + " gems";
            magicInfoText[magicId].text = magicName[magicId].ToString() +
                                          "\n\nLevel: " + magicLevel[magicId].ToString() +
                                          "\nDamage: " + (baseDamage[magicId] + damagePerLevel[magicId] * (magicLevel[magicId] - 1)).ToString() +
                                          "\n" + magicEffect[magicId].ToString() + ": " + (baseEffect[magicId] + effectPerLevel[magicId] * (magicLevel[magicId] - 1)).ToString();
        }
    }

    private void LoadPurchasedMagic()
    {
        for (int i = 0; i < buyButton.Length; i++)
        {
            if (PlayerPrefs.HasKey(magicNames[i]))
            {
                if (PlayerPrefs.GetInt(magicNames[i]) == 1)
                {
                    buyButton[i].SetActive(false);
                    upgradeButton[i].SetActive(true);
                    skillButton[i].SetActive(true);
                }
            }
            else  PlayerPrefs.SetInt(magicNames[i], 0);            
        }
        for (int i = 0; i < buyButton.Length; i++)
        {
            buyButtonText[i].text = "Buy\n" + buyPrice[i].ToString() + " gems";
        }
    }

    private void LoadMagicLevels()
    {
        for (int i = 0; i < magicLevelNames.Length; i++)
        {
            if (PlayerPrefs.HasKey(magicLevelNames[i])) magicLevel[i] = PlayerPrefs.GetInt(magicLevelNames[i]);        
            else magicLevel[i] = 1;
            int upgradeCost = 1 + Mathf.RoundToInt(magicLevel[i] / 100);
            upgradeButtonText[i].text = "Upgrade\n" + upgradeCost.ToString() + " gems";
        }

        for (int i = 0; i < magicInfoText.Length; i++)
        {
               magicInfoText[i].text = magicName[i].ToString() +
                                       "\n\nLevel: " + magicLevel[i].ToString() +
                                       "\nDamage: " + (baseDamage[i] + damagePerLevel[i] * (magicLevel[i] - 1)).ToString() +
                                       "\n" + magicEffect[i].ToString() + ": " + (baseEffect[i] + effectPerLevel[i] * (magicLevel[i] - 1)).ToString();
        }
    }
}
