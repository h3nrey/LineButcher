using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject[] enemiesPrefab;
    [SerializeField] Transform[] spawnPoints;
    [SerializeField] private Transform lastSpawnPoint;
    [SerializeField] float spawnRate;
    [SerializeField] int maxSpawn;
    public int currentActiveEnemies;


    private void Start() {
        InvokeRepeating("SpawnEnemy", 0.3f, spawnRate);
    }

    private void SpawnEnemy() {
        if(currentActiveEnemies < maxSpawn) {
            GameObject eemyPrefab = enemiesPrefab[Random.Range(0, enemiesPrefab.Length)];
            Transform currentSpawnPoint = RandomSpawnPoint();
            GameObject currentEnemy = Instantiate(eemyPrefab, currentSpawnPoint.position, currentSpawnPoint.rotation) as GameObject;
            currentEnemy.GetComponent<SnakeBehaviour>().direction = currentSpawnPoint.right;
            currentActiveEnemies++;
        }
        
    }

    private Transform RandomSpawnPoint() {
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        while (lastSpawnPoint == spawnPoint) {
            spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

            if (spawnPoint != lastSpawnPoint) {
                break;
            }
        }

        lastSpawnPoint = spawnPoint;
        return spawnPoint;
    }
}
