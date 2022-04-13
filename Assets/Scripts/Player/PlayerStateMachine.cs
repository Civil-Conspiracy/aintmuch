using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    [SerializeField] private PlayerData data;

    #region STATE MACHINE
    public StateMachine StateMachine { get; private set; }
        public PlayerIdleState IdleState { get; private set; }
        public PlayerRunState RunState { get; private set; }
        public PlayerJumpState JumpState { get; private set; }
        public PlayerInAirState InAirState { get; private set; }
        public PlayerWallSlideState WallSlideState { get; private set; }
        public PlayerWallJumpState WallJumpState { get; private set; }
        public PlayerDashState DashState { get; private set; }
        public PlayerAxeSwingState AxeSwingState { get; private set; }

        [ReadOnly] public string CurrentState;
    #endregion

    #region COMPONENTS
    public Rigidbody2D RB { get; private set; }
    public Animator PlayerAnimator { get; private set; }
    public Animator AxeAnimator { get; private set; }
    #endregion

    #region STATE PARAMETERS
    public bool IsFacingRight { get; private set; }
    public float LastOnGroundTime { get; private set; }
    public float LastOnWallTime { get; private set; }
    public float LastOnWallRightTime { get; private set; }
    public float LastOnWallLeftTime { get; private set; }
    #endregion

    #region INPUT PARAMETERS
    public float LastPressedJumpTime { get; private set; }
    public float LastPressedDashTime { get; private set; }
    public bool IsAxeSwingPressed { get; private set; }
    #endregion

    #region CHECK PARAMETERS
    [Header("Checks")]
    [SerializeField] private Transform _groundCheckPoint;
    [SerializeField] private Vector2 _groundCheckSize;
    [Space(5)]
    [SerializeField] private Transform _rightWallCheckPoint;
    [SerializeField] private Transform _leftWallCheckPoint;
    [SerializeField] private Vector2 _wallCheckSize;
    #endregion

    #region LAYERS AND TAGS
    [Header("Layers & Tags")]
    [SerializeField] private LayerMask _groundLayer;
    #endregion

    private void Awake()
    {
        #region STATE MACHINE
        StateMachine = new StateMachine();

        IdleState = new PlayerIdleState(this, StateMachine, data);
        RunState = new PlayerRunState(this, StateMachine, data);
        JumpState = new PlayerJumpState(this, StateMachine, data);
        InAirState = new PlayerInAirState(this, StateMachine, data);
        WallSlideState = new PlayerWallSlideState(this, StateMachine, data);
        WallJumpState = new PlayerWallJumpState(this, StateMachine, data);
        DashState = new PlayerDashState(this, StateMachine, data);
        AxeSwingState = new PlayerAxeSwingState(this, StateMachine, data);
        #endregion

        RB = GetComponent<Rigidbody2D>();
        PlayerAnimator = GetComponent<Animator>();
        AxeAnimator = transform.Find("Axe").GetComponent<Animator>();
    }

    private void Start()
    {
        InputManager.instance.OnJumpPressed += args => OnJump(args);
        InputManager.instance.OnJumpReleased += args => OnJumpReleased(args);
        InputManager.instance.OnDash += args => OnDash(args);
        InputManager.instance.OnAxeSwing += args => OnAxeSwing(args);

        StateMachine.Initialize(this, IdleState);

        SetGravityScale(data.gravityScale);
        IsFacingRight = true;
    }

    private void Update()
    {
        StateMachine.CurrentState.LogicUpdate();

        LastOnGroundTime -= Time.deltaTime;
        LastOnWallTime -= Time.deltaTime;
        LastOnWallRightTime -= Time.deltaTime;
        LastOnWallLeftTime -= Time.deltaTime;

        LastPressedJumpTime -= Time.deltaTime;
        LastPressedDashTime -= Time.deltaTime;

        //Ground Check
        if (Physics2D.OverlapBox(_groundCheckPoint.position, _groundCheckSize, 0, _groundLayer))
            LastOnGroundTime = data.jumpGraceTime;
        //Right Wall Check
        if ((Physics2D.OverlapBox(_rightWallCheckPoint.position, _wallCheckSize, 0, _groundLayer) && IsFacingRight)
            || (Physics2D.OverlapBox(_leftWallCheckPoint.position, _wallCheckSize, 0, _groundLayer) && !IsFacingRight))
            LastOnWallRightTime =  data.jumpGraceTime;   
        //Left Wall Check
        if ((Physics2D.OverlapBox(_rightWallCheckPoint.position, _wallCheckSize, 0, _groundLayer) && !IsFacingRight)
            || (Physics2D.OverlapBox(_leftWallCheckPoint.position, _wallCheckSize, 0, _groundLayer) && IsFacingRight))
            LastOnWallLeftTime =  data.jumpGraceTime;

        LastOnWallTime = Mathf.Max(LastOnWallLeftTime, LastOnWallRightTime);
    }

    private void FixedUpdate()
    {
        StateMachine.CurrentState.PhysicsUpdate();
    }

    #region INPUT CALLBACKS
    public void OnJump(InputManager.InputArgs args)
    {
        LastPressedJumpTime = data.jumpBufferTime;
    }

    public void OnJumpReleased(InputManager.InputArgs args)
    {
        if (JumpState.CanJumpCut() || WallJumpState.CanJumpCut())
            JumpCut();
    }

    public void OnDash(InputManager.InputArgs args)
    {
        LastPressedDashTime = data.dashBufferTime;
    }

    public void OnAxeSwing(InputManager.InputArgs args)
    {
        IsAxeSwingPressed = args.context.ReadValueAsButton();
    }
    #endregion

    #region MOVEMENT METHODS
    public void SetGravityScale(float scale)
    {
        RB.gravityScale = scale;
    }
    public void Drag(float amount)
    {
        Vector2 force = amount * RB.velocity.normalized;
        force.x = Mathf.Min(Mathf.Abs(RB.velocity.x), Mathf.Abs(force.x)); // slow player down
        force.y = Mathf.Min(Mathf.Abs(RB.velocity.y), Mathf.Abs(force.y));
        force.x *= Mathf.Sign(RB.velocity.x); // finds dir to apply force
        force.y *= Mathf.Sign(RB.velocity.y);

        RB.AddForce(-force, ForceMode2D.Impulse);
    }
    public void Run(float lerpAmount)
    {
        float targetSpeed = InputManager.instance.MoveInput.x * data.runMaxSpeed; // calculate desired velocity direction
        float speedDif = targetSpeed - RB.velocity.x; // calculate difference between current and desired velocities
        #region Acceleration Rate
        float accelRate;
        if (LastOnGroundTime > 0)
            accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? data.runAccel : data.runDeccel;
        else
            accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? data.runAccel * data.accelInAir : data.runDeccel * data.accelInAir;

        // if player wants to run, but is already going faster than max speed
        if (((RB.velocity.x > targetSpeed && targetSpeed > 0.01f) || (RB.velocity.x < targetSpeed && targetSpeed < -0.01f)) && data.doKeepRunMomentum)
            accelRate = 0;
        #endregion

        #region Velocity Power
        float velPower;
        if(Mathf.Abs(targetSpeed) < 0.01f)
        {
            velPower = data.stopPower;
        }
        else if (Mathf.Abs(RB.velocity.x) > 0 && (Mathf.Sign(targetSpeed) != Mathf.Sign(RB.velocity.x)))
        {
            velPower = data.turnPower;
        }
        else
        {
            velPower = data.accelPower;
        }
        #endregion

        float movement = Mathf.Pow(Mathf.Abs(speedDif) * accelRate, velPower) * Mathf.Sign(speedDif);
        movement = Mathf.Lerp(RB.velocity.x, movement, lerpAmount);

        RB.AddForce(movement * Vector2.right); // applies force, multiply by Vector2.right so it only affects the X axis

        if (InputManager.instance.MoveInput.x != 0)
            CheckDirectionToFace(InputManager.instance.MoveInput.x > 0);
    }

    private void Turn()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;

        IsFacingRight = !IsFacingRight;
    }

    public void Jump()
    {
        //ensures only one jump happens regardless of number of presses
        LastPressedJumpTime = 0;
        LastOnGroundTime = 0;

        float adjustedJumpForce = data.jumpForce;
        if (RB.velocity.y < 0)
            adjustedJumpForce -= RB.velocity.y;

        RB.AddForce(Vector2.up * adjustedJumpForce, ForceMode2D.Impulse);
    }

    public void WallJump(int dir)
    {
        LastPressedJumpTime = 0;
        LastOnGroundTime = 0;
        LastOnWallLeftTime = 0;
        LastOnWallLeftTime = 0;

        Vector2 force = new Vector2(data.wallJumpForce.x, data.wallJumpForce.y);
        force.x *= dir; // apply force in opposite direction of wall

        if (Mathf.Sign(RB.velocity.x) != Mathf.Sign(force.x))
            force.x -= RB.velocity.x;
        if (RB.velocity.y < 0) //checks if player is falling
            force.y -= RB.velocity.y;

        RB.AddForce(force, ForceMode2D.Impulse);
    }

    private void JumpCut()
    {
        // applies force downward when the jump button is released, allowing the player to control jump height.
        RB.AddForce((1 - data.jumpCutMultiplier) * RB.velocity.y * Vector2.down, ForceMode2D.Impulse);
    }

    public void Slide()
    {
        float targetSpeed = 0;
        float speedDif = targetSpeed - RB.velocity.y;

        float movement = Mathf.Pow(Mathf.Abs(speedDif) * data.slideAccel, data.slidePower) * Mathf.Sign(speedDif);
        RB.AddForce(movement * Vector2.up, ForceMode2D.Force);
    }

    public void Dash(Vector2 dir)
    {
        LastOnGroundTime = 0;
        LastPressedDashTime = 0;

        RB.velocity = dir.normalized * data.dashSpeed;

        SetGravityScale(0);
    }
    #endregion

    #region INTERACTION METHODS
    public void DetectHit(int damage)
    {
        Vector2 newPos = transform.position;
        newPos.y -= 0.5f;

        Vector2 dir = new Vector2((IsFacingRight) ? 1 : -1, -0.25f);

        Debug.DrawRay(newPos, dir, Color.red, 1.15f);

        RaycastHit2D[] hits = Physics2D.RaycastAll(newPos, dir, 1.15f);

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider.CompareTag("Damageable"))
            {
                Camera.main.GetComponent<CameraController>().Shake(0.05f);
                hit.collider.gameObject.GetComponent<IDamageable>().Damage(damage);
            }
        }
    }
    #endregion



    #region UPDATE METHODS
    public void CheckDirectionToFace(bool isMovingRight)
    {
        if (isMovingRight != IsFacingRight)
            Turn();
    }

    #endregion

    #region ANIMATION METHODS
    public void PlayAnimation()
    {
        switch (CurrentState)
        {
            case "PlayerRunState":
                PlayerAnimator.Play("goblinguy_Walk");
                AxeAnimator.Play("axe_Walk");
                break;
            case "PlayerIdleState":
                PlayerAnimator.Play("goblinguy_Idle");
                AxeAnimator.Play("axe_Idle");
                break;
            case "PlayerAxeSwingState":
                PlayerAnimator.Play("goblinguy_AxeSwing");
                AxeAnimator.Play("axe_AxeSwing");
                break;
            default:
                PlayerAnimator.Play("goblinguy_Idle");
                AxeAnimator.Play("axe_Idle");
                break;
        }
    }
    #endregion
}
