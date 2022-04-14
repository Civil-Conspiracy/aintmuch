using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAxeSwingState : PlayerInActionState
{
    public PlayerAxeSwingState(PlayerStateMachine player, StateMachine stateMachine, PlayerData data) : base(player, stateMachine, data) { }

    public bool SwingWasCanceled { get; private set; }
    bool attacking;

    public override void Enter()
    {
        base.Enter();

        attacking = true;
        SwingWasCanceled = false;
        player.SetGravityScale(data.axeSwingGravity);
    }

    public override void Exit()
    {
        base.Exit();

        attacking = false;
        player.SetGravityScale(data.gravityScale);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (!attacking && player.LastPressedJumpTime > 0)
        {
            player.StateMachine.ChangeState(player.JumpState);
        }
        else if (!attacking && player.LastPressedDashTime > 0 && player.DashState.CanDash())
        {
                player.StateMachine.ChangeState(player.DashState);
        }
        else if (!attacking && InputManager.instance.MoveInput.x != 0)
        {
            CancelAttack();
            player.StateMachine.ChangeState(player.RunState);
        }
        else if (!attacking && player.IsAxeSwingPressed)
        {
            CancelAttack();
            player.StateMachine.ChangeState(player.IdleState);
        }
        else if (Time.time - startTime > data.swingAttackTime + data.swingEndTime)// swingTime over transition to another state
        {
            if (player.LastOnGroundTime > 0)
                player.StateMachine.ChangeState(player.IdleState);
            else
                player.StateMachine.ChangeState(player.InAirState);
        }
            
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        if (attacking)
        {
            if (Time.time - startTime > data.swingAttackTime) //initial startup over, detect hit;
            {
                player.DetectHit(data.damage);
                attacking = false;
            }
            else if (InputManager.instance.MoveInput.x != 0)
            {
                if (InputManager.instance.MoveInput.x < 0)
                    player.CheckDirectionToFace(false);
                if (InputManager.instance.MoveInput.x > 0)
                    player.CheckDirectionToFace(true);
            }
            player.Drag(data.dragAmount);
            player.Slide();
        }

    }

    public bool CanSwing()
    {
        return (Time.time - startTime > data.swingAttackTime + data.swingEndTime);
    }
    public bool CanSwingFromCancel()
    {
        return (Time.time - startTime > data.swingAttackTime);
    }

    private void CancelAttack()
    {
        attacking = false;
        SwingWasCanceled = true;
        player.SetGravityScale(data.gravityScale);
    }
}
