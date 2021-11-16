using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(Animator))]
public class CharacterInput : MonoBehaviour
{
    [SerializeField]
    private Animator anim;
    public Animator Anim => anim;

    #region Inspector Data
    [SerializeField]
    private float moveSpeed;
    public float MoveSpeed => moveSpeed;
    [SerializeField]
    private float sprintSpeed;
    public float SprintSpeed => sprintSpeed;

    [SerializeField]
    private float idleSecondsToPlayIdleAnim = 3.0f;
    public float IdleSecondsToPlayIdleAnim => idleSecondsToPlayIdleAnim;

    [SerializeField]
    private float attackRadius = 1.0f;
    public float AttackRadius => attackRadius;
    [SerializeField]
    private float attackAngleDegrees = 45.0f;
    public float AttackAngleDegrees => attackAngleDegrees;
    //rework into separate component if we decide to use powerups
    [SerializeField]
    private int attackDamage;
    public int AttackDamage => attackDamage;

    [SerializeField]
    private float dashDistance = 3.0f;
    public float DashDistance => dashDistance;

    [SerializeField]
    private float dashSeconds = 0.2f;
    public float DashSeconds => dashSeconds;
    [SerializeField]
    private Collider dashAttackHitbox;
    public Collider DashAttackHitbox => dashAttackHitbox;

    [SerializeField]
    private GameObject attackVfx;
    public GameObject AttackVfx => attackVfx;
    [SerializeField]
    private GameObject dashSfx;
    public GameObject DashSfx => dashSfx;
    #endregion

    private PlayerStateManager psm;

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position + transform.forward * attackRadius, attackRadius);
        Gizmos.DrawLine(transform.position, transform.position + Quaternion.Euler(0, 0, attackAngleDegrees) * transform.up * attackRadius * 2);
        Gizmos.DrawLine(transform.position, transform.position + Quaternion.Euler(0, 0, -attackAngleDegrees) * transform.up * attackRadius * 2);
    }

    private void Awake()
    {
        psm = new PlayerStateManager(this);
        anim ??= GetComponent<Animator>();
    }

    private void Update()
    {
        psm.Update();
    }

    /// <summary>
    /// All input events from PlayerInput go here
    /// </summary>
    /// <param name="ctx"></param>
    public void ConsumeInput(InputAction.CallbackContext ctx)
    {
        psm.ConsumeInput(ctx);
    }

}
