using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RotateJump : Capacity
{
    public override void Use()
    {
        base.Use();

        player.transform.DOJump(transform.position, 1, 1, 0.4f)
        .OnComplete(() =>
        {
            CameraController.Instance.Shake(0.2f, 0.2f);
        });
    }

}
