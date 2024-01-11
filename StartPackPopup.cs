using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartPackPopup : MonoBehaviour
{
    public ConfigPackData configPackData;
    public List<ItemPack> listItemPack;
    public static StartPackPopup instance;
    public BBUIButton btnClose, btnBuy;
    public GameObject popup;
    public Text txtPrice;
    public GameObject lockGroup;
    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        PurchaserManager.InitializeSucceeded += PurchaserManager_InitializeSucceeded;
        InitIAP();
        // InitPacks();
        btnClose.OnPointerClickCallBack_Completed.AddListener(TouchClose);
        btnBuy.OnPointerClickCallBack_Completed.AddListener(TouchBuy);
        Debug.Log("Config.IAP_ID.starter_pack.ToString()): " + Config.IAP_ID.tileworld_starter_pack.ToString());
        popup.GetComponent<BBUIView>().HideBehavior.onCallback_Completed.AddListener(HidePopup_Finished);
    }


    private void OnDestroy()
    {
        PurchaserManager.InitializeSucceeded -= PurchaserManager_InitializeSucceeded;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void PurchaserManager_InitializeSucceeded() {
        InitIAP();
    }


    public void InitIAP() {
    }

    public void InitPacks() {
        for (int i = 0; i < configPackData.configItemShopDatas.Count; i++) {
            listItemPack[i].SetItemPack(configPackData.configItemShopDatas[i]);
        }
    }


    public void ShowStartPack() {
        gameObject.SetActive(true);
        InitViews();
    }

    public void TouchClose()
    {
        lockGroup.gameObject.SetActive(true);
        popup.GetComponent<BBUIView>().HideView();
    }

    public void TouchBuy() {
        
        lockGroup.gameObject.SetActive(true);
#if UNITY_EDITOR
        btnBuy.Interactable = false;
        BuyStartPackSuccess();
        return;
#endif
        PurchaserManager.instance.BuyConsumable(Config.IAP_ID.tileworld_starter_pack, (string _iapID, PurchaserManager.IAP_CALLBACK_STATE _state) =>
        {
            if (_state == PurchaserManager.IAP_CALLBACK_STATE.SUCCESS)
            {
                lockGroup.gameObject.SetActive(false);
                if (_iapID.Equals(Config.IAP_ID.tileworld_starter_pack.ToString()))
                {
                    //Buy
                    btnBuy.Interactable = false;
                    BuyStartPackSuccess();
                }
            }
            else
            {
                lockGroup.gameObject.SetActive(false);
                NotificationPopup.instance.AddNotification("Buy Fail!");
            }
        });
    }

    public void BuyStartPackSuccess() {
        //for (int i = 0; i < configPackData.configItemShopDatas.Count; i++)
        //{
        //    Config.BuySucces_ItemShop(configPackData.configItemShopDatas[i]);
        //}

        //popup.GetComponent<BBUIView>().HideView();
        Config.SetRemoveAd();
        Config.SetBuyIAP(configPackData.idPack);
        NotificationPopup.instance.AddNotification("Buy Success!");
        
        if (MenuManager.instance != null && MenuManager.instance.isActiveAndEnabled)
        {
            MenuManager.instance.SetBuyStarterPackSuccess();
            MenuManager.instance.OpenRewardPopup(configPackData.configItemShopDatas, false);
        }
        gameObject.SetActive(false);
    }

    public void HidePopup_Finished() {
        gameObject.SetActive(false);
    }

    public void InitViews() {
        SoundManager.instance.PlaySound_Popup();
        lockGroup.gameObject.SetActive(false);
        popup.gameObject.SetActive(false);
        btnBuy.gameObject.SetActive(false);
        btnClose.gameObject.SetActive(false);
        
        for (int i = 0; i < listItemPack.Count; i++)
        {
            listItemPack[i].itemPackView.SetActive(false);
        }

        InitView_ShowView();
    }

    public void InitView_ShowView() {
        Sequence sequenceShowView = DOTween.Sequence();
        sequenceShowView.InsertCallback(0.01f, () =>
        {
            popup.gameObject.SetActive(true);
            popup.GetComponent<BBUIView>().ShowView();
        });
        sequenceShowView.InsertCallback(0.2f, () =>
        {
            btnBuy.gameObject.SetActive(false);
            btnBuy.gameObject.GetComponent<BBUIView>().ShowView();
        });
        
        sequenceShowView.InsertCallback(1f, () =>
        {
            btnClose.gameObject.SetActive(false);
            btnClose.gameObject.GetComponent<BBUIView>().ShowView();
        });
    }
}
