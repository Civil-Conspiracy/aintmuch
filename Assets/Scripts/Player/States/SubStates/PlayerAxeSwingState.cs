using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAxeSwingState : PlayerInActionState
{
    public PlayerAxeSwingState(StateMachine stateMachine, PlayerData data, PlayerStateManager stateList, PlayerController player, PlayerMotor motor)
        : base(stateMachine, data, stateList, player, motor) { }

    public bool SwingWasCanceled { get; private set; }
    bool attacking;

    public override void Enter()
    {
        base.Enter();

        attacking = true;
        SwingWasCanceled = false;
        motor.SetGravityScale(data.axeSwingGravity);
    }

    public override void Exit()
    {
        base.Exit();

        attacking = false;
        motor.SetGravityScale(data.gravityScale);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (!attacking && data.LastPressedJumpTime > 0)
        {
            stateMachine.ChangeState(states.JumpState);
        }
        else if (attacking && data.LastPressedDashTime > 0 && states.DashState.CanDash())
        {
                CancelAttack();
            stateMachine.ChangeState(states.DashState);
        }
        else if (!attacking && InputManager.instance.MoveInput.x != 0)
        {
            CancelAttack();
            stateMachine.ChangeState(states.RunState);
        }
        else if (!attacking && data.IsAxeSwingPressed && data.LastOnGroundTime > 0)
        {
            CancelAttack();
            stateMachine.ChangeState(states.IdleState);
        }
        else if (Time.time - startTime > data.swingAttackTime + data.swingEndTime)// swingTime over transition to another state
        {
            if (data.LastOnGroundTime > 0)
                stateMachine.ChangeState(states.IdleState);
            else
                stateMachine.ChangeState(states.InAirState);
        }
            
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        if (attacking)
        {
            if (Time.time - startTime > data.swingAttackTime) //initial startup over, detect hit;
            {
                player.DetectHit(data.damage);
                attacking = false;
                motor.SetGravityScale(data.gravityScale);
            }
            else if (InputManager.instance.MoveInput.x != 0)
            {
                if (InputManager.instance.MoveInput.x < 0)
                    motor.CheckDirectionToFace(false);
                if (InputManager.instance.MoveInput.x > 0)
                    motor.CheckDirectionToFace(true);
            }
            motor.Drag(data.dragAmount);
            motor.Slide();
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
        motor.SetGravityScale(data.gravityScale);
    }
}
