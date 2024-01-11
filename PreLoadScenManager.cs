using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PreLoadScenManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
#if UNITY_EDITOR
        Debug.unityLogger.logEnabled = true;
#else
        Debug.unityLogger.logEnabled = false;
#endif
        Application.targetFrameRate = 60;
        SceneManager.LoadSceneAsync("LoadScene");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
