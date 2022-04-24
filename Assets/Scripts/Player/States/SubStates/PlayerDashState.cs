using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : PlayerInActionState
{
    Vector2 dir;
    int dashesLeft;
    bool dashAttacking;

    public PlayerDashState(StateMachine stateMachine, PlayerData data) : base(stateMachine, data) { }

    public override void Enter()
    {
        base.Enter();

        data.States.WallJumpState.ResetWallJumpCount();

        dashesLeft--;

        dir = Vector2.zero;
        if (InputManager.instance.MoveInput == Vector2.zero)
            dir.x = (data.IsFacingRight) ? 1 : -1;
        else
            dir = InputManager.instance.MoveInput;

        dashAttacking = true;
        data.Motor.Dash(dir);

    }

    public override void Exit()
    {
        base.Exit();
        data.Motor.SetGravityScale(data.gravityScale);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(Time.time - startTime > data.dashAttackTime + data.dashEndTime)// dashTime over transition to another state
        {
            if (data.LastOnGroundTime > 0)
                stateMachine.ChangeState(data.States.IdleState);
            else
                stateMachine.ChangeState(data.States.InAirState);
        } 
        else if (Time.time - startTime > data.dashAttackTime && data.LastPressedDashTime > 0 && data.States.DashState.CanDash())
            stateMachine.ChangeState(data.States.DashState);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        if (Time.time - startTime > data.dashAttackTime)
        {
            //initial dash phase over, slow down and resume player control
            data.Motor.Drag(data.dragAmount);
            data.Motor.Run(data.dashEndRunLerp);

            if (dashAttacking)
                StopDash();
        } 
        else
            data.Motor.Drag(data.dashAttackDragAmount);
    }

    private void StopDash()
    {
        dashAttacking = false;
        data.Motor.SetGravityScale(data.gravityScale);

        if (dir.y > 0)
        {
            if (dir.x == 0)
                data.Player.RB.AddForce((1 - data.dashUpEndMult) * data.Player.RB.velocity.y * Vector2.down, ForceMode2D.Impulse);
            else
                data.Player.RB.AddForce((1 - data.dashUpEndMult) * .7f * data.Player.RB.velocity.y * Vector2.down, ForceMode2D.Impulse);
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
