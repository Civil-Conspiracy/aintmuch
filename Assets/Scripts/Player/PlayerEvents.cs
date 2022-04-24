using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEvents : MonoBehaviour
{
    protected PlayerData data;
    public void Initialize(PlayerData _data)
    {
        data = _data;
    }

    private void Start()
    {
        //Gameplay
        InputManager.instance.OnJumpPressed += args => OnJump(args);
        InputManager.instance.OnJumpReleased += args => OnJumpReleased(args);
        InputManager.instance.OnDash += args => OnDash(args);
        InputManager.instance.OnAxeSwing += args => OnAxeSwing(args);
        InputManager.instance.OnInteract += args => OnInteract(args);

        //HUD
        InputManager.instance.OnScrollForward += args => OnScrollForward(args);
        InputManager.instance.OnScrollBackward += args => OnScrollBackward(args);

        //Debug
        InputManager.instance.OnDebugB += args => OnDebugB(args);
        InputManager.instance.OnDebugC += args => OnDebugC(args);
    }
    #region GAMEPLAY
    public void OnJump(InputManager.InputArgs args)
    {
        data.LastPressedJumpTime = data.jumpBufferTime;
    }

    public void OnJumpReleased(InputManager.InputArgs args)
    {
        if (data.States.JumpState.CanJumpCut() || data.States.WallJumpState.CanJumpCut())
            data.Motor.JumpCut();
    }

    public void OnDash(InputManager.InputArgs args)
    {
        data.LastPressedDashTime = data.dashBufferTime;
    }

    public void OnAxeSwing(InputManager.InputArgs args)
    {
        data.IsAxeSwingPressed = args.context.ReadValueAsButton();
    }

    private void OnInteract(InputManager.InputArgs args)
    {
        Debug.Log("wow");
        data.Player.Interact();
    }
    #endregion

    #region HUD
    public void OnScrollForward(InputManager.InputArgs args)
    {
            bool scrolled = PlayerInventoryManager.instance.ScrollForward();
        if (scrolled)
            Debug.Log("Successfully Scrolled Forward!");
        else
            Debug.Log("No Items to Scroll!");
    }
    public void OnScrollBackward(InputManager.InputArgs args)
    {
        bool scrolled = PlayerInventoryManager.instance.ScrollBackward();
        if (scrolled)
            Debug.Log("Successfully Scrolled Backward!");
        else
            Debug.Log("No Items to Scroll!");
    }
    #endregion

    #region DEBUG
    public void OnDebugB(InputManager.InputArgs args)
    {
        data.Player.SpawnFloorItem(data.Debug.DEBUGITEM, data.Debug.go_defaultItem, 1);
    }
    public void OnDebugC(InputManager.InputArgs args)
    {
        bool dropItem = PlayerInventoryManager.instance.DropItemFromOffhand(data.IsFacingRight, data.Player.transform.position);
        if (dropItem)
            Debug.Log("Dropped Item from Offhand");
        else
            Debug.Log("No Items to Drop!");
    }
    #endregion
}
