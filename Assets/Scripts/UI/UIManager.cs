using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    static public UIManager Instance;

    public UIPowers PowerSection;

    void Awake()
    {
        Instance = this;
    }
}
