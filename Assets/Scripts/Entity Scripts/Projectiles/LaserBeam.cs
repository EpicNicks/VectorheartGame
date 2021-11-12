using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class LaserBeam : MonoBehaviour
{

    private Collider2D col;

    [System.Flags]
    enum CanHurt
    {
        PLAYER = 1,
        ENEMY = 2
    }

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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (canHurt.HasFlag(CanHurt.PLAYER))
            {
                PlayerHP hp = collision.gameObject.GetComponent<PlayerHP>();
                hp.HP -= damage;
            }
        }
        else if (collision.CompareTag("Enemy"))
        {
            if (canHurt.HasFlag(CanHurt.ENEMY))
            {
                EnemyHP hp = collision.gameObject.GetComponent<EnemyHP>();
                hp.HP -= damage;
            }
        }
        else
        {
            curBounces++;
        }
    }

    private void CheckBounce()
    {
        Ray2D ray = new Ray2D(transform.position, transform.up);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Time.deltaTime * speed + col.bounds.size.y, collisionMask);
        if (hit.collider != null)
        {
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

    private void Awake()
    {
        col = GetComponent<Collider2D>();
        col.isTrigger = true;
    }

    private void Start()
    {
    }

    void Update()
    {
        transform.Translate(Vector3.up * Time.deltaTime * speed);
        CheckBounce();
    }
}
