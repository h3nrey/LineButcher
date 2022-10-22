using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public class PlayerCollisionController : MonoBehaviour
{
    PlayerBehaviour _player => PlayerBehaviour.Player;
    private Transform groundChecker => PlayerBehaviour.Player.groundChecker;
    private LayerMask groundLayer => PlayerBehaviour.Player.groundLayer;
    private float groundCheckerRange => PlayerBehaviour.Player.groundCheckerRange;
    private bool onLastFloor => _player.onLastFloor;
    private bool hasClone => _player.hasClone;
    private Collider2D[] floors => _player.floors;
    private float enableColTime => _player.enableColTime;
    private bool grounded {
        get => PlayerBehaviour.Player.grounded;
        set => PlayerBehaviour.Player.grounded = value;
    }

    private void Start() {
        _player.OnUp.AddListener(() => {
            ChangeCollisionWithFloor(true);
            Coroutines.DoAfter(() => ChangeCollisionWithFloor(false), enableColTime, this);
        });
        _player.OnDown.AddListener(() => {
            ChangeCollisionWithFloor(true);
            Coroutines.DoAfter(() => ChangeCollisionWithFloor(false), enableColTime, this);
        });
    }

    private void OnDestroy() {
        _player.OnUp.RemoveListener(() => {
            ChangeCollisionWithFloor(true);
            Coroutines.DoAfter(() => ChangeCollisionWithFloor(false), enableColTime, this);
        });
        _player.OnDown.RemoveListener(() => {
            ChangeCollisionWithFloor(true);
            Coroutines.DoAfter(() => ChangeCollisionWithFloor(false), enableColTime, this);
        });
    }

    private void FixedUpdate() {
        CheckIfTouchGround();
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
        if (other.gameObject.CompareTag("Enemy")) {
            GetComponent<LifeController>().TakeDamage(1);
        }
    }
}
