using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyBehaviour : MonoBehaviour
{
    public UnityEvent onCollisionWithEnd;

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("End")) {
            if (GameManager.Game) GameManager.Game.EndGame();
        }

        if(other.gameObject.CompareTag("projectille")) {
            GetComponent<LifeController>().TakeDamage(1);
            Destroy(other.gameObject);
        }
    }

    public void RechargeBlood() {
        if(PlayerBehaviour.Player.currentBlood < PlayerBehaviour.Player.maxBlood) {
            PlayerBehaviour.Player.currentBlood++;
        }
    }
}
