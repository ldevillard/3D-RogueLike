using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugManager : MonoBehaviour
{
    public KeyCode ResetKey;

    void Start()
    {
#if !UNITY_EDITOR
        Application.targetFrameRate = 60;
#endif
    }

    // #if UNITY_EDITOR
    void Update()
    {
        if (Input.GetKeyDown(ResetKey))
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }
    // #endif
}
