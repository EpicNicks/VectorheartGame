using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathReporter : MonoBehaviour
{

    public EnemySpawner mySpawner;

    private void OnDestroy()
    {
        mySpawner?.Report();
    }
}
