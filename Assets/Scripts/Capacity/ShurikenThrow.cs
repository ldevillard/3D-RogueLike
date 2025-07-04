using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShurikenThrow : Capacity
{
    [SerializeField] Projectile shurikenProjectile;

    public override void Use()
    {
        Projectile p = Instantiate(shurikenProjectile, player.GetPosition(), player.transform.rotation);
        // p.transform.position += Vector3.right * Random.Range(-1f, 1f);
        p.Init(data.damage);
        p.transform.forward = player.Model.transform.forward;
    }
}
