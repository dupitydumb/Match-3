using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ItemPack : MonoBehaviour
{
    public Text txtCountItem;
    public GameObject itemPackView;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetItemPack(ConfigItemShopData configItemShopData) {
        txtCountItem.text = $"x{configItemShopData.countItem}";
    }

    public void ShowItemPack() {
        itemPackView.GetComponent<BBUIView>().ShowView();
    }
}
