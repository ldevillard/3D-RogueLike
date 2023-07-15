using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class SideKick : Capacity
{
    [SerializeField] ParticleSystem hitParticle;

    public override void Use()
    {
        base.Use();
        player.DamageTarget(data.damage);
        DOVirtual.DelayedCall(0.15f, () =>
        {
            ParticleSystem p = Instantiate(hitParticle, player.body.leftFoot.position, hitParticle.transform.rotation);
            p.transform.localScale /= 2;
            p.CleanPlay();
        });
    }
}
