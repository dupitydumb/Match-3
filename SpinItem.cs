using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SpinItem : MonoBehaviour
{
    public Text txtCountItem;
    private ConfigItemShopData configItemShopData;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void InitSpinItem(ConfigItemShopData _configItemShopData) {
        configItemShopData = _configItemShopData;
        txtCountItem.text = $"+{configItemShopData.countItem}";
    }
}
