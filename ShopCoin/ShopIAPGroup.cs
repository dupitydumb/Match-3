using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopIAPGroup : MonoBehaviour
{
    public List<ItemShopIAP> listItemShopIAPs;
    public ItemShopIAP itemShopRemoveAd;
    public ItemShopIAP itemShopRemoveAd_And_Combo10;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitShopIAP() {
        if (Config.GetRemoveAd()) {
            itemShopRemoveAd.gameObject.SetActive(false);
            itemShopRemoveAd_And_Combo10.gameObject.SetActive(false);
        }

        for (int i=0; i< listItemShopIAPs.Count;i++) {
            listItemShopIAPs[i].InitIAP();
        }
    }
}
