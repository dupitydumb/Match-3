using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class IconButtonEffect : MonoBehaviour
{
    public BBUIView view;

    public GameObject iconEffect;
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
        Sequence sequence = DOTween.Sequence();
        sequence.Insert(0f, iconEffect.transform.DOScale(Vector3.one * 1.1f,0.4f).SetEase(Ease.Linear).SetLoops(5, LoopType.Yoyo));
        sequence.Insert(2f,iconEffect.transform.DOPunchRotation(new Vector3(0f, 0f, 20f), 1f, 6, 2).SetEase(Ease.OutQuart));
        sequence.Insert(2f,iconEffect.transform.DOPunchScale(new Vector3(0.1f, 0.1f,0.1f), 1f, 2, 1).SetEase(Ease.OutQuart));
        sequence.SetLoops(-1, LoopType.Yoyo);
    }
}
