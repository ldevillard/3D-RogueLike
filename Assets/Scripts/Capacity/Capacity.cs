using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public abstract class Capacity : MonoBehaviour
{
    public AnimationClip clip;
    public float clipSpeed = 1;
    public CapacityData data;

    [ReadOnly] public bool InUse;

    protected PlayerController player;

    protected virtual void Start()
    {
        player = PlayerController.Instance;
    }

    public abstract void Use();

    protected abstract IEnumerator DurationCoroutine();
}
