using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinearProjectile : Projectile
{
    public float DetectionStep = 0.2f;

    protected override void Logic()
    {
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, DetectionStep, targetMask))
        {
            if (hit.collider.TryGetComponent(out Entity e))
            {
                Impact(e);
            }
        }

        transform.position += transform.forward * Speed * Time.fixedDeltaTime;
    }
}
