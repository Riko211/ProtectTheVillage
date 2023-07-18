using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Money : MonoBehaviour
{
    public Text goldText;
    public Text gemsText;
    public Text expText;

    [SerializeField] private float gold;
    [SerializeField] private int gems;
    [SerializeField] private float exp;
    void Start()
    {
        LoadGold();
        LoadGems();
        LoadExp();
    }

    public void AddGold(float goldAmount)
    {
        gold += goldAmount;
        RedrawGold();
        PlayerPrefs.SetFloat("Gold", gold);
    }
    public void AddGems(int gemsAmount)
    {
        gems += gemsAmount;
        RedrawGems();
        PlayerPrefs.SetInt("Gems", gems);
    }
    public void AddExp(float expAmount)
    {
        exp += expAmount;
        RedrawExp();
        PlayerPrefs.SetFloat("Exp", exp);
    }

    private void LoadGold()
    {
        if (PlayerPrefs.HasKey("Gold"))
        {
            gold = PlayerPrefs.GetFloat("Gold");
            RedrawGold();
        }
        else
        {
            gold = 0f;
            PlayerPrefs.SetFloat("Gold", 0f);
            RedrawGold();
        }
    }
    private void LoadGems()
    {
        if (PlayerPrefs.HasKey("Gems"))
        {
            gems = PlayerPrefs.GetInt("Gems");
            RedrawGems();
        }
        else
        {
            gems = 0;
            PlayerPrefs.SetInt("Gems", 0);
            RedrawGems();
        }
    }
    private void LoadExp()
    {
        if (PlayerPrefs.HasKey("Exp"))
        {
            exp = PlayerPrefs.GetFloat("Exp");
            RedrawExp();
        }
        else
        {
            exp = 0f;
            PlayerPrefs.SetFloat("Exp", 0f);
            RedrawExp();
        }
    }
    private void RedrawGold()
    {
        goldText.text = Mathf.Round(gold).ToString();
    }
    private void RedrawGems()
    {
        gemsText.text = gems.ToString();
    }
    private void RedrawExp()
    {
        expText.text = Mathf.Round(exp).ToString();
    }
    public float GetGold()
    {
        return gold;
    }
    public int GetGems()
    {
        return gems;
    }
    public float GetExp()
    {
        return exp;
    }
}
