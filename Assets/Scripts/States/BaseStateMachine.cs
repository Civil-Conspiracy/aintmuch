using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseStateMachine : MonoBehaviour
{
    public BaseState CurrentState { get { return m_currentState; } set { m_currentState = value; } }


    protected BaseState m_currentState;
    protected BaseStateFactory m_states;

    private void Awake()
    {
        m_states = new BaseStateFactory(this);
        m_currentState = m_states.Idle();
        m_currentState.EnterState();
    }

    protected void Update()
    {
        m_currentState.UpdateState();
    }
}
