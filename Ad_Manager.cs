using EasyMobile;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
public class Ad_Manager : MonoBehaviour
{
    public static Ad_Manager instance;
    bool ad_watched , free_coin;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
            Destroy(this);
        
        free_coin = false;
    }
    void Start() => Load_Rewarded();
    public void Show_Banner() => Advertising.ShowBannerAd(BannerAdPosition.Bottom, BannerAdSize.SmartBanner);
    public void Hide_Banner() => Advertising.HideBannerAd();
    public void Destroy_Banner() => Advertising.DestroyBannerAd();
    public void Load_Rewarded() => Advertising.LoadRewardedAd();
    public void Show_Interstitial()
    {
        if (Advertising.IsInterstitialAdReady())
            Advertising.ShowInterstitialAd();
    }
    public void Show_Rewarded()
    {
        if (Advertising.IsRewardedAdReady())
            Advertising.ShowRewardedAd();
        else
            Load_Rewarded();
    }

    public void Show_VideoAd()
    {
        free_coin = true;
        Show_Rewarded();
    }

    void Update()
    {
        if (ad_watched)
            Skip_Level();
    }
    void Skip_Level()
    {
        print("Video Watched");

        if(free_coin)
        {
            Config.SetCoin(Config.currCoin + 10);
            free_coin = false;
        }
        if(LosePopup2.instance.is_continue)
        {
            LosePopup2.instance.TouchContinue_Video();
            LosePopup2.instance.is_continue = false;
        }
        
        ad_watched = false;
    }
    void Check_Rewarded_Ads()
    {
        ad_watched = true;
    }
    void RewardedAdCompletedHandler(RewardedAdNetwork network, AdPlacement location)
    {
        Check_Rewarded_Ads();
        Load_Rewarded();
    }
    void RewardedAdSkippedHandler(RewardedAdNetwork network, AdPlacement location)
    {
        // NativeUI.ShowToast("You Lost Your Reward!!!");
    }
    void OnEnable()
    {
        Advertising.RewardedAdCompleted += RewardedAdCompletedHandler;
        Advertising.RewardedAdSkipped += RewardedAdSkippedHandler;
    }
    void OnDisable()
    {
        Advertising.RewardedAdCompleted -= RewardedAdCompletedHandler;
        Advertising.RewardedAdSkipped -= RewardedAdSkippedHandler;
    }
}
