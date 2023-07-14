using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class AnimatorEvents : MonoBehaviour
{
    [ReadOnly] public Entity entity;

    public void MoveAnimationEvent()
    {
        entity.MoveAnimationEvent();
    }
}
