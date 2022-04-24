using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallSlideState : PlayerOnWallState
{
    public PlayerWallSlideState(StateMachine stateMachine, PlayerData data, PlayerStateManager stateList, PlayerController player, PlayerMotor motor)
        : base(stateMachine, data, stateList, player, motor) { }

    public override void Enter()
    {
        base.Enter();
        motor.SetGravityScale(data.wallSlideGravity);
    }

    public override void Exit()
    {
        base.Exit();
        motor.SetGravityScale(data.gravityScale);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (data.LastPressedJumpTime > 0 && states.WallJumpState.CanWallJump())
        {
            stateMachine.ChangeState(states.WallJumpState);
        }
        else if ((data.LastOnWallLeftTime > 0 && InputManager.instance.MoveInput.x >= 0) || (data.LastOnWallRightTime > 0 && InputManager.instance.MoveInput.x <= 0))
        {
            stateMachine.ChangeState(states.InAirState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        motor.Drag(data.dragAmount);
        motor.Slide();
    }
}
