using System.Collections;
using System.Collections.Generic;
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

    private Vector2 move;
    public bool isSprinting { get; private set; }

    public PlayerStateManager(CharacterInput player)
    {
        this.player = player;
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
        state = state.OnUpdate();
    }

    private abstract class PlayerState
    {
        protected PlayerStateManager psm;

        public PlayerState(PlayerStateManager psm)
        {
            this.psm = psm;
        }
        public abstract PlayerState OnInput(InputAction.CallbackContext ctx);
        public abstract PlayerState DoState(InputAction.CallbackContext ctx);
        public abstract PlayerState OnUpdate();
    }

    private class IdleState : PlayerState
    {
        private float curIdleTime = 0.0f;

        public IdleState(PlayerStateManager psm) : base(psm){}
        public override PlayerState OnInput(InputAction.CallbackContext ctx)
        {
            if (ctx.action.name.Equals("Move"))
            {
                return new MovingState(psm);
            }
            return this;
        }

        public override PlayerState DoState(InputAction.CallbackContext ctx)
        {
            return this;
        }

        public override PlayerState OnUpdate()
        {
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
        public MovingState(PlayerStateManager psm) : base(psm) { }
        public override PlayerState OnInput(InputAction.CallbackContext ctx)
        {
            if (ctx.action.name.Equals("Move"))
            {
                return this;
            }
            if (ctx.action.name.Equals("Attack"))
            {
                return new AttackingState(psm);
            }
            if (ctx.action.name.Equals("Dash") && ctx.action.phase == InputActionPhase.Started)
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
            // simple move
            // psm.player.transform.position += (Vector3)move * (psm.isSprinting ? psm.player.SprintSpeed : psm.player.MoveSpeed) * Time.deltaTime;

            float angle = -(90 - Mathf.Atan2(psm.move.y, psm.move.x) * Mathf.Rad2Deg);
            psm.player.transform.rotation = Quaternion.AngleAxis(angle, psm.player.transform.forward);
            psm.player.transform.position += psm.player.transform.up * (psm.isSprinting ? psm.player.SprintSpeed : psm.player.MoveSpeed) * Time.deltaTime;
            return this;
        }
    }

    private class AttackingState : MovingState
    {
        public AttackingState(PlayerStateManager psm) : base(psm) { }
        public override PlayerState OnInput(InputAction.CallbackContext ctx)
        {
            //auto transition
            return base.OnInput(ctx);
        }

        public override PlayerState DoState(InputAction.CallbackContext ctx)
        {
            Vector3 frontOfPlayer = psm.player.transform.position + psm.player.transform.up * psm.player.AttackRadius;
            foreach (RaycastHit2D hit in Physics2D.CircleCastAll(frontOfPlayer, psm.player.AttackRadius, Vector2.up))
            {
                float hitAngle = Vector2.Angle(psm.player.transform.position, hit.point);
                if (hitAngle > -psm.player.AttackAngleDegrees && hitAngle < psm.player.AttackAngleDegrees)
                {
                    EnemyHP enemyHp = hit.transform.GetComponent<EnemyHP>();
                    if (enemyHp != null)
                    {
                        enemyHp.HP -= psm.player.AttackDamage;
                    }
                }
            }
            return base.DoState(ctx);
        }

        public override PlayerState OnUpdate()
        {
            return base.OnUpdate();
        }
    }

    private class DashState : PlayerState
    {
        private bool hasStartedDash = false;
        private bool hasCompletedDash = false;
        
        public DashState(PlayerStateManager psm) : base(psm) { }
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
                psm.player.transform.position = Vector2.Lerp(initialPos, destination, elapsed / psm.player.DashSeconds);
                elapsed += Time.deltaTime;
                yield return null;
            }

            psm.player.transform.position = destination;
            if (psm.player.DashAttackHitbox)
            {
                psm.player.DashAttackHitbox.enabled = false;
            }
            hasCompletedDash = true;
        }

        public override PlayerState OnUpdate()
        {
            if (hasCompletedDash)
            {
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
