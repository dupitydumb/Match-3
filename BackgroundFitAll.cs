using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundFitAll : MonoBehaviour
{
    float targetAspect = 1242f / 2208f;
    public Camera cameraBG;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(cameraBG.aspect);
        Debug.Log(targetAspect);
        gameObject.transform.localScale = Vector3.one;
        if (targetAspect < cameraBG.aspect)
        {
            gameObject.transform.localScale = Vector3.one * (cameraBG.aspect / targetAspect + 0.05f);
            Debug.Log(gameObject.transform.localScale);
        }

        
    }
}
