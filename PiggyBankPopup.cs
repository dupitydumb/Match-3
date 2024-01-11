using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using bonbon;
public class PiggyBankPopup : MonoBehaviour
{
    public static PiggyBankPopup instance;

    public BBUIButton btnClose, btnBuy;
    public GameObject popup;
    public GameObject lockGroup;
    public Text txtMinBankCoin;
    public Text txtMaxBankCoin;
    public Text txtCurrBankCoin;
    public Text txtPrice;
    public Image imgBankCoinProgress;
    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        btnClose.OnPointerClickCallBack_Completed.AddListener(TouchClose);
        btnBuy.OnPointerClickCallBack_Completed.AddListener(TouchBuy);

        popup.GetComponent<BBUIView>().HideBehavior.onCallback_Completed.AddListener(HidePopup_Finished);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenPiggyBank()
    {
        gameObject.SetActive(true);
        InitViews();
    }

    public void TouchClose()
    {
        lockGroup.gameObject.SetActive(true);
        popup.GetComponent<BBUIView>().HideView();

    }

    public void TouchBuy()
    {

        //WatchAds
        //BuyPiggyBankCoin_Success();
        lockGroup.gameObject.SetActive(true);
        PurchaserManager.instance.BuyConsumable(Config.IAP_ID.piggy_bank, (string _iapID, PurchaserManager.IAP_CALLBACK_STATE _state) =>
        {
            if (_state == PurchaserManager.IAP_CALLBACK_STATE.SUCCESS)
            {
                lockGroup.gameObject.SetActive(false);
                if (_iapID.Equals(Config.IAP_ID.piggy_bank.ToString())) {
                    BuyPiggyBankCoin_Success();
                }
            }
            else {
                lockGroup.gameObject.SetActive(false);
                NotificationPopup.instance.AddNotification("Buy Fail!");
            }
        });
    }


    public void BuyPiggyBankCoin_Success()
    {
        //Config.SetCoin(Config.currCoin + Config.currPiggyBankCoin * Config.PIGGYBANK_COIN_xVALUE);
        ConfigItemShopData configItemShopData = new ConfigItemShopData();
        configItemShopData.countItem = Config.currPiggyBankCoin * Config.PIGGYBANK_COIN_xVALUE;
        configItemShopData.shopItemType = Config.SHOPITEM.COIN;
        configItemShopData.price = 0;
        GamePlayManager.instance.OpenRewardPopup(new List<ConfigItemShopData>() { configItemShopData },false);

        Config.SetPiggyBank(0);
        gameObject.SetActive(false);
    }


    public void InitViews()
    {
        SoundManager.instance.PlaySound_Popup();
        lockGroup.gameObject.SetActive(false);
        popup.gameObject.SetActive(false);
        btnBuy.gameObject.SetActive(false);
        txtPrice.text = PurchaserManager.instance.GetLocalizedPriceString(Config.IAP_ID.piggy_bank.ToString());
        if (Config.currPiggyBankCoin >= Config.PIGGYBANK_COIN_MIN)
        {
            btnBuy.Interactable = true;
        }
        else {
            btnBuy.Interactable = false;
        }
        btnClose.gameObject.SetActive(false);
        txtMaxBankCoin.text = $"{Config.PIGGYBANK_COIN_MAX}";
        txtMinBankCoin.text = $"{Config.PIGGYBANK_COIN_MIN}";
        txtCurrBankCoin.text = $"{Config.currPiggyBankCoin}";
        imgBankCoinProgress.fillAmount = 0;
        imgBankCoinProgress.DOFillAmount((Config.currPiggyBankCoin * 1f) / Config.PIGGYBANK_COIN_MAX, 0.2f).SetEase(Ease.OutQuad);
        InitView_ShowView();
    }

    public void InitView_ShowView()
    {
        Sequence sequenceShowView = DOTween.Sequence();
        sequenceShowView.InsertCallback(0.01f, () =>
        {
            popup.gameObject.SetActive(true);
            popup.GetComponent<BBUIView>().ShowView();
        });
        
        sequenceShowView.InsertCallback(0.4f, () =>
        {
            btnBuy.gameObject.SetActive(false);
            btnBuy.gameObject.GetComponent<BBUIView>().ShowView();
        });
        sequenceShowView.InsertCallback(1.2f, () =>
        {
            btnClose.gameObject.SetActive(false);
            btnClose.gameObject.GetComponent<BBUIView>().ShowView();
        });
    }

    public void HidePopup_Finished()
    {
        GamePlayManager.instance.CloseShopSucces();
        gameObject.SetActive(false);
    }
}
