using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;
using System.Linq;

public class Flickerer : MonoBehaviour
{
    public int flickerTimes = 2;
    public float flickerDuration = 0.25f;

    public Material flickerMat;

    public List<Renderer> renderers = new List<Renderer>();
    protected Dictionary<Renderer, Material[]> materialDico = new Dictionary<Renderer, Material[]>();
    public Sequence s;

    protected virtual void Awake()
    {
        renderers = renderers.Where(x => x != null).ToList();
        UpdateRenderers();
    }

    public virtual void UpdateRenderers()
    {
        if (renderers == null)
            return;
        materialDico.Clear();
        foreach (Renderer rd in renderers)
        {
            materialDico.Add(rd, rd.sharedMaterials);
        }
    }

    [Button("Flicker")]
    public virtual void Flicker()
    {
        if (s != null) s.Kill(true);
        s = DOTween.Sequence();
        s.AppendCallback(() =>
        {
            SetFlicker();
        });
        s.AppendInterval(flickerDuration / 2);
        s.AppendCallback(() =>
        {
            Reset();
        });
        s.AppendInterval(flickerDuration / 2);
        s.SetLoops(flickerTimes);
    }

    public virtual void SetFlicker()
    {
        foreach (Renderer rd in renderers)
            rd.sharedMaterials = new Material[] { flickerMat };
    }

    public virtual void Reset()
    {
        foreach (var item in materialDico)
        {
            item.Key.sharedMaterials = item.Value;
        }
    }

    private void OnDisable()
    {
        s?.Kill(true);
        Reset();
    }

    private void OnDestroy()
    {
        s?.Kill(true);
    }

    [Button("Fetch Renderers")]
    public void FetchRenderers()
    {
        renderers = GetComponentsInChildren<Renderer>().ToList();
    }
}