using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : PlayerInActionState
{
    Vector2 dir;
    int dashesLeft;
    bool dashAttacking;

    public PlayerDashState(PlayerStateMachine player, StateMachine stateMachine, PlayerData data) : base(player, stateMachine, data) { }

    public override void Enter()
    {
        base.Enter();

        player.WallJumpState.ResetWallJumpCount();

        dashesLeft--;

        dir = Vector2.zero;
        if (InputManager.instance.MoveInput == Vector2.zero)
            dir.x = (player.IsFacingRight) ? 1 : -1;
        else
            dir = InputManager.instance.MoveInput;

        dashAttacking = true;
        player.Dash(dir);

    }

    public override void Exit()
    {
        base.Exit();
        player.SetGravityScale(data.gravityScale);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(Time.time - startTime > data.dashAttackTime + data.dashEndTime)// dashTime over transition to another state
        {
            if (player.LastOnGroundTime > 0)
                player.StateMachine.ChangeState(player.IdleState);
            else
                player.StateMachine.ChangeState(player.InAirState);
        } 
        else if (Time.time - startTime > data.dashAttackTime && player.LastPressedDashTime > 0 && player.DashState.CanDash())
            player.StateMachine.ChangeState(player.DashState);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        if (Time.time - startTime > data.dashAttackTime)
        {
            //initial dash phase over, slow down and resume player control
            player.Drag(data.dragAmount);
            player.Run(data.dashEndRunLerp);

            if (dashAttacking)
                StopDash();
        } 
        else
            player.Drag(data.dashAttackDragAmount);
    }

    private void StopDash()
    {
        dashAttacking = false;
        player.SetGravityScale(data.gravityScale);

        if (dir.y > 0)
        {
            if (dir.x == 0)
                player.RB.AddForce((1 - data.dashUpEndMult) * player.RB.velocity.y * Vector2.down, ForceMode2D.Impulse);
            else
                player.RB.AddForce((1 - data.dashUpEndMult) * .7f * player.RB.velocity.y * Vector2.down, ForceMode2D.Impulse);
        }
    }
    //public bool CanDash { get { if (dashesLeft > 0) return true; else return false; } }

    public bool CanDash()
    {
        return dashesLeft > 0;
    }

    public void ResetDashes()
    {
       dashesLeft = data.dashAmount;
    }
}
