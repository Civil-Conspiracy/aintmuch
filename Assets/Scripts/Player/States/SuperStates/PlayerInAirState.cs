using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInAirState : PlayerState
{
    public PlayerInAirState(StateMachine stateMachine, PlayerData data) : base(stateMachine, data) { }

    public override void Enter()
    {
        base.Enter();
    }
    public override void Exit()
    {
        base.Exit();

        data.Motor.SetGravityScale(data.gravityScale);
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
        else if (data.LastPressedJumpTime > 0 && data.LastOnWallTime > 0 && data.States.WallJumpState.CanWallJump())
        {
            stateMachine.ChangeState(data.States.WallJumpState);
        }
        else if ((data.LastOnWallLeftTime > 0 && InputManager.instance.MoveInput.x < 0) || (data.LastOnWallRightTime > 0 && InputManager.instance.MoveInput.x > 0))
        {
            stateMachine.ChangeState(data.States.WallSlideState);
        }
        else if (data.IsAxeSwingPressed && data.States.AxeSwingState.SwingWasCanceled && data.States.AxeSwingState.CanSwingFromCancel())
        {
            stateMachine.ChangeState(data.States.AxeSwingState);
        }
        else if (data.IsAxeSwingPressed && data.States.AxeSwingState.CanSwing())
        {
            stateMachine.ChangeState(data.States.AxeSwingState);
        }

        else if (data.Player.RB.velocity.y < 0)
        {
            if(InputManager.instance.MoveInput.y < 0)
                data.Motor.SetGravityScale(data.gravityScale * data.quickFallGravityMult);

            else
                data.Motor.SetGravityScale(data.gravityScale * data.fallGravityMult);
        }
    }
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        data.Motor.Drag(data.dragAmount);
        data.Motor.Run(1);
    }
}
