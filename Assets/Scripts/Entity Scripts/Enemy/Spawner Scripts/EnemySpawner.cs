using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private int waveId = 1;
    public int WaveId => waveId;
    [SerializeField]
    private float spawnDelaySeconds = 1.0f;
    private float curSpawnSeconds = 0.0f;
    [SerializeField]
    private float maxEnemies = Mathf.Infinity;
    private float enemiesSpawned = 0;

    //check if all enemies were destroyed
    private int deadCount = 0;
    public bool Exhausted => deadCount >= maxEnemies;

    [SerializeField]
    private List<GameObject> enemyPrefabs = new List<GameObject> { };

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, 0.5f);
    }

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
        if (enemyPrefabs.Count > 0)
        {
            if (!Exhausted)
            {
                Instantiate(enemyPrefabs[(int)enemiesSpawned++ % enemyPrefabs.Count], transform.position, Quaternion.identity, transform);
            }
        }
    }

    public void Report()
    {
        deadCount++;
    }
}
