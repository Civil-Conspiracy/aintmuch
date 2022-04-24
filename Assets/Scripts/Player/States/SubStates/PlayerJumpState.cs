using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerInActionState
{
    public PlayerJumpState(StateMachine stateMachine, PlayerData data, PlayerStateManager stateList, PlayerController player, PlayerMotor motor) : base(stateMachine, data, stateList, player, motor) { }

    public override void Enter()
    {
        base.Enter();

        motor.Jump();
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

        else if (data.LastPressedJumpTime > 0 && data.LastOnWallTime > 0)
            stateMachine.ChangeState(states.WallJumpState);

        else if (player.RB.velocity.y <= 0)
            stateMachine.ChangeState(states.InAirState);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        motor.Drag(data.dragAmount);
        motor.Run(1);
    }

    public bool CanJumpCut()
    {
        if (states.StateMachine.CurrentState == this && player.RB.velocity.y > 0)
            return true;
        else
            return false;
    }
}
