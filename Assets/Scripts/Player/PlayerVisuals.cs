using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class PlayerVisuals : MonoBehaviour
{
    //private Animator anim => PlayerBehaviour.Player.anim;
    PlayerBehaviour _player => PlayerBehaviour.Player;
    private Animator anim;
    float moveInput => PlayerBehaviour.Player.moveInput;
    bool canAttack => PlayerBehaviour.Player.canAttack;
    bool grounded => PlayerBehaviour.Player.grounded;
    Vector2 rbVel => PlayerBehaviour.Player.rbVel;
    bool readyToUseAbility => _player.readyToUseAbility;

    [ReadOnly] public bool attackFinished;

    [SerializeField] SpriteOutline outline;
    [SerializeField] Color onReadyToUseAbilityOutlineColor;

    private void Awake() {
        anim = GetComponent<Animator>();
    }

    private void Start() {
        outline = GetComponent<SpriteOutline>();
        PlayerBehaviour.Player.OnDash.AddListener(() => PlayDashAnimation());
        PlayerBehaviour.Player.OnAttack.AddListener(() => PlayAttackAnimation());
        PlayerBehaviour.Player.OnHurt.AddListener(PlayHurtAnimation);
    }

    private void OnDestroy() {
        PlayerBehaviour.Player.OnDash.RemoveListener(() => PlayDashAnimation());
        PlayerBehaviour.Player.OnAttack.RemoveListener(() => PlayAttackAnimation());
        PlayerBehaviour.Player.OnHurt.RemoveListener(PlayHurtAnimation);
    }

    void Update()
    {
        SettingAnimation();

        if (readyToUseAbility) {
            outline.outlineSize = 1;
            outline.color = onReadyToUseAbilityOutlineColor;
        }
        else {
            outline.outlineSize = 0;
            outline.color = new Color(0, 0, 0, 0);    
        }
    }

    private void SettingAnimation() {
        anim.SetFloat("vel", Mathf.Abs(moveInput));
        anim.SetBool("canAttack", canAttack);
        anim.SetBool("grounded", grounded);
        anim.SetFloat("velY", Mathf.Abs(rbVel.y));
        anim.SetBool("holdingAttack", PlayerBehaviour.Player.holdingSpecialButton);
    }

    public void PlayAttackAnimation() {
        if (!attackFinished) {

            anim.SetTrigger("attack");
        }
        else {
            anim.SetTrigger("attack2");
            attackFinished = false;
        }
    }

    public void SetAttackFinished() {
        attackFinished = true;
    }

    public void FinishAttack2() {
        attackFinished = false;
    }
    public void PlayDashAnimation() {
        if (canAttack) {
            anim.SetTrigger("dash");
        }
    }

    public void PlayHurtAnimation() {
        anim.SetTrigger("Hurt");
    }
}
