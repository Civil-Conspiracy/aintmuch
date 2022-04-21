using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager instance;

    private Controls controls;

    [Header("Input Values")]
    public Action<InputArgs> OnJumpPressed;
    public Action<InputArgs> OnJumpReleased;
    public Action<InputArgs> OnDash;
    public Action<InputArgs> OnInteract;
    public Action<InputArgs> OnAxeSwing;

    public Action<InputArgs> OnDebugB;
    public Action<InputArgs> OnDebugC;

    public Vector2 MoveInput { get; private set; }

    private void Awake()
    {
        #region Singleton
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        #endregion
        controls = new Controls();

        controls.Player.Move.performed += ctx => MoveInput = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => MoveInput = Vector2.zero;

        controls.Player.Jump.performed += ctx => OnJumpPressed(new InputArgs { context = ctx });
        controls.Player.JumpRelease.performed += ctx => OnJumpReleased(new InputArgs { context = ctx });

        controls.Player.Dash.performed += ctx => OnDash(new InputArgs { context = ctx });

        controls.Player.Interact.performed += ctx => OnInteract(new InputArgs { context = ctx });

        controls.Player.AxeSwing.performed += ctx => OnAxeSwing(new InputArgs { context = ctx });
        controls.Player.AxeSwing.canceled += ctx => OnAxeSwing(new InputArgs { context = ctx });

        controls.Debug.DebugB.performed += ctx => OnDebugB(new InputArgs { context = ctx });
        controls.Debug.DebugC.performed += ctx => OnDebugC(new InputArgs { context = ctx });
    }

    // Event Args
    public class InputArgs
    {
        public InputAction.CallbackContext context;
    }

    private void OnEnable()
    {
        controls.Enable();
    }
    private void OnDisable()
    {
        controls.Disable();
    }
}
