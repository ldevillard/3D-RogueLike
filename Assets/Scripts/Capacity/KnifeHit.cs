using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeHit : Capacity
{
    public override void Use()
    {
        InUse = true;
        StartCoroutine(DurationCoroutine());
    }

    protected override IEnumerator DurationCoroutine()
    {
        yield return new WaitForSeconds(data.duration);
        InUse = false;
    }
}
