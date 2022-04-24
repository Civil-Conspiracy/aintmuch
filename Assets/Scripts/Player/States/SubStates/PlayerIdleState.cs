using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerGroundedState
{
    public PlayerIdleState(StateMachine stateMachine, PlayerData data) : base(stateMachine, data) { }

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
            stateMachine.ChangeState(data.States.RunState);
        }
        else if (data.IsAxeSwingPressed && data.States.AxeSwingState.SwingWasCanceled && data.States.AxeSwingState.CanSwingFromCancel())
        {
            stateMachine.ChangeState(data.States.AxeSwingState);
        }
        else if (data.IsAxeSwingPressed && data.States.AxeSwingState.CanSwing())
        {
            stateMachine.ChangeState(data.States.AxeSwingState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        data.Motor.Run(1);
    }
}
