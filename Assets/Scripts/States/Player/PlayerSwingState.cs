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
        if (p_ctx.WalkStateArgs && !p_ctx.GetComponent<PlayerMotor>().DirectionLocked)
            SwitchState(p_factory.Walk());
        if (!p_ctx.GetComponent<PlayerAttack>().Attacking)
        {
            SwitchState(p_factory.Idle());
        }
    }

    public override void EnterState()
    {
        p_ctx.Motor.CurrentSpeed = 0;
    }

    public override void ExitState()
    {
        p_ctx.GetComponent<PlayerAttack>().Attacking = false;
        p_ctx.StopCoroutine(p_ctx.GetComponent<PlayerAttack>().C);
        p_ctx.GetComponent<PlayerMotor>().DirectionLocked = false;
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
    }
}
