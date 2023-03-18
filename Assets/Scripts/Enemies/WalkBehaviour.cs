using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyBehaviour))]
public class WalkBehaviour : MonoBehaviour
{
    [SerializeField] EnemyBehaviour enemy;
    float enemySpeed => enemy.speed;
    Rigidbody2D rb;

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
    }
    private void FixedUpdate() {
        if (GetComponent<EnemyBehaviour>().canMove)
            rb.velocity = enemySpeed * Time.fixedDeltaTime * Vector2.left;
    }
}
