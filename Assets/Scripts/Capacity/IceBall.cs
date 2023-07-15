using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBall : Capacity
{
    [SerializeField] Projectile iceProjectile;

    public override void Use()
    {
        base.Use();

        Projectile p = Instantiate(iceProjectile, player.GetPosition(), player.transform.rotation);
        p.Init(data.damage);
        p.transform.forward = player.Model.transform.forward;
    }
}
