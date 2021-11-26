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
    private float dashAttackCooldownSeconds;
    public float DashAttackCooldownSeconds => dashAttackCooldownSeconds;
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

    public event System.Action<int> OnEnergyChanged;

    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(transform.position, transform.forward * 5);
        Gizmos.DrawWireSphere(transform.position + transform.forward * attackRadius, attackRadius);
        Gizmos.DrawLine(transform.position, transform.position + Quaternion.Euler(0, 0, attackAngleDegrees) * transform.forward * attackRadius * 2);
        Gizmos.DrawLine(transform.position, transform.position + Quaternion.Euler(0, 0, -attackAngleDegrees) * transform.forward * attackRadius * 2);
    }

    private void Awake()
    {
        psm = new PlayerStateManager(this);
        psm.OnEnergyChanged += OnEnergyChangedEvent;
        anim ??= GetComponent<Animator>();
    }

    private void OnEnergyChangedEvent(int energy)
    {
        OnEnergyChanged?.Invoke(energy);
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

    public void Ultimate(InputAction.CallbackContext ctx)
    {
        psm.Ultimate();
    }

    public void UltimateStart()
    {
        psm.ForceIdle();
    }

    public void UltimateHitbox()
    {
        foreach (var enemyHP in FindObjectsOfType<EnemyHP>())
        {
            enemyHP.HP = 0;
        }
    }

    public void UltimateEnd()
    {

    }

    public void Die()
    {
        Destroy(gameObject);
    }
}
