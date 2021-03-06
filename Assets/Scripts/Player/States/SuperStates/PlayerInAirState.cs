using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInAirState : PlayerState
{
    public PlayerInAirState(PlayerStateMachine player, StateMachine stateMachine, PlayerData data) : base(player, stateMachine, data) { }

    public override void Enter()
    {
        base.Enter();
    }
    public override void Exit()
    {
        base.Exit();

        player.SetGravityScale(data.gravityScale);
    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (player.LastPressedDashTime > 0 && player.DashState.CanDash())
        {
            player.StateMachine.ChangeState(player.DashState);
        }
        else if (player.LastOnGroundTime > 0)
        {
            player.StateMachine.ChangeState(player.IdleState);
        }
        else if (player.LastPressedJumpTime > 0 && player.LastOnWallTime > 0 && player.WallJumpState.CanWallJump())
        {
            player.StateMachine.ChangeState(player.WallJumpState);
        }
        else if ((player.LastOnWallLeftTime > 0 && InputManager.instance.MoveInput.x < 0) || (player.LastOnWallRightTime > 0 && InputManager.instance.MoveInput.x > 0))
        {
            player.StateMachine.ChangeState(player.WallSlideState);
        }
        else if (player.IsAxeSwingPressed && player.AxeSwingState.SwingWasCanceled && player.AxeSwingState.CanSwingFromCancel())
        {
            player.StateMachine.ChangeState(player.AxeSwingState);
        }
        else if (player.IsAxeSwingPressed && player.AxeSwingState.CanSwing())
        {
            player.StateMachine.ChangeState(player.AxeSwingState);
        }

        else if (player.RB.velocity.y < 0)
        {
            if(InputManager.instance.MoveInput.y < 0)
                player.SetGravityScale(data.gravityScale * data.quickFallGravityMult);

            else
                player.SetGravityScale(data.gravityScale * data.fallGravityMult);
        }
    }
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        player.Drag(data.dragAmount);
        player.Run(1);
    }
}
