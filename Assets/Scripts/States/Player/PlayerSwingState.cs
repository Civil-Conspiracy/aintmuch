using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSwingState : PlayerBaseState
{
    public PlayerSwingState(PlayerStateMachine context, PlayerStateFactory factory) : base(context, factory) 
    {
        m_AnimationName = "_AxeSwing";
    }

    public override void CheckSwitchStates()
    {
        PlayerStateMachine p_ctx = (PlayerStateMachine)m_ctx;
        PlayerStateFactory p_factory = (PlayerStateFactory)m_factory;

        if (!p_ctx.GetComponent<PlayerAttack>().Attacking)
        {
            if (p_ctx.WalkStateArgs)
            {
                SwitchState(p_factory.Walk());
            }
            if (!p_ctx.WalkStateArgs)
                SwitchState(p_factory.Idle());
        }
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
