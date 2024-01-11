using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EasyMobile;
using Firebase.Analytics;
public class IAP_Manager : MonoBehaviour
{
    public static IAP_Manager instance;

    public GameObject shop_panel , starter_panel;

    bool no_ads;
    [HideInInspector]

    void Awake() => instance = this;

    public void open_shop_panel()
    {
        shop_panel.SetActive(true);
    }
    public void open_starter_panel()
    {
        starter_panel.SetActive(true);
    }
    public void close_shop_panel()
    {
        shop_panel.SetActive(false);
    }
    public void close_starter_panel()
    {
        starter_panel.SetActive(false);
    }

    public void Buy_SpecialOffer_Pack()
    {
	    InAppPurchasing.Purchase(EM_IAPConstants.Product_Special_Offer);
    }
    public void Buy_Starter_Pack()
    {
	    InAppPurchasing.Purchase(EM_IAPConstants.Product_Starter_Pack);
    }
    public void Buy_Master_Bundle_Pack()
    {
	    InAppPurchasing.Purchase(EM_IAPConstants.Product_Master_Bundle);
    }
    public void Buy_Mega_Bundle_Pack()
    {
	    InAppPurchasing.Purchase(EM_IAPConstants.Product_Mega_Bundle);
    }
    public void Buy_Coin_Pack1()
    {
	    InAppPurchasing.Purchase(EM_IAPConstants.Product_Coin_Pack1);
    }
    public void Buy_Coin_Pack2()
    {
	    InAppPurchasing.Purchase(EM_IAPConstants.Product_Coin_Pack2);
    }

    public void Deliver_Remove_Ads()
    {
         Remove_Ads();
        //  NativeUI.ShowToast("Ads Were Removed!");
    }
    public void Remove_Ads() => Advertising.RemoveAds();

    void PurchaseCompletedHandler(IAPProduct product)
    {
            switch (product.Name)
            {
                case EM_IAPConstants.Product_Special_Offer:
                    Special_Offer_IAP();
                    break;
                case EM_IAPConstants.Product_Starter_Pack:
                    Starter_Pack_IAP();
                    break;
                case EM_IAPConstants.Product_Master_Bundle:
                    Master_Bundle_IAP();
                    break;
                case EM_IAPConstants.Product_Mega_Bundle:
                    Mega_Bundle_IAP();
                    break;
                case EM_IAPConstants.Product_Coin_Pack2:
                    Coin_Pack2_IAP();
                    break;
                case EM_IAPConstants.Product_Coin_Pack1:        
                    Coin_Pack1_IAP();
                    break;
        }
        // NativeUI.ShowToast("Purchase Successfully Completed!");
    }

    void Special_Offer_IAP()
    {
        Config.SetCount_ItemHelp(Config.ITEMHELP_TYPE.UNDO, Config.GetCount_ItemHelp(Config.ITEMHELP_TYPE.UNDO) + 5);
        Config.SetCount_ItemHelp(Config.ITEMHELP_TYPE.SHUFFLE, Config.GetCount_ItemHelp(Config.ITEMHELP_TYPE.SHUFFLE) + 5);
        Config.SetCount_ItemHelp(Config.ITEMHELP_TYPE.SUGGEST, Config.GetCount_ItemHelp(Config.ITEMHELP_TYPE.SUGGEST) + 5);
        Config.SetCoin(Config.currCoin + 300);
        GamePlayManager.instance.SetUpdate_CountItem();
    }

    void Starter_Pack_IAP()
    {
        Config.SetCount_ItemHelp(Config.ITEMHELP_TYPE.UNDO, Config.GetCount_ItemHelp(Config.ITEMHELP_TYPE.UNDO) + 10);
        Config.SetCount_ItemHelp(Config.ITEMHELP_TYPE.SHUFFLE, Config.GetCount_ItemHelp(Config.ITEMHELP_TYPE.SHUFFLE) + 10);
        Config.SetCount_ItemHelp(Config.ITEMHELP_TYPE.SUGGEST, Config.GetCount_ItemHelp(Config.ITEMHELP_TYPE.SUGGEST) + 10);
        Config.SetCoin(Config.currCoin + 200);
        GamePlayManager.instance.SetUpdate_CountItem();
        Deliver_Remove_Ads();
    }

    void Master_Bundle_IAP()
    {
        Config.SetCount_ItemHelp(Config.ITEMHELP_TYPE.UNDO, Config.GetCount_ItemHelp(Config.ITEMHELP_TYPE.UNDO) + 10);
        Config.SetCount_ItemHelp(Config.ITEMHELP_TYPE.SHUFFLE, Config.GetCount_ItemHelp(Config.ITEMHELP_TYPE.SHUFFLE) + 10);
        Config.SetCount_ItemHelp(Config.ITEMHELP_TYPE.SUGGEST, Config.GetCount_ItemHelp(Config.ITEMHELP_TYPE.SUGGEST) + 10);
        Config.SetCoin(Config.currCoin + 1000);
        GamePlayManager.instance.SetUpdate_CountItem();
    }

    void Mega_Bundle_IAP()
    {
        Config.SetCount_ItemHelp(Config.ITEMHELP_TYPE.UNDO, Config.GetCount_ItemHelp(Config.ITEMHELP_TYPE.UNDO) + 20);
        Config.SetCount_ItemHelp(Config.ITEMHELP_TYPE.SHUFFLE, Config.GetCount_ItemHelp(Config.ITEMHELP_TYPE.SHUFFLE) + 20);
        Config.SetCount_ItemHelp(Config.ITEMHELP_TYPE.SUGGEST, Config.GetCount_ItemHelp(Config.ITEMHELP_TYPE.SUGGEST) + 20);
        Config.SetCoin(Config.currCoin + 2000);
        GamePlayManager.instance.SetUpdate_CountItem();
    }

    void Coin_Pack1_IAP()
    {
        Config.SetCoin(Config.currCoin + 500);
        GamePlayManager.instance.SetUpdate_CountItem();
    }

    void Coin_Pack2_IAP()
    {
        Config.SetCoin(Config.currCoin + 1000);
        GamePlayManager.instance.SetUpdate_CountItem();
    }
     
    void PurchaseFailedHandler(IAPProduct product, string failureReason)
    {
    	// NativeUI.ShowToast("Purchase Failed!");
    }
 
    void OnEnable()
    {            
    	InAppPurchasing.PurchaseCompleted += PurchaseCompletedHandler;
    	InAppPurchasing.PurchaseFailed += PurchaseFailedHandler;
    }

    void OnDisable()
    {            
    	InAppPurchasing.PurchaseCompleted -= PurchaseCompletedHandler;
    	InAppPurchasing.PurchaseFailed -= PurchaseFailedHandler;
    }
}
