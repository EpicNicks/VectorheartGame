using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shake : MonoBehaviour
{
    // Start is called before the first frame update
    public CameraShake cameraShake;

    // Update is called once per frame
    public void ShakeCamera()
    {
        StartCoroutine(cameraShake.Shake(.15f, .1f));
    }
}
