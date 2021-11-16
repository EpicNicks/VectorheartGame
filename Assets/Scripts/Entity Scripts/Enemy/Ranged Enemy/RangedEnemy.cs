using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyHP))]
public class RangedEnemy : MonoBehaviour
{
    private EnemyHP hp;
    private GameObject player;

    [SerializeField]
    private float moveSpeed = 1.0f;
    [SerializeField]
    private float attackFromDist = 5.0f;
    [SerializeField]
    private float attackCooldownSeconds = 3.0f;
    private float curAttackCooldownSeconds = 0.0f;

    [SerializeField]
    private GameObject projectile;

    [SerializeField]
    private GameObject attackSfx;

    private void Awake()
    {
        hp = GetComponent<EnemyHP>();
        player = FindObjectOfType<CharacterInput>().gameObject;
        hp.OnEnemyHPChanged += Hp_OnEnemyHPChanged;
    }

    private void Hp_OnEnemyHPChanged(int newHP)
    {
        //play hurt animation
        if (newHP <= 0)
        {
            //do death animation or whatever else
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        MoveToPlayer();
        Attack();
    }

    private void MoveToPlayer()
    {
        transform.rotation = Quaternion.LookRotation(player.transform.position - transform.position, -Vector3.forward);
        if (Vector2.Distance(transform.position, player.transform.position) >= attackFromDist)
        {
            transform.position += transform.forward * moveSpeed * Time.deltaTime;
        }
    }

    private void Attack()
    {
        if (curAttackCooldownSeconds >= attackCooldownSeconds)
        {
            Instantiate(projectile, transform.position, transform.rotation);
            curAttackCooldownSeconds = 0.0f;
        }
        curAttackCooldownSeconds += Time.deltaTime;
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}
