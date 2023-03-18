using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "enemy/shooter")]
public class ShooterEnemy : Enemy
{
    public GameObject projectille;
    public float shootRate;
    public float projectilleSpeed;
}
