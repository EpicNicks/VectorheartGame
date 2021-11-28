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

    private List<Collider> collided = new List<Collider>{};

    private void Awake()
    {
        if (col == null)
        {
            col = GetComponent<Collider>();
            col.isTrigger = true;
        }
        if (player == null)
        {
            player = FindObjectOfType<CharacterInput>();
        }
    }


    private void OnTriggerEnter(Collider collision)
    {
        collided.Add(collision);
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
            foreach (var col in collided)
            {
                player.DealDamageToEnemy(col.GetComponent<EnemyHP>(), damage);
            }
        }
        collided.Clear();
        col.enabled = false;
    }
}