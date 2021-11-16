using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnerMaster : MonoBehaviour
{
    [SerializeField]
    private int numOfWaves = 1;
    private int curWave = 0;
    private bool waveStarted = false;
    private List<EnemySpawner> spawners;

    [SerializeField]
    [Tooltip("The amount of seconds all spawners will wait before starting to spawn")]
    private float waveDelaySeconds = 3.0f;
    private float curDelaySeconds = 0.0f;

    private void Awake()
    {
        spawners = FindObjectsOfType<EnemySpawner>().ToList();
        curWave = spawners.Select(s => s.WaveId).Min();
        spawners.ForEach(s => s.enabled = false);
    }

    private void Update()
    {
        if (curWave < numOfWaves)
        {
            if (curDelaySeconds >= waveDelaySeconds)
            {
                EnableSpawners();
            }
            if (spawners.Where(s => s.WaveId == curWave).All(s => s.Exhausted))
            {
                curWave++;
            }
        }
        curDelaySeconds += Time.deltaTime;
    }


    private void EnableSpawners()
    {
        if (!waveStarted)
        {
            waveStarted = true;
            spawners.Where(s => s.WaveId == curWave).ToList().ForEach(s => s.enabled = true);
        }
    }
    
}
