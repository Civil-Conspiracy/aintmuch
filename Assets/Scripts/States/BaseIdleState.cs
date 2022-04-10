using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseIdleState : BaseState
{
    public BaseIdleState(BaseStateMachine context, BaseStateFactory factory) : base(context, factory) { }
    public override void CheckSwitchStates()
    {

    }

    public override void EnterState()
    {
        Debug.Log("Entered Idle State!");
    }

    public override void ExitState()
    {
        Debug.Log("Exited Idle State!");
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
    }
}
