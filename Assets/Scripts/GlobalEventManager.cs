using System;
using UnityEngine;
using UnityEngine.Events;
public class GlobalEventManager
{
    public static UnityEvent OnEnemySpawned = new UnityEvent();
    public static UnityEvent OnEnemyKilled = new UnityEvent();
    public static UnityEvent OnPlayerDeath = new UnityEvent();
    public static UnityEvent OnBattleStart = new UnityEvent();
    public static UnityEvent OnBattleEnd = new UnityEvent();
    public static UnityEvent OnWeaponUpgrade = new UnityEvent();

    void Start()
    {
        
    }
    public static void SendEnemySpawned()
    {
        OnEnemySpawned.Invoke();
    }
    public static void SendEnemyKilled()
    {
        OnEnemyKilled.Invoke();
    }
    public static void SendPlayerDied()
    {
        OnPlayerDeath.Invoke();
    }
    public static void SendBattleStarted()
    {
        OnBattleStart.Invoke();
    }
    public static void SendBattleEnded()
    {
        OnBattleEnd.Invoke();
    }
    public static void SendWeaponUpgraded()
    {
        OnWeaponUpgrade.Invoke();
    }
}
