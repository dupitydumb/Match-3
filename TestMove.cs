using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class TestMove : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        gameObject.transform.DOLocalMove(Vector3.zero, 0.5f).SetEase(Ease.OutQuad).SetDelay(1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
