using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : BaseStateMachine
{
    [SerializeField] Animator player_animator;
    [SerializeField] Animator axe_animator;

    private void Awake()
    {
        m_states = new PlayerStateFactory(this);
        m_currentState = m_states.Idle();
        m_currentState.EnterState();
    }

    private new void Update()
    {
        base.Update();
        PlayAnimation();
    }

    public void PlayAnimation()
    {
        PlayerBaseState current_state = (PlayerBaseState)m_currentState;

        player_animator.Play("goblinguy" + current_state.m_AnimationName);
        axe_animator.Play("axe" + current_state.m_AnimationName);
    }

    public bool WalkStateArgs
    {
        get
        {
            if (GetComponent<PlayerMotor>().Direction != 0)
                return true;
            else
                return false;
        }
    }

    public bool SwingStateArgs
    {
        get
        {
            if (GetComponent<PlayerAttack>().Attacking)
                return true;
            else
                return false;
        }
    }
}
