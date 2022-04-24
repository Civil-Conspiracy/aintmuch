using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerInActionState
{
    public PlayerJumpState(StateMachine stateMachine, PlayerData data) : base(stateMachine, data) { }

    public override void Enter()
    {
        base.Enter();

        data.Motor.Jump();
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

        else if (data.LastPressedJumpTime > 0 && data.LastOnWallTime > 0)
            stateMachine.ChangeState(data.States.WallJumpState);

        else if (data.Player.RB.velocity.y <= 0)
            stateMachine.ChangeState(data.States.InAirState);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        data.Motor.Drag(data.dragAmount);
        data.Motor.Run(1);
    }

    public bool CanJumpCut()
    {
        if (data.States.StateMachine.CurrentState == this && data.Player.RB.velocity.y > 0)
            return true;
        else
            return false;
    }
}
