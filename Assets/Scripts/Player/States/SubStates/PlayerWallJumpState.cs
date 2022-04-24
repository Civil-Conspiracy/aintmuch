using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallJumpState : PlayerInActionState
{
    private int jumpDir;
    int jumpsLeft;
    public PlayerWallJumpState(StateMachine stateMachine, PlayerData data, PlayerStateManager stateList, PlayerController player, PlayerMotor motor) 
        : base(stateMachine, data, stateList, player, motor) { }

    public override void Enter()
    {
        base.Enter();

        jumpsLeft--;
        jumpDir = data.LastOnWallRightTime > 0 ? -1 : 1;
        motor.WallJump(jumpDir);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (data.LastPressedDashTime > 0 && states.DashState.CanDash())
            stateMachine.ChangeState(states.DashState);
        else if (data.LastOnGroundTime > 0)
            stateMachine.ChangeState(states.IdleState);
        else if (data.LastPressedJumpTime > 0 && CanWallJump() && ((data.LastOnWallRightTime > 0 && jumpDir == 1)  || (data.LastOnWallLeftTime > 0 && jumpDir == -1)))
            stateMachine.ChangeState(states.WallJumpState);
        else 
            stateMachine.ChangeState(states.InAirState);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        motor.Drag(data.dragAmount);
        motor.Run(data.wallJumpRunLerp);
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
        if (stateMachine.CurrentState == this && player.RB.velocity.y > 0)
            return true;
        else
            return false;
    }
}
