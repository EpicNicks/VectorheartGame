using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(EnemyHP))]
public class MeleeEnemy : MonoBehaviour
{

    private EnemyHP hp;
    private GameObject player;

    public Animator animator;

    [SerializeField]
    private float moveSpeed = 1.0f;
    [SerializeField]
    private float attackFromDist = 1.0f;
    [SerializeField]
    private float attackCooldownSeconds = 3.0f;
    private float curAttackCooldownSeconds = 0.0f;



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
            Destroy(gameObject);
        }
    }
    // Update is called once per frame
    void Update()
    {
        MoveToPlayer();
        Attack();
    }
    private void MoveToPlayer()
    {
        //float rot_z = Mathf.Atan2(player.transform.position.y, player.transform.position.x) * Mathf.Rad2Deg;
        //transform.rotation = Quaternion.Euler(0f, 0f, 90 - rot_z);
        transform.up = player.transform.position - transform.position;
        if (Vector2.Distance(transform.position, player.transform.position) >= attackFromDist)
        {
            transform.position += transform.up * moveSpeed * Time.deltaTime;
        }
    }

    private void Attack()
    {
        if (curAttackCooldownSeconds >= attackCooldownSeconds)
        {
            //Make a cone slash
            animator.SetTrigger("Attack");
            curAttackCooldownSeconds = 0.0f;
        }
        curAttackCooldownSeconds += Time.deltaTime;
    }
}
