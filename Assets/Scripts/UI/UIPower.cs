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

    public void Init()
    {
        mask.fillAmount = 0;

        switch (Type)
        {
            case UIPowerType.Dash:
                PlayerController.OnDash += EventCallback;
                break;
            case UIPowerType.Attack:
                break;
            case UIPowerType.Special1:
                break;
            case UIPowerType.Special2:
                break;
            default:
                break;
        }
    }

    void EventCallback(float time)
    {
        mask.fillAmount = 1;
        mask.DOFillAmount(0, time).SetEase(Ease.Linear)
        .OnComplete(() =>
        {
            rect.DOPunchScale(Vector3.one * 0.2f, 0.2f);
        });
    }

    void OnDestroy()
    {
        switch (Type)
        {
            case UIPowerType.Dash:
                PlayerController.OnDash -= EventCallback;
                break;
            case UIPowerType.Attack:
                break;
            case UIPowerType.Special1:
                break;
            case UIPowerType.Special2:
                break;
            default:
                break;
        }
    }
}
