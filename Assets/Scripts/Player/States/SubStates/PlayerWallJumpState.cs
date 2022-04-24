using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallJumpState : PlayerInActionState
{
    private int jumpDir;
    int jumpsLeft;
    public PlayerWallJumpState(StateMachine stateMachine, PlayerData data) : base(stateMachine, data) { }

    public override void Enter()
    {
        base.Enter();

        jumpsLeft--;
        jumpDir = data.LastOnWallRightTime > 0 ? -1 : 1;
        data.Motor.WallJump(jumpDir);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (data.LastPressedDashTime > 0 && data.States.DashState.CanDash())
            stateMachine.ChangeState(data.States.DashState);
        else if (data.LastOnGroundTime > 0)
            stateMachine.ChangeState(data.States.IdleState);
        else if (data.LastPressedJumpTime > 0 && CanWallJump() && ((data.LastOnWallRightTime > 0 && jumpDir == 1)  || (data.LastOnWallLeftTime > 0 && jumpDir == -1)))
            stateMachine.ChangeState(data.States.WallJumpState);
        else 
            stateMachine.ChangeState(data.States.InAirState);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        data.Motor.Drag(data.dragAmount);
        data.Motor.Run(data.wallJumpRunLerp);
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
        if (stateMachine.CurrentState == this && data.Player.RB.velocity.y > 0)
            return true;
        else
            return false;
    }
}
