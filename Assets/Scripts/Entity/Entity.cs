using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;
using DG.Tweening;

public abstract class Entity : MonoBehaviour, IEntity
{
    public Collider cl;
    public Flickerer flicker;

    public GameObject Model;

    public int health;
    public int Health
    {
        get => health;
        set => health = value;
    }

    public event Action<int, int> OnHealthChanged;
    public event Action OnDie;

    [ReadOnly] public int maxHealth;
    public int MaxHealth
    {
        get => maxHealth;
        set => maxHealth = value;
    }

    void Start()
    {
        Init();
    }

    protected virtual void Init()
    {
        MaxHealth = Health;
    }

    Tween shakePos;
    public void Damage(int damage)
    {
        int oldHealth = Health;
        Health -= damage;
        flicker.Flicker();

        OnHealthChanged?.Invoke(oldHealth, Health);

        if (shakePos != null) shakePos.Kill(true);
        shakePos = Model.transform.DOShakePosition(0.2f, 0.5f);
        if (IsDead()) _Die();
    }

    public void Die()
    {
        OnDie?.Invoke();
        if (shakePos != null) shakePos.Kill(true);
    }

    protected virtual void _Die()
    {
        Die();
    }

    public bool IsDead()
    {
        return Health <= 0;
    }
}
