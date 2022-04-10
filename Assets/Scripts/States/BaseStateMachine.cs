using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseStateMachine : MonoBehaviour
{
    public BaseState CurrentState { get { return m_currentState; } set { m_currentState = value; } }


    BaseState m_currentState;
    BaseStateFactory m_states;

    protected void Awake()
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
