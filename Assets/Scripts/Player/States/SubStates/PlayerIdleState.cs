using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerGroundedState
{
    public PlayerIdleState(StateMachine stateMachine, PlayerData data, PlayerStateManager stateList, PlayerController player, PlayerMotor motor) 
        : base(stateMachine, data, stateList, player, motor) { }

    public override void Enter()
    {
        base.Enter();
    }
    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (InputManager.instance.MoveInput.x != 0)
        {
            stateMachine.ChangeState(states.RunState);
        }
        else if (data.IsAxeSwingPressed && states.AxeSwingState.SwingWasCanceled && states.AxeSwingState.CanSwingFromCancel())
        {
            stateMachine.ChangeState(states.AxeSwingState);
        }
        else if (data.IsAxeSwingPressed && states.AxeSwingState.CanSwing())
        {
            stateMachine.ChangeState(states.AxeSwingState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        motor.Run(1);
    }
}
