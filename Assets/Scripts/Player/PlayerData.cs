using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Player/Player Data")]
public class PlayerData : ScriptableObject
{
    [SerializeField] private DebugData defaultDebugData;
    public DebugData Debug { get { return defaultDebugData; } }
    public enum WallSides
    {
        NONE,
        LEFT,
        RIGHT
    }
    //PHYSICS
    [Header("Gravity")]
    public float gravityScale; // overrides rb.gravityScale
    public float fallGravityMult;
    public float quickFallGravityMult;
    public float axeSwingGravity;
    public float wallSlideGravity;

    [Header("Drag")]
    public float dragAmount; // drag in air
    public float frictionAmount; //drag on ground

    [Header("Other Physics")]
    [Range(0, 0.5f)] public float jumpGraceTime; // grace time to jump after the player has fallen off of a platform

    //GROUND
    [Header("Run")]
    public float runMaxSpeed;
    public float runAccel;
    public float runDeccel;
    [Range(0, 1)] public float accelInAir;
    [Range(0, 1)] public float deccelInAir;
    [Space(5)]
    [Range(.5f, 2f)] public float accelPower;
    [Range(.5f, 2f)] public float stopPower;
    [Range(.5f, 2f)] public float turnPower;

    //JUMP
    [Header("Jump")]
    public float jumpForce;
    [Range(0, 1)] public float jumpCutMultiplier;
    [Space(10)]
    [Range(0, 0.5f)] public float jumpBufferTime; //time after pressing the jump button where if the requirements are met a jump will be automatically performed

    [Header("Wall Jump")]
    public int wallJumpAmount;
    public Vector2 wallJumpForce;
    [Space(5)]
    [Range(0f, 1f)] public float wallJumpRunLerp; //slows the affect of player movement while wall jumping
    [Range(0f, 1.5f)] public float wallJumpTime;
    public WallSides lastWallTouched;

    //WALL
    [Header("Slide")]
    public float slideAccel;
    [Range(.5f, 2f)] public float slidePower;

    //ABILITIES
    [Header("Dash")]
    public int dashAmount;
    public float dashSpeed;
    [Space(5)]
    public float dashAttackTime;
    public float dashAttackDragAmount;
    [Space(5)]
    public float dashEndTime; //time after you finish the inital drag phase, smoothing the transition back to idle (or any standard state)
    [Range(0f, 1f)] public float dashUpEndMult; //slows down player when moving up, makes dash feel more responsive (used in Celeste)
    [Range(0f, 1f)] public float dashEndRunLerp; //slows the affect of player movement while dashing
    [Space(5)]
    [Range(0, 0.5f)] public float dashBufferTime;
    
    [Header("Axe Swing")]
    public int damage;
    [Space(1)]
    public float swingAttackTime;
    public float swingEndTime;
    [Space(5)]
    [Range(0, 0.5f)]  public float swingBufferTime;

    //OTHER
    [Header("Other Settings")]
    public bool doKeepRunMomentum; //player movement will not decrease speed if above maxSpeed, letting only drag do so. Allows for conservation of momentum
    public bool doTurnOnWallJump; //player will rotate to face wall jumping direction
    public bool doDebug;
    [Space(5)]
    [Header("State Data")]
    private float _lastOnGroundTime;
    private float _lastOnWallTime;
    private float _lastOnWallLeftTime;
    private float _lastOnWallRightTime;
    private float _lastOnSlopeTime;
    public bool _isFacingRight;
    [Space(10)]
    public float _lastPressedJumpTime;
    public float _lastPressedJumpUpTime;
    public float _lastPressedDashTime;
    public bool _isAxeSwingPressed;
    public float LastOnGroundTime { get { return _lastOnGroundTime; } set { _lastOnGroundTime = value; } }
    public float LastOnWallTime { get { return _lastOnWallTime; } set { _lastOnWallTime = value; } }
    public float LastOnWallLeftTime { get { return _lastOnWallLeftTime; } set { _lastOnWallLeftTime = value; } }
    public float LastOnWallRightTime { get { return _lastOnWallRightTime; } set { _lastOnWallRightTime = value; } }
    public float LastOnSlopeTime { get { return _lastOnSlopeTime; } set { _lastOnSlopeTime = value; } }
    public bool IsFacingRight { get { return _isFacingRight; } set { _isFacingRight = value; } }
    public float LastPressedJumpTime { get { return _lastPressedJumpTime; } set { _lastPressedJumpTime = value; } }
    public float LastPressedJumpUpTime { get { return _lastPressedJumpUpTime; } set { _lastPressedJumpUpTime = value; } }
    public float LastPressedDashTime { get { return _lastPressedDashTime; } set { _lastPressedDashTime = value; } }
    public bool IsAxeSwingPressed { get { return _isAxeSwingPressed; } set { _isAxeSwingPressed = value; } }

}
