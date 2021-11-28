using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(EnemyHP))]
public class MeleeEnemy : MonoBehaviour
{
    private EnemyHP hp;
    private GameObject player;

    public Animator animator;

    public int AttackAngleDegrees = 60;
    public int AttackDamage = 15;
    public int AttackRadius = 1;

    [SerializeField]
    private float moveSpeed = 1.0f;
    [SerializeField]
    private float attackFromDist = 1.0f;
    [SerializeField]
    private float attackCooldownSeconds = 2.0f;
    private float curAttackCooldownSeconds = 0.0f;

    [SerializeField]
    private GameObject attackSfx;

    [SerializeField]
    private GameObject attackVfx;

    private void Awake()
    {
        gameObject.AddComponent<DeathReporter>();
    }

    void Start()
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
            animator.SetBool("isDead", true);
        }
    }
    void Update()
    {
        if (!animator.GetBool("isDead"))
        {
            MoveToPlayer();
            Attack();
        }
    }
    private void MoveToPlayer()
    {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            transform.rotation = Quaternion.LookRotation(player.transform.position - transform.position, -Vector3.forward);
            if (Vector2.Distance(transform.position, player.transform.position) >= attackFromDist)
            {
                transform.position += transform.forward * moveSpeed * Time.deltaTime;
                animator.SetBool("isRunning", true);
            }
        }
    }

    private void Attack()
    {
        if (curAttackCooldownSeconds >= attackCooldownSeconds)
        {
            //Make a cone slash
            animator.SetTrigger("Attack");
        }
        curAttackCooldownSeconds += Time.deltaTime;
    }

    public void AttackAnimationEvent()
    {
        curAttackCooldownSeconds = 0.0f;
        Vector3 frontOfPlayer = transform.position + transform.up * AttackRadius;
        foreach (RaycastHit hit in Physics.SphereCastAll(frontOfPlayer, AttackRadius, Vector2.up))
        {
            float hitAngle = Vector2.Angle(transform.position, hit.point);
            if (hitAngle > -AttackAngleDegrees && hitAngle < AttackAngleDegrees)
            {
                PlayerHP playerHP = hit.transform.GetComponent<PlayerHP>();
                if (playerHP != null)
                {
                    playerHP.HP -= AttackDamage;
                }
            }
        }
        Instantiate(attackVfx, transform.position, transform.rotation, transform);
    }

    public void Die()
    {
        FindObjectOfType<GameHandler>().Score += 100;
        Destroy(gameObject);
    }
}
