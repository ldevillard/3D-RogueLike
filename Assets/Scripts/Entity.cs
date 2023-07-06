using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour, IEntity
{
    int health;
    public int Health
    {
        get => health;
        set => health = value;
    }

    public void Damage(int damage)
    {
        Health -= damage;
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
