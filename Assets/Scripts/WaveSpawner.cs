using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    [Header("Enemy")]
    public GameObject enemyPrefab;

    [Header("Spawners")]
    public List<Transform> spawnPoints;

    [Header("Wave Settings")]
    public int enemiesPerSpawner = 3;
    public float timeBetweenWaves = 5f;

    private int enemiesAlive = 0;
    private int currentWave = 1;
    private bool waitingNextWave = false;

    void Start()
    {
        StartWave();
    }

    void Update()
    {
        if (enemiesAlive <= 0 && !waitingNextWave)
        {
            waitingNextWave = true;
            Invoke(nameof(StartNextWave), timeBetweenWaves);
        }
    }

    void StartNextWave()
    {
        currentWave++;
        enemiesPerSpawner += 1;
        StartWave();
        waitingNextWave = false;
    }

    void StartWave()
    {
        foreach (Transform spawnPoint in spawnPoints)
        {
            for (int i = 0; i < enemiesPerSpawner; i++)
            {
                Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
                enemiesAlive++;
            }
        }
    }

    public void EnemyDied()
    {
        enemiesAlive--;
    }
}
