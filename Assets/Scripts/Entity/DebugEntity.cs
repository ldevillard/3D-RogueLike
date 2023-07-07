using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DebugEntity : Entity
{
    [SerializeField] ParticleSystem deathParticles;

    protected override void _Die()
    {
        base.Die();
        Model.transform.DOScale(0, 0.3f).SetEase(Ease.InBack)
        .OnComplete(() =>
        {
            ParticleSystem p = Instantiate(deathParticles, transform.position, deathParticles.transform.rotation);
            // p.transform.localScale /= 2;
            p.CleanPlay();
            Destroy(gameObject);
        });
    }
}
