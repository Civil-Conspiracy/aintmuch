using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallSlideState : PlayerOnWallState
{
    public PlayerWallSlideState(PlayerStateMachine player, StateMachine stateMachine, PlayerData data) : base(player, stateMachine, data) { }

    public override void Enter()
    {
        base.Enter();
        player.SetGravityScale(data.wallSlideGravity);
    }

    public override void Exit()
    {
        base.Exit();
        player.SetGravityScale(data.gravityScale);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (player.LastPressedJumpTime > 0 && player.WallJumpState.CanWallJump())
        {
            player.StateMachine.ChangeState(player.WallJumpState);
        }
        else if ((player.LastOnWallLeftTime > 0 && InputManager.instance.MoveInput.x >= 0) || (player.LastOnWallRightTime > 0 && InputManager.instance.MoveInput.x <= 0))
        {
            player.StateMachine.ChangeState(player.InAirState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        player.Drag(data.dragAmount);
        player.Slide();
    }
}
