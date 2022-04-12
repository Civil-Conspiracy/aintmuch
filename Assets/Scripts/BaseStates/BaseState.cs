public abstract class BaseState
{
    protected BaseStateMachine m_ctx;
    protected BaseStateFactory m_factory;

    public BaseState(BaseStateMachine context, BaseStateFactory factory)
    {
        m_ctx = context;
        m_factory = factory;
    }
    public abstract void EnterState();

    public abstract void UpdateState();

    public abstract void CheckSwitchStates();

    public abstract void ExitState();

    protected void SwitchState(BaseState newState)
    {
        ExitState();

        newState.EnterState();
        m_ctx.CurrentState = newState;
    }

}