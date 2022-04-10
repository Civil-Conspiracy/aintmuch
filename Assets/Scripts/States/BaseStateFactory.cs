public class BaseStateFactory
{
    readonly BaseStateMachine m_context;

    public BaseStateFactory(BaseStateMachine context)
    {
        m_context = context;
    }

    public virtual BaseState Idle()
    {
        return new BaseIdleState(m_context, this);
    }

}
