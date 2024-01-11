using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using bonbon;
public class SpinPopup : MonoBehaviour
{
    public static SpinPopup instance;

    public BBUIButton btnClose, btnWatchAds,btnCoin;
    public GameObject popup;
    public GameObject lockGroup;
    public GameObject spinObj;

    [Header("ConfigDatas")]
    public List<ConfigItemShopData> listDatas = new List<ConfigItemShopData>();
    [Header("Percent Random")]
    public List<float> listPercents = new List<float>();
    public List<ConfigItemShopData> listSpinDatas = new List<ConfigItemShopData>();


    [Header("LIST ITEM PREFAB")]
    public SpinItem spinItem_Coin_Prefab;
    public SpinItem spinItem_Undo_Prefab;
    public SpinItem spinItem_Suggest_Prefab;
    public SpinItem spinItem_Shuffle_Prefab;

    [Header("LIST POSITION ITEMS")]
    public List<Transform> listItemsPos = new List<Transform>();
    
    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        btnClose.OnPointerClickCallBack_Completed.AddListener(TouchClose);
        btnWatchAds.OnPointerClickCallBack_Completed.AddListener(TouchWatchAds);
        btnCoin.OnPointerClickCallBack_Completed.AddListener(TouchSpinCoin);

        popup.GetComponent<BBUIView>().HideBehavior.onCallback_Completed.AddListener(HidePopup_Finished);
    }

    // Update is called once per frame
    int rotateSoundIndex = 0;
    void Update()
    {
        if (isSpin) {
            int spinIndex = Mathf.FloorToInt(spinObj.GetComponent<RectTransform>().eulerAngles.z / 45f);
            if (rotateSoundIndex != spinIndex)
            {
                rotateSoundIndex = spinIndex;
                SoundManager.instance.PlaySound_Spin();
            }
        }
    }

    public void OpenSpinPopup()
    {
        gameObject.SetActive(true);
        InitViews();
    }

    public void TouchClose()
    {
        lockGroup.gameObject.SetActive(true);
        popup.GetComponent<BBUIView>().HideView();

    }


    private void TouchSpinCoin()
    {
        if (Config.currCoin >= Config.SPIN_PRICE)
        {
            Config.SetCoin(Config.currCoin - Config.SPIN_PRICE);
            WatchAds_Finished();
        }
        else
        {
            NotificationPopup.instance.AddNotification("Not enough Coin!");
            GamePlayManager.instance.OpenShopPopup();
        }
    }

    public void TouchWatchAds()
    {
        btnWatchAds.Interactable = false;
        lockGroup.gameObject.SetActive(true);
        AdmobManager.instance.ShowRewardAd_CallBack((AdmobManager.ADS_CALLBACK_STATE state) => {
            Debug.Log("SpinPopup TouchWatchAdsTouchWatchAds:"+state);
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
    int indexResult;
    bool isSpin = false;
    public void WatchAds_Finished()
    {
        //Spin
        indexResult = RandomExtension.RandomPercentage(listPercents.ToArray());
        //indexResult = 1;
        if (indexResult == 8) indexResult = 0;
        Debug.Log("indexResult"+indexResult);
        lockGroup.gameObject.SetActive(true);
        isSpin = true;
        spinObj.transform.DORotate(new Vector3(0f, 0f, -4 * 360 - (8-indexResult) * 45), 5f).SetEase(Ease.InOutExpo).OnComplete(() =>
        {
            isSpin = false;
            lockGroup.gameObject.SetActive(false);
            Debug.Log(listSpinDatas[indexResult].shopItemType);
            Debug.Log(listSpinDatas[indexResult].countItem);
            //Config.BuySucces_ItemShop(listSpinDatas[indexResult]);
            //GamePlayManager.instance.SetUpdate_CountItem();

            // gameObject.SetActive(false);
            Config.SetSpin_LastTime();
            GamePlayManager.instance.OpenRewardPopup(new List<ConfigItemShopData>() { listSpinDatas[indexResult] });
        });
    }

    public void HidePopup_Finished()
    {
        GamePlayManager.instance.CloseShopSucces();
        gameObject.SetActive(false);
    }

    public void InitViews()
    {
        SoundManager.instance.PlaySound_Popup();
        lockGroup.gameObject.SetActive(false);
        popup.gameObject.SetActive(false);
        btnWatchAds.gameObject.SetActive(false);
        btnCoin.gameObject.SetActive(false);
        Debug.Log("isRewardAds_Avaiable"+ AdmobManager.instance.isRewardAds_Avaiable());
        long currTimeLastTime = Config.GetSpinLastTime();
        if (currTimeLastTime + Config.TIME_SPIN < Config.GetTimeStamp())
        {
            btnWatchAds.Interactable = true;
        }
        else
        {
            btnWatchAds.Interactable = false;
        }
        
        btnClose.gameObject.SetActive(false);
        for (int i = 0; i < listItemsPos.Count; i++) {
            listItemsPos[i].gameObject.SetActive(false);
        }
        InitListItems();



        InitView_ShowView();
    }


    public void InitListItems() {
        for (int i = 0; i < listItemsPos.Count; i++) {
            foreach (Transform child in listItemsPos[i]) {
                Destroy(child.gameObject);
            }
        }


        List<ConfigItemShopData> listSpinDatas_Temp = new List<ConfigItemShopData>(listDatas);
        listSpinDatas.Clear();

        for (int i = 0; i < listDatas.Count; i++) {
            int index = Random.Range(0, listSpinDatas_Temp.Count);
            listSpinDatas.Add(listSpinDatas_Temp[index]);
            listSpinDatas_Temp.RemoveAt(index);
        }

        for (int i = 0; i < listItemsPos.Count; i++)
        {
            SpinItem spinItem = null;
            if (listSpinDatas[i].shopItemType == Config.SHOPITEM.COIN) {
                spinItem = Instantiate(spinItem_Coin_Prefab, listItemsPos[i]);
            }
            else if (listSpinDatas[i].shopItemType == Config.SHOPITEM.UNDO)
            {
                spinItem = Instantiate(spinItem_Undo_Prefab, listItemsPos[i]);
            }
            else if (listSpinDatas[i].shopItemType == Config.SHOPITEM.SUGGEST)
            {
                spinItem = Instantiate(spinItem_Suggest_Prefab, listItemsPos[i]);
            }
            else if (listSpinDatas[i].shopItemType == Config.SHOPITEM.SHUFFLE)
            {
                spinItem = Instantiate(spinItem_Shuffle_Prefab, listItemsPos[i]);
            }

            spinItem.InitSpinItem(listSpinDatas[i]);
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

        sequenceShowView.InsertCallback(0.5f, () =>
        {
            SoundManager.instance.PlaySound_WinStarPop();
            listItemsPos[0].gameObject.SetActive(true);
            listItemsPos[0].GetComponent<BBUIView>().ShowView();
        });

        sequenceShowView.InsertCallback(0.6f, () =>
        {
            SoundManager.instance.PlaySound_WinStarPop();
            listItemsPos[1].gameObject.SetActive(true);
            listItemsPos[1].GetComponent<BBUIView>().ShowView();
        });

        sequenceShowView.InsertCallback(0.7f, () =>
        {
            SoundManager.instance.PlaySound_WinStarPop();
            listItemsPos[2].gameObject.SetActive(true);
            listItemsPos[2].GetComponent<BBUIView>().ShowView();
        });

        sequenceShowView.InsertCallback(0.8f, () =>
        {
            SoundManager.instance.PlaySound_WinStarPop();
            listItemsPos[3].gameObject.SetActive(true);
            listItemsPos[3].GetComponent<BBUIView>().ShowView();
        });

        sequenceShowView.InsertCallback(0.9f, () =>
        {
            SoundManager.instance.PlaySound_WinStarPop();
            listItemsPos[4].gameObject.SetActive(true);
            listItemsPos[4].GetComponent<BBUIView>().ShowView();
        });

        sequenceShowView.InsertCallback(1f, () =>
        {
            SoundManager.instance.PlaySound_WinStarPop();
            listItemsPos[5].gameObject.SetActive(true);
            listItemsPos[5].GetComponent<BBUIView>().ShowView();
        });

        sequenceShowView.InsertCallback(1.1f, () =>
        {
            SoundManager.instance.PlaySound_WinStarPop();
            listItemsPos[6].gameObject.SetActive(true);
            listItemsPos[6].GetComponent<BBUIView>().ShowView();
        });

        sequenceShowView.InsertCallback(1.2f, () =>
        {
            SoundManager.instance.PlaySound_WinStarPop();
            listItemsPos[7].gameObject.SetActive(true);
            listItemsPos[7].GetComponent<BBUIView>().ShowView();
        });

        sequenceShowView.InsertCallback(1.3f, () =>
        {
            SoundManager.instance.PlaySound_WinStarPop();
            listItemsPos[8].gameObject.SetActive(true);
            listItemsPos[8].GetComponent<BBUIView>().ShowView();
        });


        sequenceShowView.InsertCallback(1.5f, () =>
        {
            btnWatchAds.gameObject.SetActive(false);
            btnWatchAds.gameObject.GetComponent<BBUIView>().ShowView();
        });
        
        sequenceShowView.InsertCallback(1.7f, () =>
        {
            btnCoin.gameObject.SetActive(false);
            btnCoin.gameObject.GetComponent<BBUIView>().ShowView();
        });
        
        sequenceShowView.InsertCallback(2f, () =>
        {
            btnClose.gameObject.SetActive(false);
            btnClose.gameObject.GetComponent<BBUIView>().ShowView();
        });
    }
}
