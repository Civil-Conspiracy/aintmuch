using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallJumpState : PlayerInActionState
{
    private int jumpDir;
    int jumpsLeft;
    public PlayerWallJumpState(PlayerStateMachine player, StateMachine stateMachine, PlayerData data) : base(player, stateMachine, data) { }

    public override void Enter()
    {
        base.Enter();

        jumpsLeft--;
        jumpDir = player.LastOnWallRightTime > 0 ? -1 : 1;
        player.WallJump(jumpDir);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (player.LastPressedDashTime > 0 && player.DashState.CanDash())
            player.StateMachine.ChangeState(player.DashState);
        else if (player.LastOnGroundTime > 0)
            player.StateMachine.ChangeState(player.IdleState);
        else if (player.LastPressedJumpTime > 0 && CanWallJump() && ((player.LastOnWallRightTime > 0 && jumpDir == 1)  || (player.LastOnWallLeftTime > 0 && jumpDir == -1)))
            player.StateMachine.ChangeState(player.WallJumpState);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        player.Drag(data.dragAmount);
        player.Run(data.wallJumpRunLerp);
    }

    public bool CanWallJump()
    {
        if (jumpsLeft > 0)
            return true;
        else
            return false;
    }

    public void ResetWallJumpCount()
    {
        jumpsLeft = data.wallJumpAmount;
    }

    public bool CanJumpCut()
    {
        if (player.StateMachine.CurrentState == this && player.RB.velocity.y > 0)
            return true;
        else
            return false;
    }
}
