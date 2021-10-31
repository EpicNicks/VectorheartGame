using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnerMaster : MonoBehaviour
{
    private bool waveStarted = false;
    private List<EnemySpawner> spawners;

    [SerializeField]
    [Tooltip("The amount of seconds all spawners will wait before starting to spawn")]
    private float waveDelaySeconds = 3.0f;
    private float curDelaySeconds = 0.0f;

    private void Awake()
    {
        spawners = FindObjectsOfType<EnemySpawner>().ToList();
        spawners.ForEach(s => s.enabled = false);
    }

    private void Update()
    {
        if (curDelaySeconds >= waveDelaySeconds)
        {
            EnableSpawners();
        }
    }


    private void EnableSpawners()
    {
        if (!waveStarted)
        {
            waveStarted = true;
            spawners.ForEach(s => s.enabled = true);
        }
    }
    
}
