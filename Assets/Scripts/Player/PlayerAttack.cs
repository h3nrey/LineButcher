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
        private Attack[] allAbilities => PlayerBehaviour.Player.allAbilities;
        private int currentBlood => PlayerBehaviour.Player.currentBlood;
        private bool holdingSpecialButton => PlayerBehaviour.Player.holdingSpecialButton;
        private int attackDamage => PlayerBehaviour.Player.attackDamage;

        private bool canAttack {
            get => PlayerBehaviour.Player.canAttack;
            set => PlayerBehaviour.Player.canAttack = value;
        }
        private float currentAttackRange {
            get => PlayerBehaviour.Player.currentAttackRange;
            set => PlayerBehaviour.Player.currentAttackRange = value;
        }

        private Attack currentAbilityMode {
                get => PlayerBehaviour.Player.currentAbilityMode;
                set => PlayerBehaviour.Player.currentAbilityMode = value;
        }

        private float focusingTime {
            get => PlayerBehaviour.Player.focusingTime;
            set => PlayerBehaviour.Player.focusingTime = value;
        }

        private bool hasClone {
            get => PlayerBehaviour.Player.hasClone;
            set => PlayerBehaviour.Player.hasClone = value;
        }

        private int currentAbilityIndex {
            get => PlayerBehaviour.Player.currentAbilityIndex;
            set => PlayerBehaviour.Player.currentAbilityIndex = value;
        }

        private bool readyToUseAbility {
            get => PlayerBehaviour.Player.readyToUseAbility;
            set => PlayerBehaviour.Player.readyToUseAbility = value;
        }
    #endregion


    private void Awake() {
        PlayerBehaviour.Player.OnAttack.AddListener(ExecuteAttack);
        PlayerBehaviour.Player.OnChange.AddListener(ChangeAbility);
    }

    private void Start() {
        currentAttackPoint = attackPoint;
        currentAbilityMode = allAbilities[0];
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
        HandleTimeToAttack();

        if(focusingTime >= currentAbilityMode.focusTime) {
            readyToUseAbility = true;
        } else readyToUseAbility = false;
    }
    private void HandleTimeToAttack() {
        if (holdingSpecialButton) {
            focusingTime += Time.deltaTime;
        }
        else {
            focusingTime = 0;
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
        if(currentAbilityIndex > allAbilities.Length - 1) {
            currentAbilityIndex = 0;
        }
        currentAbilityMode = allAbilities[currentAbilityIndex];
        GameManager.Game.UI.UpdateAbilityIcon(currentAbilityMode.abilityImage);
    }

    private void UseAbility() {
        if (hasClone) return;
        if (readyToUseAbility && currentBlood >= currentAbilityMode.bloodCost) {
            PlayerBehaviour.Player.currentBlood -= currentAbilityMode.bloodCost;
            GameManager.Game.UI.ChangeBloodBarFillAmount(-(float)currentAbilityMode.bloodCost);

            switch (currentAbilityMode.attackName) {
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
        GameObject clone = Instantiate(currentAbilityMode.projectillePrefab, new Vector2(transform.position.x, transform.position.y + 2f), Quaternion.identity, this.transform) as GameObject;
        clone.name = "Clone UP";
        activeClones.Add(clone);
        currentAttackPoint = PlayerBehaviour.Player.cloneAttackPoint;
        hasClone = true;
        yield return new WaitForSeconds(currentAbilityMode.abilityActiveTime);
        hasClone = false;
        foreach (GameObject item in activeClones) {
            Destroy(item);
        }
        currentAttackPoint = attackPoint;
        PlayerBehaviour.Player.canTeleport = true;
        yield break;
    }

    public void LaunchProjectille() {
        Instantiate(currentAbilityMode.projectillePrefab, attackPoint.position, Quaternion.Euler(transform.right));
        //projectille.GetComponent<Rigidbody2D>().velocity += PlayerBehaviour.Player.lastDir * 200f * Time.deltaTime;
        return;
    }
}
