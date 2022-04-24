using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;

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
    [ReadOnly] public WallSides lastWallTouched;

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
    [ReadOnly] public float LastOnGroundTime;
    [ReadOnly] public float LastOnWallTime;
    [ReadOnly] public float LastOnWallLeftTime;
    [ReadOnly] public float LastOnWallRightTime;
    [ReadOnly] public bool IsFacingRight;
    [Space(10)]
    [ReadOnly] public float LastPressedJumpTime;
    [ReadOnly] public float LastPressedJumpUpTime;
    [ReadOnly] public float LastPressedDashTime;
    [ReadOnly] public bool IsAxeSwingPressed;
}
