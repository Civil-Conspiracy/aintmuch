using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{
    readonly PlayerStateMachine p_ctx;
    readonly PlayerStateFactory p_factory;
    public PlayerIdleState(PlayerStateMachine context, PlayerStateFactory factory) : base(context, factory) 
    {
        m_AnimationName = "_Idle";
        p_ctx = (PlayerStateMachine)m_ctx;
        p_factory = (PlayerStateFactory)m_factory;
    }

    public override void CheckSwitchStates()
    {

        if (p_ctx.WalkStateArgs)
            SwitchState(p_factory.Walk());
        if (p_ctx.SwingStateArgs)
            SwitchState(p_factory.Swing());
    }

    public override void EnterState()
    {
        p_ctx.Motor.CurrentSpeed = 0;
    }

    public override void ExitState()
    {
        
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
    }
}
