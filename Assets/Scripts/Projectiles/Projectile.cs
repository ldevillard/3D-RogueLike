using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Sirenix.OdinInspector;
public abstract class Projectile : MonoBehaviour
{
    public LayerMask targetMask;
    public float Speed = 10;

    public bool IsExploding;
    [ShowIf("IsExploding")]
    public float ExplosionRadius = 0.5f;

    int Damage;

    protected float lifeTime = 5;

    [SerializeField] ParticleSystem impactParticle;

    bool used;

    public void Init(int damage)
    {
        Damage = damage;
    }

    void FixedUpdate()
    {
        Logic();
        lifeTime -= Time.fixedDeltaTime;
        if (lifeTime <= 0)
            Destroy(gameObject);
    }

    protected abstract void Logic();

    protected virtual void Impact(Entity e = null)
    {
        if (used) return;
        used = true;

        if (e != null)
            e.Damage(Damage);

        if (impactParticle != null)
        {
            ParticleSystem p = Instantiate(impactParticle, transform.position, Quaternion.identity);
            p.CleanPlay();
        }

        if (IsExploding)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, ExplosionRadius, targetMask);
            foreach (Collider c in colliders)
            {
                if (c.TryGetComponent(out Entity entity))
                {
                    entity.Damage(Damage);
                }
            }
        }

        Destroy(gameObject, 0.5f);
    }

    public void ArcLaunch(Transform target, float duration, float delay)
    {
        DOVirtual.DelayedCall(delay, () =>
        {
            transform.SetParent(null);
            Vector3[] path;

            Vector3 midPoint = (transform.position + target.position) / 2;  // point milieu entre la source et la cible

            int i = Random.Range(0, 2);

            if (i == 0)
            {
                // Si le projectile est à droite de la cible, courbe à droite
                path = new Vector3[]
                {
            transform.position,
            new Vector3(midPoint.x + 1, midPoint.y, midPoint.z),
            target.position
                };
            }
            else
            {
                // Si le projectile est à gauche de la cible, courbe à gauche
                path = new Vector3[]
                {
            transform.position,
            new Vector3(midPoint.x - 1, midPoint.y, midPoint.z),
            target.position
                };
            }

            transform.DOPath(path, duration, PathType.CatmullRom).SetEase(Ease.InOutSine)
            .OnComplete(() => Impact());

        });
    }
}
