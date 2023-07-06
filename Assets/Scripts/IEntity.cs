using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEntity
{
    public int Health { get; set; }

    public void Die();
    public void Damage(int damage);

    public bool IsDead();
}
