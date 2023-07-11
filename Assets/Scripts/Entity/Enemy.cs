using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;
using DG.Tweening;

public abstract class Enemy : Entity
{
    public NavMeshMovement movement;
    [ReadOnly] public Entity Target;

    protected override void Init()
    {
        base.Init();
        EntityManager.Instance.AddEnemy(this);
    }

    Tween shakePos;
    public override void Damage(int damage)
    {
        if (shakePos != null) shakePos.Kill(true);
        shakePos = Model.transform.DOShakePosition(0.2f, 0.5f);

        base.Damage(damage);
    }

    public override void Die()
    {
        EntityManager.Instance.RemoveEnemy(this);
        if (shakePos != null) shakePos.Kill(true);
        base.Die();
    }

    protected abstract void Attack();

    protected virtual void Update()
    {
        if (IsDead()) return;
        if (Target == null) Target = FindTarget();
    }

    void PickTarget()
    {
        Entity target = FindTarget();
        if (target != null)
        {
            Target = target;
            movement.SetTarget(Target.transform);
        }
    }

    Entity FindTarget()
    {
        Entity target = null;
        foreach (Entity e in EntityManager.Instance.Entities)
        {
            if (target == null || Extender.Distance(e.transform.position, transform.position) < Extender.Distance(target.transform.position, transform.position))
            {
                target = e;
            }
        }
        return target;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, movement.RangeDistance);
    }
}
