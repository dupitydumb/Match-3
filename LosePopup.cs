using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class LosePopup : MonoBehaviour
{
    public static LosePopup instance;

    [Header("Popup")]
    public GameObject popup;
    public BBUIButton btnRevive, btnReplay;
    //public BBUIButton btnClose;

    public Text txtLevel;
    public Image logo;
    //public NativeAdPanel nativeAdPanel;
    public GameObject lockGroup;

    private void Awake()
    {
        instance = this;
    }

    enum LOSE_TYPE_ACTION {
        REVIVE,
        RESTART,
        CLOSE
    }

    LOSE_TYPE_ACTION typeAction = LOSE_TYPE_ACTION.CLOSE;

    // Start is called before the first frame update
    void Start()
    {
        btnRevive.OnPointerClickCallBack_Completed.AddListener(TouchRevive);
        btnReplay.OnPointerClickCallBack_Completed.AddListener(TouchReplay);
        //btnClose.OnPointerClickCallBack_Completed.AddListener(TouchClose);


        popup.GetComponent<BBUIView>().HideBehavior.onCallback_Completed.AddListener(HidePopup_Finished);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TouchRevive()
    {
        if (AdmobManager.instance.isRewardAds_Avaiable())
        {
            lockGroup.gameObject.SetActive(true);
            AdmobManager.instance.ShowRewardAd_CallBack((AdmobManager.ADS_CALLBACK_STATE state) =>
            {
                if (state == AdmobManager.ADS_CALLBACK_STATE.SUCCESS)
                {
                    lockGroup.gameObject.SetActive(false);
                    Revive_Finished();
                }
                else
                {
                    lockGroup.gameObject.SetActive(false);
                }
            });
        }
        else
        {
            GamePlayManager.instance.SetReplayGame();
        }
    }

    public void Revive_Finished() {
        typeAction = LOSE_TYPE_ACTION.REVIVE;
        lockGroup.gameObject.SetActive(true);
        GameLevelManager.instance.Revive();
        popup.GetComponent<BBUIView>().HideView();

        GamePlayManager.instance.SetRevive_Success();
    }


    public void TouchReplay()
    {
        if (Config.interstitialAd_countLose % Config.interstitialAd_SHOW_LOSE_INTERVAL == 0)
        {
            if (AdmobManager.instance.isInterstititalAds_Avaiable())
            {
                lockGroup.gameObject.SetActive(true);
                AdmobManager.instance.ShowInterstitialAd_CallBack((AdmobManager.ADS_CALLBACK_STATE state) =>
                {
                    typeAction = LOSE_TYPE_ACTION.RESTART;
                    lockGroup.gameObject.SetActive(true);
                    popup.GetComponent<BBUIView>().HideView();

                    GamePlayManager.instance.SetReplayGame();
                });
            }
            else
            {
                typeAction = LOSE_TYPE_ACTION.RESTART;
                lockGroup.gameObject.SetActive(true);
                popup.GetComponent<BBUIView>().HideView();

                GamePlayManager.instance.SetReplayGame();
            }
        }
        else
        {
            typeAction = LOSE_TYPE_ACTION.RESTART;
            lockGroup.gameObject.SetActive(true);
            popup.GetComponent<BBUIView>().HideView();

            GamePlayManager.instance.SetReplayGame();
        }
        
    }

    public void TouchClose()
    {
        if (Config.currLevel >= Config.interstitialAd_levelShowAd &&  Config.interstitialAd_countLose % Config.interstitialAd_SHOW_LOSE_INTERVAL == 0)
        {
            if (AdmobManager.instance.isInterstititalAds_Avaiable())
            {
                lockGroup.gameObject.SetActive(true);
                AdmobManager.instance.ShowInterstitialAd_CallBack((AdmobManager.ADS_CALLBACK_STATE state) =>
                {
                    typeAction = LOSE_TYPE_ACTION.CLOSE;
                    lockGroup.gameObject.SetActive(true);
                    popup.GetComponent<BBUIView>().HideView();

                    SceneManager.LoadScene("Menu");
                });
            }
            else
            {
                typeAction = LOSE_TYPE_ACTION.CLOSE;
                lockGroup.gameObject.SetActive(true);
                popup.GetComponent<BBUIView>().HideView();

                SceneManager.LoadScene("Menu");
            }
        }
        else
        {
            typeAction = LOSE_TYPE_ACTION.CLOSE;
            lockGroup.gameObject.SetActive(true);
            popup.GetComponent<BBUIView>().HideView();

            SceneManager.LoadScene("Menu");
        }
        
    }

    private int level;
    private bool isRevive;
    public void ShowLosePopup(int _level,bool _isRevive)
    {
        AdmobManager.instance.Request_Banner();
        if(_level < 2)
        {
            AdmobManager.instance.HideBannerAd();
        }
        else
        {
            Config.interstitialAd_countLose++;
        }
        SoundManager.instance.PlaySound_GameOver();
       
        level = _level;
        isRevive = _isRevive;
        gameObject.SetActive(true);
        InitViews();
    }

    private void InitViews()
    {
        lockGroup.gameObject.SetActive(false);
        popup.gameObject.SetActive(false);

        btnRevive.gameObject.SetActive(false);
        if (AdmobManager.instance.isRewardAds_Avaiable() && !isRevive)
        {
            btnRevive.Interactable = true;
        }
        else
        {
            btnRevive.Interactable = false;
        }

        //if (AdmobManager.instance.nativeAd != null)
        //{
        //    logo.gameObject.SetActive(false);
        //    nativeAdPanel.SetInitNativeAd(AdmobManager.instance.nativeAd);
        //    FirebaseManager.instance.LogShowNative(Config.currLevel);
        //}
        //else
        //{
        //    logo.gameObject.SetActive(true);
        //    nativeAdPanel.gameObject.SetActive(false);
        //}

        btnReplay.gameObject.SetActive(false);
        //btnClose.gameObject.SetActive(false);
        txtLevel.text = $"Lv:{level}";
        InitViews_ShowView();
    }

    

    public void InitViews_ShowView()
    {
        Debug.Log("InitViews_ShowViewInitViews_ShowView");
        Sequence sequenceShowView = DOTween.Sequence();
        sequenceShowView.InsertCallback(0.01f, () =>
        {
            Debug.Log("AAAAAAA");
            popup.gameObject.SetActive(true);
            popup.GetComponent<BBUIView>().ShowView();
        });
        
        sequenceShowView.InsertCallback(0.4f, () =>
        {
            btnRevive.gameObject.SetActive(true);
            btnRevive.GetComponent<BBUIView>().ShowView();
        });

        sequenceShowView.InsertCallback(0.6f, () =>
        {
            btnReplay.gameObject.SetActive(true);
            btnReplay.GetComponent<BBUIView>().ShowView();
        });

       /* sequenceShowView.InsertCallback(1.2f, () =>
        {
            btnClose.gameObject.SetActive(true);
            btnClose.GetComponent<BBUIView>().ShowView();
        });*/
    }


    public void HidePopup_Finished()
    {
        //if (typeAction == LOSE_TYPE_ACTION.CLOSE)
        //{
        //    SceneManager.LoadScene("Menu");
        //}
        //else if (typeAction == LOSE_TYPE_ACTION.RESTART) {
        //    GamePlayManager.instance.SetReplayGame();
        //}
        //if (typeAction == LOSE_TYPE_ACTION.REVIVE)
        //{
        //    GamePlayManager.instance.SetRevive_Success();

        //}
        
        //AdmobManager.instance.HideBannerAd();
        gameObject.SetActive(false);
    }
}
