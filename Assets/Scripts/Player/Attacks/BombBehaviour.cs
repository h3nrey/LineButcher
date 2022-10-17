using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombBehaviour : MonoBehaviour {
    private Transform spriteChild;
    private SpriteRenderer sprite;
    [SerializeField] int damage;
    [SerializeField] float bombSpeed;
    [SerializeField] float timeBeforeExplosion;
    [SerializeField] float explosionDuration;
    [SerializeField] public Vector2 direction;
    public float launchForce;

    [SerializeField] Vector2 explosionRange;
    [SerializeField] LayerMask whatsEnemy;


    bool onBeforeExplosion;
    bool onExplosion;

    [Header("Rotation")]
    [SerializeField] float rotationRate;

    [Header("Pulse")]
    [SerializeField] float pulseRate;
    [SerializeField] float pulseAmplitude;

    private void Start() {
        damage = PlayerBehaviour.Player.explosionDamage;
        spriteChild = transform.GetChild(0);
        sprite = spriteChild.GetComponent<SpriteRenderer>();
        StartCoroutine(Explode());

        onBeforeExplosion = true;
    }

    private void Update() {
        if(onBeforeExplosion) {
            spriteChild.rotation = Quaternion.Euler(new Vector3(0, 0, spriteChild.eulerAngles.z - rotationRate * Time.deltaTime));
            spriteChild.localScale = new Vector3(Mathf.Sin(Time.time * pulseRate) * pulseAmplitude, Mathf.Sin(Time.time * pulseRate) * pulseAmplitude, 1);
        }
        if(!onBeforeExplosion) {
            spriteChild.rotation = Quaternion.identity;
            if(spriteChild.localScale.x < new Vector2(10,10).x) {
                spriteChild.localScale = Vector3.MoveTowards(spriteChild.localScale, new Vector3(8f,8f,1f), 10 * Time.deltaTime);
            }
        }
    }

    IEnumerator Explode() {
        yield return new WaitForSeconds(timeBeforeExplosion);
        onBeforeExplosion = false;
        Collider2D[] enemiesOnRange = Physics2D.OverlapBoxAll(transform.position, explosionRange, 0f, whatsEnemy);
        foreach (Collider2D enemy in enemiesOnRange) {
            if(enemy.GetComponent<LifeController>()) {
                enemy.GetComponent<LifeController>().TakeDamage(damage);
            }
        }
        yield return new WaitForSeconds(timeBeforeExplosion + explosionDuration);
        Destroy(this.gameObject);
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(transform.position, explosionRange);
    }
}
