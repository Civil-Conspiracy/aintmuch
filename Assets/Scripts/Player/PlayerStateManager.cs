using UnityEngine;
public class PlayerStateManager
{
    protected PlayerData data;
    protected PlayerController player;
    protected PlayerMotor motor;

    public PlayerStateManager(PlayerData data, PlayerController player, PlayerMotor motor)
    {
        this.player = player;
        this.data = data;
        this.motor = motor;
    }

    public StateMachine Machine { get; private set; }
    public PlayerIdleState IdleState { get; private set; }
    public PlayerRunState RunState { get; private set; }
    public PlayerJumpState JumpState { get; private set; }
    public PlayerInAirState InAirState { get; private set; }
    public PlayerWallSlideState WallSlideState { get; private set; }
    public PlayerWallJumpState WallJumpState { get; private set; }
    public PlayerDashState DashState { get; private set; }
    public PlayerAxeSwingState AxeSwingState { get; private set; }

    public void Initialize()
    {
        Machine = new StateMachine();
        IdleState = new PlayerIdleState(Machine, data, this, player, motor);
        RunState = new PlayerRunState(Machine, data, this, player, motor);
        JumpState = new PlayerJumpState(Machine, data, this, player, motor);
        InAirState = new PlayerInAirState(Machine, data, this, player, motor);
        WallSlideState = new PlayerWallSlideState(Machine, data, this, player, motor);
        WallJumpState = new PlayerWallJumpState(Machine, data, this, player, motor);
        DashState = new PlayerDashState(Machine, data, this, player, motor);
        AxeSwingState = new PlayerAxeSwingState(Machine, data, this, player, motor);

        Machine.Initialize(IdleState, data, player);

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
        if (Physics2D.OverlapBox(player.GroundCheckPoint.position, player.GroundCheckSize, 0, player.GroundLayer)
            || (Physics2D.OverlapBox(player.GroundCheckPoint.position, player.GroundCheckSize, 0, player.WallLayer)))
            data.LastOnGroundTime = data.jumpGraceTime;
        //Right Wall Check
        if ((Physics2D.OverlapBox(player.RightWallCheckPoint.position, player.WallCheckSize, 0, player.WallLayer) && data.IsFacingRight)
            || (Physics2D.OverlapBox(player.LeftWallCheckPoint.position, player.WallCheckSize, 0, player.WallLayer) && !data.IsFacingRight)
            || (Physics2D.OverlapBox(player.RightWallCheckPoint.position, player.WallCheckSize, 0, player.TreeLayer) && data.IsFacingRight)
            || (Physics2D.OverlapBox(player.LeftWallCheckPoint.position, player.WallCheckSize, 0, player.TreeLayer) && !data.IsFacingRight))
            data.LastOnWallRightTime = data.jumpGraceTime;
        //Left Wall Check
        if ((Physics2D.OverlapBox(player.RightWallCheckPoint.position, player.WallCheckSize, 0, player.WallLayer) && !data.IsFacingRight)
            || (Physics2D.OverlapBox(player.LeftWallCheckPoint.position, player.WallCheckSize, 0, player.WallLayer) && data.IsFacingRight)
            || (Physics2D.OverlapBox(player.RightWallCheckPoint.position, player.WallCheckSize, 0, player.TreeLayer) && !data.IsFacingRight)
            || (Physics2D.OverlapBox(player.LeftWallCheckPoint.position, player.WallCheckSize, 0, player.TreeLayer) && data.IsFacingRight))
            data.LastOnWallLeftTime = data.jumpGraceTime;

        data.LastOnWallTime = Mathf.Max(data.LastOnWallLeftTime, data.LastOnWallRightTime);
    }

}
