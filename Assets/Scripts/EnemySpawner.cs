using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemyPrefabs;
    public Transform[] spawnPoints;
    public float spawnInterval = 3f;
    public int maxEnemiesAlive = 10;
    private List<GameObject> aliveEnemies = new List<GameObject>();
    private Coroutine spawnRoutine;

    private void Start()
    {
        StartSpawning();
    }

    public void StartSpawning()
    {
        if (spawnRoutine == null)
        {
            spawnRoutine = StartCoroutine(SpawnLoop());
        }
    }

    public void StopSpawning()
    {
        if (spawnRoutine != null)
        {
            StopCoroutine(spawnRoutine);
            spawnRoutine = null;
        }
    }

    private IEnumerator SpawnLoop()
    {
        while (true)
        {
            if (aliveEnemies.Count < maxEnemiesAlive)
            {
                SpawnEnemy();
            }
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    public void SpawnEnemy()
    {
        GameObject prefabToSpawn = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject newEnemy = Instantiate(prefabToSpawn, spawnPoint.position, spawnPoint.rotation);
        aliveEnemies.Add(newEnemy);
    }

}
