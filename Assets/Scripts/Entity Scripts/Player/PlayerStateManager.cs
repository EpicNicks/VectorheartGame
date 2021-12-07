using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

/*
 * PLAYER ACTIONS:
 * MOVE + SPRINT
 * DASH
 * ATTACK
 */
public class PlayerStateManager
{
    private CharacterInput player;
    private PlayerState state;

    public int MaxCharge => 100;
    private int curCharge = 0;
    public int CurCharge
    {
        get => curCharge;
        set
        {
            curCharge = Mathf.Min(value, MaxCharge);
            OnEnergyChanged?.Invoke(curCharge);
        }
    }

    public event Action<int> OnEnergyChanged;

    /// <summary>
    /// if charge >= 75
    /// </summary>
    public bool isCharged => curCharge >= 75;
    public bool fullCharged => curCharge == MaxCharge;

    public event Action<float> DashCooldownSecondsChangedEvent;

    public void Ultimate()
    {
        if (isCharged)
        {
            CurCharge = 0;
            player.Anim.SetTrigger("UltimateAbility");
        }
    }


    private Vector2 move;
    public bool isSprinting { get; private set; }

    public PlayerStateManager(CharacterInput player)
    {
        this.player = player;
        state = new IdleState(this);
    }

    public void ForceIdle()
    {
        state = new IdleState(this);
    }

    public void ConsumeInput(InputAction.CallbackContext ctx)
    {
        if (ctx.action.name.Equals("Sprint"))
        {
            if (ctx.action.phase == InputActionPhase.Started)
            {
                isSprinting = true;
            }
            else if (ctx.action.phase == InputActionPhase.Canceled)
            {
                isSprinting = false;
            }
        }
        else
        {
            if (ctx.action.name.Equals("Move"))
            {
                move = ctx.ReadValue<Vector2>();
            }
            state = state.OnInput(ctx).DoState(ctx);
        }
    }

    public void Update()
    {
        player.Anim.SetBool("isCharged", isCharged);
        state = state.OnUpdate();
    }

    private abstract class PlayerState
    {
        protected PlayerStateManager psm;

        protected static float curDashCooldownSeconds;
        public float CurDashCooldownSeconds
        {
            get => curDashCooldownSeconds;
            set
            {
                curDashCooldownSeconds = value;
                psm.DashCooldownSecondsChangedEvent?.Invoke(value);
            }
        }

        public PlayerState(PlayerStateManager psm)
        {
            this.psm = psm;
        }
        public abstract PlayerState OnInput(InputAction.CallbackContext ctx);
        public abstract PlayerState DoState(InputAction.CallbackContext ctx);
        public virtual PlayerState OnUpdate()
        {
            if (curDashCooldownSeconds > 0)
            {
                curDashCooldownSeconds -= Time.deltaTime;
            }
            return this;
        }
    }

    private class IdleState : PlayerState
    {
        private float curIdleTime = 0.0f;

        public IdleState(PlayerStateManager psm) : base(psm)
        {
            psm.player.Anim?.SetBool("isRunning", false);
        }
        public override PlayerState OnInput(InputAction.CallbackContext ctx)
        {
            if (ctx.action.name.Equals("Move"))
            {
                return new MovingState(psm);
            }
            if (ctx.action.name.Equals("Attack") && ctx.action.phase == InputActionPhase.Started)
            {
                return new AttackingState(psm);
            }
            return this;
        }

        public override PlayerState DoState(InputAction.CallbackContext ctx)
        {
            return this;
        }

        public override PlayerState OnUpdate()
        {
            base.OnUpdate();
            if (curIdleTime >= psm.player.IdleSecondsToPlayIdleAnim)
            {
                curIdleTime = 0.0f;
            }
            curIdleTime += Time.deltaTime;
            return this;
        }
    }

    private class MovingState : PlayerState
    {
        public MovingState(PlayerStateManager psm) : base(psm) 
        {
            psm.player.Anim?.SetBool("isRunning", true);
        }
        public override PlayerState OnInput(InputAction.CallbackContext ctx)
        {
            if (ctx.action.name.Equals("Move"))
            {
                return this;
            }
            if (ctx.action.name.Equals("Attack") && ctx.action.phase == InputActionPhase.Started)
            {
                return new AttackingState(psm);
            }
            if (ctx.action.name.Equals("Dash") && ctx.action.phase == InputActionPhase.Started && curDashCooldownSeconds <= 0)
            {
                return new DashState(psm);
            }
            return this;
        }

        public override PlayerState DoState(InputAction.CallbackContext ctx)
        {
            if (psm.move == Vector2.zero)
            {
                return new IdleState(psm);
            }
            // TODO
            return this;
        }

        public override PlayerState OnUpdate()
        {
            base.OnUpdate();
            psm.player.transform.rotation = Quaternion.LookRotation(new Vector3(psm.move.x, psm.move.y, 0), psm.player.transform.up);
            
            var rc = Physics2D.RaycastAll(psm.player.transform.position + 2 * psm.player.transform.forward, psm.player.transform.forward, 0.8f);
            if (rc.Where(c => !c.transform.CompareTag("Enemy")).Count() == 0)
            {
                psm.player.transform.position += psm.player.transform.forward * (psm.isSprinting ? psm.player.SprintSpeed : psm.player.MoveSpeed) * Time.deltaTime;
            }
            return this;
        }
    }

    private class AttackingState : MovingState
    {
        private string[] attackStateNames = { "mixamo_com", "PlayerAttackNormal", "PlayerSpinComboAttack" };

        public AttackingState(PlayerStateManager psm) : base(psm) 
        {
            if (!attackStateNames.Any(s => psm.player.Anim.GetCurrentAnimatorStateInfo(0).IsName(s)))
            {
                psm.player.Anim?.SetTrigger("isAttack");
            }
        }
        public override PlayerState OnInput(InputAction.CallbackContext ctx)
        {
            //auto transition
            return base.OnInput(ctx);
        }

        public override PlayerState DoState(InputAction.CallbackContext ctx)
        {
            Vector3 frontOfPlayer = psm.player.transform.position + psm.player.transform.forward * psm.player.AttackRadius;
            if (psm.player.AttackVfx)
            {
                UnityEngine.Object.Instantiate(psm.player.AttackVfx, psm.player.transform.position + psm.player.transform.forward, Quaternion.identity, psm.player.transform);
            }
            return base.DoState(ctx);
        }

        public override PlayerState OnUpdate()
        {
            return base.OnUpdate();
        }
    }

    // TODO: dash attack animation
    private class DashState : PlayerState
    {
        private bool hasStartedDash = false;
        private bool hasCompletedDash = false;
        
        public DashState(PlayerStateManager psm) : base(psm)
        {
            psm.player.DashSfx.SetActive(true);
            curDashCooldownSeconds = psm.player.DashAttackCooldownSeconds;
        }
        public override PlayerState OnInput(InputAction.CallbackContext ctx)
        {
            // this state auto-transitions
            return this;
        }

        public override PlayerState DoState(InputAction.CallbackContext ctx)
        {
            if (!hasStartedDash)
            {
                hasStartedDash = true;
                psm.player.StartCoroutine(DashStrike());
            }
            return this;
        }

        private IEnumerator DashStrike()
        {
            psm.player.Anim.SetBool("Dash", true);
            Vector2 dashDirection = psm.move;
            Vector2 initialPos = psm.player.transform.position;
            Vector2 destination = initialPos + dashDirection * psm.player.DashDistance;
            float elapsed = 0.0f;
            if (psm.player.DashAttackHitbox)
            {
                psm.player.DashAttackHitbox.enabled = true;
            }
            
            while (elapsed < psm.player.DashSeconds)
            {
                // Don't bother with LayerMasks. The walls are Wall, the player is Wall. The enemies are Wall. I live in your walls. I live in your walls.
                // I live in your walls. I live in your walls. I live in your walls. I live in your walls. I live in your walls. I live in your walls.
                // I live in your walls. I live in your walls. I live in your walls. I live in your walls. I live in your walls. I live in your walls.
                // I live in your walls. I live in your walls. I live in your walls. I live in your walls. I live in your walls. I live in your walls.
                var rc = Physics2D.RaycastAll(psm.player.transform.position + 2 * psm.player.transform.forward, psm.player.transform.forward, 0.8f);
                if (rc.Where(c => !c.transform.CompareTag("Enemy")).Count() == 0)
                {
                    psm.player.transform.position = Vector2.Lerp(initialPos, destination, elapsed / psm.player.DashSeconds);
                }
                elapsed += Time.deltaTime;
                yield return null;
            }

            if (psm.player.DashAttackHitbox)
            {
                psm.player.DashAttackHitbox.enabled = false;
            }
            hasCompletedDash = true;
            psm.player.Anim.SetBool("Dash", false);
        }

        public override PlayerState OnUpdate()
        {
            if (hasCompletedDash)
            {
                psm.player.DashSfx.SetActive(false);
                if (psm.move != Vector2.zero)
                {
                    return new MovingState(psm);
                }
                return new IdleState(psm);
            }
            return this;
        }
    }
}
