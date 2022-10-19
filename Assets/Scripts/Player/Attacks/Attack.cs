using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Attack")]
[System.Serializable]
public class Attack : ScriptableObject
{
    public string attackName;
    public int bloodCost;
    public int damage;
    public float focusTime;
    public GameObject projectillePrefab;
}
