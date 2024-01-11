//using GoogleMobileAds.Api;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Events;
using UnityEngine.UI;
using System;

public class AdmobManager : MonoBehaviour
{
    public static AdmobManager instance;
    [Header("COnfig")]
    [Header("Interstitial ADS")]
    private string InterstitialAd_Android_ID = "ca-app-pub-9179752697212712/4369945140";
    private string InterstitialAd_IOS_ID = "ca-app-pub-9179752697212712/6636601519";
    [Header("Reward ADS")]
    private string RewardedAd_Android_ID = "ca-app-pub-9179752697212712/2219436281";
    private string RewardedAd_IOS_ID = "ca-app-pub-9179752697212712/9070181442";
    [Header("Native ADS")]
    private string NativeAd_Android_ID = "ca-app-pub-9179752697212712/4654027937";
    private string NativeAd_IOS_ID = "ca-app-pub-9179752697212712/9620984391";

    [Header("Banner ADS")]
    private string Banner_Android_ID = "ca-app-pub-9179752697212712/1763548784";
    private string Banner_IOS_ID = "ca-app-pub-9179752697212712/1384274835";

    private bool isRewardAd_Loaded = false;



    public enum ADS_CALLBACK_STATE
    {
        SUCCESS,
        FAIL
    }



    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this);
    }
    // Start is called before the first frame update
    void Start()
    {
    }



    #region INTERSTITIAL ADS
    public const int TIME_SHOWINTERTITIAL_NOT_SHOWINTERTITIAL = 45;
    public long timeLastShowIntertitial = 0;
    public void Init_InterstitialAd()
    {
    }
    public void RequestAndLoadInterstitialAd()
    {
        Init_InterstitialAd();
    }
    private void ShowInterstitialAd()
    {

    }

    public void DestroyInterstitialAd()
    {
    }


    public void HandleOnAdLoaded_InterstitialAd(object sender, EventArgs args)
    {
        Debug.Log("HandleOnAdLoaded_InterstitialAd event received");

    }


    public void HandleOnAdOpened_InterstitialAd(object sender, EventArgs args)
    {
        Debug.Log("HandleOnAdOpened_InterstitialAd event received");

    }

    public void HandleOnAdClosed_InterstitialAd(object sender, EventArgs args)
    {
        timeLastShowIntertitial = Config.GetTimeStamp();
        Debug.Log("HandleOnAdClosed_InterstitialAd event received");
        InterstitialAd_CallBack.Invoke(ADS_CALLBACK_STATE.SUCCESS);
        RequestAndLoadInterstitialAd();
    }

    public void HandleOnAdLeavingApplication_InterstitialAd(object sender, EventArgs args)
    {
        Debug.Log("HandleOnAdLeavingApplication_InterstitialAd event received");
    }


    public bool isInterstititalAds_Avaiable()
    {
        if (Config.GetRemoveAd()) return false;
        if (Config.GetTimeStamp() - timeLastShowReward <= TIME_SHOWREWARD_NOT_SHOWINTERTITIAL)
        {
            Debug.Log("NOT Show    InterstitialAd");
            return false;
        }
        Debug.Log("isInterstititalAds_AvaiableisInterstititalAds_AvaiableisInterstititalAds_Avaiable");
        Debug.Log(Config.GetTimeStamp());
        Debug.Log(timeLastShowIntertitial);
        Debug.Log(Config.GetTimeStamp() - timeLastShowIntertitial);
        if (Config.GetTimeStamp() - timeLastShowIntertitial <= TIME_SHOWINTERTITIAL_NOT_SHOWINTERTITIAL)
        {
            Debug.Log("NOT Show AAAAAAAAAAAAAAAAAA  InterstitialAd");
            return true;
        }
        return false;
    }

    private Action<ADS_CALLBACK_STATE> InterstitialAd_CallBack = delegate (ADS_CALLBACK_STATE _state) { };
    public void ShowInterstitialAd_CallBack(Action<ADS_CALLBACK_STATE> _interstitialAd_CallBack)
    {
        InterstitialAd_CallBack = _interstitialAd_CallBack;
        ShowInterstitialAd();
        // #if UNITY_EDITOR
        //         InterstitialAd_CallBack.Invoke(ADS_CALLBACK_STATE.SUCCESS);
        // #endif
    }
    #endregion


    #region REWARDED ADS
    public const int TIME_SHOWREWARD_NOT_SHOWINTERTITIAL = 30;
    public long timeLastShowReward = 0;

    public void InitLoadRewardedAd()
    {
    }
    public void RequestAndLoadRewardedAd()
    {

    }

    private void ShowRewardedAd()
    {
    }


    public void HandleRewardedAdLoaded(object sender, EventArgs args)
    {
        Debug.Log("HandleRewardedAdLoaded event received");
        isRewardAd_Loaded = true;
    }

    public void HandleRewardedAdOpening(object sender, EventArgs args)
    {
        Debug.Log("HandleRewardedAdOpening event received");
    }

    public void HandleRewardedAdClosed(object sender, EventArgs args)
    {
        Debug.Log("HandleRewardedAdClosed event received");

        // RewardAd_CallBack.Invoke(ADS_CALLBACK_STATE.FAIL);
        RequestAndLoadRewardedAd();
    }

    public void HandleUserEarnedReward(object sender)
    {
        timeLastShowReward = Config.GetTimeStamp();
        RewardAd_CallBack.Invoke(ADS_CALLBACK_STATE.SUCCESS);
        //RequestAndLoadRewardedAd();
    }

    private Action<ADS_CALLBACK_STATE> RewardAd_CallBack = delegate (ADS_CALLBACK_STATE _state) { };
    public void ShowRewardAd_CallBack(Action<ADS_CALLBACK_STATE> _rewardAd_CallBack, string where = "", int level = 0)
    {
        RewardAd_CallBack = _rewardAd_CallBack;
        RewardAd_CallBack.Invoke(ADS_CALLBACK_STATE.SUCCESS);
    }

    public bool isRewardAds_Avaiable()
    {
        return true;
    }

    #endregion


    #region BANNER_ADS
    public void Init_Banner()
    {
    }

    public void Request_Banner()
    {
    }


    public void HandleOnAdLoaded_BanenrAd(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdLoaded event received");

    }


    public void HandleOnAdOpened_BanenrAd(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdOpened event received");
    }

    public void HandleOnAdClosed_BanenrAd(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdClosed event received");
    }

    public void HandleOnAdLeavingApplication_BanenrAd(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdLeavingApplication event received");
    }

    public void ShowBannerAd()
    {
    }

    public void HideBannerAd()
    {
    }
    #endregion
}
