using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWalkState : PlayerBaseState
{
    readonly PlayerStateMachine p_ctx;
    readonly PlayerStateFactory p_factory;
    public PlayerWalkState(PlayerStateMachine context, PlayerStateFactory factory) : base(context, factory) 
    {
        m_AnimationName = "_Walk";
        p_ctx = (PlayerStateMachine)m_ctx;
        p_factory = (PlayerStateFactory)m_factory;
    }

    public override void CheckSwitchStates()
    {

        if (!p_ctx.WalkStateArgs)
            SwitchState(p_factory.Idle());
        if (p_ctx.SwingStateArgs)
            SwitchState(p_factory.Swing());
    }

    public override void EnterState()
    {
        p_ctx.Motor.CurrentSpeed = p_ctx.Motor.BaseSpeed;
    }

    public override void ExitState()
    {
        // not implemented
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
    }

}
