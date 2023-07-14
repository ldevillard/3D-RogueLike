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
    public Animator Anim;

    public Weapon weapon;

    protected override void Init()
    {
        base.Init();
        if (weapon != null) weapon.entity = this;
        EntityManager.Instance.AddEnemy(this);
        Anim.SetFloat("MoveSpeedFactor", movement.MoveSpeedAnimationFactor);
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

    protected virtual void Update()
    {
        if (IsDead()) return;
        if (Target == null) PickTarget();

        if (Target != null)
        {
            if (weapon != null && weapon.IsAttacking)
            {
                movement.Stop();
                return;
            }
            if (!movement.isStopped && !movement.reachedTarget)
            {
                Model.transform.rotation = Quaternion.Lerp(Model.transform.rotation, Quaternion.LookRotation(movement.GetVelocity()), Time.deltaTime * 20);
                Anim.Play("Move");
                weapon.UpdateCooldown();
            }
            else if (movement.reachedTarget)
            {
                Model.transform.rotation = Quaternion.Lerp(Model.transform.rotation, Quaternion.LookRotation(Target.transform.position - Model.transform.position), Time.deltaTime * 20);
                Anim.SetTrigger("Idle");
                //Check if the model is facing the target to attack it
                if (Vector3.Angle(Model.transform.forward, Target.transform.position - Model.transform.position) < 10)
                    weapon.TryUse();
            }
            else
            {
                Anim.Play("Idle");
            }
        }
    }

    public override void WeaponUsedCallback()
    {
        Anim.SetTrigger("Attack");
    }

    void PickTarget()
    {
        Entity target = FindTarget();
        if (target != null)
        {
            Target = target;
            movement.SetTarget(Target.transform);
            weapon.SetTarget(Target);
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
