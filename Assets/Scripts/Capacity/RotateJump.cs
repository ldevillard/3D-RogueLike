using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RotateJump : Capacity
{
    public override void Use()
    {
        InUse = true;
        StartCoroutine(DurationCoroutine());

        player.transform.DOJump(transform.position, 1, 1, 0.4f)
        .OnComplete(() =>
        {
            CameraController.Instance.Shake(0.2f, 0.2f);
        });
    }

    protected override IEnumerator DurationCoroutine()
    {
        yield return new WaitForSeconds(data.duration);
        InUse = false;
    }
}
