using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    public LayerMask targetMask;
    public float Speed = 10;

    int Damage;

    protected float lifeTime = 5;

    [SerializeField] ParticleSystem impactParticle;

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
        if (e != null)
            e.Damage(Damage);

        if (impactParticle != null)
        {
            ParticleSystem p = Instantiate(impactParticle, transform.position, Quaternion.identity);
            p.CleanPlay();
        }

        Destroy(gameObject);
    }
}
