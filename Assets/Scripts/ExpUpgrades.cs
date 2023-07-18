using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExpUpgrades : MonoBehaviour
{
    
    [SerializeField] private string[] upgradeName;
    [SerializeField] private Text[] upgradeInfo;

    [SerializeField] private int[] upgradeLevel;
    [SerializeField] private int[] basicPrice;
    [SerializeField] private int[] pricePerLevel;
    [SerializeField] private int[] priceScale;
    [SerializeField] private float[] StatPerLevel;

    private Player player;
    private Money playerMoney;
    private void Start()
    {
        player = FindObjectOfType<Player>();
        playerMoney = player.GetComponent<Money>();
        LoadLevels();
        //for (int i = 0; i < upgradeName.Length; i++) PlayerPrefs.DeleteKey(upgradeName[i]);
    }
    public void UpgradeCharacterStat(int statId)
    {
        int upgradeCost = basicPrice[statId] + pricePerLevel[statId] * upgradeLevel[statId] * priceScale[statId];
        if (playerMoney.GetExp() >= upgradeCost)
        {
            playerMoney.AddExp(-upgradeCost);
            upgradeLevel[statId] += 1;
            PlayerPrefs.SetInt(upgradeName[statId], upgradeLevel[statId]);
            RedrawStatInfo(statId);
            player.LoadCharacterUpgrades();
        }
    }
    private void RedrawStatInfo(int statId)
    {
        upgradeInfo[statId].text = upgradeName[statId].ToString() + " upgrade\n\n" +
                                   upgradeName[statId].ToString() + ": +" + StatPerLevel[statId] * upgradeLevel[statId] +
                                   "\nUpgrade level: " + upgradeLevel[statId] +
                                   "\nUpgrade cost: " + (basicPrice[statId] + pricePerLevel[statId] * upgradeLevel[statId] * priceScale[statId]).ToString();
    }
    private void LoadLevels()
    {
        for (int i = 0; i < upgradeName.Length; i++)
        {
            if (PlayerPrefs.HasKey(upgradeName[i]))
            {
                upgradeLevel[i] = PlayerPrefs.GetInt(upgradeName[i]);
                upgradeInfo[i].text = upgradeName[i].ToString() + " upgrade\n\n" +
                                      upgradeName[i].ToString() + ": +" + StatPerLevel[i] * upgradeLevel[i] +
                                      "\nUpgrade level: " + upgradeLevel[i] +
                                      "\nUpgrade cost: " + (basicPrice[i] + pricePerLevel[i] * upgradeLevel[i] * priceScale[i]).ToString();
            }
            else
            {
                upgradeLevel[i] = 0;
                PlayerPrefs.SetInt(upgradeName[i], upgradeLevel[i]);
                upgradeInfo[i].text = upgradeName[i].ToString() + " upgrade\n\n" +
                                      upgradeName[i].ToString() + ": +" + StatPerLevel[i] * upgradeLevel[i] +
                                      "\nUpgrade level: " + upgradeLevel[i] +
                                      "\nUpgrade cost: " + (basicPrice[i] + pricePerLevel[i] * upgradeLevel[i] * priceScale[i]).ToString();
            }
        }
    }
}
