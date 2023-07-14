using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    public LayerMask targetMask;
    public float Speed = 10;

    int Damage;

    [SerializeField] ParticleSystem impactParticle;

    public void Init(int damage)
    {
        Damage = damage;
    }

    void FixedUpdate()
    {
        Logic();
    }

    protected abstract void Logic();

    protected virtual void Impact(Entity e)
    {
        e.Damage(Damage);

        if (impactParticle != null)
        {
            ParticleSystem p = Instantiate(impactParticle, transform.position, Quaternion.identity);
            p.CleanPlay();
        }

        Destroy(gameObject);
    }
}
