using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
    protected StateMachine stateMachine;
    protected PlayerData data;
    protected PlayerStateManager states;
    protected PlayerController player;
    protected PlayerMotor motor;

    protected float startTime;
    public bool ExitingState { get; protected set; }

    public PlayerState(StateMachine stateMachine, PlayerData data, PlayerStateManager stateList, PlayerController player, PlayerMotor motor)
    {
        this.stateMachine = stateMachine;
        this.data = data;
        this.states = stateList;
        this.player = player;
        this.motor = motor;
    }

    public virtual void Enter()
    {
        startTime = Time.time;
        ExitingState = false;
        player.PlayAnimation();
    }
    public virtual void Exit()
    {
        ExitingState = true;
    }

    public virtual void LogicUpdate() { }
    public virtual void PhysicsUpdate() { }
}
