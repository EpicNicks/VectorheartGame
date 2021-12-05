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

    private float speedMul = 1.0f;

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
        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>();
        }
    }

    void Start()
    {
        hp = GetComponent<EnemyHP>();
        player = FindObjectOfType<CharacterInput>().gameObject;
        hp.OnEnemyHPChanged += Hp_OnEnemyHPChanged;
    }

    private void Hp_OnEnemyHPChanged(int newHP)
    {
        animator.SetTrigger("isHit");//play hurt animation
        if (newHP <= 0)
        {
            Debug.Log($"Enemy {name} hp changed to {newHP}");
            //do death animation or whatever else
            animator.SetBool("isDead", true);
        }
    }
    void Update()
    {
        // Debug.Log(animator.GetBool("isDead"));
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
            transform.position += transform.forward * moveSpeed * Time.deltaTime * speedMul;
            animator.SetBool("isRunning", true);
        }
    }

    private void Attack()
    {
        if (curAttackCooldownSeconds >= attackCooldownSeconds)
        {
            //Make a cone slash
            animator.SetTrigger("isAttack");
            Debug.Log("Attacking");
        }
        curAttackCooldownSeconds += Time.deltaTime;
    }

    public void StopMoving()
    {
        speedMul = 0.0f;
    }

    public void ResumeMoving()
    {
        speedMul = 1.0f;
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
