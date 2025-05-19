using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    [Header("Enemy")]
    public GameObject enemyPrefab;

    [Header("Spawners")]
    public List<Transform> spawnPoints;

    [Header("Wave Settings")]
    public int baseEnemiesPerSpawner = 3;
    public float timeBetweenWaves = 5f;
    public int maxWaves = 7;

    private int currentWave = 1;
    private int enemiesAlive = 0;
    private bool waitingNextWave = false;
    private bool gameWon = false;

    void Start()
    {
        StartWave();
    }

    void Update()
    {
        if (gameWon) return;

        if (enemiesAlive <= 0 && !waitingNextWave)
        {
            if (currentWave >= maxWaves)
            {
                WinGame();
                return;
            }

            waitingNextWave = true;
            Invoke(nameof(StartNextWave), timeBetweenWaves);
        }
    }

    void StartNextWave()
    {
        currentWave++;
        waitingNextWave = false;
        StartWave();
    }

    void StartWave()
    {
        int enemiesThisWave = baseEnemiesPerSpawner + (currentWave - 1); // aumenta a cada wave

        foreach (Transform spawnPoint in spawnPoints)
        {
            for (int i = 0; i < enemiesThisWave; i++)
            {
                Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
                enemiesAlive++;
            }
        }

        Debug.Log($"Wave {currentWave} iniciada com {enemiesThisWave * spawnPoints.Count} inimigos.");
    }

    public void EnemyDied()
    {
        enemiesAlive--;
    }

    void WinGame()
    {
        gameWon = true;
        Debug.Log("Você venceu o jogo!");
        // Aqui pode ativar UI de vitória ou carregar cena de vitória
    }
}
