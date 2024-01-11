using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class ItemTileSlot : MonoBehaviour
{
    [HideInInspector] public ItemTile itemTile;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetItemTile(ItemTile _itemTile) {
        itemTile = _itemTile;
    }

    public void SetItemSlot_Match3() {
        itemTile.SetItemTile_Match3(()=> {
            ItemTileSlot_Match3_CallBack();
        });
    }

    public void ItemTileSlot_Match3_CallBack() {
        Destroy(gameObject);
    }

    public void ItemTileSlot_ResetPos(int pos) {
        
    }

    Sequence moveResetPosSlot_Sequence;
    public void ResetPosSlot(int indexSlot)
    {
        itemTile.ResetPosSlot(indexSlot);
        DOTween.Kill(gameObject.transform);
        moveResetPosSlot_Sequence = DOTween.Sequence();
        moveResetPosSlot_Sequence.Insert(0f, gameObject.transform.DOLocalMove(new Vector3(-3 * ItemTile.TILE_SIZE + indexSlot * ItemTile.TILE_SIZE, 0f, 0f), 0.2f).SetEase(Ease.OutQuad));
    }
}
