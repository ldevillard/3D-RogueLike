using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface IEntity
{
    public event Action<int, int> OnHealthChanged;

    public int MaxHealth { get; set; }
    public int Health { get; set; }

    public void Die();
    public void Damage(int damage);

    public bool IsDead();
}
