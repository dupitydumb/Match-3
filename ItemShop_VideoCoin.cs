using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
public class ItemShop_VideoCoin : MonoBehaviour
{
    [Header("ITEM SHOP DATA")]
    public ConfigItemShopData itemShopData;

    public Text txtCounItem;
    public Text txtCountVideo;
    public BBUIButton btnBuy;
    public GameObject lockGroup;

    // Start is called before the first frame update
    void Start()
    {
        btnBuy.OnPointerClickCallBack_Completed.AddListener(TouchBuy);
        txtCounItem.text = $"x{itemShopData.countItem}";
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
        lockGroup.SetActive(true);
        if (AdmobManager.instance.isRewardAds_Avaiable())
        {
            AdmobManager.instance.ShowRewardAd_CallBack((AdmobManager.ADS_CALLBACK_STATE state) =>
            {
                if (state == AdmobManager.ADS_CALLBACK_STATE.SUCCESS)
                {
                    lockGroup.SetActive(false);
                    SoundManager.instance.PlaySound_Cash();
        
                    Config.SetDaily_FreeCoin();
                    Config.SetDaily_VideoCoin_AddCount();
        
                    Config.BuySucces_ItemShop(itemShopData);

                    if (Config.GetDaily_VideoCoin_Count() > 0)
                    {
                        txtCountVideo.text = $"{Config.GetDaily_VideoCoin_Count()}";
                        btnBuy.Interactable = true;
                    }
                    else
                    {
                        txtCountVideo.text = $"0";
                        btnBuy.Interactable = false;
                    }
                }
                else
                {
                    lockGroup.gameObject.SetActive(false);
                    NotificationPopup.instance.AddNotification("Claim Reward Fail!");
                }
            });
        }
        else
        {
            lockGroup.gameObject.SetActive(false);
            NotificationPopup.instance.AddNotification("No Video Available!");
        }
        
    }

    private void Init_ItemShop()
    {
        if (Config.CheckDaily_VideoCoin())
        {
            Config.SetDaily_VideoCoin();
            Config.SetDaily_VideoCoin_Count(Config.DAILY_VIDEOCOIN_MAX);

            txtCountVideo.text = $"{Config.GetDaily_VideoCoin_Count()}";
            btnBuy.Interactable = true;
        }
        else
        {
            if (Config.GetDaily_VideoCoin_Count() > 0)
            {
                txtCountVideo.text = $"{Config.GetDaily_VideoCoin_Count()}";
                btnBuy.Interactable = true;
            }
            else
            {
                
                txtCountVideo.text = $"0";
                btnBuy.Interactable = false;
            }
            
            
        }
    }
}