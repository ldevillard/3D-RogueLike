using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;
using UnityEngine.UI;

public class UIPower : MonoBehaviour
{
    public enum UIPowerType
    {
        Dash = 0,
        Attack = 1,
        Special1 = 2,
        Special2 = 3
    }

    public UIPowerType Type;

    public Image mask;
    public RectTransform rect;

    Vector3 startScale;

    public void Init()
    {
        mask.fillAmount = 0;
        startScale = rect.localScale;

        switch (Type)
        {
            case UIPowerType.Dash:
                PlayerController.OnDash += EventCallback;
                break;
            case UIPowerType.Attack:
                PlayerController.OnAttack += FeedbackCallback;
                break;
            case UIPowerType.Special1:
                PlayerController.OnSpecial1 += EventCallback;
                break;
            case UIPowerType.Special2:
                PlayerController.OnSpecial2 += EventCallback;
                break;
            default:
                break;
        }
    }


    Tween punchTween;
    void EventCallback(float time)
    {
        mask.fillAmount = 1;

        if (punchTween != null)
        {
            punchTween.Kill(true);
            rect.localScale = startScale;
        }

        mask.DOFillAmount(0, time).SetEase(Ease.Linear)
        .OnComplete(() =>
        {
            punchTween = rect.DOPunchScale(Vector3.one * 0.2f, 0.2f);
        });
    }

    void FeedbackCallback()
    {
        punchTween = rect.DOPunchScale(Vector3.one * 0.2f, 0.2f);
    }

    void OnDestroy()
    {
        if (punchTween != null)
            punchTween.Kill();

        switch (Type)
        {
            case UIPowerType.Dash:
                PlayerController.OnDash -= EventCallback;
                break;
            case UIPowerType.Attack:
                PlayerController.OnAttack -= FeedbackCallback;
                break;
            case UIPowerType.Special1:
                PlayerController.OnSpecial1 -= EventCallback;
                break;
            case UIPowerType.Special2:
                PlayerController.OnSpecial2 -= EventCallback;
                break;
            default:
                break;
        }
    }
}
