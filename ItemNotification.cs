using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class ItemNotification : MonoBehaviour
{
    public Image bg;
    public Text txtContent;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowNotification(string content) {
        txtContent.text = $"{content}";
        AutoHideView();
    }


    public void AutoHideView() {
        Sequence sequence = DOTween.Sequence();
        sequence.Insert(0.1f, bg.DOFade(0f, 0.5f).SetEase(Ease.InExpo));
        sequence.Insert(0.1f, bg.GetComponent<RectTransform>().DOAnchorPosY(300f,0.5f).SetEase(Ease.OutQuad).SetRelative(true));
        sequence.Insert(0.1f, txtContent.DOFade(0f, 0.5f).SetEase(Ease.InExpo));
        sequence.OnComplete(()=> {
            Destroy(gameObject);
        });
    }
}
