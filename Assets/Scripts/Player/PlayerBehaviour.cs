using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using Utils;
using NaughtyAttributes;

public class PlayerBehaviour : MonoBehaviour
{
    public static PlayerBehaviour Player;

    public int playerLife;

    [Header("States")]
    public bool grounded;
    public bool canAttack;
    public bool canDash;
    public bool canMove;
    [ReadOnly] public bool canJump;
    [ReadOnly] public bool canTeleport;
    [ReadOnly] public bool onLastFloor;
    [ReadOnly] public bool hasClone;

    [Header("Input Events")]
    public UnityEvent OnAttack;
    public UnityEvent OnDash;
    public UnityEvent OnUp;
    public UnityEvent OnDown;
    public static event OnLaunch onLaunch;
    public UnityEvent OnChange;
    public UnityEvent OnReleaseAbility;
    public bool holdingSpecialButton;

    public delegate void OnLaunch(InputAction.CallbackContext context);

    [Header("Vertical Movement")]
    [SerializeField] public float verticalSpacing;
    [SerializeField] public float[] verticalBounderies = new float[2];
    [SerializeField] Transform groundChecker;
    [SerializeField] float groundCheckerRange;
    [SerializeField] LayerMask groundLayer;

    [Header("Movement")]
    public float moveInput;
    public Vector2 lastDir;
    public Vector2 pos;
    public Vector2 rbVel;
    public float speed;
    [SerializeField] float maxPosX;
    public  Transform[] tpPoints;
    public List<GameObject> activePlayers;

    [Header("Attack")]
    public int attackDamage;
    [SerializeField] public float attackRange;
    public float currentAttackRange;
    [SerializeField] public LayerMask enemyLayer;
    [SerializeField] public Transform attackPoint;
    public Transform currentAttackPoint;
    public Transform cloneAttackPoint;
    public float cloneAttackRange;
    [SerializeField] public float attackCooldown;

    [Header("Ability")]
    public Attack currentAttackMode; // todo: change to currentAbilityMode
    public Attack[] allAtacks; // todo; change to allAbilities
    public int currentAbilityIndex;


    [Header("Blood Manager")]
    [SerializeField] public float timeToAttack;
    [SerializeField] public int maxBlood;
    [SerializeField] public int currentBlood;

    [Header("Explosion")]
    [SerializeField] public int explosionDamage;
    [SerializeField] public GameObject bombPrefab;

    [Header("Dash")]
    public float dashForce;
    public float dashCooldown;

    [Header("Collision")]
    [SerializeField] Collider2D[] floors;
    [SerializeField] float enableColTime;

    [Header("Components")]
    [SerializeField] public Animator anim;
    public Rigidbody2D rb;
    private void Awake() {
        if(!Player) {
            Player = this;
        }
    }

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        canAttack = true;
        grounded = true;
        canDash = true;
        canMove = true;
        playerLife = GetComponent<LifeController>().life;
        currentBlood = maxBlood;
        canJump = true;
        canTeleport = true;

        //events
        OnUp.AddListener(() => {
            ChangeCollisionWithFloor(true);
            Coroutines.DoAfter(() => ChangeCollisionWithFloor(false), enableColTime, this);
        });
        OnDown.AddListener(() => {
            ChangeCollisionWithFloor(true);
            Coroutines.DoAfter(() => ChangeCollisionWithFloor(false), enableColTime, this);
        });
    }

    private void OnDestroy() {
        OnUp.RemoveListener(() => {
            ChangeCollisionWithFloor(true);
            Coroutines.DoAfter(() => ChangeCollisionWithFloor(false), enableColTime, this);
        });
        OnDown.RemoveListener(() => {
            ChangeCollisionWithFloor(true);
            Coroutines.DoAfter(() => ChangeCollisionWithFloor(false), enableColTime, this);
        });
    }

    private void FixedUpdate() {
        rbVel = rb.velocity;
        CheckIfTouchGround();
    }

    private void Update() {
        playerLife = GetComponent<LifeController>().actualLife;
        pos = transform.position;

        if(Mathf.Abs(moveInput) > 0) {
            lastDir.x = moveInput;
        }

        SettingFacing();
    }

    private void SettingFacing() {
        if (moveInput > 0) transform.localScale = new Vector3(1, 1, 1);
        else if (moveInput < 0) transform.localScale = new Vector3(-1, 1, 1);
    }

    private void CheckIfTouchGround() {
        grounded = Physics2D.OverlapCircle(groundChecker.position, groundCheckerRange, groundLayer);
    }
    void ChangeCollisionWithFloor(bool ignore) {
        if (onLastFloor && hasClone) return;
        foreach (Collider2D floor in floors) {
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), floor, ignore);
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.CompareTag("Enemy")) {
            GetComponent<LifeController>().TakeDamage(1);
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(currentAttackPoint.position, currentAttackRange);
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(groundChecker.position, groundCheckerRange);
    }
}
