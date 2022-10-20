using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public class SnakeBehaviour : MonoBehaviour
{
    [SerializeField] Enemy data;

    [SerializeField] float speed;
    [SerializeField] public Vector2 direction;
    [SerializeField] public Vector2 knockbackDirection;
    private Rigidbody2D rb;
    private bool canMove;
    [SerializeField] float knockbackForce;

    private void Start() {
        speed = data.speed;
        rb = GetComponent<Rigidbody2D>();
        if(direction.x == -1)
        transform.localScale = new Vector3(1, 1, 1);
        canMove = true;
    }

    private void FixedUpdate() {
        if(canMove)
        rb.velocity =  speed * Time.fixedDeltaTime * direction;

        knockbackDirection = PlayerBehaviour.Player.lastDir;
    }

    public void KnockBack() {
        canMove = false;
        print("knockback");
        rb.velocity = Vector2.zero;
        print(-direction * knockbackForce);
        rb.velocity = knockbackDirection * knockbackForce;
        Coroutines.DoAfter(() => canMove = true, 0.2f, this);
    }

    public void PlayHurtAnimation() {
        GetComponent<Animator>().SetTrigger("Hurt");
    }
}
