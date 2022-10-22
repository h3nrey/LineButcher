using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Attack")]
[System.Serializable]
public class Attack : ScriptableObject
{
    public Attacks attackName;
    public int bloodCost;
    public int damage;
    public float focusTime;
    public GameObject projectillePrefab;
    public float abilityActiveTime; // is the time that the ability can stay active
    public Sprite abilityImage;
}

public enum Attacks {
    Getsuga = 0,
    Clone = 1,
    Beserk = 2,
}
