using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

[RequireComponent(typeof(Rigidbody2D))]
public class Knockback : MonoBehaviour { 

    public void ExecuteKnockback(Vector2 direction, float knockbackForce, bool canMove = true) {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        canMove = false;
        rb.velocity = Vector2.zero;
        rb.velocity = direction * knockbackForce;
        Coroutines.DoAfter(() => canMove = true, 0.2f, this);
    }
}
