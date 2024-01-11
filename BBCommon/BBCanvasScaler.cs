using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BBCanvasScaler : MonoBehaviour
{
    private CanvasScaler canvasScaler;
    private void Awake()
    {
        canvasScaler = GetComponent<CanvasScaler>();
    }
    // Start is called before the first frame update
    void Start()
    {
        if (Screen.width * 1f / Screen.height >  1242f/ 2208f)
        {
            canvasScaler.matchWidthOrHeight = 1f;
        } else{
            canvasScaler.matchWidthOrHeight = 0f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
