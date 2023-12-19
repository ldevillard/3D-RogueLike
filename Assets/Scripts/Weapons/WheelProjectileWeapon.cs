using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class WheelProjectileWeapon : Weapon
{
    public LinearProjectile ProjectilePrefab;
    public int ProjectileCount;
    public float wheelRadius;

    List<Projectile> projectilePool = new List<Projectile>();

    protected override void Use()
    {
        base.Use();
        if (Delay > 0)
            StartCoroutine(DelayRoutine());
        else
            SpawnWheel();
    }

    void SpawnWheel()
    {
        Vector3 tempPos = transform.position;
        projectilePool.Clear();
        for (int i = 0; i < ProjectileCount; i++)
        {
            LinearProjectile p = Instantiate(ProjectilePrefab, tempPos, ProjectilePrefab.transform.rotation);
            projectilePool.Add(p);
            p.transform.parent = transform;
            p.transform.position -= Vector3.back;
            p.transform.localScale = Vector3.zero;

            Vector3 dir = tempPos - new Vector3(tempPos.x, tempPos.y, tempPos.z + wheelRadius);
            Vector3 pos = tempPos + (Quaternion.AngleAxis((360 / ProjectileCount) * i, Vector3.up) * dir);

            p.transform.position = pos;
            p.transform.DOScale(1, 0.5f).SetEase(Ease.OutSine);
            p.Control();
            p.ArcLaunch(target.transform, 0.3f, 2f + i * 0.1f);
        }
    }

    protected override void OnEntityDie(Entity e)
    {
        base.OnEntityDie(e);

        foreach (var p in projectilePool)
        {
            if (p != null)
                p.transform.SetParent(null);
        }
    }

    IEnumerator DelayRoutine()
    {
        yield return new WaitForSeconds(Delay);
        SpawnWheel();
    }
}