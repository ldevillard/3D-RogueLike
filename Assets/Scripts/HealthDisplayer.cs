using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

public class HealthDisplayer : MonoBehaviour
{
    public TextMeshPro healthText;
    public Image fill;
    [SerializeField] GameObject preview;
    [SerializeField] Entity entity;

    [SerializeField] private Transform _pivot;

    public bool avoidActiveAtLeastOnce = false;
    bool wasActivatedAtLeastOnce;

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        entity.OnHealthChanged += HandleHealthChange;

        HandleHealthChange(entity.Health, entity.Health);
        healthText.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        entity.OnHealthChanged -= HandleHealthChange;
    }

    private void HandleHealthChange(int oldHealth, int newHealth)
    {
        if (!preview.activeSelf && newHealth < entity.MaxHealth) { preview.SetActive(true); if (!avoidActiveAtLeastOnce) wasActivatedAtLeastOnce = true; }
        else if (preview.activeSelf && newHealth >= entity.MaxHealth && !wasActivatedAtLeastOnce) preview.SetActive(false);
        // fill.fillAmount = newHealth / entity.MaxHealth;
        _pivot.transform.localScale = _pivot.transform.localScale.SetX(newHealth / entity.MaxHealth);

        healthText.text = (Mathf.CeilToInt(newHealth)).ToString();

    }

    public void Show()
    {
        preview.transform.DOKill();
        preview.transform.DOScale(Vector3.one, 0.25f).SetEase(Ease.OutBack);
    }

    public void Hide()
    {
        preview.transform.DOKill();
        preview.transform.DOScale(Vector3.zero, 0.25f).SetEase(Ease.InBack);
    }
}
