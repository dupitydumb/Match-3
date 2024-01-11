using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurchaserManager : MonoBehaviour
{
    public static PurchaserManager instance;

    // Product identifiers for all products capable of being purchased: 
    // "convenience" general identifiers for use with Purchasing, and their store-specific identifier 
    // counterparts for use with and outside of Unity Purchasing. Define store-specific identifiers 
    // also on each platform's publisher dashboard (iTunes Connect, Google Play Developer Console, etc.)

    // General product identifiers for the consumable, non-consumable, and subscription products.
    // Use these handles in the code to reference which product to purchase. Also use these values 
    // when defining the Product Identifiers on the store. Except, for illustration purposes, the 
    // kProductIDSubscription - it has custom Apple and Google identifiers. We declare their store-
    // specific mapping to Unity Purchasing's AddProduct, below.
    public static string kProductIDConsumable = "consumable";
    public static string kProductIDNonConsumable = "nonconsumable";
    public static string kProductIDSubscription = "subscription";

    // Apple App Store-specific product identifier for the subscription product.
    private static string kProductNameAppleSubscription = "com.unity3d.subscription.new";

    // Google Play Store-specific product identifier subscription product.
    private static string kProductNameGooglePlaySubscription = "com.unity3d.subscription.original";

    private Action<string,IAP_CALLBACK_STATE> PurchaserManager_Callback = delegate (string _iapID, IAP_CALLBACK_STATE _callBackState) { };
    public static Action InitializeSucceeded;
    public static event Action RestoreCompleted;
    public static event Action RestoreFailed;
    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this);
    }
    void Start()
    {
        // If we haven't set up the Unity Purchasing reference
    }

    public void InitializePurchasing()
    {
    }


    public void BuyConsumable()
    {
        // Buy the consumable product using its general identifier. Expect a response either 
        // through ProcessPurchase or OnPurchaseFailed asynchronously.
        BuyProductID(kProductIDConsumable);
    }


    public void BuyConsumable(Config.IAP_ID iapID, Action<string,IAP_CALLBACK_STATE> _purchaserManager_Callback)
    {
        PurchaserManager_Callback = _purchaserManager_Callback;
        // Buy the consumable product using its general identifier. Expect a response either 
        // through ProcessPurchase or OnPurchaseFailed asynchronously.
        BuyProductID(iapID.ToString());
    }


    public void BuyNonConsumable(Config.IAP_ID iapID, Action<string, IAP_CALLBACK_STATE> _purchaserManager_Callback)
    {
        PurchaserManager_Callback = _purchaserManager_Callback;
        // Buy the non-consumable product using its general identifier. Expect a response either 
        // through ProcessPurchase or OnPurchaseFailed asynchronously.
        BuyProductID(iapID.ToString());
    }
    public void BuyNonConsumable()
    {
        // Buy the non-consumable product using its general identifier. Expect a response either 
        // through ProcessPurchase or OnPurchaseFailed asynchronously.
        BuyProductID(kProductIDNonConsumable);
    }


    public void BuySubscription()
    {
        // Buy the subscription product using its the general identifier. Expect a response either 
        // through ProcessPurchase or OnPurchaseFailed asynchronously.
        // Notice how we use the general product identifier in spite of this ID being mapped to
        // custom store-specific identifiers above.
        BuyProductID(kProductIDSubscription);
    }


    void BuyProductID(string productId)
    {
    }


    // Restore purchases previously made by this customer. Some platforms automatically restore purchases, like Google. 
    // Apple currently requires explicit purchase restoration for IAP, conditionally displaying a password prompt.
    public void RestorePurchases()
    {
    }


    //  
    // --- IStoreListener
    //


    public string GetLocalizedPriceString(string iapID)
    {
        return "";
    }
    public enum IAP_CALLBACK_STATE
    {
        SUCCESS,
        FAIL
    }
}
