using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class BackLightRotateEffect : MonoBehaviour
{
    public BBUIView view;

    public GameObject objBackLight;
    // Start is called before the first frame update
    void Start()
    {
        view.ShowBehavior.onCallback_Completed.AddListener(ShowView_Finished);
    }

    private void OnDestroy()
    {
        view.ShowBehavior.onCallback_Completed.RemoveAllListeners();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void ShowView_Finished()
    {
        objBackLight.transform.DORotate(new Vector3(0f, 0f, 90f), 1f).SetRelative(true).SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Incremental);
    }
}
