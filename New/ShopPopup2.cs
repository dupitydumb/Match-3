using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UI;

public class ShopPopup2 : MonoBehaviour
{
    public static ShopPopup2 instance;
    
    public BBUIView popup;

    public BBUIButton btnClose;
    public GameObject lockGroup;
    public ScrollRect scrollView;
    
    private void Awake()
    {
        instance = this;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        PurchaserManager.InitializeSucceeded += PurchaserManager_InitializeSucceeded;
        popup.ShowBehavior.onCallback_Completed.AddListener(ShowView_Finished);
        popup.HideBehavior.onCallback_Completed.AddListener(HideView_Finished);
        
        btnClose.OnPointerClickCallBack_Completed.AddListener(TouchClose);
        btnLoadMores.OnPointerClickCallBack_Completed.AddListener(TouchLoadMore);

        InitIAP();
    }

    private void OnDestroy()
    {
        PurchaserManager.InitializeSucceeded -= PurchaserManager_InitializeSucceeded;
        popup.ShowBehavior.onCallback_Completed.RemoveAllListeners();
        popup.HideBehavior.onCallback_Completed.RemoveAllListeners();
        
        btnClose.OnPointerClickCallBack_Completed.RemoveAllListeners();
        btnLoadMores.OnPointerClickCallBack_Completed.RemoveAllListeners();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [Button("OpenPopup")]
    public void OpenPopup()
    {
        lockGroup.SetActive(true);
        gameObject.SetActive(true);
        
        InitLoadMores();
        
        scrollView.verticalNormalizedPosition = 1f;
        ShowViews();
    }

    private void ShowViews()
    {
        SoundManager.instance.PlaySound_Popup();
        lockGroup.SetActive(true);

       

        StartCoroutine(ShowViews_IEnumerator());
    }

    private IEnumerator ShowViews_IEnumerator()
    {
        popup.gameObject.SetActive(false);
        btnClose.gameObject.SetActive(false);
        
        yield return new WaitForSeconds(0.1f);
        popup.gameObject.SetActive(true);
        popup.ShowView();
        
        yield return new WaitForSeconds(0.1f);
        btnClose.gameObject.SetActive(true);
        btnClose.GetComponent<BBUIView>().ShowView();
    }
    
    private void ShowView_Finished()
    {
        lockGroup.SetActive(false);
    }
    private void HideView_Finished()
    {
        gameObject.SetActive(false);
    }

    private void TouchClose()
    {
        lockGroup.SetActive(true);
        popup.HideView();
    }


    #region LOADMORES
    [Header("LOAD MORES")]
    public BBUIButton btnLoadMores;

    public GameObject loadMoreObj;
    public List<GameObject> listLoadMore_ItemShops = new List<GameObject>();

    private void TouchLoadMore()
    {
        for (int i = 0; i < listLoadMore_ItemShops.Count; i++)
        {
            listLoadMore_ItemShops[i].SetActive(true);
        }
        
        loadMoreObj.SetActive(false);
        InitIAP_RemoveAd();
    }

    private void InitLoadMores()
    {
        loadMoreObj.SetActive(true);
        for (int i = 0; i < listLoadMore_ItemShops.Count; i++)
        {
            listLoadMore_ItemShops[i].SetActive(false);
        }
    }
    
    
    
    #endregion

    #region REMOVE_AD

    public ItemShopIAP iapRemoveAd;

    private void InitIAP_RemoveAd()
    {
        if (Config.GetRemoveAd())
        {
            iapRemoveAd.gameObject.SetActive(false);
        }
        else
        {
            iapRemoveAd.gameObject.SetActive(true);
        }
    }
 
    #endregion


    
    

    #region IAP
    public List<ItemShopIAP> listItemShopIAPs = new List<ItemShopIAP>();
    public void TouchBuy_ShopItem(ConfigPackData configPackData) {
        Debug.Log("TouchBuy_ShopItem"+configPackData.idPack.ToString());
        lockGroup.gameObject.SetActive(true);

        PurchaserManager.instance.BuyConsumable(configPackData.idPack, (string _iapID, PurchaserManager.IAP_CALLBACK_STATE _state) =>
        {
            if (_state == PurchaserManager.IAP_CALLBACK_STATE.SUCCESS)
            {
                
                Debug.Log("SUCCESSSUCCESS"+_iapID);
                
                lockGroup.gameObject.SetActive(false);
                if (_iapID.Equals(Config.IAP_ID.removead.ToString()))
                {
                    Config.SetRemoveAd();
                    Config.SetBuyIAP(configPackData.idPack);
                    NotificationPopup.instance.AddNotification("RemoveAd Success!");

                    InitIAP_RemoveAd();
                }
                else
                {
                    BuyPackSuccess(configPackData);
                }
            }
            else
            {
                Debug.Log("Buy Fail!Buy Fail!Buy Fail!Buy Fail!Buy Fail!");
                lockGroup.gameObject.SetActive(false);
                NotificationPopup.instance.AddNotification("Buy Fail!");
            }
        });


    }

    private void BuyPackSuccess(ConfigPackData configPackData)
    {
        if (MenuManager.instance != null && MenuManager.instance.isActiveAndEnabled)
        {
            MenuManager.instance.OpenRewardPopup(configPackData.configItemShopDatas, false);
        }
        else if (GamePlayManager.instance != null && GamePlayManager.instance.isActiveAndEnabled)
        {
            GamePlayManager.instance.OpenRewardPopup(configPackData.configItemShopDatas, false);
        }
        
        gameObject.SetActive(false);
    }


    private void PurchaserManager_InitializeSucceeded() {
        InitIAP();
    }
    
    private void InitIAP() {
        for (int i = 0; i < listItemShopIAPs.Count; i++)
        {
            listItemShopIAPs[i].InitIAP();
        }

        
    }
    #endregion
}
