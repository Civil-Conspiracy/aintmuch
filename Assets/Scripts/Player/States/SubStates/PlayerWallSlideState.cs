using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallSlideState : PlayerOnWallState
{
    public PlayerWallSlideState(StateMachine stateMachine, PlayerData data) : base(stateMachine, data) { }

    public override void Enter()
    {
        base.Enter();
        data.Motor.SetGravityScale(data.wallSlideGravity);
    }

    public override void Exit()
    {
        base.Exit();
        data.Motor.SetGravityScale(data.gravityScale);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (data.LastPressedJumpTime > 0 && data.States.WallJumpState.CanWallJump())
        {
            stateMachine.ChangeState(data.States.WallJumpState);
        }
        else if ((data.LastOnWallLeftTime > 0 && InputManager.instance.MoveInput.x >= 0) || (data.LastOnWallRightTime > 0 && InputManager.instance.MoveInput.x <= 0))
        {
            stateMachine.ChangeState(data.States.InAirState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        data.Motor.Drag(data.dragAmount);
        data.Motor.Slide();
    }
}
