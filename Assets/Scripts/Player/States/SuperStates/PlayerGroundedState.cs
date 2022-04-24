using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : PlayerState
{
    public PlayerGroundedState(StateMachine stateMachine, PlayerData data) : base(stateMachine, data) { }

    public override void Enter()
    {
        base.Enter();

        data.States.DashState.ResetDashes();
        data.States.WallJumpState.ResetWallJumpCount();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (data.LastPressedDashTime > 0 && data.States.DashState.CanDash())
        {
            stateMachine.ChangeState(data.States.DashState);
        }
        else if (data.LastPressedJumpTime > 0)
        {
            stateMachine.ChangeState(data.States.JumpState);
        }
        else if (data.LastOnGroundTime <= 0)
        {
            stateMachine.ChangeState(data.States.InAirState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        data.Motor.Drag(data.frictionAmount);
    }

}
