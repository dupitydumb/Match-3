using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using bonbon;
public class ChestPopup : MonoBehaviour
{
    public static ChestPopup instance;

    public BBUIButton btnClose, btnWatchAds;
    public GameObject popup;
    public GameObject lockGroup;
    public GameObject objLight;
    public GameObject objChest;
    public GameObject objChest2;
    
    public List<ConfigItemShopData> listDatas = new List<ConfigItemShopData>();

    [Header("LIST ITEM PREFAB")]
    public RewardItem rewardItem_Coin_Prefab;
    public RewardItem rewardItem_Undo_Prefab;
    public RewardItem rewardItem_Suggest_Prefab;
    public RewardItem rewardItem_Shuffle_Prefab;
    
    public Transform contentReward;

    [Header("FX")]
    public ParticleSystem fx1, fx2, fx3;
    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        btnClose.OnPointerClickCallBack_Completed.AddListener(TouchClose);
        btnWatchAds.OnPointerClickCallBack_Completed.AddListener(TouchWatchAds);

        popup.GetComponent<BBUIView>().HideBehavior.onCallback_Completed.AddListener(HidePopup_Finished);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenChestPopup(bool isFullStar)
    {
        ConfigItemShopData configItemShopData_Coin = new ConfigItemShopData();
        configItemShopData_Coin.shopItemType = Config.SHOPITEM.COIN;
        if (isFullStar)
        {
            configItemShopData_Coin.countItem = Random.Range(50, 100);
        }
        else {
            configItemShopData_Coin.countItem = Random.Range(50, 100);
        }
        configItemShopData_Coin.price = 0;
        listDatas.Add(configItemShopData_Coin);

        ConfigItemShopData configItemShopData_Undo = new ConfigItemShopData();
        configItemShopData_Undo.shopItemType = Config.SHOPITEM.UNDO;
        if (isFullStar)
        {
            configItemShopData_Undo.countItem = Random.Range(0, 2);
        }
        else {
            configItemShopData_Undo.countItem = Random.Range(0, 2);
        }
        configItemShopData_Undo.price = 0;
        if (configItemShopData_Undo.countItem > 0)
        {
            listDatas.Add(configItemShopData_Undo);
        }

        ConfigItemShopData configItemShopData_Suggest = new ConfigItemShopData();
        configItemShopData_Suggest.shopItemType = Config.SHOPITEM.SUGGEST;
        if (isFullStar)
        {
            configItemShopData_Suggest.countItem = Random.Range(1, 3);
        }
        else {
            configItemShopData_Suggest.countItem = Random.Range(0, 2);
        }
        configItemShopData_Suggest.price = 0;
        if (configItemShopData_Suggest.countItem > 0)
        {
            listDatas.Add(configItemShopData_Suggest);
        }

        ConfigItemShopData configItemShopData_Shuffle = new ConfigItemShopData();
        configItemShopData_Shuffle.shopItemType = Config.SHOPITEM.SHUFFLE;
        if (isFullStar)
        {
            configItemShopData_Shuffle.countItem = Random.Range(0, 2);
        }
        else {
            configItemShopData_Shuffle.countItem = Random.Range(0, 2);
        }
        configItemShopData_Shuffle.price = 0;
        if (configItemShopData_Shuffle.countItem > 0)
        {
            listDatas.Add(configItemShopData_Shuffle);
        }

        gameObject.SetActive(true);
        InitViews();
    }

    public void TouchClose()
    {
        /*SoundManager.instance.PlaySound_Cash();
        for (int i = 0; i < listDatas.Count; i++)
        {
            Config.BuySucces_ItemShop(listDatas[i]);
        }*/
        lockGroup.gameObject.SetActive(true);
        popup.GetComponent<BBUIView>().HideView();

    }

    public void TouchWatchAds()
    {
        lockGroup.gameObject.SetActive(true);
        AdmobManager.instance.ShowRewardAd_CallBack((AdmobManager.ADS_CALLBACK_STATE state) => {
            if (state == AdmobManager.ADS_CALLBACK_STATE.SUCCESS)
            {
                lockGroup.gameObject.SetActive(false);
                //WatchAds
                WatchAds_Finished();
            }
            else
            {
                lockGroup.gameObject.SetActive(false);
            }
        });
       
    }


    public void WatchAds_Finished()
    {
        SoundManager.instance.PlaySound_Cash();
        for (int i = 0; i < listDatas.Count; i++) {
            listDatas[i].countItem = listDatas[i].countItem;

            Config.BuySucces_ItemShop(listDatas[i]);
        }

        lockGroup.gameObject.SetActive(true);
        popup.GetComponent<BBUIView>().HideView();
        if (GamePlayManager.instance != null && GamePlayManager.instance.isActiveAndEnabled)
        {
            GamePlayManager.instance.SetUpdate_CountItem();
        }
    }


    public void InitViews()
    {
        SoundManager.instance.PlaySound_Popup();
        lockGroup.gameObject.SetActive(false);
        popup.gameObject.SetActive(false);
        btnWatchAds.gameObject.SetActive(false);
        if (AdmobManager.instance.isRewardAds_Avaiable())
        {
            btnWatchAds.Interactable = true;
        }
        else {
            btnWatchAds.Interactable = false;
        }
        btnClose.gameObject.SetActive(false);
        foreach (Transform child in contentReward) {
            Destroy(contentReward);
        }
        objLight.transform.localScale = Vector3.zero;
        objChest.transform.localScale = Vector3.zero;
        objChest2.transform.localScale = Vector3.zero;
        objChest2.GetComponent<RectTransform>().anchoredPosition = new Vector2(390f, -173f);
        InitListRewards();



        InitView_ShowView();
    }

    public void InitListRewards() {
        for (int i = 0; i < listDatas.Count; i++)
        {
            RewardItem rewardItem = null;
            if (listDatas[i].shopItemType == Config.SHOPITEM.COIN)
            {
                rewardItem = Instantiate(rewardItem_Coin_Prefab, contentReward);
            }
            else if (listDatas[i].shopItemType == Config.SHOPITEM.UNDO)
            {
                rewardItem = Instantiate(rewardItem_Undo_Prefab, contentReward);
            }
            else if (listDatas[i].shopItemType == Config.SHOPITEM.SUGGEST)
            {
                rewardItem = Instantiate(rewardItem_Suggest_Prefab, contentReward);
            }
            else if (listDatas[i].shopItemType == Config.SHOPITEM.SHUFFLE)
            {
                rewardItem = Instantiate(rewardItem_Shuffle_Prefab, contentReward);
            }

            rewardItem.InitSpinItem(listDatas[i]);
        }
    }

    public void InitView_ShowView()
    {
        float timeOpenChest = 2.5f;
        Sequence sequenceShowView = DOTween.Sequence();
        sequenceShowView.InsertCallback(0f, () =>
        {
            objChest2.gameObject.SetActive(true);
            objChest2.transform.localScale = new Vector3(0.35f, 0.35f, 0.35f);
            objChest2.GetComponent<RectTransform>().DOAnchorPos(new Vector2(0f, -338f),0.3f).SetEase(Ease.OutQuad);
            objChest2.transform.DOScale(Vector3.one,0.3f).SetEase(Ease.OutBack);
        });
        sequenceShowView.InsertCallback(0.5f, () =>
        {
            Debug.Log("AAAAAAAAAAAAA");
            //objChest2.transform.DOShakeRotation(0.5f, new Vector3(0, 0, 10f), 5, 3);
            objChest2.transform.DOPunchPosition(new Vector3(10f, 10f, 10f), 2f, 10, 3).SetEase(Ease.InQuart);
            objChest2.transform.DOPunchRotation(new Vector3(0f, 0f, 20f), 2f, 10, 3).SetEase(Ease.InQuart);
            //objChest2.transform.DOShakeRotation(0.5f, new Vector3(0, 0, 10f), 5, 3);
        });
        sequenceShowView.InsertCallback(0.1f + timeOpenChest, () =>
        {
            popup.gameObject.SetActive(true);
            popup.GetComponent<BBUIView>().ShowView();
        });
        sequenceShowView.InsertCallback(0.2f + timeOpenChest, () =>
        {
            objChest2.gameObject.SetActive(false);
            fx1.gameObject.SetActive(true);
            fx1.Play();

            fx2.gameObject.SetActive(true);
            fx2.Play();


            SoundManager.instance.PlaySound_PhaoGiay();
        });
        sequenceShowView.Insert(0.2f + timeOpenChest, objLight.transform.DOScale(1f, 0.3f).SetEase(Ease.OutBack));
        sequenceShowView.Insert(0.2f + timeOpenChest, objChest.transform.DOScale(1f, 0.3f).SetEase(Ease.OutBack));
        sequenceShowView.InsertCallback(0.6f + timeOpenChest, () =>
        {
            SetRotateLight();

            fx3.gameObject.SetActive(true);
            fx3.Play();
        });
        sequenceShowView.InsertCallback(0.6f + timeOpenChest, () =>
        {
            foreach (Transform child in contentReward)
            {
                child.GetComponent<RewardItem>().ShowView();
            }
        });
        sequenceShowView.InsertCallback(1f + timeOpenChest, () =>
        {
            btnWatchAds.gameObject.SetActive(false);
            btnWatchAds.gameObject.GetComponent<BBUIView>().ShowView();
        });
        sequenceShowView.InsertCallback(4f + timeOpenChest, () =>
        {
            btnClose.gameObject.SetActive(false);
            btnClose.gameObject.GetComponent<BBUIView>().ShowView();
        });
    }

    public void HidePopup_Finished()
    {
        if (GamePlayManager.instance != null && GamePlayManager.instance.isActiveAndEnabled)
        {
            GamePlayManager.instance.CloseChestPopup();
        }

        fx1.gameObject.SetActive(false);
        fx2.gameObject.SetActive(false);
        fx3.gameObject.SetActive(false);
        gameObject.SetActive(false);
        
        
    }

    private void SetRotateLight()
    {
        //Debug.Log("SetRotateLightSetRotateLight");
        objLight.transform.DORotate(new Vector3(0f, 0f, -90f), 1f).SetRelative(true).SetEase(Ease.Linear).SetLoops(-1, LoopType.Incremental);
    }
}
