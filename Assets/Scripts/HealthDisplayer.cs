using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class HealthDisplayer : MonoBehaviour
{
    [SerializeField] Image fill;
    [SerializeField] Enemy entity;
    [SerializeField] CanvasGroup preview;

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        entity.OnHealthChanged += HandleHealthChange;
        entity.OnDie += HandleDie;
        fill.fillAmount = 1;
    }

    private void OnDestroy()
    {
        entity.OnHealthChanged -= HandleHealthChange;
        entity.OnDie -= HandleDie;

        if (fill != null)
            fill.DOKill();
    }

    private void HandleHealthChange(int oldHealth, int newHealth)
    {
        fill.DOKill();

        float newHealthPct = (float)newHealth / entity.MaxHealth;
        if (newHealthPct > 0.5f)
        {
            // Green to yellow
            fill.color = Color.Lerp(Color.green, Color.yellow, (1f - newHealthPct) * 2);
        }
        else
        {
            // Yellow to red
            fill.color = Color.Lerp(Color.yellow, Color.red, (0.5f - newHealthPct) * 2);
        }

        fill.DOFillAmount(newHealthPct, 0.2f).SetEase(Ease.OutBack);
    }

    void HandleDie()
    {
        Hide(() => Destroy(gameObject));
    }

    public void Show(Action onComplete = null)
    {
        preview.DOKill();
        preview.DOFade(1, 0.25f)
        .OnComplete(() => onComplete?.Invoke());
    }

    public void Hide(Action onComplete = null)
    {
        preview.DOKill();
        preview.DOFade(0, 0.25f)
        .OnComplete(() => onComplete?.Invoke());
    }
}
