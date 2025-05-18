using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float spawnInterval = 5f;
    public Transform[] spawnPoints;

    private float timer = 0f;
    private int wave = 1;

    void Start()
    {
        timer = spawnInterval;
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            SpawnEnemies();
            timer = spawnInterval;
        }
    }

    void SpawnEnemies()
    {
        int enemiesToSpawn = Mathf.Clamp(wave, 1, 10); // aumenta com o tempo

        for (int i = 0; i < enemiesToSpawn; i++)
        {
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);

            // Aumentar dificuldade com o tempo
            EnemyController ec = enemy.GetComponent<EnemyController>();
            if (ec != null)
            {
                ec.moveSpeed += wave * 0.1f;
                ec.damage += wave / 3;
                ec.TakeDamage(-wave); // aumenta a vida inicial
            }
        }

        wave++; // próxima wave será maior
    }
}
