using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class WinPopup2 : MonoBehaviour
{
    public static WinPopup2 instance;
    
    public List<GameObject> listStars;
    public GameObject bgStars;
    
    [Header("Popup Reward")]
    public BBUIView popupReward;

    public Slider sliderReward;
    private int xReward = 2;
    
    
    public BBUIButton btnClaim;
    public Text txtClaimCoin;
    
    public BBUIButton btnClaimxReward;
    public Text txtClaimxRewardCoin;
    
    [Header("Popup Action")]
    public BBUIView popupAction;

    public BBUIButton btnNextLevel;
    public BBUIButton btnHome;

    public GameObject lockGroup;
    
    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        popupReward.ShowBehavior.onCallback_Completed.AddListener(PopupReward_ShowView_Finished);
        popupReward.HideBehavior.onCallback_Completed.AddListener(PopupReward_HideView_Finished);
        
        popupAction.ShowBehavior.onCallback_Completed.AddListener(PopupAction_ShowView_Finished);
        popupAction.HideBehavior.onCallback_Completed.AddListener(PopupAction_HideView_Finished);
        
        btnClaim.OnPointerClickCallBack_Completed.AddListener(TouchClaim);
        btnClaimxReward.OnPointerClickCallBack_Completed.AddListener(TouchClaimxReward);
        btnClaimxReward.OnPointerClickCallBack_Start.AddListener(TouchClaimxReward_Start);
        
        btnNextLevel.OnPointerClickCallBack_Completed.AddListener(TouchNextLevel);
        btnHome.OnPointerClickCallBack_Completed.AddListener(TouchHome);
        
    }


    private void OnDestroy()
    {
        popupReward.ShowBehavior.onCallback_Completed.RemoveAllListeners();
        popupReward.HideBehavior.onCallback_Completed.RemoveAllListeners();
        
        popupAction.ShowBehavior.onCallback_Completed.RemoveAllListeners();
        popupAction.HideBehavior.onCallback_Completed.RemoveAllListeners();
        
        btnClaim.OnPointerClickCallBack_Completed.RemoveAllListeners();
        btnClaimxReward.OnPointerClickCallBack_Completed.RemoveAllListeners();
        btnClaimxReward.OnPointerClickCallBack_Start.RemoveAllListeners();
        
        btnNextLevel.OnPointerClickCallBack_Completed.RemoveAllListeners();
        btnHome.OnPointerClickCallBack_Completed.RemoveAllListeners();
        
       // AdmobManager.instance.HideBannerAd();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    int countStar, coinValue;
    int addStar, level;
    
    public void ShowWinPopup(int _level, int _countStar, int _coinValue)
    {

        level = _level;
        countStar = _countStar;
        if(countStar == 3)
        {
            coinValue = 5;
        }
        if (countStar == 2)
        {
            coinValue = 3;
        }
        if (countStar == 1)
        {
            coinValue = 2;
        }
        Debug.Log("coinValue " + coinValue);
        xReward = 2;
        
        if (_level < 2)
        {
            //AdmobManager.instance.HideBannerAd();
        }
        else
        {
           // AdmobManager.instance.Request_Banner();
        }
        
        addStar = countStar - Config.LevelStar(Config.currSelectLevel);
        Config.SetLevelStar(_level, _countStar);
        Config.SetChestCountStar(Config.GetChestCountStar() + addStar);
        
        txtClaimCoin.text = $"+{coinValue}";
        txtClaimxRewardCoin.text = $"+{coinValue * xReward}";
        
        ShowViews();
    }

    private void ShowViews()
    {
        gameObject.SetActive(true);
        lockGroup.SetActive(true);
        popupReward.gameObject.SetActive(false);
        btnClaim.gameObject.SetActive(false);
        btnClaimxReward.gameObject.SetActive(false);
        popupAction.gameObject.SetActive(false);
        btnNextLevel.gameObject.SetActive(false);
        btnHome.gameObject.SetActive(false);
        // bgStars.SetActive(false);
        for (int i = 0; i < listStars.Count; i++)
        {
            listStars[i].gameObject.SetActive(false);
        }

        sliderReward.value = 0f;
        StartCoroutine(ShowViews_IEnumerator());

        TouchClaim();
    }

    private IEnumerator ShowViews_IEnumerator()
    {
        yield return new WaitForSeconds(0.1f);
        popupReward.gameObject.SetActive(true);
        popupReward.GetComponent<BBUIView>().ShowView();
        
        
        yield return new WaitForSeconds(0.4f);
        if (countStar >= 1)
        {
            SoundManager.instance.PlaySound_WinStarPop();
            listStars[0].gameObject.SetActive(true);
            listStars[0].GetComponent<BBUIView>().ShowView();
        }
         
        if (countStar >= 2)
        {
            yield return new WaitForSeconds(0.1f);
            SoundManager.instance.PlaySound_WinStarPop();
            listStars[1].gameObject.SetActive(true);
            listStars[1].GetComponent<BBUIView>().ShowView();
        }
        
        if (countStar >= 3)
        {
            yield return new WaitForSeconds(0.1f);
            SoundManager.instance.PlaySound_WinStarPop();
            listStars[2].gameObject.SetActive(true);
            listStars[2].GetComponent<BBUIView>().ShowView();
        }
        
        yield return new WaitForSeconds(0.1f);
        btnClaimxReward.gameObject.SetActive(true);
        btnClaimxReward.GetComponent<BBUIView>().ShowView();

        InitSlider_Reward();
        lockGroup.SetActive(false);
        
        yield return new WaitForSeconds(0.3f);
        btnClaim.gameObject.SetActive(true);
        btnClaim.GetComponent<BBUIView>().ShowView();
    }

    #region SLIDER REWARD

    private void InitSlider_Reward()
    {
        sliderReward.DOValue(1f, 1f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo).OnUpdate(() =>
            {
                Slider_UpdateValue();
            });
    }

    public void Show_Static_Ad() => Ad_Manager.instance.Show_Interstitial();

    private void Slider_UpdateValue()
    {
        if (sliderReward.value < 0.114f)
        {
            xReward = 2;
        }
        else if (sliderReward.value < 0.24f)
        {
            xReward = 3;
        }
        else if (sliderReward.value < 0.4f)
        {
            xReward = 4;
        }
        else if (sliderReward.value < 0.6f)
        {
            xReward = 5;
        }
        else if (sliderReward.value < 0.76f)
        {
            xReward = 4;
        }
        else if (sliderReward.value < 0.886f)
        {
            xReward = 3;
        }
        else
        {
            xReward = 2;
        }
        
        txtClaimxRewardCoin.text = $"+{coinValue * xReward}";
    }

    #endregion


    private void PopupReward_ShowView_Finished()
    {
        
    }
    private void PopupReward_HideView_Finished()
    {
        StartCoroutine(OpenPopup_Action());
    }
    
    private void PopupAction_ShowView_Finished()
    {
        
    }
    private void PopupAction_HideView_Finished()
    {
        
    }

    private void TouchClaim()
    {
        lockGroup.SetActive(true);
        Config.SetCoin(Config.currCoin + coinValue);
        DOTween.Kill(sliderReward);
        StartCoroutine(Claim_Finished());
    }

    private void TouchClaimxReward_Start()
    {
        lockGroup.SetActive(true);
        DOTween.Kill(sliderReward);
    }
    private void TouchClaimxReward()
    {
        lockGroup.SetActive(true);
        DOTween.Kill(sliderReward);
        if (AdmobManager.instance.isRewardAds_Avaiable())
        {
            AdmobManager.instance.ShowRewardAd_CallBack((AdmobManager.ADS_CALLBACK_STATE state) =>
            {
                if (state == AdmobManager.ADS_CALLBACK_STATE.SUCCESS)
                {
                    Config.SetCoin(Config.currCoin + coinValue * xReward);
                    StartCoroutine(Claim_Finished());
                }
                else
                {
                    lockGroup.gameObject.SetActive(false);
                    NotificationPopup.instance.AddNotification("Claim Reward Fail!");
                }
            });
        }
        else
        {
            lockGroup.gameObject.SetActive(false);
            NotificationPopup.instance.AddNotification("No Video Available!");
        }
    }


    private IEnumerator Claim_Finished()
    {
        yield return new WaitForSeconds(0.2f);
        popupReward.HideView();
    }

    private IEnumerator OpenPopup_Action()
    {
        lockGroup.SetActive(true);
        
        popupAction.gameObject.SetActive(true);
        popupAction.ShowView();
        
        yield return new WaitForSeconds(0.1f);
        btnNextLevel.gameObject.SetActive(true);
        btnNextLevel.GetComponent<BBUIView>().ShowView();
        lockGroup.SetActive(false);
        
        yield return new WaitForSeconds(2f);
        btnHome.gameObject.SetActive(true);
        btnHome.GetComponent<BBUIView>().ShowView();
        
        
    }


    private void TouchNextLevel()
    {
        lockGroup.gameObject.SetActive(true);
        if (AdmobManager.instance.isInterstititalAds_Avaiable())
        {
            lockGroup.gameObject.SetActive(true);
            AdmobManager.instance.ShowInterstitialAd_CallBack((AdmobManager.ADS_CALLBACK_STATE state) =>
            {
                
                GamePlayManager.instance.SetNextGame();
            });
        }
        else
        {
            Show_Static_Ad();   
            Time.timeScale = 1f;
            GamePlayManager.instance.SetNextGame();
        }
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
    
}
