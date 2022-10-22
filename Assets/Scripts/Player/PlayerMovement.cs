using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public class PlayerMovement : MonoBehaviour
{
    private Vector2 pos {
        get => PlayerBehaviour.Player.pos;
        set => PlayerBehaviour.Player.pos = value;
    }
    private Vector2 lastDir {
        get => PlayerBehaviour.Player.lastDir;
        set => PlayerBehaviour.Player.lastDir = value;
    }
    private bool canMove => PlayerBehaviour.Player.canMove;
    private bool grounded => PlayerBehaviour.Player.grounded;
    private float moveInput => PlayerBehaviour.Player.moveInput;
    private float speed => PlayerBehaviour.Player.speed;
    private Rigidbody2D rb => PlayerBehaviour.Player.rb;
    private float verticalSpacing => PlayerBehaviour.Player.verticalSpacing;

    private Vector2 rbVel { 
        get => PlayerBehaviour.Player.rbVel;
        set => PlayerBehaviour.Player.rbVel = value;
    }
    private bool canJump {
        get => PlayerBehaviour.Player.canJump;
        set => PlayerBehaviour.Player.canJump = value;
    }

    private void Start() {
        PlayerBehaviour.Player.OnUp.AddListener(() => MoveVertical(1));
        PlayerBehaviour.Player.OnDown.AddListener(() => MoveVertical(-1));
    }

    private void OnDestroy() {
        PlayerBehaviour.Player.OnUp.RemoveListener(() => MoveVertical(1));
        PlayerBehaviour.Player.OnDown.RemoveListener(() => MoveVertical(-1));
    }

    private void Update() {
        pos = transform.position;
        SettingFacing();

        if (Mathf.Abs(moveInput) > 0) {
            lastDir = new Vector2(moveInput, 0);
        }
        PlayerBehaviour.Player.onLastFloor = pos.y < -5 ? true : false;
    }

    private void FixedUpdate() {
        rbVel = rb.velocity;
        Move();
    }

    private void SettingFacing() {
        if (moveInput > 0) transform.localScale = new Vector3(1, 1, 1);
        else if (moveInput < 0) transform.localScale = new Vector3(-1, 1, 1);
    }

    private void Move() {
        if (canMove) {
            rb.velocity = new Vector2(moveInput * Time.fixedDeltaTime * speed, rb.velocity.y);
        }
    }

    private void MoveVertical(int movementSense) {
        if (!PlayerBehaviour.Player.canTeleport && pos.y >= 0.6 && movementSense > 0) return;
        if (grounded && canJump) {
            canJump = false;
            rb.AddForce(Vector2.up * movementSense * verticalSpacing, ForceMode2D.Impulse);
            Coroutines.DoAfter(() => canJump = true, 0.2f, this);
        }
    }
}
