using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;
using NaughtyAttributes;

public class PlayerAttack : MonoBehaviour {
    private bool attackIsToucingEnemy;
    [ReadOnly] public Collider2D[] touchingAttack;

    private Transform attackPoint => PlayerBehaviour.Player.attackPoint;
    private float attackRange => PlayerBehaviour.Player.attackRange;
    private LayerMask enemyLayer => PlayerBehaviour.Player.enemyLayer;
    private float attackCooldown => PlayerBehaviour.Player.attackCooldown;

    private bool canAttack {
        get => PlayerBehaviour.Player.canAttack;
        set => PlayerBehaviour.Player.canAttack = value;
    }

    private void FixedUpdate() {
        //CheckAttackRange();
    }


    private Collider2D[] CheckAttackRange() {
        touchingAttack = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);
        return touchingAttack;
    }
    public void EnableCanAttack() {
        canAttack = true;
    }

    public void ExecuteAttack() {
        if (canAttack) {
            foreach (Collider2D enemy in CheckAttackRange()) {
                enemy.gameObject.GetComponent<LifeController>().TakeDamage(1);
            }
            canAttack = false;
            Coroutines.DoAfter(EnableCanAttack, attackCooldown, this);
        }
    }
}
