using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class RatePopup : MonoBehaviour
{
    [Header("LINK RATE")]
    private string rateLinkAndorid = "market://details";
    private string rateLinkIOS = "";

    public static RatePopup instance;

    [Header("Popup")]
    public GameObject popup;
    public BBUIButton btnClose;
    public BBUIButton btnLike;
    [Header("LockGroup")]
    public GameObject lockGroup;
    // Start is called before the first frame update
    void Start()
    {
        btnClose.OnPointerClickCallBack_Completed.AddListener(TouchClose);
        btnLike.OnPointerClickCallBack_Completed.AddListener(TouchLike);

        popup.GetComponent<BBUIView>().HideBehavior.onCallback_Completed.AddListener(PopupHideView_Finished);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenRatePopup() {
        gameObject.SetActive(true);
        InitViews();
    }

    private void InitViews()
    {
        lockGroup.gameObject.SetActive(false);

        popup.gameObject.SetActive(false);
        btnClose.gameObject.SetActive(false);
        btnLike.gameObject.SetActive(false);
       // AdmobManager.instance.HideBannerAd();
        InitViews_ShowView();
    }

    public void InitViews_ShowView()
    {
        Sequence sequenceShowView = DOTween.Sequence();
        sequenceShowView.InsertCallback(0.01f, () =>
        {
            popup.gameObject.SetActive(true);
            popup.GetComponent<BBUIView>().ShowView();
        });

        sequenceShowView.InsertCallback(0.4f, () =>
        {
            btnClose.gameObject.SetActive(true);
            btnClose.GetComponent<BBUIView>().ShowView();
        });

        sequenceShowView.InsertCallback(0.5f, () =>
        {
            btnLike.gameObject.SetActive(true);
            btnLike.GetComponent<BBUIView>().ShowView();
        });
    }

    private void TouchClose()
    {
        lockGroup.gameObject.SetActive(true);
        popup.GetComponent<BBUIView>().HideView();
    }

    public void TouchLike() {
        Config.SetRate();
#if UNITY_ANDROID

        Application.OpenURL(rateLinkAndorid);

#else
        Application.OpenURL(rateLinkIOS);
#endif
        gameObject.SetActive(false);
    }

    private void PopupHideView_Finished()
    {
        gameObject.SetActive(false);
    }
}
