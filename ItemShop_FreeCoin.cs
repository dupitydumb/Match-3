using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ItemShop_FreeCoin : MonoBehaviour
{
    [Header("ITEM SHOP DATA")]
    public ConfigItemShopData itemShopData;

    public Text txtCounItem;
    public BBUIButton btnBuy;

    // Start is called before the first frame update
    void Start()
    {
        btnBuy.OnPointerClickCallBack_Completed.AddListener(TouchBuy);
        txtCounItem.text = $"{itemShopData.countItem}";
    }

    private void OnEnable()
    {
        Init_ItemShop();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void TouchBuy()
    {
        btnBuy.Interactable = false;
        
        Config.SetDaily_FreeCoin();
        
        SoundManager.instance.PlaySound_Cash();
        Config.BuySucces_ItemShop(itemShopData);
    }

    private void Init_ItemShop()
    {
        if (Config.CheckDaily_FreeCoin())
        {
            btnBuy.Interactable = true;
        }
        else
        {
            btnBuy.Interactable = false;
        }
    }
}