using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class PlayerVisuals : MonoBehaviour
{
    private Animator anim => PlayerBehaviour.Player.anim;
    float moveInput => PlayerBehaviour.Player.moveInput;
    bool canAttack => PlayerBehaviour.Player.canAttack;
    bool grounded => PlayerBehaviour.Player.grounded;
    Vector2 rbVel => PlayerBehaviour.Player.rbVel;

    [ReadOnly] public bool attackFinished;

    void Update()
    {
        SettingAnimation();
    }

    private void SettingAnimation() {
        anim.SetFloat("vel", Mathf.Abs(moveInput));
        anim.SetBool("canAttack", canAttack);
        anim.SetBool("grounded", grounded);
        anim.SetFloat("velY", Mathf.Abs(rbVel.y));
        anim.SetBool("holdingAttack", PlayerBehaviour.Player.holdingSpecialButton);
    }

    public void PlayAttackAnimation() {
        if(canAttack) {
            //anim.SetTrigger("attack");
            if (!attackFinished) {
                
                anim.SetTrigger("attack");
            }
            else {
                anim.SetTrigger("attack2");
                attackFinished = false;
            }
        }   
    }

    public void SetAttackFinished() {
        print("enter attack 1");
        attackFinished = true;
    }

    public void FinishAttack2() {
        print("Finish Attack 2");
        attackFinished = false;
    }
    public void PlayDashAnimation() {
        if (canAttack) {
            anim.SetTrigger("dash");
        }
    }
}
