using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAxeSwingState : PlayerInActionState
{
    public PlayerAxeSwingState(StateMachine stateMachine, PlayerData data) : base(stateMachine, data) { }

    public bool SwingWasCanceled { get; private set; }
    bool attacking;

    public override void Enter()
    {
        base.Enter();

        attacking = true;
        SwingWasCanceled = false;
        data.Motor.SetGravityScale(data.axeSwingGravity);
    }

    public override void Exit()
    {
        base.Exit();

        attacking = false;
        data.Motor.SetGravityScale(data.gravityScale);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (!attacking && data.LastPressedJumpTime > 0)
        {
            stateMachine.ChangeState(data.States.JumpState);
        }
        else if (attacking && data.LastPressedDashTime > 0 && data.States.DashState.CanDash())
        {
                CancelAttack();
            stateMachine.ChangeState(data.States.DashState);
        }
        else if (!attacking && InputManager.instance.MoveInput.x != 0)
        {
            CancelAttack();
            stateMachine.ChangeState(data.States.RunState);
        }
        else if (!attacking && data.IsAxeSwingPressed && data.LastOnGroundTime > 0)
        {
            CancelAttack();
            stateMachine.ChangeState(data.States.IdleState);
        }
        else if (Time.time - startTime > data.swingAttackTime + data.swingEndTime)// swingTime over transition to another state
        {
            if (data.LastOnGroundTime > 0)
                stateMachine.ChangeState(data.States.IdleState);
            else
                stateMachine.ChangeState(data.States.InAirState);
        }
            
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        if (attacking)
        {
            if (Time.time - startTime > data.swingAttackTime) //initial startup over, detect hit;
            {
                data.Player.DetectHit(data.damage);
                attacking = false;
            }
            else if (InputManager.instance.MoveInput.x != 0)
            {
                if (InputManager.instance.MoveInput.x < 0)
                    data.Motor.CheckDirectionToFace(false);
                if (InputManager.instance.MoveInput.x > 0)
                    data.Motor.CheckDirectionToFace(true);
            }
            data.Motor.Drag(data.dragAmount);
            data.Motor.Slide();
        }

    }

    public bool CanSwing()
    {
        return (Time.time - startTime > data.swingAttackTime + data.swingEndTime);
    }
    public bool CanSwingFromCancel()
    {
        return (Time.time - startTime > data.swingAttackTime);
    }

    private void CancelAttack()
    {
        attacking = false;
        SwingWasCanceled = true;
        data.Motor.SetGravityScale(data.gravityScale);
    }
}
