using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWalkState : PlayerBaseState
{
    public PlayerWalkState(PlayerStateMachine context, PlayerStateFactory factory) : base(context, factory) 
    {
        m_AnimationName = "_Walk";
    }

    public override void CheckSwitchStates()
    {
        PlayerStateMachine p_ctx = (PlayerStateMachine)m_ctx;
        PlayerStateFactory p_factory = (PlayerStateFactory)m_factory;

        if (!p_ctx.WalkStateArgs)
            SwitchState(p_factory.Idle());
        if (p_ctx.SwingStateArgs)
            SwitchState(p_factory.Swing());
    }

    public override void EnterState()
    {
        
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
