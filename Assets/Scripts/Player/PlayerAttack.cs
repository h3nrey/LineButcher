using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;
using NaughtyAttributes;

public class PlayerAttack : MonoBehaviour {
    private bool attackIsToucingEnemy;
    [ReadOnly] public Collider2D[] touchingAttack;
    private Transform currentAttackPoint {
        get => PlayerBehaviour.Player.currentAttackPoint;
        set => PlayerBehaviour.Player.currentAttackPoint = value;
    }

    public List<GameObject> activeClones;
    #region Player Data
        private Transform attackPoint => PlayerBehaviour.Player.attackPoint;
        private float attackRange => PlayerBehaviour.Player.attackRange;
        private LayerMask enemyLayer => PlayerBehaviour.Player.enemyLayer;
        private float attackCooldown => PlayerBehaviour.Player.attackCooldown;
        private Attack[] allAttacks => PlayerBehaviour.Player.allAtacks;
        private int currentBlood => PlayerBehaviour.Player.currentBlood;
        private bool holdingSpecialButton => PlayerBehaviour.Player.holdingSpecialButton;
        private int attackDamage => PlayerBehaviour.Player.attackDamage;

        //bomb 
        private GameObject bomb => PlayerBehaviour.Player.bombPrefab;

        private bool canAttack {
            get => PlayerBehaviour.Player.canAttack;
            set => PlayerBehaviour.Player.canAttack = value;
        }
        private float currentAttackRange {
            get => PlayerBehaviour.Player.currentAttackRange;
            set => PlayerBehaviour.Player.currentAttackRange = value;
        }

        private Attack currentAttackMode {
                get => PlayerBehaviour.Player.currentAttackMode;
                set => PlayerBehaviour.Player.currentAttackMode = value;
        }

        private float timeToAttack {
            get => PlayerBehaviour.Player.timeToAttack;
            set => PlayerBehaviour.Player.timeToAttack = value;
        }

        private bool hasClone {
            get => PlayerBehaviour.Player.hasClone;
            set => PlayerBehaviour.Player.hasClone = value;
        }

        private int currentAbilityIndex {
            get => PlayerBehaviour.Player.currentAbilityIndex;
            set => PlayerBehaviour.Player.currentAbilityIndex = value;
        }
    #endregion


    private void Awake() {
        PlayerBehaviour.Player.OnAttack.AddListener(ExecuteAttack);
        PlayerBehaviour.Player.OnChange.AddListener(ChangeAbility);
        PlayerBehaviour.onLaunch += (context) => LaunchProjectille();
    }

    private void Start() {
        currentAttackPoint = attackPoint;
        currentAttackMode = allAttacks[0];
        currentAttackRange = 0;

        PlayerBehaviour.Player.OnReleaseAbility.AddListener(() => UseAbility());
    }

    private void OnDestroy() {
        PlayerBehaviour.Player.OnAttack.RemoveListener(ExecuteAttack);
        PlayerBehaviour.Player.OnReleaseAbility.RemoveListener(() => UseAbility());
    }

    private void FixedUpdate() {
        //CheckAttackRange();
    }

    private void Update() {
        if (holdingSpecialButton) {
            timeToAttack += Time.deltaTime;
        }
        else {
            timeToAttack = 0;
        }
    }

    private Collider2D[] CheckAttackRange() {
        touchingAttack = Physics2D.OverlapCircleAll(currentAttackPoint.position, currentAttackRange, enemyLayer);
        return touchingAttack;
    }
    public void EnableCanAttack() {
        canAttack = true;
    }

    public void ExecuteAttack() {
        if (canAttack) {
            Coroutines.DoAfter(() => {
                if(!hasClone) {
                    currentAttackRange = attackRange;
                } else  {
                    currentAttackRange = PlayerBehaviour.Player.cloneAttackRange;
                }
                foreach (Collider2D enemy in CheckAttackRange()) {
                    enemy.gameObject.GetComponent<LifeController>().TakeDamage(attackDamage);
                }
                Coroutines.DoAfter(() => currentAttackRange = 0, 0.15f, this);
            }, 0.1f, this);
            
            canAttack = false;
            Coroutines.DoAfter(EnableCanAttack, attackCooldown, this);
        }
    }

    private void ChangeAbility() {
        currentAbilityIndex++;
        if(currentAbilityIndex > allAttacks.Length - 1) {
            currentAbilityIndex = 0;
        }
        currentAttackMode = allAttacks[currentAbilityIndex];
        GameManager.Game.UI.UpdateAbilityIcon(currentAttackMode.abilityImage);
    }

    private void UseAbility() {
        if (PlayerBehaviour.Player.timeToAttack > currentAttackMode.focusTime && currentBlood >= currentAttackMode.bloodCost) {
            PlayerBehaviour.Player.currentBlood -= currentAttackMode.bloodCost;
            print(-(float)currentAttackMode.bloodCost);
            GameManager.Game.UI.ChangeBloodBarFillAmount(-(float)currentAttackMode.bloodCost);

            switch (currentAttackMode.attackName) {
                case Attacks.Getsuga:
                    LaunchProjectille();
                    break;
                case Attacks.Clone:
                    PlayerBehaviour.Player.canTeleport = false;
                    StartCoroutine(CreateClones());
                    break;
            }
        }
    }

    public IEnumerator CreateClones() {
        GameObject clone = Instantiate(currentAttackMode.projectillePrefab, new Vector2(transform.position.x, transform.position.y + 2f), Quaternion.identity, this.transform) as GameObject;
        clone.name = "Clone UP";
        activeClones.Add(clone);
        currentAttackPoint = PlayerBehaviour.Player.cloneAttackPoint;
        hasClone = true;
        yield return new WaitForSeconds(currentAttackMode.abilityActiveTime);
        hasClone = false;
        foreach (GameObject item in activeClones) {
            Destroy(item);
        }
        currentAttackPoint = attackPoint;
        PlayerBehaviour.Player.canTeleport = true;
        yield break;
    }

    public void LaunchProjectille() {
        Instantiate(currentAttackMode.projectillePrefab, attackPoint.position, Quaternion.Euler(transform.right));
        //projectille.GetComponent<Rigidbody2D>().velocity += PlayerBehaviour.Player.lastDir * 200f * Time.deltaTime;
        return;
    }
}
