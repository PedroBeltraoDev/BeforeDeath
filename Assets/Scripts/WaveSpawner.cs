using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Configurações")]
    [SerializeField] private GameObject inimigoPrefab; // Prefab do inimigo a ser spawnado
    [SerializeField] private Transform[] spawnPoints;  // Lista de pontos de spawn
    [SerializeField] private float intervaloSpawn = 5f; // Tempo entre spawns

    private void Start()
    {
        StartCoroutine(SpawnarInimigos());
    }

    private IEnumerator SpawnarInimigos()
    {
        while (true)
        {
            foreach (Transform spawnPoint in spawnPoints)
            {
                SpawnarInimigo(spawnPoint);
            }
            yield return new WaitForSeconds(intervaloSpawn);
        }
    }

    private void SpawnarInimigo(Transform ponto)
    {
        Instantiate(inimigoPrefab, ponto.position, ponto.rotation);
    }
}
