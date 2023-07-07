using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class JumpAttack : Capacity
{
    [SerializeField] ParticleSystem groundExplosion;
    public float radius = 1;

    public override void Use()
    {
        InUse = true;
        StartCoroutine(DurationCoroutine());

        DOVirtual.DelayedCall(0.5f, () =>
        {
            CameraController.Instance.Shake(0.2f, 0.4f);
            ParticleSystem p = Instantiate(groundExplosion, player.transform.position, groundExplosion.transform.rotation);
            p.transform.localScale /= 2;
            p.CleanPlay();

            Collider[] colliders = Physics.OverlapSphere(player.transform.position, radius);

            foreach (var item in colliders)
            {
                if (item.TryGetComponent<Entity>(out var e))
                    e.Damage(data.damage);
            }
        });
    }

    protected override IEnumerator DurationCoroutine()
    {
        yield return new WaitForSeconds(data.duration);
        InUse = false;
    }
}
