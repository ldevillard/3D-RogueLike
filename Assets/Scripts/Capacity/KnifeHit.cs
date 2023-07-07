using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class KnifeHit : Capacity
{
    [SerializeField] ParticleSystem hitParticle;

    public override void Use()
    {
        InUse = true;
        player.DamageTarget(data.damage);
        DOVirtual.DelayedCall(0.15f, () =>
        {
            ParticleSystem p = Instantiate(hitParticle, player.body.rightHand.position, hitParticle.transform.rotation);
            p.CleanPlay();
        });
        StartCoroutine(DurationCoroutine());
    }

    protected override IEnumerator DurationCoroutine()
    {
        yield return new WaitForSeconds(data.duration);
        InUse = false;
    }
}
