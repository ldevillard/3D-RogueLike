using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class LinearProjectile : Projectile
{
    [ReadOnly] public float DetectionStep;

    public bool SphereCast = false;
    [ShowIf("SphereCast")] public float Radius = 0.25f;

    bool isControlled; //All us to control the projectile before throwing it

    void Start()
    {
        DetectionStep = Speed * Time.fixedDeltaTime;
    }

    protected override void Logic()
    {
        if (isControlled) return;
        if (SphereCast)
        {
            if (Physics.SphereCast(transform.position, Radius, transform.forward, out RaycastHit hit, DetectionStep, targetMask))
            {
                if (hit.collider.TryGetComponent(out Entity e))
                {
                    Impact(e);
                }
                Impact();
            }
        }
        else
        {
            if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, DetectionStep, targetMask))
            {
                if (hit.collider.TryGetComponent(out Entity e))
                {
                    Impact(e);
                }
                Impact();
            }
        }

        transform.position += transform.forward * Speed * Time.fixedDeltaTime;
    }

    public void Control()
    {
        isControlled = true;
    }

    public void Throw()
    {
        isControlled = false;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * DetectionStep);
        if (SphereCast)
            Gizmos.DrawWireSphere(transform.position + transform.forward * DetectionStep, Radius);
    }
}
