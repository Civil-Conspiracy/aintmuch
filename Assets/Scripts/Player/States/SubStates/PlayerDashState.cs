using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : PlayerInActionState
{
    Vector2 dir;
    int dashesLeft;
    bool dashAttacking;

    public PlayerDashState(StateMachine stateMachine, PlayerData data, PlayerStateManager stateList, PlayerController player, PlayerMotor motor) 
        : base(stateMachine, data, stateList, player, motor) { }

    public override void Enter()
    {
        base.Enter();

        states.WallJumpState.ResetWallJumpCount();
        states.WallJumpState.lastJumpPoint = PlayerData.WallSides.NONE;

        dashesLeft--;

        dir = Vector2.zero;
        if (InputManager.instance.MoveInput == Vector2.zero)
            dir.x = (data.IsFacingRight) ? 1 : -1;
        else
            dir = InputManager.instance.MoveInput;

        dashAttacking = true;
        motor.Dash(dir);

    }

    public override void Exit()
    {
        base.Exit();
        motor.SetGravityScale(data.gravityScale);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(Time.time - startTime > data.dashAttackTime + data.dashEndTime)// dashTime over transition to another state
        {
            if (data.LastOnGroundTime > 0)
                stateMachine.ChangeState(states.IdleState);
            else
                stateMachine.ChangeState(states.InAirState);
        } 
        else if (Time.time - startTime > data.dashAttackTime && data.LastPressedDashTime > 0 && states.DashState.CanDash())
            stateMachine.ChangeState(states.DashState);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        if (Time.time - startTime > data.dashAttackTime)
        {
            //initial dash phase over, slow down and resume player control
            motor.Drag(data.dragAmount);
            motor.Run(data.dashEndRunLerp);

            if (dashAttacking)
                StopDash();
        } 
        else
            motor.Drag(data.dashAttackDragAmount);
    }

    private void StopDash()
    {
        dashAttacking = false;
        motor.SetGravityScale(data.gravityScale);

        if (dir.y > 0)
        {
            if (dir.x == 0)
                player.RB.AddForce((1 - data.dashUpEndMult) * player.RB.velocity.y * Vector2.down, ForceMode2D.Impulse);
            else
                player.RB.AddForce((1 - data.dashUpEndMult) * .7f * player.RB.velocity.y * Vector2.down, ForceMode2D.Impulse);
        }
    }

    public bool CanDash()
    {
        return dashesLeft > 0;
    }

    public void ResetDashes()
    {
       dashesLeft = data.dashAmount;
    }
}
