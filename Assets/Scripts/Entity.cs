using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;
using DG.Tweening;

public class Entity : MonoBehaviour, IEntity
{
    public Collider cl;
    public Flickerer flicker;

    public int health;
    public int Health
    {
        get => health;
        set => health = value;
    }

    public event Action<int, int> OnHealthChanged;

    [ReadOnly] public int maxHealth;
    public int MaxHealth
    {
        get => maxHealth;
        set => maxHealth = value;
    }

    Tween shakeScale;
    public void Damage(int damage)
    {
        Health -= damage;
        flicker.Flicker();
        if (shakeScale != null) shakeScale.Kill(true);
        shakeScale = transform.DOShakeScale(0.2f, 0.5f);
        if (IsDead()) Die();
    }

    public void Die()
    {
        Destroy(gameObject);
    }

    public bool IsDead()
    {
        return Health <= 0;
    }
}
