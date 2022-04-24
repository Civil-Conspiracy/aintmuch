using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
    protected StateMachine stateMachine;
    protected PlayerData data;

    protected float startTime;
    public bool ExitingState { get; protected set; }

    public PlayerState(StateMachine stateMachine, PlayerData data)
    {
        this.stateMachine = stateMachine;
        this.data = data;
    }

    public virtual void Enter()
    {
        startTime = Time.time;
        ExitingState = false;
        data.Player.PlayAnimation();
    }
    public virtual void Exit()
    {
        ExitingState = true;
    }

    public virtual void LogicUpdate() { }
    public virtual void PhysicsUpdate() { }
}
