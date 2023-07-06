using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CapacityData", menuName = "ScriptableObjects/CapacityData", order = 1)]
public class CapacityData : ScriptableObject
{
    public float cooldown;
    public float duration;
    public float damage;
}
