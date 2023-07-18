using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyCounter : MonoBehaviour
{
    public GameObject remainingEnemies;
    public Text remainingEnemiesText;

    private int enemyCount;

    private void Start()
    {
        GlobalEventManager.OnEnemySpawned.AddListener(AddEnemyToCounter);
        GlobalEventManager.OnEnemyKilled.AddListener(RemoveEnemyFromCounter);
        GlobalEventManager.OnBattleStart.AddListener(TurnOnCounter);
        GlobalEventManager.OnBattleEnd.AddListener(TurnOffCounter);
    }

    private void AddEnemyToCounter()
    {
        enemyCount += 1;
        remainingEnemiesText.text = "Remaining enemies: " + enemyCount.ToString();
    }
    private void RemoveEnemyFromCounter()
    {
        enemyCount -= 1;
        remainingEnemiesText.text = "Remaining enemies: " + enemyCount.ToString();
    }
    private void TurnOnCounter()
    {
        remainingEnemies.SetActive(true);
        enemyCount = 0;
        remainingEnemiesText.text = "Remaining enemies: " + enemyCount.ToString();
    }
    private void TurnOffCounter()
    {
        remainingEnemies.SetActive(false);
    }
}
