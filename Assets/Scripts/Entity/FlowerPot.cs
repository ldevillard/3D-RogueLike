using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FlowerPot : Enemy
{
    [SerializeField] ParticleSystem moveParticles;

    public override void MoveAnimationEvent()
    {
        ParticleSystem p = Instantiate(moveParticles, transform.position + Model.transform.forward * 0.5f, moveParticles.transform.rotation);
        p.CleanPlay();
    }

    public override void WeaponUsedCallback()
    {
        base.WeaponUsedCallback();
    }
}
