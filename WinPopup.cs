using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;
public class WinPopup : MonoBehaviour
{
    public static WinPopup instance;

    [Header("Popup")]
    public GameObject popup;
    public BBUIButton btnReward, btnNext, btnChest;

    public Image objLight;
    public List<GameObject> listStars;
    public GameObject bgStars;

    public Text txtRewardCoin, txtBtnReward, txtBtnReward2;
    public GameObject lockGroup;
    public Image logo;
    //public NativeAdPanel nativeAdPanel;


    public Image bgPopup;

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        //btnxReward.OnPointerClickCallBack_Completed.AddListener(TouchXReward);
        btnChest.OnPointerClickCallBack_Completed.AddListener(TouchOpenChest);
        btnReward.OnPointerClickCallBack_Completed.AddListener(TouchXReward);
        btnNext.OnPointerClickCallBack_Completed.AddListener(TouchNext);
    }

    void OnEnable()
    {
       
    }

    public void BuyRemoveAd(ConfigPackData configPackData)
    {
        lockGroup.gameObject.SetActive(true);
        //#if UNITY_EDITOR
        //        btnBuy.Interactable = false;
        //        BuyStartPackSuccess();
        //        return;
        //#endif

        PurchaserManager.instance.BuyConsumable(configPackData.idPack, (string _iapID, PurchaserManager.IAP_CALLBACK_STATE _state) =>
        {
            if (_state == PurchaserManager.IAP_CALLBACK_STATE.SUCCESS)
            {
                lockGroup.gameObject.SetActive(false);
                if (_iapID.Equals(Config.IAP_ID.removead.ToString()))
                {
                    Config.SetRemoveAd();
                    NotificationPopup.instance.AddNotification("RemoveAd Success!");

                    itemShopRemoveAd.SetActive(false);
                }
            }
            else
            {
                lockGroup.gameObject.SetActive(false);
                NotificationPopup.instance.AddNotification("Buy Fail!");
            }
        });
    }

    public void TouchHome()
    {
        lockGroup.gameObject.SetActive(true);
        if (AdmobManager.instance.isInterstititalAds_Avaiable())
        {
            AdmobManager.instance.ShowInterstitialAd_CallBack((AdmobManager.ADS_CALLBACK_STATE state) =>
            {
                SceneManager.LoadScene("Menu");
            });
        }
        else {
            SceneManager.LoadScene("Menu");
        }
    }


    public void TouchNext()
    {
        if (Config.currLevel <= 10 && Config.interstitialAd_countWin % 3 == 0)
        {
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
                GamePlayManager.instance.SetNextGame();
            }
        }
        else if (Config.currLevel > 10 && Config.currLevel <= 20 && Config.interstitialAd_countWin % 2 == 0)
        {
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
                GamePlayManager.instance.SetNextGame();
            }
        }
        else if (Config.currLevel > 20)
        {
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
                GamePlayManager.instance.SetNextGame();
            }
        }
        else
        {
            GamePlayManager.instance.SetNextGame();
        }

    }

    public void TouchXReward()
    {
        if (AdmobManager.instance.isRewardAds_Avaiable())
        {
            lockGroup.gameObject.SetActive(true);
            AdmobManager.instance.ShowRewardAd_CallBack((AdmobManager.ADS_CALLBACK_STATE state) => {
                if (state == AdmobManager.ADS_CALLBACK_STATE.SUCCESS)
                {
                    lockGroup.gameObject.SetActive(false);
                    btnReward.Interactable = false;
                    xReward_Finished();
                }
                else
                {
                    lockGroup.gameObject.SetActive(false);
                }
            });
        }
        else
        {
            GamePlayManager.instance.SetNextGame();
        }

    }
    IEnumerator WaitToNextGame(float x)
    {

        yield return new WaitForSeconds(x);

        GamePlayManager.instance.SetNextGame();

    }
    public void xReward_Finished()
    {
        txtRewardCoin.text = $"{10 * coinValue}";
        txtRewardCoin.transform.DOPunchScale(new Vector3(0.2f, 0.2f, 0.2f), 2f, 5, 1).SetEase(Ease.InQuart);
        Config.SetCoin(Config.GetCoin() + 9 * coinValue);
        StartCoroutine(WaitToNextGame(1.5f));
    }

    public void TouchOpenChest()
    {
        /*lockGroup.gameObject.SetActive(true);
        AdmobManager.instance.ShowRewardAd_CallBack((AdmobManager.ADS_CALLBACK_STATE state) => {
            if (state == AdmobManager.ADS_CALLBACK_STATE.SUCCESS)
            {
                btnChest.GetComponent<Image>().color = new Color(200f / 255f, 200f / 255f, 200f / 255f, 1f);
                btnChest.Interactable = false;
                sequence_iconChestWatchVideo.Kill();
                iconChestWatchVideo.gameObject.SetActive(false);
                GamePlayManager.instance.OpenChestPopup(false);
            }
            else
            {
                lockGroup.gameObject.SetActive(false);
            }
        });*/

    }

    int countStar, coinValue;
    int addStar, thisLevel;

    public void ShowWinPopup(int _level, int _countStar, int _coinValue)
    {
        AdmobManager.instance.Request_Banner();

        if (_level < 2)
        {
            AdmobManager.instance.HideBannerAd();
        }

        SoundManager.instance.PlaySound_Win();
        thisLevel = _level;
        Config.interstitialAd_countWin++;
        Debug.Log("_countStar" + _countStar + "_" + Config.currSelectLevel + "_" + Config.LevelStar(Config.currSelectLevel));
        countStar = _countStar;
        coinValue = 4 * countStar;
        gameObject.SetActive(true);
        txtRewardCoin.text = $"{coinValue}";
        itemShopRemoveAd.GetComponent<ItemShopIAP>().InitIAP();

        Config.SetCoin(Config.GetCoin() + coinValue);
        addStar = countStar - Config.LevelStar(Config.currSelectLevel);
        if (addStar < 0) addStar = 0;
        Debug.Log("AddStar " + addStar);
        Config.SetLevelStar(_level, _countStar);
        InitViews();
    }
    [SerializeField] private Text txtOffer, txtOfferShadow;
    [SerializeField] private GameObject itemShopRemoveAd;
    private void InitViews()
    {
        lockGroup.gameObject.SetActive(false);
        popup.gameObject.SetActive(false);
        btnNext.gameObject.SetActive(false);
        btnReward.gameObject.SetActive(false);
        txtOffer.text = $"{10 * coinValue}";
        txtOfferShadow.text = $"{10 * coinValue}";

        for (int i = 0; i < listStars.Count; i++)
        {
            listStars[i].gameObject.SetActive(false);
        }
        //Debug.Log("AdmobManager.instance.nativeAd_iconTexture");
        //Debug.Log(AdmobManager.instance.nativeAd);

        //if (AdmobManager.instance.nativeAd != null)
        //{
        //    SetEnableNativeAd();
        //    logo.gameObject.SetActive(false);
        //    nativeAdPanel.SetInitNativeAd(AdmobManager.instance.nativeAd);
        //    FirebaseManager.instance.LogShowNative(Config.currLevel);
        //}
        //else
        //{
        //    SetDisableNativeAd();
        //    logo.gameObject.SetActive(true);
        //    nativeAdPanel.gameObject.SetActive(false);
        //}

        InitViews_ShowView();

        InitChestStar();
    }

    Sequence sequenceShowView;
    public void InitViews_ShowView()
    {
        if (sequenceShowView != null)
        {
            sequenceShowView.Kill();
        }
        sequenceShowView = DOTween.Sequence();
        sequenceShowView.InsertCallback(0.01f, () =>
        {
            popup.gameObject.SetActive(true);
            popup.GetComponent<BBUIView>().ShowView();
        });

        sequenceShowView.InsertCallback(0.4f, () =>
        {
            if (countStar >= 1)
            {
                SoundManager.instance.PlaySound_WinStarPop();
                listStars[0].gameObject.SetActive(true);
                listStars[0].GetComponent<BBUIView>().ShowView();
            }
        });

        sequenceShowView.InsertCallback(0.6f, () =>
        {
            if (countStar >= 2)
            {
                SoundManager.instance.PlaySound_WinStarPop();
                listStars[1].gameObject.SetActive(true);
                listStars[1].GetComponent<BBUIView>().ShowView();
            }
        });

        sequenceShowView.InsertCallback(0.8f, () =>
        {
            if (countStar >= 3)
            {
                SoundManager.instance.PlaySound_WinStarPop();
                listStars[2].gameObject.SetActive(true);
                listStars[2].GetComponent<BBUIView>().ShowView();
            }
        });




        sequenceShowView.InsertCallback(0.8f, () =>
        {
            SetRotateLight();
        });

        sequenceShowView.InsertCallback(1.5f, () =>
        {
            btnReward.gameObject.SetActive(true);
            btnReward.Interactable = true;
            if (!Config.GetRemoveAd())
            {
                if(thisLevel >= 4)
                    itemShopRemoveAd.gameObject.SetActive(true);
            }
            else
            {
                itemShopRemoveAd.gameObject.SetActive(false);
            }

            btnReward.GetComponent<BBUIView>().ShowView();
        });

        float showBtnNext = 3.5f;
        if(thisLevel == 1)
        {
            showBtnNext = 1.8f;
        }
        sequenceShowView.InsertCallback(showBtnNext, () =>
        {
            btnNext.gameObject.SetActive(true);
            btnNext.GetComponent<BBUIView>().ShowView();
        });

        sequenceShowView.InsertCallback(1.61f, () =>
        {
            if (thisLevel == 1)
            {
                GamePlayManager.instance.ShowTut_NextLevel(btnNext.transform.position);
            }
        });


        sequenceShowView.OnComplete(() => {
            ShowViewFinished();
        });
    }

    public void ShowViewFinished()
    {

    }

    private void SetRotateLight()
    {
        //Debug.Log("SetRotateLightSetRotateLight");
        objLight.transform.DORotate(new Vector3(0f, 0f, -90f), 1f).SetRelative(true).SetEase(Ease.Linear).SetLoops(-1, LoopType.Incremental);
    }



    #region CHEST
    [Header("Chest")]
    public Text txtChestCountStar;
    public Image imgChestProgress;
    //public Image iconChestWatchVideo;
    public List<Image> listChest_EfxStars = new List<Image>();

    public void InitChestStar()
    {
        txtChestCountStar.text = $"{Config.GetChestCountStar()}/{Config.CHEST_STAR_MAX}";
        imgChestProgress.fillAmount = Config.GetChestCountStar() * 1f / Config.CHEST_STAR_MAX;
        // iconChestWatchVideo.gameObject.SetActive(false);
        btnChest.Interactable = false;


        StartCoroutine(AddChestStar_Finished());
    }
    public void AddChestStar(int stars)
    {

        for (int i = 0; i < stars; i++)
        {
            listChest_EfxStars[i].gameObject.SetActive(true);
            listChest_EfxStars[i].GetComponent<RectTransform>().DOAnchorPos(new Vector2(276f, 8f), 0.5f).OnComplete(() => {
                listChest_EfxStars[i].gameObject.SetActive(false);
            });
            listChest_EfxStars[i].GetComponent<RectTransform>().DOScale(Vector3.one * 0.2f, 0.4f);
        }
    }

    public IEnumerator AddChestStar_Finished()
    {
        yield return new WaitForSeconds(1.5f);

        AddChestStar(addStar);
        yield return new WaitForSeconds(0.45f);
        Config.SetChestCountStar(Config.GetChestCountStar() + addStar);
        txtChestCountStar.text = $"{Config.GetChestCountStar()}/{Config.CHEST_STAR_MAX}";
        imgChestProgress.DOFillAmount(Config.GetChestCountStar() * 1f / Config.CHEST_STAR_MAX, 0.2f);
        //if (Config.GetChestCountStar() >= Config.CHEST_STAR_MAX)
        //{
        //    btnChest.GetComponent<Image>().color = new Color(200f / 255f, 200f / 255f, 200f / 255f, 1f);
        //    Config.SetChestCountStar(0);
        //    GamePlayManager.instance.OpenChestPopup(true);
        //    //AdmobManager.instance.HideBannerAd();
        //}
        //else
        //{
        //    btnChest.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
        //    if (AdmobManager.instance.isRewardAds_Avaiable())
        //    {
        //        btnChest.Interactable = true;
        //        ShowChestButtonWathcVideoAvaiable();
        //    }
        //    else
        //    {
        //        btnChest.Interactable = false;
        //        //iconChestWatchVideo.gameObject.SetActive(true);
        //    }

        //}
    }

    Sequence sequence_iconChestWatchVideo;
    public void ShowChestButtonWathcVideoAvaiable()
    {
        /*iconChestWatchVideo.gameObject.SetActive(true);
        Debug.Log("ShowSpinButtonAvaiable");
        if (sequence_iconChestWatchVideo != null)
        {
            sequence_iconChestWatchVideo.Kill();
        }
        sequence_iconChestWatchVideo = DOTween.Sequence();
        //sequence_iconChestWatchVideo.Insert(0f, iconChestWatchVideo.transform.DOPunchPosition(new Vector3(5f, 5f, 0.1f), Random.Range(1f, 2f), 5, 1).SetEase(Ease.InBounce));
        sequence_iconChestWatchVideo.Insert(0f, iconChestWatchVideo.transform.DOPunchRotation(new Vector3(0f, 0f, 10f), Random.Range(1f, 2f), 5, 1).SetEase(Ease.InOutBack));
        sequence_iconChestWatchVideo.Insert(0f, iconChestWatchVideo.transform.DOPunchScale(new Vector3(0.1f, 0.1f, 0.1f), Random.Range(1f, 2f), 5, 1).SetEase(Ease.InBounce));
        sequence_iconChestWatchVideo.Insert(2f, iconChestWatchVideo.transform.DOScale(1.1f, Random.Range(0.5f, 1f)).SetEase(Ease.Linear).SetLoops(Random.Range(2, 4), LoopType.Yoyo));
        sequence_iconChestWatchVideo.SetLoops(-1, LoopType.Yoyo);*/
    }
    #endregion


    public void SetDisableNativeAd()
    {
        bgPopup.raycastTarget = true;
    }

    public void SetEnableNativeAd()
    {
        bgPopup.raycastTarget = false;
    }


    private void OnDestroy()
    {
        //AdmobManager.instance.HideBannerAd();
    }
}