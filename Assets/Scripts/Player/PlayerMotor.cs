using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMotor : MonoBehaviour
{
    Rigidbody2D rb;

    float m_MoveDirection;
    float m_CurrentDirection;
    float m_CurrentSpeed;

    public bool grounded;

    public bool Grounded
    {
        set { grounded = value; }
    }

    public float MoveDirection { get { return m_MoveDirection; } }
    public float CurrentDirection { get { return m_CurrentDirection; } }
    public float Direction { get { return m_MoveDirection; } }

    public float CurrentSpeed { get { return m_CurrentSpeed; } set { m_CurrentSpeed = value; } }
    public float JumpPower { get { return m_JumpPower; } }
    public float BaseSpeed { get { return m_BaseSpeed; } }

    public bool DirectionLocked { get { return m_DirLock; } set { m_DirLock = value;  } }

    [SerializeField] float m_JumpPower;
    [SerializeField] float m_BaseSpeed = 5f;

    private bool m_DirLock = false;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        PlayerManager.Instance.Input.Player.Jump.performed += Jump_performed;
    }

    private void Jump_performed(InputAction.CallbackContext obj)
    {
        Jump();
    }

    private void Update()
    {
        GetMove();
    }

    private void FixedUpdate()
    {
        Move();
    }

    // Method sets the current direction based on the movement keys pressed.
    private void GetMove()
    {
        m_MoveDirection = PlayerManager.Instance.Input.Player.Move.ReadValue<float>();
        if (m_MoveDirection != 0 && !DirectionLocked)
            m_CurrentDirection = m_MoveDirection;
    }

    // Method that sets the velocity of the player if an input key is pressed.
    private void Move()
    {
        rb.velocity = new Vector2(m_MoveDirection * m_CurrentSpeed, rb.velocity.y);
    }

    // Method that performs a jump by manipulating the players velocity.
    private void Jump()
    {
        if (grounded)
            rb.velocity = new Vector2(rb.velocity.x, m_JumpPower);
    }
}
