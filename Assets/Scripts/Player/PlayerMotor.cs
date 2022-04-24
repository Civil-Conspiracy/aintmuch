using UnityEngine;

public class PlayerMotor
{
    protected PlayerData data;
    protected PlayerController player;
    public PlayerMotor(PlayerData _data, PlayerController _player)
    {
        data = _data;
        player = _player;
    }
    public void SetGravityScale(float scale)
    {
        player.RB.gravityScale = scale;
    }
    public void Drag(float amount)
    {
        Vector2 force = amount * player.RB.velocity.normalized;
        force.x = Mathf.Min(Mathf.Abs(player.RB.velocity.x), Mathf.Abs(force.x)); // slow player down
        force.y = Mathf.Min(Mathf.Abs(player.RB.velocity.y), Mathf.Abs(force.y));
        force.x *= Mathf.Sign(player.RB.velocity.x); // finds dir to apply force
        force.y *= Mathf.Sign(player.RB.velocity.y);

        player.RB.AddForce(-force, ForceMode2D.Impulse);
    }
    public void Run(float lerpAmount)
    {
        float targetSpeed = InputManager.instance.MoveInput.x * data.runMaxSpeed; // calculate desired velocity direction
        float speedDif = targetSpeed - player.RB.velocity.x; // calculate difference between current and desired velocities
        #region Acceleration Rate
        float accelRate;
        if (data.LastOnGroundTime > 0)
            accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? data.runAccel : data.runDeccel;
        else
            accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? data.runAccel * data.accelInAir : data.runDeccel * data.accelInAir;

        // if player wants to run, but is already going faster than max speed
        if (((player.RB.velocity.x > targetSpeed && targetSpeed > 0.01f) || (player.RB.velocity.x < targetSpeed && targetSpeed < -0.01f)) && data.doKeepRunMomentum)
            accelRate = 0;
        #endregion

        #region Velocity Power
        float velPower;
        if (Mathf.Abs(targetSpeed) < 0.01f)
        {
            velPower = data.stopPower;
        }
        else if (Mathf.Abs(player.RB.velocity.x) > 0 && (Mathf.Sign(targetSpeed) != Mathf.Sign(player.RB.velocity.x)))
        {
            velPower = data.turnPower;
        }
        else
        {
            velPower = data.accelPower;
        }
        #endregion

        float movement = Mathf.Pow(Mathf.Abs(speedDif) * accelRate, velPower) * Mathf.Sign(speedDif);
        movement = Mathf.Lerp(player.RB.velocity.x, movement, lerpAmount);

        player.RB.AddForce(movement * Vector2.right); // applies force, multiply by Vector2.right so it only affects the X axis

        if (InputManager.instance.MoveInput.x != 0)
            CheckDirectionToFace(InputManager.instance.MoveInput.x > 0);
    }

    public void Turn()
    {
        Vector3 scale = player.transform.localScale;
        scale.x *= -1;
        player.transform.localScale = scale;

        data.IsFacingRight = !data.IsFacingRight;
    }

    public void Jump()
    {
        //ensures only one jump happens regardless of number of presses
        data.LastPressedJumpTime = 0;
        data.LastOnGroundTime = 0;

        float adjustedJumpForce = data.jumpForce;
        if (player.RB.velocity.y < 0)
            adjustedJumpForce -= player.RB.velocity.y;

        player.RB.AddForce(Vector2.up * adjustedJumpForce, ForceMode2D.Impulse);
    }

    public void WallJump(int dir)
    {
        data.LastPressedJumpTime = 0;
        data.LastOnGroundTime = 0;
        data.LastOnWallLeftTime = 0;
        data.LastOnWallLeftTime = 0;

        Vector2 force = new Vector2(data.wallJumpForce.x, data.wallJumpForce.y);
        force.x *= dir; // apply force in opposite direction of wall

        if (Mathf.Sign(player.RB.velocity.x) != Mathf.Sign(force.x))
            force.x -= player.RB.velocity.x;
        if (player.RB.velocity.y < 0) //checks if player is falling
            force.y -= player.RB.velocity.y;

        player.RB.AddForce(force, ForceMode2D.Impulse);
    }

    public void JumpCut()
    {
        // applies force downward when the jump button is released, allowing the player to control jump height.
        player.RB.AddForce((1 - data.jumpCutMultiplier) * player.RB.velocity.y * Vector2.down, ForceMode2D.Impulse);
    }

    public void Slide()
    {
        float targetSpeed = 0;
        float speedDif = targetSpeed - player.RB.velocity.y;

        float movement = Mathf.Pow(Mathf.Abs(speedDif) * data.slideAccel, data.slidePower) * Mathf.Sign(speedDif);
        player.RB.AddForce(movement * Vector2.up, ForceMode2D.Force);
    }

    public void Dash(Vector2 dir)
    {
        data.LastOnGroundTime = 0;
        data.LastPressedDashTime = 0;

        player.RB.velocity = dir.normalized * data.dashSpeed;

        SetGravityScale(0);
    }
    public void CheckDirectionToFace(bool isMovingRight)
    {
        if (isMovingRight != data.IsFacingRight)
            Turn();
    }
}
