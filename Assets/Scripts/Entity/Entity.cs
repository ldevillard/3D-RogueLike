using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Sirenix.OdinInspector;
using DG.Tweening;

public abstract class Entity : MonoBehaviour
{
    public event Action<int, int> OnHealthChanged;
    public event Action OnDie;

    public Collider cl;
    public Flickerer flicker;

    public GameObject Model;

    public int health;
    public int Health
    {
        get => health;
        set => health = value;
    }

    [ReadOnly] public int maxHealth;
    public int MaxHealth
    {
        get => maxHealth;
        set => maxHealth = value;
    }

    [SerializeField] protected ParticleSystem deathParticles;

    protected virtual void Start()
    {
        Init();
    }

    protected virtual void Init()
    {
        MaxHealth = Health;

        AnimatorEvents e = GetComponentInChildren<AnimatorEvents>();
        if (e != null)
        {
            e.entity = this;
        }
    }

    public bool IsDead()
    {
        return Health <= 0;
    }

    public virtual void Die()
    {
        OnDie?.Invoke();
        Model.transform.DOScale(0, 0.3f).SetEase(Ease.InBack)
        .OnComplete(() =>
        {
            ParticleSystem p = Instantiate(deathParticles, transform.position, deathParticles.transform.rotation);
            p.CleanPlay();
            Destroy(gameObject);
        });
    }

    public virtual void Damage(int damage)
    {
        int oldHealth = Health;
        Health -= damage;
        if (flicker != null) flicker.Flicker();

        OnHealthChanged?.Invoke(oldHealth, Health);

        if (IsDead()) Die();
    }

    public virtual Vector3 GetPosition()
    {
        return transform.position;
    }

    public abstract void MoveAnimationEvent();
    public virtual void WeaponUsedCallback() { }
}