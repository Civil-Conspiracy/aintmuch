using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerGroundedState
{
    public PlayerIdleState(PlayerStateMachine player, StateMachine stateMachine, PlayerData data) : base(player, stateMachine, data) { }

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
            stateMachine.ChangeState(player.RunState);
        }
        else if (player.IsAxeSwingPressed && player.AxeSwingState.SwingWasCanceled && player.AxeSwingState.CanSwingFromCancel())
        {
            player.StateMachine.ChangeState(player.AxeSwingState);
        }
        else if (player.IsAxeSwingPressed && player.AxeSwingState.CanSwing())
        {
            player.StateMachine.ChangeState(player.AxeSwingState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        player.Run(1);
    }
}
