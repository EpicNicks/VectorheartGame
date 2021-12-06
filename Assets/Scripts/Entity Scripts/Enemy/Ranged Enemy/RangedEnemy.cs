using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyHP))]
public class RangedEnemy : MonoBehaviour
{
    private EnemyHP hp;
    private GameObject player;

    public Animator animator;

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
        gameObject.AddComponent<DeathReporter>();
        hp = GetComponent<EnemyHP>();
        player = FindObjectOfType<CharacterInput>().gameObject;
        hp.OnEnemyHPChanged += Hp_OnEnemyHPChanged;
    }


    private void Hp_OnEnemyHPChanged(int newHP)
    {
        animator.SetTrigger("isHit");//play hurt animation
        if (newHP <= 0)
        {
            //do death animation or whatever else
            animator.SetBool("isDead", true);
        }
    }

    private void Update()
    {
        if (!animator.GetBool("isDead")) 
        {
            MoveToPlayer();
            Attack();
        }
    }

    private void MoveToPlayer()
    {
        transform.rotation = Quaternion.LookRotation(player.transform.position - transform.position, -Vector3.forward);
        if (Vector2.Distance(transform.position, player.transform.position) >= attackFromDist)
        {
            transform.position += transform.forward * moveSpeed * Time.deltaTime;
            animator.SetBool("isRunning", true);
        }
        else
        {
            animator.SetBool("isRunning", false);
        }
    }

    private void Attack()
    {
        if (curAttackCooldownSeconds >= attackCooldownSeconds)
        {
            GameObject proj = Instantiate(projectile, transform.position + transform.forward * 3, transform.rotation);
            proj.transform.up = transform.forward;
            animator.SetTrigger("isShooting");
            curAttackCooldownSeconds = 0.0f;
        }
        curAttackCooldownSeconds += Time.deltaTime;
    }

    public void Die()
    {
        FindObjectOfType<GameHandler>().Score += 200;
        Destroy(gameObject);
    }
}
