using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ItemShopIAP : MonoBehaviour
{
    public ConfigPackData configPackData;

    public BBUIButton btnBuy;
    public Text txtPrice;
    // Start is called before the first frame update
    public virtual void Start()
    {
        btnBuy.OnPointerClickCallBack_Completed.AddListener(TouchBuy);

        // InitIAP();
    }

    private void OnDestroy()
    {
        btnBuy.OnPointerClickCallBack_Completed.RemoveAllListeners();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void TouchBuy()
    {
        if (ShopPopup2.instance != null && ShopPopup2.instance.isActiveAndEnabled)
        {
            ShopPopup2.instance.TouchBuy_ShopItem(configPackData);
        }
        else if(WinPopup.instance != null && WinPopup.instance.isActiveAndEnabled)
        {
            WinPopup.instance.BuyRemoveAd(configPackData);
        }
       
    }


    public void InitIAP() {
    }
}
