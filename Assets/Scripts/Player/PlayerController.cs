using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerData defaultData;
    private PlayerData data;

    [Header("Audio Testing")]
    public AudioSource audioSource;
    [SerializeField] AudioClip chopAudio;

    #region STATE MACHINE
    [ReadOnly] public string CurrentState;
    #endregion

    #region COMPONENTS
    public Rigidbody2D RB { get; private set; }
    public Animator PlayerAnimator { get; private set; }
    public Animator AxeAnimator { get; private set; }
    public TrailRenderer TrailRenderer { get; private set; }
    [SerializeField] private PlayerInventoryManager inventory;
    private PlayerEvents events;
    #endregion

    #region CHECK PARAMETERS
    public Transform GroundCheckPoint { get { return _groundCheckPoint; } }
    public Vector2 GroundCheckSize { get { return _groundCheckSize; } }
    public Transform RightWallCheckPoint { get { return _rightWallCheckPoint; } }
    public Transform LeftWallCheckPoint { get { return _leftWallCheckPoint; } } 
    public Vector2 WallCheckSize { get { return _wallCheckSize; } }

    [Header("Checks")]
    [SerializeField] private Transform _groundCheckPoint;
    [SerializeField] private Vector2 _groundCheckSize;
    [Space(5)]
    [SerializeField] private Transform _rightWallCheckPoint;
    [SerializeField] private Transform _leftWallCheckPoint;
    [SerializeField] private Vector2 _wallCheckSize;
    #endregion

    #region LAYERS AND TAGS
    public LayerMask GroundLayer { get { return _groundLayer; } }
    public LayerMask WallLayer { get { return _wallLayer; } }
    public LayerMask TreeLayer { get { return _treeLayer; } }
    [Header("Layers & Tags")]
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private LayerMask _wallLayer;
    [SerializeField] private LayerMask _treeLayer;
    #endregion

    private void Awake()
    {
        RB = GetComponent<Rigidbody2D>();
        PlayerAnimator = GetComponent<Animator>();
        AxeAnimator = transform.Find("Axe").GetComponent<Animator>();
        TrailRenderer = GetComponent<TrailRenderer>();
        events = GetComponent<PlayerEvents>();

        if (data == null)
        {
            data = Instantiate(defaultData);
            data.Initialize(this, inventory, events);
            events.Initialize(data);
        }
    }

    private void Start()
    {
        data.Motor.SetGravityScale(data.gravityScale);
    }

    private void Update()
    {
        data.States.LogicUpdate();
        data.States.StateMachine.CurrentState.LogicUpdate();
    }

    private void FixedUpdate()
    {
        data.States.StateMachine.CurrentState.PhysicsUpdate();
    }


    #region INTERACTION METHODS
    public void SpawnFloorItem(Item item, GameObject floorItem, int quantity)
    {
        GameObject lootDrop = Instantiate(floorItem, transform.position, Quaternion.identity);
        lootDrop.GetComponent<FloorItem>().SetInfo(Instantiate(item), quantity);
    }
    public void DetectHit(int damage)
    {
        Vector2 newPos = transform.position;
        newPos.y -= 0.17f;

        Vector2 dir = new Vector2((data.IsFacingRight) ? 1 : -1, -0.25f);

        Debug.DrawRay(newPos, dir, Color.red, 1.15f);

        RaycastHit2D[] hits = Physics2D.RaycastAll(newPos, dir, 0.65f);

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider.CompareTag("Damageable"))
            {
                Camera.main.GetComponent<CameraController>().Shake(0.05f);
                audioSource.Play();
                hit.collider.gameObject.GetComponent<IDamageable>().Damage(damage);
            }
        }
    }
    public void Interact()
    {
        LayerMask mask = LayerMask.GetMask("Interactables");

        Vector2 dir = Vector2.zero;
        dir.x = (data.IsFacingRight) ? 1 : -1;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, 1.3f, mask);

        if (hit.collider.gameObject.GetComponent<IInteractable>() != null)
        {
            hit.collider.gameObject.GetComponent<IInteractable>().Interact(gameObject);
        }
    }
    #endregion

    #region ANIMATION METHODS
    public void PlayAnimation()
    {
        switch (CurrentState)
        {
            case "PlayerRunState":
                PlayerAnimator.Play("playerSprite_Run");
                AxeAnimator.Play("axeSprite_Run");
                TrailRenderer.enabled = false;
                break;
            case "PlayerIdleState":
                PlayerAnimator.Play("playerSprite_Idle");
                AxeAnimator.Play("axeSprite_Idle");
                TrailRenderer.enabled = false;
                break;
            case "PlayerJumpState":
                PlayerAnimator.Play("playerSprite_Jump");
                AxeAnimator.Play("axeSprite_Jump");
                TrailRenderer.enabled = false;
                break;
            case "PlayerInAirState":
                PlayerAnimator.Play("playerSprite_InAir");
                AxeAnimator.Play("axeSprite_InAir");
                TrailRenderer.enabled = false;
                break;
            case "PlayerWallJumpState":
                PlayerAnimator.Play("playerSprite_Jump");
                AxeAnimator.Play("axeSprite_Jump");
                TrailRenderer.enabled = false;
                break;
            case "PlayerDashState":
                PlayerAnimator.Play("playerSprite_InAir");
                AxeAnimator.Play("axeSprite_InAir");
                TrailRenderer.enabled = true;
                break;
            case "PlayerWallSlideState":
                PlayerAnimator.Play("playerSprite_OnWall");
                AxeAnimator.Play("axeSprite_OnWall");
                TrailRenderer.enabled = false;
                break;
            case "PlayerAxeSwingState":
                PlayerAnimator.Play("playerSprite_AxeSwing");
                AxeAnimator.Play("axeSprite_AxeSwing");
                TrailRenderer.enabled = false;
                audioSource.clip = chopAudio;
                break;
            default:
                PlayerAnimator.Play("playerSprite_Idle");
                AxeAnimator.Play("axeSprite_Idle");
                TrailRenderer.enabled = false;
                break;
        }
    }
    #endregion
}
