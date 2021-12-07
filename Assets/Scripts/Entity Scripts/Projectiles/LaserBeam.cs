using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    [SerializeField]
    private AudioClip bounceAudioClip;
    [SerializeField]
    private AudioClip onDestroyAudioClip;

    [SerializeField]
    private AudioSource audioSource;

    [SerializeField]
    [Range(0, 1)]
    private float bounceVolume;
    [SerializeField]
    [Range(0, 1)]
    private float onDestroyVolume;

    private void Awake()
    {
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

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
            //if (hit.collider.CompareTag("Player"))
            //{
            //    if (canHurt.HasFlag(CanHurt.PLAYER))
            //    {
            //        Debug.Log("hit player");
            //        PlayerHP hp = hit.collider.GetComponent<PlayerHP>();
            //        hp.HP -= damage;
            //        Destroy(gameObject);
            //    }
            //}
            //else if (hit.collider.CompareTag("Enemy"))
            //{
            //    if (canHurt.HasFlag(CanHurt.ENEMY))
            //    {
            //        EnemyHP hp = hit.collider.GetComponent<EnemyHP>();
            //        hp.HP -= damage;
            //        Destroy(gameObject);
            //    }
            //}
            if (curBounces < numDeflectionBounces)
            {
                Vector2 reflectDir = Vector2.Reflect(ray.direction, hit.normal);
                float rot = 90 - Mathf.Atan2(reflectDir.y, reflectDir.x) * Mathf.Rad2Deg;
                transform.eulerAngles = new Vector3(0, 0, -rot);
                curBounces++;
                if (bounceAudioClip != null)
                {
                    audioSource.PlayOneShot(bounceAudioClip, bounceVolume);
                }
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    private void CheckHit3D()
    {
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, 0.4f, transform.up, Time.deltaTime * speed + colRadius, collisionMask);
        if (hits.Any(h => h.transform.CompareTag("Player")))
        {
            RaycastHit player = hits.First(h => h.transform.CompareTag("Player"));
            PlayerHP playerHp = player.transform.GetComponent<PlayerHP>();
            if (playerHp != null)
            {
                Debug.Log("hit player 3D");
                playerHp.HP -= damage;
                Destroy(gameObject);
            }
        }
    }

    void Update()
    {
        transform.Translate(Vector3.up * Time.deltaTime * speed);
        CheckBounce();
        CheckHit3D();
    }

    private void OnDestroy()
    {
        if (onDestroyAudioClip != null)
        {
            audioSource.PlayOneShot(onDestroyAudioClip, onDestroyVolume);
        }
    }
}
