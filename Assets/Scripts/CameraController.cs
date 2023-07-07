using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraController : MonoBehaviour
{
    static public CameraController Instance;

    public Transform playerTransform;
    public float smoothSpeed = 5;
    public Vector3 offset;
    public Camera cam;

    void Awake()
    {
        Instance = this;
    }

    private void FixedUpdate()
    {
        Vector3 desiredPosition = playerTransform.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition.SetY(transform.position.y), smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;

        // pivot.transform.LookAt(playerTransform);
    }

    public void Shake(float duration, float strength = 3)
    {
        cam.DOShakePosition(duration, strength);
    }
}
