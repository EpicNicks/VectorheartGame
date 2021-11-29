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
    public float CurDashCooldownSeconds => psm.CurDashCooldownSeconds;
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
    public PlayerStateManager Psm => psm;

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

    public void SpawnHitboxCone(string parameters)
    {
        string[] splitParams = parameters.Split(',');
        if (splitParams.Length < 3)
        {
            Debug.LogError($"BAD INPUT: {parameters}");
        }
        Vector2 offset = new Vector2(float.Parse(splitParams[0]), float.Parse(splitParams[1]));
        float radius = float.Parse(splitParams[2]);
        int? damage = null;
        if (splitParams.Length >= 4)
            damage = int.Parse(splitParams[3]);
        float? angleLR = null;
        if (splitParams.Length >= 5)
            angleLR = int.Parse(splitParams[4]);
        SpawnHitboxCone(offset, radius, damage, angleLR);
    }

    public void SpawnHitboxCircle(string parameters)
    {
        string[] splitParams = parameters.Split(',');
        if (splitParams.Length < 3)
        {
            Debug.LogError($"BAD INPUT: {parameters}");
        }
        Vector2 offset = new Vector2(float.Parse(splitParams[0]), float.Parse(splitParams[1]));
        float radius = float.Parse(splitParams[2]);
        int? damage = null;
        if (splitParams.Length >= 4)
             damage = int.Parse(splitParams[3]);

        SpawnHitboxCircle(offset, radius, damage);
    }

    private void SpawnHitboxCone(Vector3 offset, float radius, int? damage = null, float? angleLR = null)
    {
        int dmg = damage ?? AttackDamage;
        float angle = angleLR ?? AttackAngleDegrees;
        foreach (RaycastHit2D hit in Physics2D.CircleCastAll(offset, radius, Vector2.up))
        {
            float hitAngle = Vector2.Angle(transform.position, hit.point);
            if (hitAngle > -angle && hitAngle < angle)
            {
                DealDamageToEnemy(hit.transform.GetComponent<EnemyHP>(), dmg);
            }
        }
    }

    private void SpawnHitboxCircle(Vector3 offset, float radius, int? damage = null, float? angleLR = null)
    {
        int dmg = damage ?? AttackDamage;
        float angle = angleLR ?? AttackAngleDegrees;
        foreach (RaycastHit2D hit in Physics2D.CircleCastAll(offset, radius, Vector2.up))
        {
            DealDamageToEnemy(hit.transform.GetComponent<EnemyHP>(), dmg);
        }
    }

    public void DealDamageToEnemy(EnemyHP enemyHp, int damage)
    {
        if (enemyHp == null)
        {
            return;
        }
        enemyHp.HP -= damage;
        if (enemyHp.HP <= 0)
        {
            psm.CurCharge += enemyHp.ChargePercent;
        }
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
