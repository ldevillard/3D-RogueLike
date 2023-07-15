using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FPSCounter : MonoBehaviour
{
    public TextMeshProUGUI fpsText;
    public float updateInterval = 0.5f;
    private float deltaTime = 0f;
    private float frameCounter = 0f;
    private float timeCounter = 0f;

    void Update()
    {
        frameCounter++;
        timeCounter += Time.unscaledDeltaTime;

        if (timeCounter >= updateInterval)
        {
            float fps = frameCounter / timeCounter;
            fpsText.text = (int)fps + " FPS";

            // Reset counters
            frameCounter = 0f;
            timeCounter = 0f;
        }
    }
}
