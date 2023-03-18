using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public class PlayerCollisionController : MonoBehaviour
{
    PlayerBehaviour _player => PlayerBehaviour.Player;
    Rigidbody2D rb => _player.rb;
    private float knockbackForce => _player.knockbackForce;
    private Transform groundChecker => _player.groundChecker;
    private LayerMask groundLayer => _player.groundLayer;
    private float groundCheckerRange => _player.groundCheckerRange;
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
            _player.OnHurt?.Invoke();
            KnockBack(other.GetContact(0).normal);
            GetComponent<LifeController>().TakeDamage(1);
        }
    }

    public void KnockBack(Vector2 knockbackDirection) {
        _player.canMove = false;
        _player.rb.velocity = Vector2.zero;
        _player.rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
        Coroutines.DoAfter(() => _player.canMove = true, 0.2f, this);
    }
}
