using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class KnifeHit : Capacity
{
    [SerializeField] ParticleSystem hitParticle;

    public override void Use()
    {
        base.Use();
        player.DamageTarget(data.damage);
        DOVirtual.DelayedCall(0.15f, () =>
        {
            ParticleSystem p = Instantiate(hitParticle, player.body.rightHand.position, hitParticle.transform.rotation);
            p.CleanPlay();
        });
    }
}
