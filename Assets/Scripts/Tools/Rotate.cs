using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    public float Speed;
    public Vector3 Axis;
    public bool Local = true;

    void Update()
    {
        if (Local)
            transform.Rotate(Axis, Speed * Time.deltaTime);
        else
            transform.Rotate(Axis, Speed * Time.deltaTime, Space.World);
    }
}