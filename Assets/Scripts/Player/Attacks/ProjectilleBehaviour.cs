using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilleBehaviour : MonoBehaviour
{
    Rigidbody2D rb;
    Vector2 dir;

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        dir = PlayerBehaviour.Player.lastDir;


    }

    private void FixedUpdate() {
        rb.velocity = dir * 400f * Time.fixedDeltaTime;
    }

}
