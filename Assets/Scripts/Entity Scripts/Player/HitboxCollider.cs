using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class HitboxCollider : MonoBehaviour
{
    [SerializeField]
    private CharacterInput player;
    [SerializeField]
    private Collider2D col;

    private List<Collider2D> collided = new List<Collider2D>{};

    private void Awake()
    {
        if (col == null)
        {
            col = GetComponent<Collider2D>();
            col.isTrigger = true;
        }
        if (player == null)
        {
            player = FindObjectOfType<CharacterInput>();
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        collided.Add(collision);
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
    }
}
