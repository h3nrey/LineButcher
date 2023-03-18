using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyWave
{
    public int totalOfEnemies;
    public Enemy[] enemiesPrefab;
    public float spawnRate;
}
