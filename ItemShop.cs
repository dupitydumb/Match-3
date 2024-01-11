using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ItemShop : MonoBehaviour
{
    [Header("ITEM SHOP DATA")]
    public ConfigItemShopData itemShopData;

    public Text txtCounItem;
    public Text txtPrice;
    public BBUIButton btnBuy;

    // Start is called before the first frame update
    void Start()
    {
        btnBuy.OnPointerClickCallBack_Completed.AddListener(TouchBuy);
        txtCounItem.text = $"x{itemShopData.countItem}";
        txtPrice.text = $"{itemShopData.price}";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TouchBuy() {
        if (Config.currCoin >= itemShopData.price)
        {
            SoundManager.instance.PlaySound_Cash();
            Config.SetCoin(Config.currCoin - itemShopData.price);
            Config.BuySucces_ItemShop(itemShopData);

            NotificationPopup.instance.AddNotification("Buy Success!");
        }
        else {
            NotificationPopup.instance.AddNotification("Not enough Coin!");
            Debug.LogError("Not enough Coin!");
        }
    }
}
