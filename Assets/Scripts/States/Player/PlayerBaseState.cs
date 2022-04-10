using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBaseState : BaseState
{
    public string m_AnimationName;

    public PlayerBaseState(PlayerStateMachine context, PlayerStateFactory factory) : base(context, factory) { }

    public override void CheckSwitchStates()
    {
        throw new System.NotImplementedException();
    }

    public override void EnterState()
    {
        throw new System.NotImplementedException();
    }

    public override void ExitState()
    {
        throw new System.NotImplementedException();
    }

    public override void UpdateState()
    {
        throw new System.NotImplementedException();
    }
}
