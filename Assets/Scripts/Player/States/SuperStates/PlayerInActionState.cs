using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInActionState : PlayerState
{
    public PlayerInActionState(StateMachine stateMachine, PlayerData data) : base(stateMachine, data) { }

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
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
