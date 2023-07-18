using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class WheelProjectileWeapon : Weapon
{
    public LinearProjectile ProjectilePrefab;
    public int ProjectileCount;
    public float wheelRadius;

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
        transform.DORotate(new Vector3(0, 360, 0), 1f).SetLoops(-1, LoopType.Incremental).SetEase(Ease.Linear);
        Vector3 tempPos = transform.position;
        for (int i = 0; i < ProjectileCount; i++)
        {
            LinearProjectile p = Instantiate(ProjectilePrefab, tempPos, ProjectilePrefab.transform.rotation);
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

    IEnumerator DelayRoutine()
    {
        yield return new WaitForSeconds(Delay);
        SpawnWheel();
    }
}
