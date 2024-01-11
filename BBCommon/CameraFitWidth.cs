using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFitWidth : MonoBehaviour
{
    // initialSize is the default Camera.main.orthographicSize
    // targetAspect is the desired aspect ratio
    float initialSize = 11.04f;
    float targetAspect = 1242f / 2208f;
    // Start is called before the first frame update

    private void Awake()
    {
        Camera.main.orthographicSize = initialSize * (targetAspect / Camera.main.aspect);
    }
}
