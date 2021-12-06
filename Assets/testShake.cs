using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testShake : MonoBehaviour
{
    public CameraShake cameraShake;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("K"))
        {
            StartCoroutine(cameraShake.Shake(.15f, .4f));
        }
    }
}
