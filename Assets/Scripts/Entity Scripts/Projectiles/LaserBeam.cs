using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeam : MonoBehaviour
{

    [System.Flags]
    enum CanHurt
    {
        PLAYER = 1,
        ENEMY = 2
    }

    [SerializeField]
    private float colRadius;
    
    [SerializeField]
    private LayerMask collisionMask;

    [SerializeField]
    private CanHurt canHurt;

    [SerializeField]
    private int numDeflectionBounces;
    private int curBounces = 0;
    [SerializeField]
    private int damage;
    [SerializeField]
    private float speed;

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + transform.up * 3.0f);
        Gizmos.DrawWireSphere(transform.position, colRadius);
    }

    private void CheckBounce()
    {
        // TODO: Fix for 3D

        Ray2D ray = new Ray2D(transform.position, transform.up);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Time.deltaTime * speed + colRadius, collisionMask);
        if (hit.collider != null)
        {
            if (hit.collider.CompareTag("Player"))
            {
                if (canHurt.HasFlag(CanHurt.PLAYER))
                {
                    Debug.Log("hit player");
                    PlayerHP hp = hit.collider.GetComponent<PlayerHP>();
                    hp.HP -= damage;
                    Destroy(gameObject);
                }
            }
            else if (hit.collider.CompareTag("Enemy"))
            {
                if (canHurt.HasFlag(CanHurt.ENEMY))
                {
                    EnemyHP hp = hit.collider.GetComponent<EnemyHP>();
                    hp.HP -= damage;
                    Destroy(gameObject);
                }
            }
            if (curBounces < numDeflectionBounces)
            {
                Vector2 reflectDir = Vector2.Reflect(ray.direction, hit.normal);
                float rot = 90 - Mathf.Atan2(reflectDir.y, reflectDir.x) * Mathf.Rad2Deg;
                transform.eulerAngles = new Vector3(0, 0, -rot);
                curBounces++;
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    void Update()
    {
        transform.Translate(Vector3.up * Time.deltaTime * speed);
        CheckBounce();
    }
}
