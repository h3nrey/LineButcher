using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisuals : MonoBehaviour
{
    private Animator anim => PlayerBehaviour.Player.anim;
    float moveInput => PlayerBehaviour.Player.moveInput;
    bool canAttack => PlayerBehaviour.Player.canAttack;
    bool grounded => PlayerBehaviour.Player.grounded;
    Vector2 rbVel => PlayerBehaviour.Player.rbVel;

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
            anim.SetTrigger("attack");
        }   
    }
    public void PlayDashAnimation() {
        if (canAttack) {
            anim.SetTrigger("dash");
        }
    }
}
