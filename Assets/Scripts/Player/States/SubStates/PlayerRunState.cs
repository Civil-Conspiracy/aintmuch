using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRunState : PlayerGroundedState
{
    public PlayerRunState(StateMachine stateMachine, PlayerData data, PlayerStateManager stateList, PlayerController player, PlayerMotor motor) 
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

        if(InputManager.instance.MoveInput.x == 0)
        {
            stateMachine.ChangeState(states.IdleState);
        }
    }
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        motor.Run(1);

    }
}
