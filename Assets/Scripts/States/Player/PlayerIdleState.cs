using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{
    public PlayerIdleState(PlayerStateMachine context, PlayerStateFactory factory) : base(context, factory) 
    {
        m_AnimationName = "_Idle";
    }

    public override void CheckSwitchStates()
    {
        PlayerStateMachine p_ctx = (PlayerStateMachine)m_ctx;
        PlayerStateFactory p_factory = (PlayerStateFactory)m_factory;

        if (p_ctx.WalkStateArgs)
            SwitchState(p_factory.Walk());
        if (p_ctx.SwingStateArgs)
            SwitchState(p_factory.Swing());
    }

    public override void EnterState()
    {
        
    }

    public override void ExitState()
    {
        
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
    }
}
