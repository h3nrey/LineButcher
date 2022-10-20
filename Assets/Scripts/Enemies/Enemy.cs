using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
[CreateAssetMenu(menuName = "enemy")]
public class Enemy : ScriptableObject {
    public int life;
    public float speed;
    public float knockbackForce;
    public Sprite spr;

}
