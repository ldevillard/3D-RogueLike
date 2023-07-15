using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPowers : MonoBehaviour
{
    public UIPower[] Powers;

    void Start()
    {
        foreach (var p in Powers)
            p.Init();
    }
}
