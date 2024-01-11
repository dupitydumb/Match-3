using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class ItemTile : MonoBehaviour
{

    public static float TILE_SIZE = 1.2f;

    public SpriteRenderer bg;
    public SpriteRenderer icon;
    public SpriteRenderer shadow;
    public GameObject objGroup;

    public ItemTileCheckCollision itemTileCheckCollision;

    public int indexOnMap;
    [HideInInspector] public int indexShowTile;
    [HideInInspector] public int floorIndex;
    public Vector2Int posTile;
    [HideInInspector] public ItemData itemData;
    float pZ = 0;

    private Collider2D touchCollider2D;
    public Config.ITEMTILE_STATE itemTileState = Config.ITEMTILE_STATE.START;

    private void Awake()
    {
        touchCollider2D = GetComponent<Collider2D>();
        touchCollider2D.enabled = true;
        itemTileState = Config.ITEMTILE_STATE.START;
        itemTileCheckCollision.gameObject.SetActive(false);
    }
    // Start is called before the first frame update
    void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR
        //Debug.Log("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
#else
        if (Input.touchCount > 0) {
            isCheckTouchCount = true;
        }

        if (Input.touchCount == 0 && isCheckTouchCount) {
            //Debug.Log("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
            isCheckTouchCount = false;
            if (bg.sortingLayerName.Equals("Hover"))
            {
                SetLayer_Floor();
                objGroup.transform.DOScale(Vector3.one, 0.1f).SetEase(Ease.Linear);
            }
        }
#endif
    }

    bool isCheckTouchCount = false;

    public void InitTile(int _indexShow,int _indexOnMap, int _floorIndex, Vector2Int _posTile, ItemData _itemData) {

        indexOnMap = _indexOnMap;
        indexShowTile = _indexShow;
        itemData = _itemData;
        floorIndex = _floorIndex;
        posTile = _posTile;

        //pZ = 800 - floorIndex * 100f + posTile.y * 2 + 2 - posTile.x * 0.1f; 
        pZ = 800 - indexOnMap;
        icon.sprite = Resources.Load<Sprite>("Sprite/" + Config.currTheme.ToString() + "/" + (int)itemData.itemType);

        gameObject.transform.localPosition = new Vector3(TILE_SIZE * posTile.x, TILE_SIZE * posTile.y, 50 - floorIndex * 5);

        SetOrderLayer_Floor();
        SetLayer_Floor();
        ShowTile();
    }


    private void ShowTile() {
        itemTileState = Config.ITEMTILE_STATE.START_TO_FLOOR;
        //Debug.Log("floorIndex:"+ floorIndex);
        if (Config.listStartMoveType[floorIndex] == Config.START_MOVE_TYPE.BOTTOM) {
            gameObject.transform.localPosition = new Vector3(TILE_SIZE * posTile.x, TILE_SIZE * posTile.y - 20f, pZ);
        }
        else if (Config.listStartMoveType[floorIndex] == Config.START_MOVE_TYPE.TOP)
        {
            gameObject.transform.localPosition = new Vector3(TILE_SIZE * posTile.x, TILE_SIZE * posTile.y + 20f, pZ);
        }
        else if (Config.listStartMoveType[floorIndex] == Config.START_MOVE_TYPE.LEFT)
        {
            gameObject.transform.localPosition = new Vector3(TILE_SIZE * posTile.x - 12f, TILE_SIZE * posTile.y, pZ);
        }
        else if (Config.listStartMoveType[floorIndex] == Config.START_MOVE_TYPE.RIGHT)
        {
            gameObject.transform.localPosition = new Vector3(TILE_SIZE * posTile.x + 12f, TILE_SIZE * posTile.y, pZ);
        }

        //indexShowTile * 0.01f +
        gameObject.transform.DOLocalMove(new Vector3(TILE_SIZE * posTile.x, TILE_SIZE * posTile.y, pZ), 1f).SetEase(Ease.InBack).SetDelay(0.04f* floorIndex).OnComplete(() => {
            itemTileState = Config.ITEMTILE_STATE.FLOOR;
            itemTileCheckCollision.gameObject.SetActive(true);
        });
    }


    public void SetTouch_Avaiable(bool _isTouchAvaiable) {
        itemTileCheckCollision.gameObject.transform.localPosition = new Vector3(0f, 0f, 0.5f);
        touchCollider2D.enabled = _isTouchAvaiable;
    }

    public void SetTouch_Enable()
    {
        StartCoroutine(SetTouch_Avaiable_IEnumerator());
    }

    public IEnumerator SetTouch_Avaiable_IEnumerator() {
        yield return new WaitForSeconds(0.1f);
        SetTouch_Avaiable(true);
        SetShadow_Avaiable(false);
    }


    public void SetShadow_Avaiable(bool isShadowAvaiable) {
        shadow.gameObject.SetActive(isShadowAvaiable);
    }

    public bool GetTouch_Avaiable() {
        return touchCollider2D.enabled;
    }


    public void OnMouseUpAsButton()
    {
        SetTouchItemTile();
    }

    bool isMouseDown = false;
    public void OnMouseDown()
    {
        isMouseDown = true;
        Debug.Log("OnMouseDown:    " + gameObject.name);
    }

    public void OnMouseUp()
    {
        if (isMouseDown) {
            isMouseDown = false;
            SetTouchItemTile();
        }
        Debug.Log("OnMouseUp" + gameObject.name);
        //Debug.Log("bg.sortingLayerName" + bg.sortingLayerName);
        if (bg.sortingLayerName.Equals("Hover"))
        {
            SetLayer_Floor();
            objGroup.transform.DOScale(Vector3.one, 0.1f).SetEase(Ease.Linear);
        }

        if (isTileTutorial) {
            if (bg.sortingLayerName.Equals("TutHover"))
            {
                SetLayer_Floor();
                objGroup.transform.DOScale(Vector3.one, 0.1f).SetEase(Ease.Linear);
            }
        }
    }


    public void SetTouchItemTile() {
        if (Config.gameState == Config.GAME_STATE.PLAYING)
        {
            if (itemTileState == Config.ITEMTILE_STATE.FLOOR)
            {

                if (Config.CheckTutorial_1() && !isTileTutorial) return;
                if (isTileTutorial) GameLevelManager.instance.SetNextTut1(this);

                if (Config.CheckTutorial_2() && Config.isShowTut2) return;
                if (Config.CheckTutorial_3() && Config.isShowTut3) return;
                if (Config.CheckTutorial_4() && Config.isShowTut4) return;


                if (!GameLevelManager.instance.IsItemTileMoveToSlot()) {
                    Debug.Log("Chua the chuyen xuong slot");
                    return;
                }


                //AudioManager.instance.Play("Click");
                //Debug.Log("OnMouseUpAsButton" + gameObject.name);
                //Destroy(gameObject);


                GamePlayManager.instance.HideTut_HandGuide();
                SoundManager.instance.PlaySound_BlockClick();
                itemTileCheckCollision.gameObject.SetActive(false);

                SetLayer_Move();
                SetTouch_Avaiable(false);
                SetShadow_Avaiable(false);

                itemTileState = Config.ITEMTILE_STATE.MOVE_TO_SLOT;

                GameLevelManager.instance.AddItemSlot(this);
            }
        }
    }

    private void OnMouseEnter()
    {
        if (Config.gameState == Config.GAME_STATE.PLAYING)
        {
            if (itemTileState == Config.ITEMTILE_STATE.FLOOR)
            {
                if (Config.CheckTutorial_1() && !isTileTutorial) return;
                if (Config.CheckTutorial_2() && Config.isShowTut2) return;
                if (Config.CheckTutorial_3() && Config.isShowTut3) return;
                if (Config.CheckTutorial_4() && Config.isShowTut4) return;
#if UNITY_EDITOR
                SetLayer_Hover();
                objGroup.transform.DOScale(Vector3.one * 1.1f, 0.1f).SetEase(Ease.Linear);
#else
            if (Input.touchCount > 0)
                {
                    //isMouseDown = false;
                    //Debug.Log("OnMouseEnter" + gameObject.name);
                    SetLayer_Hover();
                    objGroup.transform.DOScale(Vector3.one * 1.1f, 0.1f).SetEase(Ease.Linear);
                }
#endif
            }
        }
    }
   

    private void OnMouseExit()
    {
        //Debug.Log("OnMouseExit" + gameObject.name);
        //if (itemTileState == Config.ITEMTILE_STATE.FLOOR)
        //{
        //    if (touchCollider2D.enabled)
        //    {
        //        Debug.Log("OnMouseExit"+gameObject.name);
        //        SetLayer_Floor();
        //        gameObject.transform.DOScale(Vector3.one, 0.1f).SetEase(Ease.Linear);
        //    }
        //}

        if (bg.sortingLayerName.Equals("Hover")) {
            SetLayer_Floor();
            objGroup.transform.DOScale(Vector3.one, 0.1f).SetEase(Ease.Linear);
        }

        if (isTileTutorial) {
            if (bg.sortingLayerName.Equals("TutHover"))
            {
                SetLayer_Floor();
                objGroup.transform.DOScale(Vector3.one, 0.1f).SetEase(Ease.Linear);
            }
        }
    }


    public void SetLayer_Move() {
       
        bg.sortingLayerName = "Move";
        icon.sortingLayerName = "Move";
        shadow.sortingLayerName = "Move";
        if (isTileTutorial)
        {
            bg.sortingLayerName = "TutMove";
            icon.sortingLayerName = "TutMove";
            shadow.sortingLayerName = "TutMove";
        }
        itemTileCheckCollision.gameObject.transform.localPosition = new Vector3(0f, 0f, 0.5f);
    }

    public void SetLayer_Hover()
    {
        bg.sortingLayerName = "Hover";
        icon.sortingLayerName = "Hover";
        shadow.sortingLayerName = "Hover";
        if (isTileTutorial)
        {
            bg.sortingLayerName = "TutHover";
            icon.sortingLayerName = "TutHover";
            shadow.sortingLayerName = "TutHover";
        }
        itemTileCheckCollision.gameObject.transform.localPosition = new Vector3(0f, 0f, 0.5f);
    }

    public void SetLayer_Floor() {
        isMouseDown = false;
        string sortingLayerName = "Floor" + (floorIndex + 1);
        if (isTileTutorial) {
            sortingLayerName = "Tut";
        }
        bg.sortingLayerName = sortingLayerName;
        icon.sortingLayerName = sortingLayerName;
        shadow.sortingLayerName = sortingLayerName;

        itemTileCheckCollision.gameObject.transform.localPosition = new Vector3(0f, 0f, 0.5f);
    }


    public void SetOrderLayer_Move(int indexSlot) {
        int sortingOrder = 10 * indexSlot;
        bg.sortingOrder = sortingOrder;
        icon.sortingOrder = sortingOrder;
        shadow.sortingOrder = sortingOrder;
    }


    public void SetOrderLayer_Floor()
    {
        int sortingOrder = 400 -20 * posTile.y + posTile.x;
        bg.sortingOrder = sortingOrder;
        icon.sortingOrder = sortingOrder;
        shadow.sortingOrder = sortingOrder;
    }

#region MOVE

    Sequence moveToSlot_Sequence;
    public void SetMoveToSlot(int indexSlot) {
        DOTween.Kill(gameObject.transform);
        
        SetOrderLayer_Move(indexSlot);
        SetLayer_Move();

        moveToSlot_Sequence = DOTween.Sequence();
        moveToSlot_Sequence.Insert(0f, objGroup.transform.DOScale(Vector3.one * 1.1f, 0.1f).SetEase(Ease.OutQuad));
        moveToSlot_Sequence.Insert(0f, objGroup.transform.DOLocalRotate(new Vector3(0f,0f,Random.Range(10f,15f)), 0.1f));
        moveToSlot_Sequence.Insert(0.1f, objGroup.transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.InQuad));
        moveToSlot_Sequence.Insert(0.1f, objGroup.transform.DOLocalRotate(Vector3.zero, 0.2f).SetEase(Ease.InQuad));
        moveToSlot_Sequence.InsertCallback(0.1f,()=> {
            SoundManager.instance.PlaySound_Wind();
        });
        moveToSlot_Sequence.Insert(0f, gameObject.transform.DOLocalMove(Vector3.zero, 0.3f).SetEase(Ease.OutQuad));
        moveToSlot_Sequence.OnComplete(()=> {
            itemTileState = Config.ITEMTILE_STATE.SLOT;
            SoundManager.instance.PlaySound_BlockMoveFinish();
            GameLevelManager.instance.SetMoveItemSlot_Finished(this);
        });
    }


    public void ResetPosSlot(int indexSlot) {
        SetOrderLayer_Move(indexSlot);
    }
#endregion

#region SLOT
    Sequence match_Sequence;
    public void SetItemTile_Match3(System.Action itemTileSlot_Match3_CallBack) {
        match_Sequence = DOTween.Sequence();
        match_Sequence.Insert(0f, objGroup.transform.DOScale(Vector3.one * 1.1f, 0.1f).SetEase(Ease.OutQuad));
        match_Sequence.Insert(0f, objGroup.transform.DOLocalRotate(new Vector3(0f, 0f, Random.Range(10f, 15f)), 0.1f));
        match_Sequence.Insert(0.1f, objGroup.transform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InQuad));
        match_Sequence.Insert(0.1f, objGroup.transform.DOLocalRotate(Vector3.zero, 0.2f).SetEase(Ease.InQuad));
        match_Sequence.OnComplete(() => {
            itemTileSlot_Match3_CallBack.Invoke();
        });
    }
#endregion

#region UNDO
    Sequence moveUndo_Sequence;
    public void SetItemTile_Undo() {
        DOTween.Kill(gameObject.transform);

        SetOrderLayer_Move(0);
        SetLayer_Move();

        moveUndo_Sequence = DOTween.Sequence();
        moveUndo_Sequence.Insert(0f, objGroup.transform.DOScale(Vector3.one * 1.1f, 0.05f).SetEase(Ease.OutQuad));
        moveUndo_Sequence.Insert(0f, objGroup.transform.DOLocalRotate(new Vector3(0f, 0f, Random.Range(10f, 15f)), 0.05f));
        moveUndo_Sequence.Insert(0.05f, objGroup.transform.DOScale(Vector3.one, 0.1f).SetEase(Ease.InQuad));
        moveUndo_Sequence.Insert(0.05f, objGroup.transform.DOLocalRotate(Vector3.zero, 0.1f).SetEase(Ease.InQuad));
        moveUndo_Sequence.Insert(0f, gameObject.transform.DOLocalMove(new Vector3(TILE_SIZE * posTile.x, TILE_SIZE * posTile.y, pZ), 0.2f).SetEase(Ease.OutQuad));
        moveUndo_Sequence.OnComplete(() => {
            itemTileState = Config.ITEMTILE_STATE.FLOOR;
            SetItemTile_Undo_Finished();
        });
    }

    public void SetItemTile_Undo_Now() {
        gameObject.transform.localPosition = new Vector3(TILE_SIZE * posTile.x, TILE_SIZE * posTile.y, pZ);
        itemTileState = Config.ITEMTILE_STATE.FLOOR;
        SetItemTile_Undo_Finished();
    }

    private void SetItemTile_Undo_Finished() {
        SetTouch_Avaiable(true);
        SetOrderLayer_Floor();
        SetLayer_Floor();
        itemTileCheckCollision.gameObject.SetActive(true);
        itemTileCheckCollision.gameObject.transform.localPosition = new Vector3(0f, 0f, 0.5f);
    }
#endregion


#region SHUFFLE
    Sequence moveShuffle_Sequence;
    public void SetItemTile_Shuffle(ItemData _itemData) {
        itemData = _itemData;
        moveShuffle_Sequence = DOTween.Sequence();
        moveShuffle_Sequence.Insert(0f, objGroup.transform.DOScale(Vector3.zero, 0.1f).SetEase(Ease.OutQuad));
        moveShuffle_Sequence.Insert(0f, objGroup.transform.DOLocalRotate(new Vector3(0f, 0f, Random.Range(10f, 15f)), 0.1f));
        moveShuffle_Sequence.InsertCallback(0.1f, () =>
        {
            SetItemTile_Shuffle_ChangeItemData();
        });
        moveShuffle_Sequence.Insert(0.1f, objGroup.transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.InQuad));
        moveShuffle_Sequence.Insert(0.1f, objGroup.transform.DOLocalRotate(Vector3.zero, 0.2f).SetEase(Ease.InQuad));
        moveShuffle_Sequence.OnComplete(()=> {
            SetLayer_Floor();
            itemTileCheckCollision.gameObject.transform.localPosition = new Vector3(0f, 0f, 0.5f);
        });
    }

    public void SetItemTile_Shuffle_ChangeItemData() {
        icon.sprite = Resources.Load<Sprite>("Sprite/" + Config.currTheme.ToString() + "/" + (int)itemData.itemType);
    }
#endregion

#region SUGGEST

    public void SetItemTileSuggest() {
        itemTileCheckCollision.gameObject.SetActive(false);
        SetLayer_Move();
        SetTouch_Avaiable(false);
        SetShadow_Avaiable(false);
        itemTileState = Config.ITEMTILE_STATE.MOVE_TO_SLOT;

        GameLevelManager.instance.AddItemSlot(this);
    }
    #endregion


    #region TUT
    bool isTileTutorial = false;
    public void SetItemTileTut() {  
        isTileTutorial = true;
        string sortingLayerName = "Tut";
        bg.sortingLayerName = sortingLayerName;
        icon.sortingLayerName = sortingLayerName;
        shadow.sortingLayerName = sortingLayerName;
    }
    #endregion
}
