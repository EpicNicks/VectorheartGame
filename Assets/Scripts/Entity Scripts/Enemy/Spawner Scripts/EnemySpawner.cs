using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private float spawnDelaySeconds = 1.0f;
    private float curSpawnSeconds = 0.0f;
    [SerializeField]
    private float maxEnemies = Mathf.Infinity;
    private float enemiesSpawned = 0;

    [SerializeField]
    private List<GameObject> enemyPrefabs = new List<GameObject> { };

    private void Awake()
    {
        if (enemyPrefabs.Count == 0)
        {
            Debug.LogWarning($"Spawner {gameObject.name} has no enemies attached to spawn!");
        }
    }

    private void Update()
    {
        if (enemiesSpawned <= maxEnemies)
        {
            if (curSpawnSeconds >= spawnDelaySeconds)
            {
                Spawn();
                curSpawnSeconds = 0.0f;
            }
            curSpawnSeconds += Time.deltaTime;
        }
    }

    private void Spawn()
    {
        Instantiate(enemyPrefabs[(int)enemiesSpawned++ % enemyPrefabs.Count], transform.position, Quaternion.identity, transform);
    }
}
