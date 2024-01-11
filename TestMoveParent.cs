using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class TestMoveParent : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        gameObject.transform.DOLocalMove(new Vector3(2f,0f,0f), 0.2f).SetRelative(true).SetEase(Ease.OutQuad).SetDelay(1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
