using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LightFlash : MonoBehaviour
{
    [SerializeField] Light lightflash;

    public Vector2 range;
    public Ease ease = Ease.Linear;
    public float time = 0.5f;

    Tween tween;

    void Start()
    {
        if (lightflash == null)
            lightflash = GetComponent<Light>();

        if (lightflash == null)
            Debug.LogError("LightFlash: No light component found!");

        tween = DOTween.To(() => lightflash.range, x => lightflash.range = x, range.y, time)
        .From(range.x).SetLoops(-1, LoopType.Yoyo).SetEase(ease);
    }

    void OnDestroy()
    {
        tween.Kill();
    }
}