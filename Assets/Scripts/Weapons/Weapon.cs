using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public abstract class Weapon : MonoBehaviour
{
    [ReadOnly] public Entity entity;
    [ReadOnly] public Entity target;

    public int Damage;
    public float Cooldown;
    public float AttackDuration;
    public float Delay;

    float timer = 0;

    [ReadOnly] public bool IsAttacking;

    public void TryUse()
    {
        if (IsAttacking) return;
        if (timer <= 0)
        {
            Use();
            timer = Cooldown;
        }
        timer -= Time.deltaTime;
    }

    public void SetTarget(Entity e)
    {
        target = e;
    }

    public void UpdateCooldown()
    {
        timer -= Time.deltaTime;
    }

    protected virtual void Use()
    {
        IsAttacking = true;
        entity.WeaponUsedCallback();
        StartCoroutine(AttackDurationRoutine());
    }

    IEnumerator AttackDurationRoutine()
    {
        yield return new WaitForSeconds(AttackDuration);
        IsAttacking = false;
    }
}
