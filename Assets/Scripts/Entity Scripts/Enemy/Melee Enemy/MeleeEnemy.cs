using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(EnemyHP))]
public class MeleeEnemy : MonoBehaviour
{
    private EnemyHP hp;
    private GameObject player;

    public Animator animator;

    public int AttackAngleDegrees;
    public int AttackDamage;
    public int AttackRadius;

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

    // Start is called before the first frame update
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
    // Update is called once per frame
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
        //float rot_z = Mathf.Atan2(player.transform.position.y, player.transform.position.x) * Mathf.Rad2Deg;
        //transform.rotation = Quaternion.Euler(0f, 0f, 90 - rot_z);
        transform.rotation = Quaternion.LookRotation(player.transform.position - transform.position, -Vector3.forward);
        //transform.forward = player.transform.position - transform.position;
        if (Vector2.Distance(transform.position, player.transform.position) >= attackFromDist)
        {
            transform.position += transform.forward * moveSpeed * Time.deltaTime;
            animator.SetBool("isRunning", true);
        }
    }

    private void Attack()
    {
        if (curAttackCooldownSeconds >= attackCooldownSeconds)
        {
            //Make a cone slash
            animator.SetTrigger("Attack");
            curAttackCooldownSeconds = 0.0f;
            Vector3 frontOfPlayer = transform.position + transform.up * AttackRadius;
            foreach (RaycastHit2D hit in Physics2D.CircleCastAll(frontOfPlayer, AttackRadius, Vector2.up))
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
        }
        curAttackCooldownSeconds += Time.deltaTime;
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}
