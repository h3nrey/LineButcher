using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public class PlayerDash : MonoBehaviour
{
    private Rigidbody2D rb => PlayerBehaviour.Player.rb;
    private float moveInput => PlayerBehaviour.Player.moveInput;
    private Vector2 lastDir => PlayerBehaviour.Player.lastDir;
    private float dashForce => PlayerBehaviour.Player.dashForce;
    private float dashCooldown => PlayerBehaviour.Player.dashCooldown;
    private bool grounded => PlayerBehaviour.Player.grounded;

    private bool canDash {
        get => PlayerBehaviour.Player.canDash;
        set => PlayerBehaviour.Player.canDash = value;
    }

    private bool canMove {
        get => PlayerBehaviour.Player.canMove;
        set => PlayerBehaviour.Player.canMove = value;
    }

    private void Start() {
        PlayerBehaviour.Player.OnDash.AddListener(ExecuteDash);
    }

    private void OnDestroy() {
        PlayerBehaviour.Player.OnDash.RemoveListener(ExecuteDash);
    }

    public void ExecuteDash() {
        if(canDash && grounded) {
            canDash = false;
            canMove = false;
            rb.velocity = Vector2.zero;
            rb.AddForce(dashForce * Vector2.right * Time.fixedDeltaTime * lastDir.x, ForceMode2D.Impulse);
            Coroutines.DoAfter(() => canMove = true, 0.3f, this);
            Coroutines.DoAfter(() => canDash = true, dashCooldown, this);
        }
    }
}
