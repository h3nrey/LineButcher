using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using Utils;

public class PlayerBehaviour : MonoBehaviour
{
    public static PlayerBehaviour Player;

    public int playerLife;

    [Header("States")]
    public bool grounded;
    public bool canAttack;
    public bool canDash;
    public bool canMove;

    [Header("Input Events")]
    public UnityEvent OnAttack;
    public UnityEvent OnDash;


    [Header("Vertical Movement")]
    [SerializeField] float verticalSpacing;
    [SerializeField] float[] verticalBounderies = new float[2];
    [SerializeField] Transform groundChecker;
    [SerializeField] float groundCheckerRange;
    [SerializeField] LayerMask groundLayer;

    [Header("Movement")]
    public float moveInput;
    public Vector2 lastDir;
    private Vector2 pos;
    public Vector2 rbVel;
    [SerializeField] float speed;
    [SerializeField] float maxPosX;
    [SerializeField] Transform[] tpPoints;
    public List<GameObject> activePlayers;

    [Header("Attack")]
    [SerializeField] public float attackRange;
    [SerializeField] public LayerMask enemyLayer;
    [SerializeField] public Transform attackPoint;
    [SerializeField] public float attackCooldown;

    [Header("Dash")]
    public float dashForce;
    public float dashCooldown;

    [Header("Collision")]
    [SerializeField] Collider2D[] floors;

    [Header("Components")]
    [SerializeField] public Animator anim;
    public Rigidbody2D rb;
    private void Awake() {
        Player = this;
    }

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        canAttack = true;
        grounded = true;
        canDash = true;
        canMove = true;
        playerLife = GetComponent<LifeController>().life;
    }

    private void FixedUpdate() {
        rbVel = rb.velocity;

        if(canMove) {
         rb.velocity = new Vector2(moveInput * Time.fixedDeltaTime * speed, rb.velocity.y);
        }
        CheckIfTouchGround();
    }

    private void Update() {
        playerLife = GetComponent<LifeController>().actualLife;
        if (Mathf.Abs(transform.position.x) > maxPosX) Teleport();
        pos = transform.position;
        if(Mathf.Abs(moveInput) > 0) {
            lastDir.x = moveInput;
        }


        if (Mathf.Abs(pos.y) > verticalBounderies[0]) {
            if (pos.y > 0) transform.position = new Vector2(pos.x, tpPoints[2].position.y);
            else transform.position = new Vector2(pos.x, tpPoints[3].position.y);
        }

        SettingFacing();
    }

    private void SettingFacing() {
        if (moveInput > 0) transform.localScale = new Vector3(1, 1, 1);
        else if (moveInput < 0) transform.localScale = new Vector3(-1, 1, 1);
    }


    private void MoveVertical(int movementSense) {
        Vector2 upPos = new Vector2(rb.position.x, Mathf.FloorToInt(rb.position.y) + (verticalSpacing * movementSense));
        rb.MovePosition(upPos);
    }

    #region Inputs

    public void ArrowUp(InputAction.CallbackContext context) {
        if (context.started) {
            MoveVertical(1);
        }
    }

    public void ArrowDown(InputAction.CallbackContext context) {
        if (context.started) {
            MoveVertical(-1);
            ChangeCollisionWithFloor(true);
            Coroutines.DoAfter(() => ChangeCollisionWithFloor(false), 0.05f, this);
        }
    }

    private void CheckIfTouchGround() {
        grounded = Physics2D.OverlapCircle(groundChecker.position, groundCheckerRange, groundLayer);
    }

    //private void OnCollisionExit2D(Collision2D other) {
    //    if(other.gameObject.layer == floors[0].gameObject.layer) {
    //        ChangeCollisionWithFloor(false);
    //    }
    //}

    void ChangeCollisionWithFloor(bool ignore) {
        foreach (Collider2D floor in floors) {
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), floor, ignore);
        }
    }

    public void GetMoveInput(InputAction.CallbackContext context) {
        moveInput = context.ReadValue<float>();
    }

    #endregion

    private void Teleport() {
        if (transform.position.x > 0) transform.position = new Vector2(tpPoints[0].position.x, transform.position.y);
        else transform.position = transform.position = new Vector2(tpPoints[1].position.x, transform.position.y);
    }

    public void Attack(InputAction.CallbackContext context) {
        if(context.started) {
            OnAttack?.Invoke();
        }
    }

    public void Dash(InputAction.CallbackContext context) {
        if (context.started) {
            OnDash?.Invoke();
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.CompareTag("Enemy")) {
            GetComponent<LifeController>().TakeDamage(1);
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(groundChecker.position, groundCheckerRange);
    }
}