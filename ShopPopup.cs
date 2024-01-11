using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class ShopPopup : MonoBehaviour
{
    public static ShopPopup instance;

    public GameObject popup;
    public BBUIButton btnClose;
    public BBUIButton btnCoin, btnStore;
    public CoinGroup coinGroup;
    public GameObject lockGroup;


    public ShopIAPGroup shopIAPGroup;
    public ShopItemGroup shopItemGroup;

    public Image imageItem, imageIAP;

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        PurchaserManager.InitializeSucceeded += PurchaserManager_InitializeSucceeded;
        btnClose.OnPointerClickCallBack_Completed.AddListener(TouchClose);
        btnCoin.OnPointerClickCallBack_Completed.AddListener(TouchCoin);
        btnStore.OnPointerClickCallBack_Completed.AddListener(TouchStore);

        popup.GetComponent<BBUIView>().HideBehavior.onCallback_Completed.AddListener(PopupHideView_Finished);

    }

    private void OnDestroy()
    {
        PurchaserManager.InitializeSucceeded -= PurchaserManager_InitializeSucceeded;
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void TouchClose() {
        lockGroup.gameObject.SetActive(true);
        popup.GetComponent<BBUIView>().HideView();
    }

    public void TouchCoin()
    {
        OpenShopItem();
    }

    public void TouchStore()
    {
        OpenShopIAP();
    }

    public void PopupHideView_Finished() {
        gameObject.SetActive(false);
    }

    public void OpenShopPopup(bool isShopIAP) {
        gameObject.SetActive(true);
        InitViews(isShopIAP);
    }

    public void InitViews(bool isShopIAP) {
        SoundManager.instance.PlaySound_Popup();
        lockGroup.gameObject.SetActive(false);

        popup.gameObject.SetActive(false);
        btnClose.gameObject.SetActive(false);
        btnCoin.gameObject.SetActive(false);
        btnStore.gameObject.SetActive(false);

        InitViews_ShowView(isShopIAP);
    }

    public void InitViews_ShowView(bool isShopIAP) {
        Sequence sequenceShowView = DOTween.Sequence();
        sequenceShowView.InsertCallback(0.01f, () =>
        {
            popup.gameObject.SetActive(true);
            popup.GetComponent<BBUIView>().ShowView();
        });

        //sequenceShowView.InsertCallback(0.5f, () =>
        //{
        //    btnCoin.gameObject.SetActive(true);
        //    btnCoin.GetComponent<BBUIView>().ShowView();
        //});

        sequenceShowView.InsertCallback(0.5f, () =>
        {
            if (isShopIAP) {
                OpenShopIAP();
            } else{
                OpenShopItem();
            }
        });

        //sequenceShowView.InsertCallback(0.8f, () =>
        //{
        //    btnStore.gameObject.SetActive(true);
        //    btnStore.GetComponent<BBUIView>().ShowView();
        //});

        sequenceShowView.InsertCallback(0.8f, () =>
        {
            btnClose.gameObject.SetActive(true);
            btnClose.GetComponent<BBUIView>().ShowView();
        });
    }


    public void TouchBuy_ShopItemCoin(ConfigPackData configPackData) {
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

                    InitIAP();
                }
                else if (_iapID.Equals(Config.IAP_ID.removead_and_combo10.ToString()))
                {
                    Config.SetRemoveAd();
                    NotificationPopup.instance.AddNotification("RemoveAd Success!");
                    BuyStartPackSuccess(configPackData);

                    InitIAP();
                }
                else
                {
                    BuyStartPackSuccess(configPackData);
                }
            }
            else
            {
                lockGroup.gameObject.SetActive(false);
                NotificationPopup.instance.AddNotification("Buy Fail!");
            }
        });


    }

    public void BuyStartPackSuccess(ConfigPackData configPackData)
    {
        MenuManager.instance.OpenRewardPopup(configPackData.configItemShopDatas, false);
        gameObject.SetActive(false);
    }


    public void PurchaserManager_InitializeSucceeded() {
        InitIAP();
    }

    public void InitIAP() {
        shopIAPGroup.InitShopIAP();
    }


    public void OpenShopItem(){
        Debug.Log("OpenShopItemOpenShopItem");

        shopIAPGroup.gameObject.SetActive(false);
        shopItemGroup.gameObject.SetActive(true);

        imageItem.gameObject.SetActive(true);
        imageIAP.gameObject.SetActive(false);

        btnCoin.gameObject.SetActive(false);
        btnStore.gameObject.SetActive(true);
    }

    public void OpenShopIAP() {
        Debug.Log("OpenShopIAPOpenShopIAP");
        shopIAPGroup.gameObject.SetActive(true);
        shopItemGroup.gameObject.SetActive(false);

        imageItem.gameObject.SetActive(false);
        imageIAP.gameObject.SetActive(true);

        btnCoin.gameObject.SetActive(true);
        btnStore.gameObject.SetActive(false);

        InitIAP();
    }
}
