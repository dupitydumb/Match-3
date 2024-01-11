using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Random = UnityEngine.Random;

public class IconNotification : MonoBehaviour
{
    private Image iconNotify;

    private void Awake()
    {
        iconNotify = GetComponent<Image>();
    }

    // Start is called before the first frame update
    void Start()
    {
        ShowAnimation();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private Sequence _sequence;
    private void ShowAnimation()
    {
        if (_sequence != null)
        {
            _sequence.Kill();
        }
        _sequence = DOTween.Sequence();
        float timeDelayStart = Random.Range(0.2f, 2f);
        float timePunchScale = Random.Range(1f, 1.5f);
        float timeNormalScale = 0.2f;
        _sequence.Insert(timeDelayStart,iconNotify.transform.DOPunchScale(Vector3.one * 0.2f, timePunchScale, 10, 2f).SetRelative(true));
        _sequence.Insert(timeDelayStart + timePunchScale,
            iconNotify.transform.DOScale(Vector3.one * 0.2f, timeNormalScale).SetEase(Ease.Linear).SetRelative(true)
                .SetLoops(Random.Range(3,6), LoopType.Yoyo));
        _sequence.SetLoops(-1, LoopType.Restart);
    }
}
