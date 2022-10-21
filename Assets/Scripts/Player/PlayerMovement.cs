using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public class PlayerMovement : MonoBehaviour
{
    private Vector2 pos => PlayerBehaviour.Player.pos;
    private bool canMove => PlayerBehaviour.Player.canMove;
    private bool grounded => PlayerBehaviour.Player.grounded;
    private float moveInput => PlayerBehaviour.Player.moveInput;
    private float speed => PlayerBehaviour.Player.speed;
    private Rigidbody2D rb => PlayerBehaviour.Player.rb;
    private float verticalSpacing => PlayerBehaviour.Player.verticalSpacing;
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
        PlayerBehaviour.Player.onLastFloor = pos.y < -5 ? true : false;
    }

    private void FixedUpdate() {
        if (canMove) {
            rb.velocity = new Vector2(moveInput * Time.fixedDeltaTime * speed, rb.velocity.y);
        }
    }

    

    public void MoveVertical(int movementSense) {
        if (!PlayerBehaviour.Player.canTeleport && pos.y >= 0.6 && movementSense > 0) return;
        if (grounded && canJump) {
            canJump = false;
            rb.AddForce(Vector2.up * movementSense * verticalSpacing, ForceMode2D.Impulse);
            Coroutines.DoAfter(() => canJump = true, 0.2f, this);
        }
    }
}
