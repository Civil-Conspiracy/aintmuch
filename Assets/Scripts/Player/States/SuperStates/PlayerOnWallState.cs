using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOnWallState : PlayerState
{
    public PlayerOnWallState(StateMachine stateMachine, PlayerData data) : base(stateMachine, data) { }

    public override void Enter()
    {
        base.Enter();
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
        else if (data.LastOnGroundTime > 0)
        {
            stateMachine.ChangeState(data.States.IdleState);
        }
        else if (data.LastOnWallTime <= 0)
        {
            stateMachine.ChangeState(data.States.InAirState);
        }
    }
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
