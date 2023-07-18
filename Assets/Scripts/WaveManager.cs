using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveManager : MonoBehaviour
{
    public GameObject[] enemies;
    public GameObject[] bosses;
    public GameObject[] aliveEnemies;
    public Transform[] spawnPoints;
    public GameObject wavesPanel;
    public GameObject[] WaveButtons;
    public Text[] WaveButtonsText;
    public GameObject arenaWall;

    [Header("Battle time bar")]
    public GameObject timeToEndBar;
    public Image timeToEndImage;
    public Text timeToEndWaveText;

    [Header("Battle time and enemies spawn rate")]
    [SerializeField] private int battleTime;
    [SerializeField] private float spawnRate;
    [SerializeField] private int minEnemySpawn;
    [SerializeField] private int maxEnemySpawn;
    [SerializeField] private int bossSpawnChance;
  
    private int maxWave;
    private int currentWave;
    private int remainingEnemies;
    private float remainingBattleTime;

    private bool battle;
    public static WaveManager _instance;

    private void Awake()
    {
        _instance = this;
    }

    void Start()
    {
        LoadMaxWave();
        RedrawWavesButtons();
        GlobalEventManager.OnEnemyKilled.AddListener(EnemyKilled);
        GlobalEventManager.OnPlayerDeath.AddListener(LoseWave);
    }
    public void StartWave(int wave)
    {       
        currentWave = maxWave - wave;
        Debug.Log("Start" + currentWave + "wave");
        GlobalEventManager.SendBattleStarted();
        timeToEndBar.SetActive(true);
        arenaWall.SetActive(true);
        timeToEndWaveText.text = "Wave: " + currentWave.ToString();
        remainingBattleTime = battleTime;
        battle = true;
        remainingEnemies = 0;
        StartCoroutine(TimeBar());
        StartCoroutine(SpawnEnemies());
        StartCoroutine(StopSpawn());
    }
    IEnumerator SpawnEnemies()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnRate);
            int enemiesSpawnCount = Random.Range(minEnemySpawn, maxEnemySpawn);
            SpawnCommonEnemies(enemiesSpawnCount);
          
        }
    }
    IEnumerator StopSpawn()
    {
        yield return new WaitForSeconds(battleTime);
        if (currentWave % 5 == 0) SpawnBoss();
        else SpawnCommonEnemies(1);

        int randomNumber = Random.Range(0, 100);
        if (randomNumber < bossSpawnChance) SpawnBoss();

        timeToEndBar.SetActive(false);
        battle = false;
        StopAllCoroutines();
    }
    IEnumerator TimeBar()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            remainingBattleTime -= 0.1f;
            timeToEndImage.fillAmount = remainingBattleTime / battleTime;
        }
    }
    public void EnemyKilled()
    {
        remainingEnemies -= 1;
        if (!battle && remainingEnemies <= 0)
        {
            WinWave();
        }
    }
    private void WinWave()
    {
        Debug.Log("Niiice niiice you win this wave");
        GlobalEventManager.SendBattleEnded();
        if (currentWave == maxWave)
        {
            maxWave += 1;
            FindObjectOfType<Money>().AddGems(1);
            PlayerPrefs.SetInt("MaxWave", maxWave);
            RedrawWavesButtons();
        }
        else
        {
            int gemChance = 100 / (maxWave - currentWave);
            int randomNumber = Random.Range(0, 100);
            if (randomNumber < gemChance)
            {
                FindObjectOfType<Money>().AddGems(1);
            }
        }
        wavesPanel.SetActive(true);
        arenaWall.SetActive(false);
    }
    public void LoseWave()
    {
        Debug.Log("Haha LOSER");
        GlobalEventManager.SendBattleEnded();
        timeToEndBar.SetActive(false);
        arenaWall.SetActive(false);
        battle = false;
        StopAllCoroutines();
        aliveEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in aliveEnemies)
        {
            Destroy(enemy);
        }
    }
    private void SpawnCommonEnemies(int n)
    {
        for (int i = 0; i < n; i++)
        {
            Instantiate(enemies[Random.Range(0, enemies.Length)], spawnPoints[Random.Range(0, spawnPoints.Length)].position, Quaternion.identity);
            GlobalEventManager.SendEnemySpawned();
            remainingEnemies += 1;
        }
    }
    private void SpawnBoss()
    {
        Instantiate(bosses[Random.Range(0, bosses.Length)], spawnPoints[Random.Range(0, spawnPoints.Length)].position, Quaternion.identity);
        GlobalEventManager.SendEnemySpawned();
        remainingEnemies += 1;
    }
    public int GetCurrentWave()
    {
        return currentWave;
    }
    private void LoadMaxWave()
    {
        if (PlayerPrefs.HasKey("MaxWave"))
        {
            maxWave = PlayerPrefs.GetInt("MaxWave");
        }
        else
        {
            maxWave = 1;
            PlayerPrefs.SetInt("MaxWave", 1);
        }
    }
    private void RedrawWavesButtons()
    {
        WaveButtonsText[0].text = "Start " + maxWave.ToString() + " wave (100% gem chance)";
        if (maxWave > 1)
        {
            WaveButtons[1].SetActive(true);
            WaveButtonsText[1].text = "Start " + (maxWave - 1).ToString() + " wave (100% gem chance)";
        }
        if (maxWave > 5)
        {
            WaveButtons[2].SetActive(true);
            WaveButtonsText[2].text = "Start " + (maxWave - 5).ToString() + " wave (20% gem chance)";
        }
        if (maxWave > 10)
        {
            WaveButtons[3].SetActive(true);
            WaveButtonsText[3].text = "Start " + (maxWave - 10).ToString() + " wave (10% gem chance)";
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            wavesPanel.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            wavesPanel.SetActive(false);
        }
    }
}
