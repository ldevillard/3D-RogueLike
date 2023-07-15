using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public abstract class Capacity : MonoBehaviour
{
    public AnimationClip clip;
    public float clipSpeed = 1;
    public CapacityData data;

    [ReadOnly] public bool InUse;
    [ReadOnly] public bool InCooldown;

    protected PlayerController player;

    protected virtual void Start()
    {
        player = PlayerController.Instance;
    }

    public virtual void Use()
    {
        InUse = true;
        StartCoroutine(DurationCoroutine());
        InCooldown = true;
        StartCoroutine(CooldownCoroutine());
    }

    protected virtual IEnumerator DurationCoroutine()
    {
        yield return new WaitForSeconds(data.duration);
        InUse = false;
    }

    protected virtual IEnumerator CooldownCoroutine()
    {
        yield return new WaitForSeconds(data.cooldown);
        InCooldown = false;
    }
}
