using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : BaseStateMachine
{
    // Local Component and Object Fields
    [SerializeField] Animator player_animator;
    [SerializeField] Animator axe_animator;
    PlayerMotor m_motor;

    // Switch State Arguement Properites
    public bool WalkStateArgs
    {
        get
        {
            if (m_motor.Direction != 0)
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
                return false;
            else
                return true;
        }
    }

    // Public Component and Object Properites
    public PlayerMotor Motor { get { return m_motor; } }

    // Local Variable fields
    string m_stateAnimName = null;


    // Initialize current states
    private void Awake()
    {
        m_motor = GetComponent<PlayerMotor>();
        m_states = new PlayerStateFactory(this);
        m_currentState = m_states.Idle();
        m_currentState.EnterState();

        Debug.Log(player_animator.GetCurrentAnimatorStateInfo(0).length);
    }
    // Update states and animations
    private new void Update()
    {
        base.Update();
        PlayAnimation();
        FlipSprite();
    }
    // Update Methods
    public void PlayAnimation()
    {
        PlayerBaseState current_state = (PlayerBaseState)m_currentState;
        if(m_stateAnimName != current_state.m_AnimationName)
        {
            m_stateAnimName = current_state.m_AnimationName;
            player_animator.Play("goblinguy" + current_state.m_AnimationName);
            axe_animator.Play("axe" + current_state.m_AnimationName);
        }
    }  /*Plays player and axe animations based on the current state's animation name */
    private void FlipSprite()
    {
        if (m_motor.CurrentDirection == 1 && transform.rotation != Quaternion.Euler(0, 180f, 0))
            transform.rotation = Quaternion.Euler(0, 180f, 0);
        else if (m_motor.CurrentDirection == -1 && transform.rotation != Quaternion.Euler(0, 0, 0))
            transform.rotation = Quaternion.Euler(0, 0, 0);


    } /*Flips player and axe sprites if player's direction is less than 0 (-1); flips back if direction is greater than 0 (1)*/

}
