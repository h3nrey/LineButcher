using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Utils;

public class LifeController : MonoBehaviour
{
    [SerializeField] public int life;
    public int actualLife;
    public UnityEvent onLivesOver;
    public UnityEvent onTookDamage;

    private void Start() {
        actualLife = life;
    }

    public void TakeDamage(int damage) {
        if(actualLife > 0) {
            actualLife -= damage;
            onTookDamage?.Invoke();

            if (actualLife <= 0) onLivesOver?.Invoke();
        }
    }

    public void DestroyThis(float destroyCooldown) {
        Coroutines.DoAfter(() => Destroy(this.gameObject), destroyCooldown, this);
    }

}
