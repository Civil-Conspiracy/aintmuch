public class PlayerOnWallState : PlayerState
{
    public PlayerOnWallState(StateMachine stateMachine, PlayerData data, PlayerStateManager stateList, PlayerController player, PlayerMotor motor)
        : base(stateMachine, data, stateList, player, motor) { }

    public override void Enter()
    {
        base.Enter();
    }
    public override void Exit()
    {
        base.Exit();
    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (data.LastPressedDashTime > 0 && states.DashState.CanDash())
        {
            stateMachine.ChangeState(states.DashState);
        }
        else if (data.LastOnGroundTime > 0)
        {
            stateMachine.ChangeState(states.IdleState);
        }
        else if (data.LastOnWallTime <= 0)
        {
            stateMachine.ChangeState(states.InAirState);
        }
    }
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
