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

    public PlayerStateManager(CharacterInput player)
    {
        this.player = player;
    }

    public void ConsumeInput(InputAction.CallbackContext ctx)
    {
        if (ctx.action.name.Equals("Sprint"))
        {

        }
        else
        {
            state = state.OnInput(ctx).DoState(ctx);
        }
    }

    public void Update()
    {
        state = state.OnUpdate();
    }

    private abstract class PlayerState
    {
        public abstract PlayerState OnInput(InputAction.CallbackContext ctx);
        public abstract PlayerState DoState(InputAction.CallbackContext ctx);
        public abstract PlayerState OnUpdate();
    }

    private class MovingState : PlayerState
    {
        public override PlayerState OnInput(InputAction.CallbackContext ctx)
        {
            throw new System.NotImplementedException();
        }

        public override PlayerState DoState(InputAction.CallbackContext ctx)
        {
            throw new System.NotImplementedException();
        }

        public override PlayerState OnUpdate()
        {
            throw new System.NotImplementedException();
        }
    }

    private class AttackingState : MovingState
    {
        public override PlayerState OnInput(InputAction.CallbackContext ctx)
        {
            return base.OnInput(ctx);
        }

        public override PlayerState DoState(InputAction.CallbackContext ctx)
        {
            return base.DoState(ctx);
        }

        public override PlayerState OnUpdate()
        {
            return base.OnUpdate();
        }
    }

    private class DashState : PlayerState
    {
        public override PlayerState OnInput(InputAction.CallbackContext ctx)
        {
            throw new System.NotImplementedException();
        }

        public override PlayerState DoState(InputAction.CallbackContext ctx)
        {
            throw new System.NotImplementedException();
        }

        public override PlayerState OnUpdate()
        {
            throw new System.NotImplementedException();
        }
    }
}
