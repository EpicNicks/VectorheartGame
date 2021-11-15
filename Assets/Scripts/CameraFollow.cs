using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{
    public GameObject player;
    private Vector3 offset;
    private Vector3 newtrans;

    void Start()
    {
        offset.x = transform.position.x - player.transform.position.x;
        offset.y = transform.position.y - player.transform.position.y;
        offset.z = transform.position.z - player.transform.position.z;
        newtrans = transform.position;
        //not taking y as we won't update y position. 

    }
    void LateUpdate()
    {
        newtrans.x = player.transform.position.x + offset.x;
        newtrans.y = player.transform.position.y + offset.y;
        newtrans.z = player.transform.position.z + offset.z;
        transform.position = newtrans;
    }

}