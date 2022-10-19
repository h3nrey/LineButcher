using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;
using NaughtyAttributes;

public class PlayerAttack : MonoBehaviour {
    private bool attackIsToucingEnemy;
    [ReadOnly] public Collider2D[] touchingAttack;


    #region Player Data
        private Transform attackPoint => PlayerBehaviour.Player.attackPoint;
        private float attackRange => PlayerBehaviour.Player.attackRange;
        private LayerMask enemyLayer => PlayerBehaviour.Player.enemyLayer;
        private float attackCooldown => PlayerBehaviour.Player.attackCooldown;
        private Attack[] allAttacks => PlayerBehaviour.Player.allAtacks;
        private int currentBlood => PlayerBehaviour.Player.currentBlood;

        //bomb 
        private GameObject bomb => PlayerBehaviour.Player.bombPrefab;

        private bool canAttack {
            get => PlayerBehaviour.Player.canAttack;
            set => PlayerBehaviour.Player.canAttack = value;
        }
        private float currentRangeAttack {
            get => PlayerBehaviour.Player.currentAttackRange;
            set => PlayerBehaviour.Player.currentAttackRange = value;
        }

    private Attack currentAttackMode {
            get => PlayerBehaviour.Player.currentAttackMode;
            set => PlayerBehaviour.Player.currentAttackMode = value;
        }
    #endregion


    private void Awake() {
        currentAttackMode = allAttacks[0];
        PlayerBehaviour.onLaunch += (context) => LaunchProjectille();
    }

    private void Start() {
        currentRangeAttack = 0;
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
            Coroutines.DoAfter(() => {
                currentRangeAttack = attackRange;
                foreach (Collider2D enemy in CheckAttackRange()) {
                    enemy.gameObject.GetComponent<LifeController>().TakeDamage(1);
                }
                Coroutines.DoAfter(() => currentRangeAttack = 0, 0.15f, this);
            }, 0.1f, this);
            
            canAttack = false;
            Coroutines.DoAfter(EnableCanAttack, attackCooldown, this);
        }
    }

    public void LaunchProjectille() {
        print("launch projectille");
        if(currentBlood >= currentAttackMode.bloodCost) {
            Instantiate(currentAttackMode.projectillePrefab, attackPoint.position, Quaternion.Euler(transform.right));
            PlayerBehaviour.Player.currentBlood -= currentAttackMode.bloodCost;
            GameManager.Game.UI.ChangeBloodBarFillAmount(-(float)currentAttackMode.damage);
        }
        //projectille.GetComponent<Rigidbody2D>().velocity += PlayerBehaviour.Player.lastDir * 200f * Time.deltaTime;
        return;
    }
}
