using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveAdButton : MonoBehaviour
{
    public BBUIButton btnRemoveAd;

    public GameObject lockGroup;
    // Start is called before the first frame update
    void Start()
    {
        btnRemoveAd.OnPointerClickCallBack_Completed.AddListener(TouchRemoveAd);
        Init_Button();
    }

    private void OnDestroy()
    {
        btnRemoveAd.OnPointerClickCallBack_Completed.RemoveAllListeners();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Init_Button()
    {
        btnRemoveAd.gameObject.SetActive(!Config.GetRemoveAd());
    }

    private void TouchRemoveAd()
    {
        PurchaserManager.instance.BuyConsumable(Config.IAP_ID.removead, (string _iapID, PurchaserManager.IAP_CALLBACK_STATE _state) =>
        {
            if (_state == PurchaserManager.IAP_CALLBACK_STATE.SUCCESS)
            {
                
                Debug.Log("SUCCESSSUCCESS"+_iapID);
                
                lockGroup.gameObject.SetActive(false);
                if (_iapID.Equals(Config.IAP_ID.removead.ToString()))
                {
                    Config.SetRemoveAd();
                    Config.SetBuyIAP(Config.IAP_ID.removead);
                    NotificationPopup.instance.AddNotification("RemoveAd Success!");

                    Init_Button();
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
}
