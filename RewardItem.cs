using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class RewardItem : MonoBehaviour
{
    public Text txtCountItem;
    private ConfigItemShopData configItemShopData;
    public BBUIView view;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    public void InitSpinItem(ConfigItemShopData _configItemShopData)
    {
        configItemShopData = _configItemShopData;
        txtCountItem.text = $"+{configItemShopData.countItem}";
        view.gameObject.SetActive(false);
    }

    public void ShowView()
    {
        view.gameObject.SetActive(true);
        view.ShowView();
    }
}
