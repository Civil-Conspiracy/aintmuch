using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSwingState : PlayerBaseState
{
    readonly PlayerStateMachine p_ctx;
    readonly PlayerStateFactory p_factory;
    public PlayerSwingState(PlayerStateMachine context, PlayerStateFactory factory) : base(context, factory) 
    {
        m_AnimationName = "_AxeSwing";
        p_ctx = (PlayerStateMachine)m_ctx;
        p_factory = (PlayerStateFactory)m_factory;
    }

    public override void CheckSwitchStates()
    {

        if (p_ctx.GetComponent<PlayerAttack>().AttackFinished)
        {
            SwitchState(p_factory.Idle());
        }
        if (p_ctx.SwingStateArgs)
        {
            SwitchState(p_factory.Swing());
        }
        else if (p_ctx.WalkStateArgs)
        {
            SwitchState(p_factory.Walk());
        }
    }

    public override void EnterState()
    {
        p_ctx.Motor.CurrentSpeed = 0;
        p_ctx.Motor.DirectionLocked = true;
        p_ctx.PlayAnimation(true);
        p_ctx.StartCoroutine(p_ctx.GetComponent<PlayerAttack>().Attack());
    }

    public override void ExitState()
    {
        p_ctx.Motor.DirectionLocked = false;
        p_ctx.StopCoroutine(p_ctx.GetComponent<PlayerAttack>().Attack());
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
    }
}
