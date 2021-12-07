using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class HitboxCollider : MonoBehaviour
{
    [SerializeField]
    private CharacterInput player;
    [SerializeField]
    private Collider col;

    private List<Collider> collided = new List<Collider> { };

    public CameraShake cameraShake;

    private void Awake()
    {
        if (col == null)
        {
            col = GetComponent<Collider>();
        }
        col.isTrigger = true;
        if (player == null)
        {
            player = FindObjectOfType<CharacterInput>();
        }
    }


    private void OnTriggerEnter(Collider collision)
    {
        if (!collided.Contains(collision))
        {
            collided.Add(collision);
            StartCoroutine(cameraShake.Shake(.15f, .1f));
        }
    }

    public void StartStrike()
    {
        col.enabled = true;

    }

    /// <summary>
    /// Use this with animation event to apply damage to all collided enemies
    /// </summary>
    public void FinishStrike(int damage)
    {
        if (collided != null)
        {
            foreach (var hp in collided.Where(c => c != null).Select(c => c.GetComponent<EnemyHP>()).Where(c => c != null))
            {
                player.DealDamageToEnemy(hp, damage);
                Debug.Log($"Dealt {damage} damage to enemy: {hp.gameObject.name}");
            }
        }
        collided.Clear();
        col.enabled = false;
    }
}
