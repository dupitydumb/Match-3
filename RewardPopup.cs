using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using bonbon;
public class RewardPopup : MonoBehaviour
{
    public static RewardPopup instance;

    public BBUIButton btnClose;// btnWatchAds;
    public GameObject popup;
    public GameObject lockGroup;
    public GameObject objLight;
    public Text txtCollect;
    public List<ConfigItemShopData> listDatas = new List<ConfigItemShopData>();

    [Header("LIST ITEM PREFAB")]
    public RewardItem rewardItem_Coin_Prefab;
    public RewardItem rewardItem_Undo_Prefab;
    public RewardItem rewardItem_Suggest_Prefab;
    public RewardItem rewardItem_Shuffle_Prefab;



    public Transform contentReward;
    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        btnClose.OnPointerClickCallBack_Completed.AddListener(TouchClose);
        // btnWatchAds.OnPointerClickCallBack_Completed.AddListener(TouchWatchAds);

        popup.GetComponent<BBUIView>().HideBehavior.onCallback_Completed.AddListener(HidePopup_Finished);
    }

    // Update is called once per frame
    void Update()
    {

    }

    bool isShowCollectx2 = true;
    public void OpenRewardPopup(List<ConfigItemShopData> _listDatas, bool _isShowCollectx2 = true)
    {
        listDatas = _listDatas;

        gameObject.SetActive(true);
        isShowCollectx2 = _isShowCollectx2;
        InitViews();
    }

    public void TouchClose()
    {
        SoundManager.instance.PlaySound_Cash();
        for (int i = 0; i < listDatas.Count; i++)
        {
            Config.BuySucces_ItemShop(listDatas[i]);
        }
        lockGroup.gameObject.SetActive(true);
        if (GamePlayManager.instance != null)
        {
            GamePlayManager.instance.SetUpdate_CountItem();
        }
        StartCoroutine(TouchClose_IEnumerator());
    }


    public IEnumerator TouchClose_IEnumerator(){
        yield return new WaitForSeconds(.1f);
        popup.GetComponent<BBUIView>().HideView();
    }

    public void TouchWatchAds()
    {
        // btnWatchAds.Interactable = false;
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
        for (int i = 0; i < listDatas.Count; i++)
        {
            listDatas[i].countItem = listDatas[i].countItem * 2;

            Config.BuySucces_ItemShop(listDatas[i]);
        }

        lockGroup.gameObject.SetActive(true);
        popup.GetComponent<BBUIView>().HideView();
        if (GamePlayManager.instance != null)
        {
            GamePlayManager.instance.SetUpdate_CountItem();
        }
    }


    public void InitViews()
    {
        //SoundManager.instance.PlaySound_Popup();
        SoundManager.instance.PlaySound_Reward();
        lockGroup.gameObject.SetActive(false);
        popup.gameObject.SetActive(false);
        // if (isShowCollectx2)
        // {
        //     btnWatchAds.gameObject.SetActive(false);
        //     if (AdmobManager.instance.isRewardAds_Avaiable())
        //     {
        //         btnWatchAds.Interactable = true;
        //     }
        //     else {
        //         btnWatchAds.Interactable = false;
        //     }
        //
        //     txtCollect.color = new Color(156f/255f, 156f / 255f, 156f / 255f,1f);
        // }
        // else {
        //     btnWatchAds.gameObject.SetActive(false);
        //     txtCollect.color = new Color(1f,1f,1f, 1f);
        // }
        btnClose.gameObject.SetActive(false);
        foreach (Transform child in contentReward)
        {
            Destroy(child.gameObject);
        }
        objLight.transform.localScale = Vector3.zero;
        InitListRewards();



        InitView_ShowView();
    }

    public void InitListRewards()
    {
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
        Sequence sequenceShowView = DOTween.Sequence();
        sequenceShowView.InsertCallback(0.1f, () =>
        {
            popup.gameObject.SetActive(true);
            popup.GetComponent<BBUIView>().ShowView();
        });
        sequenceShowView.Insert(0.2f, objLight.transform.DOScale(1f, 0.3f).SetEase(Ease.OutBack));
        sequenceShowView.InsertCallback(0.6f, () =>
        {
            SetRotateLight();
        });
        sequenceShowView.InsertCallback(0.6f, () =>
        {
            foreach (Transform child in contentReward)
            {
                SoundManager.instance.PlaySound_WinStarPop();
                child.GetComponent<RewardItem>().ShowView();
            }
        });
        // sequenceShowView.InsertCallback(1f, () =>
        // {
        //     if (isShowCollectx2)
        //     {
        //         btnWatchAds.gameObject.SetActive(true);
        //         btnWatchAds.gameObject.GetComponent<BBUIView>().ShowView();
        //     }
        // });
        sequenceShowView.InsertCallback(1f, () =>
        {
            btnClose.gameObject.SetActive(false);
            btnClose.gameObject.GetComponent<BBUIView>().ShowView();
        });
    }

    public void HidePopup_Finished()
    {
        if (MenuManager.instance != null) {
            MenuManager.instance.SetRewardPpopup_Finished();
        }

        else if (GamePlayManager.instance != null)
        {
            GamePlayManager.instance.SetRewardPpopup_Finished();
        }
        gameObject.SetActive(false);
    }

    private void SetRotateLight()
    {
        //Debug.Log("SetRotateLightSetRotateLight");
        objLight.transform.DORotate(new Vector3(0f, 0f, -90f), 1f).SetRelative(true).SetEase(Ease.Linear).SetLoops(-1, LoopType.Incremental);
    }
}
