using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public class ShooterBehaviour : MonoBehaviour
{
    [SerializeField] ShooterEnemy shooterData;
    [SerializeField] private EnemyBehaviour enemy;

    private void Start() {
        InvokeRepeating("Shoot", 0.2f, shooterData.shootRate);
    }
    private void Shoot() {
        enemy.canMove = false;
        GameObject projectille = Instantiate(shooterData.projectille, this.transform) as GameObject;
        //projectille.GetComponent<Rigidbody2D>().velocity = transform.right * shooterData.projectilleSpeed;
        Coroutines.DoAfter(() => enemy.canMove = true, 0.15f, this);
   }
}
