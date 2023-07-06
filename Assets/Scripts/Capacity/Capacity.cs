using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Capacity : MonoBehaviour
{
    public AnimationClip clip;
    public CapacityData data;

    public bool InUse;

    public abstract void Use();

    protected abstract IEnumerator DurationCoroutine();
}
