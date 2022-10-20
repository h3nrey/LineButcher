using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Utils;

[RequireComponent(typeof(LifeController))]
public class EnemyBehaviour : MonoBehaviour
{
    public UnityEvent onCollisionWithEnd;
    public Enemy _enemy;
    private int ActualEnemyLife {
        get => GetComponent<LifeController>().life;
        set => GetComponent<LifeController>().actualLife = 10;
    }

    public SpriteRenderer spriteRenderer;
    public Animator anim;
    [SerializeField] float speed;
    [SerializeField] public Vector2 direction;
    [SerializeField] public Vector2 knockbackDirection;
    private Rigidbody2D rb;
    private bool canMove;
    [SerializeField] float knockbackForce;

    private void Start() {
        GetData();
        rb = GetComponent<Rigidbody2D>();
        if (direction.x == -1)
            transform.localScale = new Vector3(1, 1, 1);
        canMove = true;

    }

    private void FixedUpdate() {
        if (canMove)
            rb.velocity = speed * Time.fixedDeltaTime * direction;

        knockbackDirection = PlayerBehaviour.Player.lastDir;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("End")) {
            if (GameManager.Game) GameManager.Game.EndGame();
        }

        if(other.gameObject.CompareTag("projectille")) {
            GetComponent<LifeController>().TakeDamage(PlayerBehaviour.Player.currentAttackMode.damage);
            Destroy(other.gameObject);
        }
    }

    private void GetData() {
        if (_enemy) {
            print($"name: {_enemy.name}, life: {_enemy.life}, speed: {_enemy.speed}");
            spriteRenderer.sprite = _enemy.spr;
            speed = _enemy.speed;         
            knockbackForce = _enemy.knockbackForce;
        }
    }

    public void KnockBack() {
        canMove = false;
        rb.velocity = Vector2.zero;
        print(-direction * knockbackForce);
        if(transform.position.x < 10) {
            rb.velocity = knockbackDirection * knockbackForce;
        }
        Coroutines.DoAfter(() => canMove = true, 0.2f, this);
    }

    public void PlayHurtAnimation() {
        anim.SetTrigger("Hurt");
    }

    public void RechargeBlood() {
        if(PlayerBehaviour.Player.currentBlood < PlayerBehaviour.Player.maxBlood) {
            PlayerBehaviour.Player.currentBlood++;
            GameManager.Game.UI.ChangeBloodBarFillAmount(1.0f);
        }
    }
}
