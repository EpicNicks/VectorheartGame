using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetDummy : MonoBehaviour
{
    private void OnTriggerEnter(Collision2D collision)
    {
        Debug.Log("Hit!");
    }
}
