using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerInput : MonoBehaviour
{
    float moveInput {
        get => PlayerBehaviour.Player.moveInput;
        set => PlayerBehaviour.Player.moveInput = value;
    }
    bool holdingSpecialButton {
        get => PlayerBehaviour.Player.holdingSpecialButton;
        set => PlayerBehaviour.Player.holdingSpecialButton = value;
    }

    public void GetMoveInput(InputAction.CallbackContext context) {
        moveInput = context.ReadValue<float>();
    }

    public void ArrowUp(InputAction.CallbackContext context) {
        if (context.started) {
            PlayerBehaviour.Player.OnUp?.Invoke();
        }
    }

    public void ArrowDown(InputAction.CallbackContext context) {
        if (context.started) {
            PlayerBehaviour.Player.OnDown?.Invoke();
        }
    }
    public void Attack(InputAction.CallbackContext context) {
        if (context.started) {
            PlayerBehaviour.Player.OnAttack?.Invoke();
        }
    }

    public void Dash(InputAction.CallbackContext context) {
        if (context.started) {
            PlayerBehaviour.Player.OnDash?.Invoke();
        }
    }

    public void Ability(InputAction.CallbackContext context) {
        if (context.started) {
            holdingSpecialButton = true;
        }

        if (context.canceled) {
            PlayerBehaviour.Player.OnReleaseAbility?.Invoke();
            holdingSpecialButton = false;
        }
    }

    public void Change(InputAction.CallbackContext context) {
        if(context.started) {
            PlayerBehaviour.Player. OnChange?.Invoke();
        }
    }
}
