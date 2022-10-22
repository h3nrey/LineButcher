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

    #region Variables
    public int playerLife;

    [Header("States")]
    [Foldout("STATES")][ReadOnly] public bool grounded;
    [Foldout("STATES")][ReadOnly] public bool canAttack;
    [Foldout("STATES")][ReadOnly] public bool canDash;
    [Foldout("STATES")][ReadOnly] public bool canMove;
    [Foldout("STATES")][ReadOnly] public bool canJump;
    [Foldout("STATES")][ReadOnly] public bool canTeleport;
    [Foldout("STATES")][ReadOnly] public bool onLastFloor;
    [Foldout("STATES")][ReadOnly] public bool hasClone;

    [Header("Input Events")]
    [HideInInspector] public UnityEvent OnAttack;
    [HideInInspector] public UnityEvent OnDash;
    [HideInInspector] public UnityEvent OnUp;
    [HideInInspector] public UnityEvent OnDown;
    public static event OnLaunch onLaunch;
    [HideInInspector] public UnityEvent OnChange;
    [HideInInspector] public UnityEvent OnReleaseAbility;
    public bool holdingSpecialButton;

    public delegate void OnLaunch(InputAction.CallbackContext context);

    [Foldout("Vertical Movement")]
    public float verticalSpacing;
    [Foldout("Vertical Movement")]
    public float[] verticalBounderies = new float[2];
    [Foldout("Vertical Movement")]
    public Transform groundChecker;
    [Foldout("Vertical Movement")]
    public float groundCheckerRange;
    [Foldout("Vertical Movement")]
    public LayerMask groundLayer;

    [Header("Movement")]
    [Foldout("Movement")] public float moveInput;
    [Foldout("Movement")] public Vector2 lastDir;
    [Foldout("Movement")] public Vector2 pos;
    [Foldout("Movement")] public Vector2 rbVel;
    [Foldout("Movement")] public float speed;
    [Foldout("Movement")][SerializeField] float maxPosX;
    [Foldout("Movement")] public Transform[] tpPoints;
    [Foldout("Movement")] public List<GameObject> activePlayers;

    [Header("Attack")]
    [Foldout("ATTACK")] public int attackDamage;
    [Foldout("ATTACK")] public float attackRange;
    [Foldout("ATTACK")] public float currentAttackRange;
    [Foldout("ATTACK")] public LayerMask enemyLayer;
    [Foldout("ATTACK")] public Transform attackPoint;
    [Foldout("ATTACK")] public Transform currentAttackPoint;
    [Foldout("ATTACK")] public Transform cloneAttackPoint;
    [Foldout("ATTACK")] public float cloneAttackRange;
    [Foldout("ATTACK")] public float attackCooldown;

    [Header("Ability")]
    [Foldout("ABILITY")] public Attack currentAttackMode; // todo: change to currentAbilityMode
    [Foldout("ABILITY")] public Attack[] allAtacks; // todo; change to allAbilities
    [Foldout("ABILITY")] public int currentAbilityIndex;


    [Header("Blood Manager")]
    [Foldout("BLOOD MANAGER")] public float timeToAttack;
    [Foldout("BLOOD MANAGER")] public int maxBlood;
    [Foldout("BLOOD MANAGER")] public int currentBlood;


    [Header("Dash")]
    [Foldout("DASH")] public float dashForce;
    [Foldout("DASH")] public float dashCooldown;

    [Header("Collision")]
    [Foldout("ABILITY")] public Collider2D[] floors;
    [Foldout("ABILITY")] public float enableColTime;

    [BoxGroup("PLAYER COMPONENTS")] public Animator anim;
    [BoxGroup("PLAYER COMPONENTS")] public Rigidbody2D rb;
    #endregion

    
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
    }

    private void Update() {
        playerLife = GetComponent<LifeController>().actualLife;
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(currentAttackPoint.position, currentAttackRange);
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(groundChecker.position, groundCheckerRange);
    }
}
