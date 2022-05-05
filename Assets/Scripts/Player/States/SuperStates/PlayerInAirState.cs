using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInAirState : PlayerState
{
    public PlayerInAirState(StateMachine stateMachine, PlayerData data, PlayerStateManager stateList, PlayerController player, PlayerMotor motor)
        : base(stateMachine, data, stateList, player, motor) { }

    public override void Enter()
    {
        base.Enter();
    }
    public override void Exit()
    {
        base.Exit();

        motor.SetGravityScale(data.gravityScale);
    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (data.LastPressedDashTime > 0 && states.DashState.CanDash())
        {
            stateMachine.ChangeState(states.DashState);
        }
        else if (data.LastOnGroundTime > 0)
        {
            stateMachine.ChangeState(states.IdleState);
        }
        else if (data.LastOnSlopeTime > 0)
        {
            stateMachine.ChangeState(states.SlideState);
        }
        else if (data.LastPressedJumpTime > 0 && data.LastOnWallTime > 0 && states.WallJumpState.CanWallJump())
        {
            stateMachine.ChangeState(states.WallJumpState);
        }
        else if ((data.LastOnWallLeftTime > 0 && InputManager.instance.MoveInput.x < 0) || (data.LastOnWallRightTime > 0 && InputManager.instance.MoveInput.x > 0))
        {
            stateMachine.ChangeState(states.WallSlideState);
        }
        else if (data.IsAxeSwingPressed && states.AxeSwingState.SwingWasCanceled && states.AxeSwingState.CanSwingFromCancel())
        {
            stateMachine.ChangeState(states.AxeSwingState);
        }
        else if (data.IsAxeSwingPressed && states.AxeSwingState.CanSwing())
        {
            stateMachine.ChangeState(states.AxeSwingState);
        }

        else if (player.RB.velocity.y < 0)
        {
            if(InputManager.instance.MoveInput.y < 0)
                motor.SetGravityScale(data.gravityScale * data.quickFallGravityMult);

            else
                motor.SetGravityScale(data.gravityScale * data.fallGravityMult);
        }
    }
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        motor.Drag(data.dragAmount);
        motor.Run(1);
    }
}
