using UnityEngine;
public class PlayerStateManager
{
    private PlayerData data;
    public PlayerStateManager()
    {
    }
    public StateMachine StateMachine { get; private set; }
    public PlayerIdleState IdleState { get; private set; }
    public PlayerRunState RunState { get; private set; }
    public PlayerJumpState JumpState { get; private set; }
    public PlayerInAirState InAirState { get; private set; }
    public PlayerWallSlideState WallSlideState { get; private set; }
    public PlayerWallJumpState WallJumpState { get; private set; }
    public PlayerDashState DashState { get; private set; }
    public PlayerAxeSwingState AxeSwingState { get; private set; }

    public void Initialize(PlayerData data, PlayerController player)
    {
        this.data = data;
        StateMachine = new StateMachine();
        IdleState = new PlayerIdleState(StateMachine, data);
        RunState = new PlayerRunState(StateMachine, data);
        JumpState = new PlayerJumpState(StateMachine, data);
        InAirState = new PlayerInAirState(StateMachine, data);
        WallSlideState = new PlayerWallSlideState(StateMachine, data);
        WallJumpState = new PlayerWallJumpState(StateMachine, data);
        DashState = new PlayerDashState(StateMachine, data);
        AxeSwingState = new PlayerAxeSwingState(StateMachine, data);

        StateMachine.Initialize(IdleState, data, player);

        data.IsFacingRight = true;
    }

    public void LogicUpdate()
    {

        data.LastOnGroundTime -= Time.deltaTime;
        data.LastOnWallTime -= Time.deltaTime;
        data.LastOnWallRightTime -= Time.deltaTime;
        data.LastOnWallLeftTime -= Time.deltaTime;

        data.LastPressedJumpTime -= Time.deltaTime;
        data.LastPressedDashTime -= Time.deltaTime;

        //Ground Check
        if (Physics2D.OverlapBox(data.Player.GroundCheckPoint.position, data.Player.GroundCheckSize, 0, data.Player.GroundLayer)
            || (Physics2D.OverlapBox(data.Player.GroundCheckPoint.position, data.Player.GroundCheckSize, 0, data.Player.WallLayer)))
            data.LastOnGroundTime = data.jumpGraceTime;
        //Right Wall Check
        if ((Physics2D.OverlapBox(data.Player.RightWallCheckPoint.position, data.Player.WallCheckSize, 0, data.Player.WallLayer) && data.IsFacingRight)
            || (Physics2D.OverlapBox(data.Player.LeftWallCheckPoint.position, data.Player.WallCheckSize, 0, data.Player.WallLayer) && !data.IsFacingRight)
            || (Physics2D.OverlapBox(data.Player.RightWallCheckPoint.position, data.Player.WallCheckSize, 0, data.Player.TreeLayer) && data.IsFacingRight)
            || (Physics2D.OverlapBox(data.Player.LeftWallCheckPoint.position, data.Player.WallCheckSize, 0, data.Player.TreeLayer) && !data.IsFacingRight))
            data.LastOnWallRightTime = data.jumpGraceTime;
        //Left Wall Check
        if ((Physics2D.OverlapBox(data.Player.RightWallCheckPoint.position, data.Player.WallCheckSize, 0, data.Player.WallLayer) && !data.IsFacingRight)
            || (Physics2D.OverlapBox(data.Player.LeftWallCheckPoint.position, data.Player.WallCheckSize, 0, data.Player.WallLayer) && data.IsFacingRight)
            || (Physics2D.OverlapBox(data.Player.RightWallCheckPoint.position, data.Player.WallCheckSize, 0, data.Player.TreeLayer) && !data.IsFacingRight)
            || (Physics2D.OverlapBox(data.Player.LeftWallCheckPoint.position, data.Player.WallCheckSize, 0, data.Player.TreeLayer) && data.IsFacingRight))
            data.LastOnWallLeftTime = data.jumpGraceTime;

        data.LastOnWallTime = Mathf.Max(data.LastOnWallLeftTime, data.LastOnWallRightTime);
    }

}
