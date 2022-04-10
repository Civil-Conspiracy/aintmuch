using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateFactory : BaseStateFactory
{
    public PlayerStateFactory(PlayerStateMachine context) : base(context) { }

    public override BaseState Idle()
    {
        return new PlayerIdleState((PlayerStateMachine)m_context, this);
    }

    public BaseState Walk()
    {
        return new PlayerWalkState((PlayerStateMachine)m_context, this);
    }
    
    public BaseState Swing()
    {
        return new PlayerSwingState((PlayerStateMachine)m_context, this);
    }
}
