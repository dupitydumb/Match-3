using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
public class LosePopup2 : MonoBehaviour
{
    public static LosePopup2 instance;
    // Start is called before the first frame update
    public bool is_continue;
    [Header("Popup Reward")]
    public BBUIView popupReward;

    public RectTransform txtCleanAllBoxs;
    public BBUIButton btnContinue_Video;
    public BBUIButton btnContinue_Coin;
    public BBUIButton btnNoThanks;
    
    [Header("Popup Action")]
    public BBUIView popupAction;

    public BBUIButton btnRestart;
    public BBUIButton btnHome;
    
    public GameObject lockGroup;
    
    
    enum REWARD_ACTION {
        REVIVE,
        CLOSE
    }

    REWARD_ACTION rewardAction = REWARD_ACTION.CLOSE;
    
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        is_continue = false;
        popupReward.ShowBehavior.onCallback_Completed.AddListener(PopupReward_ShowView_Finished);
        popupReward.HideBehavior.onCallback_Completed.AddListener(PopupReward_HideView_Finished);
        
        popupAction.ShowBehavior.onCallback_Completed.AddListener(PopupAction_ShowView_Finished);
        popupAction.HideBehavior.onCallback_Completed.AddListener(PopupAction_HideView_Finished);
        
        btnContinue_Video.OnPointerClickCallBack_Completed.AddListener(Reward_Ads);
        btnContinue_Coin.OnPointerClickCallBack_Completed.AddListener(TouchContinue_Coin);
        btnNoThanks.OnPointerClickCallBack_Completed.AddListener(TouchNoThank);
        
        btnRestart.OnPointerClickCallBack_Completed.AddListener(TouchRestart);
        btnHome.OnPointerClickCallBack_Completed.AddListener(TouchHome);
        
    }

    
    private int level;
    private bool isRevive;
    public void ShowLosePopup(int _level,bool _isRevive)
    {
        
        if(_level < 2)
        {
           // AdmobManager.instance.HideBannerAd();
        }
        else
        {
           // AdmobManager.instance.Request_Banner();
        }
        SoundManager.instance.PlaySound_GameOver();
       
        level = _level;
        
        isRevive = _isRevive;
        gameObject.SetActive(true);
        lockGroup.SetActive(true);
        popupReward.gameObject.SetActive(false);
        btnContinue_Video.gameObject.SetActive(false);
        btnContinue_Coin.gameObject.SetActive(false);
        btnNoThanks.gameObject.SetActive(false);
        popupAction.gameObject.SetActive(false);
        btnRestart.gameObject.SetActive(false);
        btnHome.gameObject.SetActive(false);

        if (isRevive)
        {
            StartCoroutine(OpenPopup_Action());
        }
        else
        {
            ShowViews();
        }
        
    }

    public void Reward_Ads() 
    {
        is_continue = true;
        Ad_Manager.instance.Show_Rewarded();
    }

    private void ShowViews()
    {
        
        
        StartCoroutine(ShowViews_IEnumerator());
    }
    
    private IEnumerator ShowViews_IEnumerator()
    {
        yield return new WaitForSeconds(0.1f);
        popupReward.gameObject.SetActive(true);
        popupReward.GetComponent<BBUIView>().ShowView();

        if (Config.CheckDaily_FreeRevive())
        {
            yield return new WaitForSeconds(0.1f);
            btnContinue_Video.gameObject.SetActive(true);
            btnContinue_Video.GetComponent<BBUIView>().ShowView();

            // txtCleanAllBoxs.DOAnchorPosY(95f, 0.2f);
        }
        else
        {
            // txtCleanAllBoxs.DOAnchorPosY(-44f, 0.2f);
        }
        
        yield return new WaitForSeconds(0.1f);
        btnContinue_Coin.gameObject.SetActive(true);
        btnContinue_Coin.GetComponent<BBUIView>().ShowView();

        
        
        lockGroup.SetActive(false);
        
        yield return new WaitForSeconds(2.5f);
        btnNoThanks.gameObject.SetActive(true);
        btnNoThanks.GetComponent<BBUIView>().ShowView();
    }
    
    
    // Update is called once per frame
    void Update()
    {
        
    }

    public void Show_Static_Ad() => Ad_Manager.instance.Show_Interstitial();
    
    private void PopupReward_ShowView_Finished()
    {
        
    }
    private void PopupReward_HideView_Finished()
    {
        Debug.Log("PopupReward_HideView_FinishedPopupReward_HideView_Finished");
        if (rewardAction == REWARD_ACTION.REVIVE)
        {
            gameObject.SetActive(false);
        }
        else if (rewardAction == REWARD_ACTION.CLOSE)
        {
            StartCoroutine(OpenPopup_Action());
        }
    }
    
    private void PopupAction_ShowView_Finished()
    {
        
    }
    private void PopupAction_HideView_Finished()
    {
        
    }


    public void TouchContinue_Video()
    {
        lockGroup.gameObject.SetActive(true);
        
        if (AdmobManager.instance.isRewardAds_Avaiable())
        {
            AdmobManager.instance.ShowRewardAd_CallBack((AdmobManager.ADS_CALLBACK_STATE state) =>
            {
                if (state == AdmobManager.ADS_CALLBACK_STATE.SUCCESS)
                {
                    Debug.Log("111111111111111111111111111");
                    rewardAction = REWARD_ACTION.REVIVE;
                    popupReward.HideView();
                    GameLevelManager.instance.Revive();
                    
                    GamePlayManager.instance.SetRevive_Success();
                    
                    Config.SetDaily_FreeRevive();
                }
                else
                {
                    lockGroup.gameObject.SetActive(false);
                    NotificationPopup.instance.AddNotification("Revive Fail!");
                }
            });
        }
        else
        {
            lockGroup.gameObject.SetActive(false);
            NotificationPopup.instance.AddNotification("No Video Available!");
        }
    }

    private void TouchContinue_Coin()
    {
        if (Config.currCoin >= Config.PRICE_COIN_REVIVE)
        {
            Config.SetCoin(Config.currCoin - Config.PRICE_COIN_REVIVE);
            
            rewardAction = REWARD_ACTION.REVIVE;
            popupReward.HideView();
            GameLevelManager.instance.Revive();
            
            GamePlayManager.instance.SetRevive_Success();
        }
        else
        {
            GamePlayManager.instance.OpenShopPopup();
        }
    }

    private void TouchNoThank()
    {
        lockGroup.SetActive(true);
        rewardAction = REWARD_ACTION.CLOSE;
        popupReward.HideView();
    }
    
    
    private IEnumerator OpenPopup_Action()
    {
        lockGroup.SetActive(true);
        
        popupAction.gameObject.SetActive(true);
        popupAction.ShowView();
        
        yield return new WaitForSeconds(0.1f);
        btnRestart.gameObject.SetActive(true);
        btnRestart.GetComponent<BBUIView>().ShowView();
        lockGroup.SetActive(false);
        
        yield return new WaitForSeconds(2f);
        btnHome.gameObject.SetActive(true);
        btnHome.GetComponent<BBUIView>().ShowView();
        
        
    }
    
    private void TouchHome()
    {
        lockGroup.gameObject.SetActive(true);
        if (AdmobManager.instance.isInterstititalAds_Avaiable())
        {
            lockGroup.gameObject.SetActive(true);
            AdmobManager.instance.ShowInterstitialAd_CallBack((AdmobManager.ADS_CALLBACK_STATE state) =>
            {
                SceneManager.LoadScene("Menu");
            });
        }
        else
        {
            Show_Static_Ad();   
            Time.timeScale = 1f;
            SceneManager.LoadScene("Menu");
        }
    }

    private void TouchRestart()
    {
        lockGroup.gameObject.SetActive(true);
        if (AdmobManager.instance.isInterstititalAds_Avaiable())
        {
            lockGroup.gameObject.SetActive(true);
            AdmobManager.instance.ShowInterstitialAd_CallBack((AdmobManager.ADS_CALLBACK_STATE state) =>
            {
                GamePlayManager.instance.SetReplayGame();
            });
        }
        else
        {
            Show_Static_Ad();   
            Time.timeScale = 1f;
            GamePlayManager.instance.SetReplayGame();
        }
    }
}
