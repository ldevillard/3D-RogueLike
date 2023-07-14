using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileWeapon : Weapon
{
    public Projectile ProjectilePrefab;

    protected override void Use()
    {
        base.Use();
        if (Delay > 0)
            StartCoroutine(DelayRoutine());
        else
            ThrowProjectile();
    }

    IEnumerator DelayRoutine()
    {
        yield return new WaitForSeconds(Delay);
        ThrowProjectile();
    }

    void ThrowProjectile()
    {
        Projectile p = Instantiate(ProjectilePrefab, transform.position, transform.rotation);
        p.Init(Damage);
        if (target != null && Vector3.Angle(entity.Model.transform.forward, target.GetPosition() - transform.position) < 30)
            p.transform.forward = (target.GetPosition() - transform.position).normalized;
        else
            p.transform.forward = entity.Model.transform.forward;
    }
}
