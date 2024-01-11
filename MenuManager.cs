using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Sirenix.OdinInspector;
public class MenuManager : MonoBehaviour
{
    public static MenuManager instance;
    [Header("Canvas")]
    public CanvasScaler canvasScaler;
    public BBUIButton btnSetting, btnGift, btnShop;
    public BBUIButton btnPlay, btnParty;
    public BBUIButton btnLevel;
    public Text txtLevel;
    public Menu_CoinGroup coinGroup;
    public BBUIButton btnRemoveAd, btnChest;
    public Image logo;
    public GameObject lockGroup;
    private void Awake()
    {
        instance = this;

        // if (Config.CheckWideScreen())
        // {
        //     canvasScaler.matchWidthOrHeight = 0.5f;
        // }
    }
    // Start is called before the first frame update
    void Start()
    {

        btnSetting.OnPointerClickCallBack_Completed.AddListener(TouchSetting);
        btnGift.OnPointerClickCallBack_Completed.AddListener(TouchGift);
        btnShop.OnPointerClickCallBack_Completed.AddListener(TouchShop);
        btnPlay.OnPointerClickCallBack_Completed.AddListener(TouchPlay);
        btnParty.OnPointerClickCallBack_Completed.AddListener(TouchParty);
        btnLevel.OnPointerClickCallBack_Completed.AddListener(TouchLevel);

        txtLevel.text = $"Level:{Config.currLevel}";
        InitViews();

        // if (Config.isShowStarterPack)
        // {
        //     startPackPopup.ShowStartPack();
        //     Config.isShowStarterPack = false;
        // }
        AdmobManager.instance.Request_Banner();
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void TouchSetting()
    {
        OpenSettingPopup();
    }

    public void TouchGift()
    {
    }

    public void SetBuyStarterPackSuccess()
    {
        btnRemoveAd.gameObject.SetActive(!Config.GetRemoveAd());
        btnGift.gameObject.SetActive(!Config.GetBuyIAP(Config.IAP_ID.tileworld_starter_pack));
    }


    public void TouchShop()
    {
        shopPopup.OpenPopup();
    }

    public void OpenShopCoin()
    {
        shopPopup.OpenPopup();

    }

    public void TouchPlay()
    {
        //SceneManager.LoadScene("Play");
        HideView();
    }
    int countHack = 0;
    public void TouchParty()
    {

        countHack++;
        if (countHack == 10)
        {
            Config.isHackMode = true;
        }

    }

    public void TouchLevel()
    {
        OpenPopupLevel();
    }


    public void InitViews()
    {
        lockGroup.gameObject.SetActive(false);
        btnSetting.gameObject.SetActive(false);
        btnGift.gameObject.SetActive(false);
        btnShop.gameObject.SetActive(false);
        coinGroup.gameObject.SetActive(false);
        btnPlay.gameObject.SetActive(false);
        btnParty.gameObject.SetActive(false);
        btnLevel.gameObject.SetActive(false);
        btnChest.gameObject.SetActive(false);
        btnRemoveAd.gameObject.SetActive(false);
        logo.gameObject.SetActive(false);



        InitViews_ShowView();
    }

    public void InitViews_ShowView()
    {

        Sequence sequenceShowView = DOTween.Sequence();
        sequenceShowView.InsertCallback(0.1f, () =>
        {
            SoundManager.instance.PlaySound_ShowView();
            btnSetting.gameObject.SetActive(true);
            btnSetting.GetComponent<BBUIView>().ShowView();

            coinGroup.gameObject.SetActive(true);
            coinGroup.GetComponent<BBUIView>().ShowView();

        });

        sequenceShowView.InsertCallback(0.3f, () =>
        {
            btnShop.gameObject.SetActive(true);
            btnShop.GetComponent<BBUIView>().ShowView();

            btnChest.gameObject.SetActive(true);
            btnChest.GetComponent<BBUIView>().ShowView();
        });

        sequenceShowView.InsertCallback(1f, () =>
        {
            btnChest.GetComponent<StarChestButton>().SetAnimation();
        });

        sequenceShowView.InsertCallback(0.4f, () =>
        {
            if (!Config.GetBuyIAP(Config.IAP_ID.tileworld_starter_pack))
            {
                btnGift.gameObject.SetActive(true);
                btnGift.GetComponent<BBUIView>().ShowView();
            }


            if (!Config.GetRemoveAd())
            {
                btnRemoveAd.gameObject.SetActive(true);
                btnRemoveAd.GetComponent<BBUIView>().ShowView();
            }
        });


        sequenceShowView.InsertCallback(0.45f, () =>
        {
            logo.gameObject.SetActive(true);
            logo.GetComponent<BBUIView>().ShowView();
        });

        sequenceShowView.InsertCallback(0.5f, () =>
        {
            btnLevel.gameObject.SetActive(true);
            btnLevel.GetComponent<BBUIView>().ShowView();
        });

        sequenceShowView.InsertCallback(0.75f, () =>
        {
            btnPlay.gameObject.SetActive(true);
            btnPlay.GetComponent<BBUIView>().ShowView();
        });

        // sequenceShowView.InsertCallback(1f, () =>
        // {
        //     btnParty.gameObject.SetActive(true);
        //     btnParty.GetComponent<BBUIView>().ShowView();
        //
        // });

        sequenceShowView.InsertCallback(1f, () =>
        {
            //btnGift.transform.DOPunchPosition(new Vector3(5f, 5f, 0.1f), 2f, 5, 1).SetEase(Ease.InQuart).SetLoops(-1,LoopType.Yoyo);
            // Sequence sequence = DOTween.Sequence();
            // sequence.Insert(0f, btnGift.transform.DOScale(Vector3.one * 1.1f,0.4f).SetEase(Ease.Linear).SetLoops(5, LoopType.Yoyo));
            // sequence.Insert(2f,btnGift.transform.DOPunchRotation(new Vector3(0f, 0f, 20f), 1f, 6, 2).SetEase(Ease.OutQuart));
            // sequence.Insert(2f,btnGift.transform.DOPunchScale(new Vector3(0.1f, 0.1f,0.1f), 1f, 2, 1).SetEase(Ease.OutQuart));
            // //sequence.Insert(2f,btnGift.gameObject.GetComponent<RectTransform>().DOAnchorPosY(-10f, 1f).SetRelative(true).SetEase(Ease.Linear));
            // sequence.SetLoops(-1, LoopType.Yoyo);

            logo.gameObject.GetComponent<RectTransform>().DOAnchorPosY(-30f, 1f).SetRelative(true).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
        });
    }


    public void HideView()
    {
        SoundManager.instance.PlaySound_HideView();
        lockGroup.gameObject.SetActive(true);
        //Sequence sequenceShowView = DOTween.Sequence();
        //sequenceShowView.InsertCallback(0.01f, () =>
        //{
        //    btnSetting.GetComponent<BBUIView>().HideView();
        //    btnGift.GetComponent<BBUIView>().HideView();
        //    btnShop.GetComponent<BBUIView>().HideView();
        //    coinGroup.GetComponent<BBUIView>().HideView();
        //    btnPlay.GetComponent<BBUIView>().HideView();
        //    btnParty.GetComponent<BBUIView>().HideView();
        //    btnLevel.GetComponent<BBUIView>().HideView();
        //    logo.GetComponent<BBUIView>().HideView();
        //});

        //sequenceShowView.InsertCallback(0.3f, () =>
        //{
        //    SceneManager.LoadScene("Play");
        //});
        SceneManager.LoadSceneAsync("Play");
    }

    [Header("SHOP")]
    public ShopPopup2 shopPopup;

    [Header("SETTING")]
    public SettingPopup settingPopup;

    public void OpenSettingPopup()
    {
        settingPopup.OpenSettingPopup();
    }

    [Header("RatePopup")]
    public RatePopup ratePopup;
    public void OpenRatePopup()
    {
        ratePopup.OpenRatePopup();
    }






    #region LEVELS
    [Header("LEVELS")]
    public LevelPopup levelPopup;


    public void OpenPopupLevel()
    {
        Debug.Log("OpenPopupLevel");
        levelPopup.ShowLevelPopup();
    }


    public void SelectLevel(int _levelSelect)
    {
        Config.isSelectLevel = true;
        Config.currSelectLevel = _levelSelect;

        HideView();
    }
    #endregion


    #region REWARDPOPUP
    [Header("REWARD POPUP")]
    public RewardPopup rewardPopup;

    public void OpenRewardPopup(List<ConfigItemShopData> _listDatas, bool _isShowCollectx2 = true)
    {
        Config.gameState = Config.GAME_STATE.WIN;
        rewardPopup.OpenRewardPopup(_listDatas, _isShowCollectx2);
    }

    public void SetRewardPpopup_Finished()
    {

    }

    #endregion

    #region CHESTPOPUP
    [Header("CHEST POPUP")]
    public ChestPopup chestPopup;
    public void OpenChestPopup(bool isFullStar)
    {
        chestPopup.OpenChestPopup(isFullStar);
    }

    public void CloseChestPopup()
    {

    }

    #endregion

    [Button("Reset SCENE")]
    public void TouchResetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
