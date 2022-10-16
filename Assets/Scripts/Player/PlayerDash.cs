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

    private bool canDash {
        get => PlayerBehaviour.Player.canDash;
        set => PlayerBehaviour.Player.canDash = value;
    }

    private bool canMove {
        get => PlayerBehaviour.Player.canMove;
        set => PlayerBehaviour.Player.canMove = value;
    }

    public void ExecuteDash() {
        print("Executed dash");
        if(canDash) {
            canDash = false;
            canMove = false;
            rb.velocity = Vector2.zero;
            //rb.velocity += new Vector2(lastDir.x * dashForce, rb.velocity.y);
            rb.AddForce(dashForce * Vector2.right * Time.fixedDeltaTime * lastDir.x, ForceMode2D.Impulse);
            Coroutines.DoAfter(() => canMove = true, 0.3f, this);
            Coroutines.DoAfter(() => canDash = true, dashCooldown, this);
        }
    }
}
