using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UI;
public class ItemShopIAP_Item : ItemShopIAP
{
    [ToggleGroup("HasItemCoin")] [SerializeField] private bool HasItemCoin;
    [ToggleGroup("HasItemCoin")] public Text txtCount_ItemCoin;
    [ToggleGroup("HasItemUndo")] [SerializeField] private bool HasItemUndo;
    [ToggleGroup("HasItemUndo")] public Text txtCount_ItemUndo;
    [ToggleGroup("HasItemSuggest")] [SerializeField] private bool HasItemSuggest;
    [ToggleGroup("HasItemSuggest")] public Text txtCount_ItemSuggest;
    [ToggleGroup("HasItemShuffle")] [SerializeField] private bool HasItemShuffle;
    [ToggleGroup("HasItemShuffle")] public Text txtCount_ItemShuffle;
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

        InitPackIAP();
    }

    public void InitPackIAP() {
        if (HasItemCoin) {
            txtCount_ItemCoin.text = "x"+Config.GetCountItem_Pack_ItemType(Config.SHOPITEM.COIN, configPackData).ToString();
        }
        if (HasItemUndo)
        {
            txtCount_ItemUndo.text = "x" + Config.GetCountItem_Pack_ItemType(Config.SHOPITEM.UNDO, configPackData).ToString();
        }
        if (HasItemSuggest)
        {
            txtCount_ItemSuggest.text = "x" + Config.GetCountItem_Pack_ItemType(Config.SHOPITEM.SUGGEST, configPackData).ToString();
        }
        if (HasItemShuffle)
        {
            txtCount_ItemShuffle.text = "x" + Config.GetCountItem_Pack_ItemType(Config.SHOPITEM.SHUFFLE, configPackData).ToString();
        }
    }
}
