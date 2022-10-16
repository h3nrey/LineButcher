using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] Transform[] spawnPoints;
    [SerializeField] private Transform lastSpawnPoint;
    [SerializeField] EnemyWave[] waves;
    private int currentWaveIndex = 0;
    private EnemyWave currentWave;
    [SerializeField] float spawnRate;
    [SerializeField] int maxSpawn;
    public int currentSpawnedEnemies;
    [SerializeField] float cooldownBetweenWaves;

    //Coroutines
    Coroutine instatiateEnemy;


    private void Start() {
        currentWave = waves[0];
        instatiateEnemy = StartCoroutine(HandleWaves());
    }

    IEnumerator HandleWaves() {
        for (int i = 0; i < waves.Length; i++) {
            currentWave = waves[i];
            while (waves[i].totalOfEnemies > currentSpawnedEnemies) {
                yield return new WaitForSeconds(waves[i].spawnRate);
                SpawnEnemy();

                if(waves[i].totalOfEnemies == currentSpawnedEnemies) {
                    currentSpawnedEnemies = 0;
                    break;
                }
            }
            yield return new WaitForSeconds(cooldownBetweenWaves);
        }
        yield break;
    }

    private void SpawnEnemy() {
        GameObject eemyPrefab = currentWave.enemiesPrefab[Random.Range(0, currentWave.enemiesPrefab.Length)];
        Transform currentSpawnPoint = RandomSpawnPoint();
        GameObject currentEnemy = Instantiate(eemyPrefab, currentSpawnPoint.position, currentSpawnPoint.rotation) as GameObject;
        currentEnemy.GetComponent<SnakeBehaviour>().direction = currentSpawnPoint.right;
        currentSpawnedEnemies++;
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
