using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using DG.Tweening;

public class GameLevelManager : MonoBehaviour
{

    public static GameLevelManager instance;
    [Title("CONFIG LEVEL")]
    public ConfigLevelGame configLevelGame;



    [InfoBox("Các item hiện trên map")]
    public List<ItemData> listItemDataOnMaps = new List<ItemData>();



    [Title("REFERENCE")]
    public ItemTile itemTilePrefab;
    [Header("TileMaps")]
    public GameObject tileMapGroup;
    public List<Tilemap> listTileMaps = new List<Tilemap>();

    public List<Transform> listFloors = new List<Transform>();


    private List<ItemTileData> listItemTileDatas = new List<ItemTileData>();

    public GameObject slotBG;

    Vector2 originalSlotBgPos;
    private void Awake()
    {
        instance = this;

        //if (Config.CheckWideScreen()) {
        //    slotBG.transform.parent.localPosition = new Vector3(0f, -4.5f, 0f);
        //}
    }

    private Vector3 posSlotBG;
    public void InitSlotPosition(Vector3 pos)
    {
        posSlotBG = pos;
        Debug.Log(posSlotBG);
        slotBG.transform.parent.position = new Vector3(0f, pos.y, 0);
        Debug.Log(slotBG.transform.parent.position);
    }
    // Start is called before the first frame update
    void Start()
    {
        slotBG.gameObject.SetActive(false);
        originalSlotBgPos = slotBG.transform.position;
        if (Config.CheckTutorial_1()) {
            slotBG.GetComponent<SpriteRenderer>().sortingLayerName = "TutUI";
        }
        InitListMoveType();

        ReadDataTileMap();


        StartCoroutine(ShowSlotBG());
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void InitListMoveType() {
        Config.listStartMoveType.Clear();
        List<Config.START_MOVE_TYPE> listTemp_StartMoveType = new List<Config.START_MOVE_TYPE>();
        listTemp_StartMoveType.Clear();
        listTemp_StartMoveType.Add(Config.START_MOVE_TYPE.TOP);
        listTemp_StartMoveType.Add(Config.START_MOVE_TYPE.BOTTOM);
        listTemp_StartMoveType.Add(Config.START_MOVE_TYPE.RIGHT);
        listTemp_StartMoveType.Add(Config.START_MOVE_TYPE.LEFT);


        List<Config.START_MOVE_TYPE> listTemp2_StartMoveType = new List<Config.START_MOVE_TYPE>();
        listTemp2_StartMoveType.Clear();
        for (int i = 0; i < 4; i++) {
            Config.START_MOVE_TYPE moveType = listTemp_StartMoveType[Random.Range(0, listTemp_StartMoveType.Count)];
            listTemp2_StartMoveType.Add(moveType);
            listTemp_StartMoveType.Remove(moveType);
        }
        
        for (int i = 0; i < listFloors.Count; i++) {
            Config.listStartMoveType.Add(listTemp2_StartMoveType[i % 4]);
        }
    }

    public void ReadDataTileMap() {
        // Debug.Log(listTileMaps[i].cellBounds.yMax - listTileMaps[i].cellBounds.yMin);
        
        listItemTileDatas.Clear();
        int indexOnMap = 0;
        for (int i = 0; i < listTileMaps.Count; i++)
        {
            for (int y = listTileMaps[i].cellBounds.yMin; y < listTileMaps[i].cellBounds.yMax; y++)
            {
                for (int x = listTileMaps[i].cellBounds.xMin; x < listTileMaps[i].cellBounds.xMax; x++)
                {
                    //Debug.Log("i:" + i + "   j:" + j);
                    if (listTileMaps[i].HasTile(new Vector3Int(x, y, 0)))
                    {
                        
                        indexOnMap++;
                        ItemTileData itemTileData = new ItemTileData(i, indexOnMap, new Vector2Int(x, y));
                        listItemTileDatas.Add(itemTileData);
                    }

                }
            }
        }

        if (GamePlayManager.instance.isPlayTest)
        {
            int height = 0;
            
            for (int i = 0; i < listTileMaps.Count; i++)
            {
                for (int x = listTileMaps[i].cellBounds.xMin; x < listTileMaps[i].cellBounds.xMax; x++)
                {
                    int yMin = 100;
                    int yMax = -100;
                    for (int y = listTileMaps[i].cellBounds.yMin; y < listTileMaps[i].cellBounds.yMax; y++)
                    {
                        //Debug.Log("i:" + i + "   j:" + j);
                        if (listTileMaps[i].HasTile(new Vector3Int(x, y, 0)))
                        {
                            if (y < yMin) yMin = y;
                            if (y > yMax) yMax = y;
                        }
                    }
                    
                    int newHeight = yMax - yMin + 1;
                        
                    if (newHeight > height) height = newHeight;
                }
            }
            Debug.Log("heightheightheightheight" + height);
            if (height >= 9)
            {
                gameObject.transform.localScale = Vector3.one;
                gameObject.transform.localPosition = new Vector3(0f,-0.5f,0f);
                InitSlotPosition(new Vector3(posSlotBG.x, posSlotBG.y,0));
            }
        }

        tileMapGroup.SetActive(false);
        GenerateItemData();
    }

    public void GenerateItemData() {
        Debug.Log("GenerateItemDataGenerateItemData:" + listItemTileDatas.Count);
        List<ItemData> listTemp_ItemDatas = new List<ItemData>();
        listTemp_ItemDatas.Clear();
        if (listItemTileDatas.Count % 3 != 0) {
            Debug.LogError("Số phần tử phải là x3: " + listItemTileDatas.Count);
            return;
        }

        List<ItemData> listItemDataOnMaps_Shuffle = new List<ItemData>();
        List<ItemData> listItemDataOnMaps_Temp = new List<ItemData>(listItemDataOnMaps);

        for (int i = 0; i < listItemDataOnMaps.Count; i++) {
            ItemData itemData = listItemDataOnMaps_Temp[Random.Range(0, listItemDataOnMaps_Temp.Count)];

            listItemDataOnMaps_Shuffle.Add(itemData);
            listItemDataOnMaps_Temp.Remove(itemData);
        }
        for (int i = 0; i < listItemTileDatas.Count / 3; i++)
        {
            ItemData itemData = listItemDataOnMaps_Shuffle[i % listItemDataOnMaps_Shuffle.Count];

            //Add 3 phan tu
            listTemp_ItemDatas.Add(itemData);
            listTemp_ItemDatas.Add(itemData);
            listTemp_ItemDatas.Add(itemData);
        }
        Debug.Log("1111111111111111111:" + listTemp_ItemDatas.Count);

        for (int i = 0; i < listItemTileDatas.Count; i++)
        {
            ItemData itemData = listTemp_ItemDatas[Random.Range(0, listTemp_ItemDatas.Count)];
            listItemTileDatas[i].itemData = itemData;
            listTemp_ItemDatas.Remove(itemData);
        }
        Debug.Log("222222222222:" + listTemp_ItemDatas.Count);

        if (GamePlayManager.instance.level == 1) {
            //Tut
            int indexItemData = Random.Range(0, listItemDataOnMaps.Count);
            ItemTileData itemTileData1 = new ItemTileData(1, 100, new Vector2Int(0, 0));
            listItemTileDatas.Add(itemTileData1);
            itemTileData1.itemData = listItemDataOnMaps[indexItemData];

            ItemTileData itemTileData2 = new ItemTileData(1, 101, new Vector2Int(-1, 0));
            listItemTileDatas.Add(itemTileData2);
            itemTileData2.itemData = listItemDataOnMaps[indexItemData];


            ItemTileData itemTileData3 = new ItemTileData(1, 102, new Vector2Int(-2, 0));
            listItemTileDatas.Add(itemTileData3);
            itemTileData3.itemData = listItemDataOnMaps[indexItemData];


            listItemTile_Tutorials.Clear();
        }

        FloorGenerateItemTileDatas();
    }


    private void FloorGenerateItemTileDatas() {
        Debug.Log("FloorGenerateItemTileDatasFloorGenerateItemTileDatas");
        for (int i = 0; i < listItemTileDatas.Count; i++) {
            ItemTile itemTile = Instantiate(itemTilePrefab, listFloors[listItemTileDatas[i].floorIndex]);
            itemTile.InitTile(i, listItemTileDatas[i].indexOnMap, listItemTileDatas[i].floorIndex, listItemTileDatas[i].posTile, listItemTileDatas[i].itemData);
            itemTile.name = "" + i;
            StartCoroutine(PlaySound_MoveBoardStart(listItemTileDatas[i].floorIndex));
            if (Config.CheckTutorial_1())
            {
                if (i >= listItemTileDatas.Count - 3) {
                    listItemTile_Tutorials.Add(itemTile);
                    itemTile.SetItemTileTut();
                }
            }
        }

        StartCoroutine(SetStartLevelGaME());

    }

    public IEnumerator SetStartLevelGaME() {
        yield return new WaitForSeconds(listFloors.Count * 0.04f +1.05f);
        GamePlayManager.instance.SetStartPlayingGame();
    }


    int indexMoveBoardStart = -1;
    private IEnumerator PlaySound_MoveBoardStart(int floorIndex) {
        yield return new WaitForSeconds(0.2f * floorIndex + 0.18f);
        if (floorIndex > indexMoveBoardStart) {
            Debug.Log("PlaySound_MoveBoardStartPlaySound_MoveBoardStartPlaySound_MoveBoardStartPlaySound_MoveBoardStart");
            indexMoveBoardStart = floorIndex;
            SoundManager.instance.PlaySound_ShowBoard();
        }

    }


    #region SLOT
    //Chua cac itemTile
    [Header("Slot")]
    public ItemTileSlot itemTileSlotPrefab;
    public List<ItemTileSlot> listItemSlots = new List<ItemTileSlot>();
    public Transform slotParentTranform;


    public void AddItemSlot(ItemTile itemTile)
    {
        if (listItemSlots.Count < 7)
        {
            //Test1
            int indexNewSlot = FindIndexAddItemTileSlot(itemTile);
            //int curCountItemSlot = listItemSlots.Count;
            ItemTileSlot itemTileSlot = Instantiate(itemTileSlotPrefab, slotParentTranform);
            itemTileSlot.transform.localPosition = new Vector3(-3 * ItemTile.TILE_SIZE + indexNewSlot * ItemTile.TILE_SIZE, 0f, 0f);
            itemTile.transform.parent = itemTileSlot.transform;
            itemTileSlot.SetItemTile(itemTile);
            itemTile.SetMoveToSlot(indexNewSlot);
            listCheckUndo_ItemTileSlots.Add(itemTileSlot);


            //Find indexAdd newItemSlot
            listItemSlots.Insert(indexNewSlot, itemTileSlot);
            StartCoroutine(SetListItemSlot_ResetPosition_Now());
        }

        if (Config.CheckTutorial_2()) {
            GamePlayManager.instance.ShowTut2();
        }
    }


    public int FindIndexAddItemTileSlot(ItemTile itemTile) {
        int indexSlot = listItemSlots.Count;
        for (int i = listItemSlots.Count - 1; i >= 0; i--) {
            if (listItemSlots[i].itemTile.itemData.itemType == itemTile.itemData.itemType) {
                return i + 1;
            }
        }
        return indexSlot;
    }

    int countWinVoice = 0;
    int countVoice = 0;
    public void SetMoveItemSlot_Finished(ItemTile _itemTile) {
        //Debug.Log("SetMoveItemSlot_Finished");
        List<ItemTileSlot> listCheckMatch3Slots = FindMatch3_ItemTile_Slots(_itemTile);



        if (listCheckMatch3Slots.Count == 3)
        {
            //playvoice

            if (countWinVoice % 3 == 0)
            {
                countWinVoice = 0;
                int voice = Random.Range(1, 3);
                SoundManager.instance.PlaySound_Voice(voice);
            }
            countWinVoice++;

            //endplayvoice

            for (int i = 0; i < 3; i++)
            {
                listItemSlots.Remove(listCheckMatch3Slots[i]);
                //Undo
                listCheckUndo_ItemTileSlots.Remove(listCheckMatch3Slots[i]);

                listCheckMatch3Slots[i].SetItemSlot_Match3();
            }
            StartCoroutine(SetListItemSlot_ResetPosition());
            StartCoroutine(CheckGameWin());
            SoundManager.instance.PlaySound_FreeBlock();
            GamePlayManager.instance.SetAddScore();
            Config.AddPiggyBank_Coin();
        }
        else {
            
            if (listItemSlots.Count == 4 || listItemSlots.Count == 6)
            {
                countVoice = 0;
                int voice = Random.Range(3, 6);
                SoundManager.instance.PlaySound_Voice(voice);
            }
            countVoice++;

            StartCoroutine(CheckGameOver_IEnumerator());
        }
    }


    public IEnumerator CheckGameOver_IEnumerator() {
        if(!IsItemTileMoveToSlot()) SoundManager.instance.PlaySound_NoMoreMove();
        yield return new WaitForSeconds(1f);
        if (CheckGameOver())
        {
            SetGameOver();
        }
    }


    public IEnumerator SetListItemSlot_ResetPosition() {
        //0.3 la thoi gian itemTile Efx Destroy
        yield return new WaitForSeconds(0.3f);
        for (int i = 0; i < listItemSlots.Count; i++) {
            listItemSlots[i].ResetPosSlot(i);
        }
    }

    public IEnumerator SetListItemSlot_ResetPosition_Now()
    {
        //0.3 la thoi gian itemTile Efx Destroy
        yield return new WaitForSeconds(0.01f);
        for (int i = 0; i < listItemSlots.Count; i++)
        {
            listItemSlots[i].ResetPosSlot(i);
        }
    }

    //Tim bo 3 voi itemTile vua dc chuyen den slot
    public List<ItemTileSlot> FindMatch3_ItemTile_Slots(ItemTile _itemTile) {
        List<ItemTileSlot> listCheckMatch3_ItemTileSlot = new List<ItemTileSlot>();
        for (int i = 0; i < listItemSlots.Count; i++) {
            if (listItemSlots[i].itemTile.itemData.itemType == _itemTile.itemData.itemType) {
                listCheckMatch3_ItemTileSlot.Add(listItemSlots[i]);

                if (listCheckMatch3_ItemTileSlot.Count == 3) {
                    return listCheckMatch3_ItemTileSlot;
                }
            }
        }

        return listCheckMatch3_ItemTileSlot;
    }
    #endregion

    public IEnumerator CheckGameWin()
    {
        bool isGameWin = CheckGameFinished();
        if (isGameWin)
        {
            SoundManager.instance.PlaySound_Clear();
        }
        yield return new WaitForSeconds(0.5f);
        if (isGameWin) {
            SetGameWin();
        }
    }

    public bool CheckGameFinished() {
        for (int i = 0; i < listFloors.Count; i++) {
            if (listFloors[i].transform.childCount > 0) {
                return false;
            }
        }

        return true;
    }
    public void SetGameWin()
    {
        Debug.LogError("GAME WIN");
        //PlayerPrefs.SetInt("CURREN_LEVEL", GamePlayManager.instance.level + 1);

        GamePlayManager.instance.SetGameWin();
        
    }

    public bool CheckGameOver()
    {
        if (listItemSlots.Count < 7) {
            return false;
        }

        for (int i = 0; i < listItemSlots.Count; i++)
        {
            int countItem_ItemTile = CountItemTileSlot_Have_ItemData(listItemSlots[i].itemTile.itemData);
            if (countItem_ItemTile == 3) return false;
        }

        return true;
    }

    public bool IsItemTileMoveToSlot() {
        if (listItemSlots.Count >= 7)
        {
            return false;
        }
        return true;
    }


    public void SetGameOver() {

        Debug.LogError("GAME OVER");
        GamePlayManager.instance.SetGameLose();
        
    }


    public IEnumerator ShowSlotBG() {
        //SoundManager.instance.PlaySound_ShowBoard();
        slotBG.transform.DOLocalMoveY(-5f, 0f);
        yield return new WaitForSeconds(0.5f);
        slotBG.gameObject.SetActive(true);
        slotBG.transform.DOLocalMoveY(0, 0.5f).SetEase(Ease.OutQuart);
    }


    #region UNDO
    [Header("UNDO")]
    public List<ItemTileSlot> listCheckUndo_ItemTileSlots = new List<ItemTileSlot>();

    public bool CheckUndoAvaiable() {
        return listCheckUndo_ItemTileSlots.Count > 0;
    }
    public void SetUndo() {
        if (listCheckUndo_ItemTileSlots.Count > 0)
        {
            ItemTileSlot itemTileSlot_Undo = listCheckUndo_ItemTileSlots[listCheckUndo_ItemTileSlots.Count - 1];
            itemTileSlot_Undo.itemTile.transform.parent = listFloors[itemTileSlot_Undo.itemTile.floorIndex];
            //itemTileSlot_Undo.itemTile.SetTouch_Avaiable(true);
            itemTileSlot_Undo.itemTile.SetItemTile_Undo();
            listCheckUndo_ItemTileSlots.Remove(itemTileSlot_Undo);
            listItemSlots.Remove(itemTileSlot_Undo);
            Destroy(itemTileSlot_Undo.gameObject);

            StartCoroutine(SetListItemSlot_ResetPosition_Now());
        }
    }

    public void SetUndoAll() {
        for (int i = listCheckUndo_ItemTileSlots.Count - 1; i >= 0; i--) {
            ItemTileSlot itemTileSlot_Undo = listCheckUndo_ItemTileSlots[listCheckUndo_ItemTileSlots.Count - 1];
            itemTileSlot_Undo.itemTile.transform.parent = listFloors[itemTileSlot_Undo.itemTile.floorIndex];
            itemTileSlot_Undo.itemTile.SetItemTile_Undo_Now();

            listCheckUndo_ItemTileSlots.Remove(itemTileSlot_Undo);
            listItemSlots.Remove(itemTileSlot_Undo);
            Destroy(itemTileSlot_Undo.gameObject);
        } 
    }
    #endregion

    
    #region SHUFFLE
    [Header("SHUFFLE")]
    public List<ItemTile> listShuffle_ItemTiles = new List<ItemTile>();

    public void SetShuffle() {
        listShuffle_ItemTiles.Clear();

        for (int i = 0; i < listFloors.Count; i++) {
            foreach (Transform child in listFloors[i]) {
                ItemTile itemTile = child.GetComponent<ItemTile>();
                listShuffle_ItemTiles.Add(itemTile);
            }
        }

        List<ItemTile> listShuffle_ItemTiles_Temp = new List<ItemTile>(listShuffle_ItemTiles);
        List<ItemData> listShuffle_ItemDatas = new List<ItemData>();
        for (int i = 0; i < listShuffle_ItemTiles.Count; i++) {
            int index = Random.Range(0, listShuffle_ItemTiles_Temp.Count);
            listShuffle_ItemDatas.Add(listShuffle_ItemTiles_Temp[index].itemData);
            listShuffle_ItemTiles_Temp.RemoveAt(index);
        }

        for (int i = 0; i < listShuffle_ItemTiles.Count; i++) {
            listShuffle_ItemTiles[i].SetItemTile_Shuffle(listShuffle_ItemDatas[i]);

        }
    }
    #endregion

    #region SUGGEST
    [Header("SUGGEST")]
    public List<ItemTile> listSuggest_ItemTiles = new List<ItemTile>();
    [ShowInInspector]
    public Dictionary<Config.ITEM_TYPE, List<ItemTile>> dicItemTiles = new Dictionary<Config.ITEM_TYPE, List<ItemTile>>();

    //Bộ dic này là cả sáng cả mờ
    public Dictionary<Config.ITEM_TYPE, List<ItemTile>> dicItemTiles_ALL = new Dictionary<Config.ITEM_TYPE, List<ItemTile>>();
    
    public void SetSuggest() {
        listSuggest_ItemTiles.Clear();

        for (int i = 0; i < listFloors.Count; i++)
        {
            foreach (Transform child in listFloors[i])
            {
                ItemTile itemTile = child.GetComponent<ItemTile>();
                listSuggest_ItemTiles.Add(itemTile);
            }
        }
        Debug.Log("listSuggest_ItemTiles:"+ listSuggest_ItemTiles.Count);
        Split_ListSuggest_ItemTiles();

        List<ItemTile> listItemTileSuggests = AI_Suggest();
        Debug.Log("listItemTileSuggests:"+ listItemTileSuggests.Count);

        if (listItemTileSuggests.Count > 0) {
            GamePlayManager.instance.SetSuggestSuccess();
            StartCoroutine(SetSuggest_IEnumerator(listItemTileSuggests));
        }
    }


    public bool CheckSuggestAvaiable() {
        listSuggest_ItemTiles.Clear();

        for (int i = 0; i < listFloors.Count; i++)
        {
            foreach (Transform child in listFloors[i])
            {
                ItemTile itemTile = child.GetComponent<ItemTile>();
                listSuggest_ItemTiles.Add(itemTile);
            }
        }
        Debug.Log("listSuggest_ItemTiles:" + listSuggest_ItemTiles.Count);
        Split_ListSuggest_ItemTiles();

        List<ItemTile> listItemTileSuggests = AI_Suggest();
        Debug.Log("listItemTileSuggests:" + listItemTileSuggests.Count);

        if (listItemTileSuggests.Count > 0)
        {
            return true;
        }
        return false;
    }

    private IEnumerator SetSuggest_IEnumerator(List<ItemTile> listItemTileSuggests) {
        yield return new WaitForSeconds(0.02f);
        for (int i = 0; i < listItemTileSuggests.Count; i++) {
            listItemTileSuggests[i].SetItemTileSuggest();
            yield return new WaitForSeconds(0.1f);
        }
    }

    private void Split_ListSuggest_ItemTiles()
    {
        List<ItemTile> listTempSuggest_ItemTiles = new List<ItemTile>(listSuggest_ItemTiles);
        dicItemTiles.Clear();
        dicItemTiles_ALL.Clear();
        for (int i = listTempSuggest_ItemTiles.Count - 1; i >= 0; i--)
        {
            if (listTempSuggest_ItemTiles[i].GetTouch_Avaiable())
            {
                if (dicItemTiles.ContainsKey(listTempSuggest_ItemTiles[i].itemData.itemType))
                {
                    dicItemTiles[listTempSuggest_ItemTiles[i].itemData.itemType].Add(listTempSuggest_ItemTiles[i]);
                }
                else
                {
                    dicItemTiles.Add(listTempSuggest_ItemTiles[i].itemData.itemType, new List<ItemTile>() { listTempSuggest_ItemTiles[i] });
                }

            } 

            if (dicItemTiles_ALL.ContainsKey(listTempSuggest_ItemTiles[i].itemData.itemType))
            {
                dicItemTiles_ALL[listTempSuggest_ItemTiles[i].itemData.itemType].Add(listTempSuggest_ItemTiles[i]);
            }
            else
            {
                dicItemTiles_ALL.Add(listTempSuggest_ItemTiles[i].itemData.itemType, new List<ItemTile>() { listTempSuggest_ItemTiles[i] });
            }
        }
    }

    private List<ItemTile> listItemTile_ItemTileSlot_Checked = new List<ItemTile>();
    public List<ItemTile> AI_Suggest() {
        List<ItemTile> listItemTile_AI_Suggest = new List<ItemTile>();
        
        if (listItemSlots.Count > 0) {
            listItemTile_AI_Suggest.Clear();
            List<ItemData> listItemData_Checked = new List<ItemData>();
            for (int i=0; i< listItemSlots.Count;i++) {
                ItemData itemData = listItemSlots[i].itemTile.itemData;
                if (listItemData_Checked.Contains(itemData)) {
                    break;
                }
                listItemData_Checked.Add(itemData);

                int countItemData = CountItemTileSlot_Have_ItemData(itemData);

                if (countItemData == 1)
                {
                    if (listItemSlots.Count <= 5)
                    {
                        if (dicItemTiles.ContainsKey(itemData.itemType))
                        {
                            List<ItemTile> listItemTile_Have_ItemData = dicItemTiles[itemData.itemType];
                            if (listItemTile_Have_ItemData.Count >= 2)
                            {
                                listItemTile_AI_Suggest.Add(listItemTile_Have_ItemData[0]);
                                listItemTile_AI_Suggest.Add(listItemTile_Have_ItemData[1]);
                                return listItemTile_AI_Suggest;
                            }
                        }
                    }
                }
                else if(countItemData == 2)
                {
                    if (listItemSlots.Count <= 6)
                    {
                        if (dicItemTiles.ContainsKey(itemData.itemType))
                        {
                            List<ItemTile> listItemTile_Have_ItemData = dicItemTiles[itemData.itemType];
                            if (listItemTile_Have_ItemData.Count >= 1)
                            {
                                listItemTile_AI_Suggest.Add(listItemTile_Have_ItemData[0]);
                                return listItemTile_AI_Suggest;
                            }
                        }
                    }
                }
            }
        }
        //Neu ko tim dc bo nao thich hop voi cac item trong slot
        //->>> Tim 1 bo 3 moi de cho vao slot voi dieu kien so itemslot hien tai nho hon hoac bang 4
        if (listItemSlots.Count <= 4)
        {
            foreach (KeyValuePair<Config.ITEM_TYPE, List<ItemTile>> kvp in dicItemTiles)
            {
                if (kvp.Value.Count >= 3)
                {
                    listItemTile_AI_Suggest.Add(kvp.Value[0]);
                    listItemTile_AI_Suggest.Add(kvp.Value[1]);
                    listItemTile_AI_Suggest.Add(kvp.Value[2]);
                    return listItemTile_AI_Suggest;
                }
            }
        }
        else{
            //Neu so itemSlot lon hon 4

            //Neu số itemSlot >= 6
            if (listItemSlots.Count >= 6)
            {
                //Duyet cac bo 2 cua itemslot ->Tim 1 item mo cho xuong
                
                Debug.Log("Tim 1 item mo cho xuong");
                List<ItemData> listItemData_Checked = new List<ItemData>();
                for (int i = 0; i < listItemSlots.Count; i++)
                {
                    ItemData itemData = listItemSlots[i].itemTile.itemData;
                    if (listItemData_Checked.Contains(itemData))
                    {
                        break;
                    }
                    listItemData_Checked.Add(itemData);

                    int countItemData = CountItemTileSlot_Have_ItemData(itemData);

                    if (countItemData == 2)
                    {
                        Debug.Log("countItemData == 2");
                        Debug.Log("itemData:" + itemData.itemType.ToString());
                        //Tim 1 item o mo cho xuong item slot
                        List<ItemTile> listItemTile_Have_ItemData = dicItemTiles_ALL[itemData.itemType];
                        if (listItemTile_Have_ItemData.Count >= 1)
                        {

                            listItemTile_AI_Suggest.Add(listItemTile_Have_ItemData[listItemTile_Have_ItemData.Count - 1]);
                            return listItemTile_AI_Suggest;
                        }
                    }
                }
                //Nếu đang có 6 itemSlot mà ko tìm đc bộ 2 nào thì ko suggest đc
                return listItemTile_AI_Suggest;
            }
            else {
                //Kiem tra listItemSlot ==5 ko? Neu co tim 1 bo 2 item cả mờ và ko sáng cho xuống
                for (int i = 0; i < listItemSlots.Count; i++)
                {
                    //Ko tìm dc bộ 2 nào thì chắc chắn 5 item này là 5 cái khác nhau
                    ItemData itemData = listItemSlots[i].itemTile.itemData;
                    List<ItemTile> listItemTile_Have_ItemData = dicItemTiles_ALL[itemData.itemType];
                    if (listItemTile_Have_ItemData.Count >= 2)
                    {

                        listItemTile_AI_Suggest.Add(listItemTile_Have_ItemData[listItemTile_Have_ItemData.Count - 1]);
                        listItemTile_AI_Suggest.Add(listItemTile_Have_ItemData[listItemTile_Have_ItemData.Count - 2]);
                        return listItemTile_AI_Suggest;
                    }
                }
                return listItemTile_AI_Suggest;
            }

           
        }


        //Nếu vẫn ko tìm đc thì tìm 3 bộ cả sáng cả mờ cho xuống 
        //Với điều kiện là listItemSlot <=4
        if (listItemSlots.Count <= 4) {
            foreach (KeyValuePair<Config.ITEM_TYPE, List<ItemTile>> kvp in dicItemTiles_ALL)
            {
                if (kvp.Value.Count >= 3)
                {
                    listItemTile_AI_Suggest.Add(kvp.Value[0]);
                    listItemTile_AI_Suggest.Add(kvp.Value[1]);
                    listItemTile_AI_Suggest.Add(kvp.Value[2]);
                    return listItemTile_AI_Suggest;
                }
            }
        }
        return listItemTile_AI_Suggest;
    }

    public int CountItemTileSlot_Have_ItemData(ItemData itemData) {
        int countItemSlot_Have_ItemData = 0;
        for (int i = 0; i < listItemSlots.Count; i++) {
            if (listItemSlots[i].itemTile.itemData == itemData) {
                countItemSlot_Have_ItemData++;
            }
        }

        return countItemSlot_Have_ItemData;
    }


    private ItemTile Find_1_ItemTile(ItemTile itemTile) {
        return Find_ItemTile(itemTile);
    }

    private List<ItemTile> Find_2_ItemTile(ItemTile itemTile) {
        List<ItemTile> listTempSuggest_ItemTiles = new List<ItemTile>(listSuggest_ItemTiles);
        List<ItemTile> listResult_ItemTiles = new List<ItemTile>();
        for (int i = listTempSuggest_ItemTiles.Count - 1; i >= 0; i--)
        {
            if (listTempSuggest_ItemTiles[i].GetTouch_Avaiable())
            {
                if (listTempSuggest_ItemTiles[i].itemData.itemType == itemTile.itemData.itemType)
                {
                    listResult_ItemTiles.Add(itemTile);

                    if (listResult_ItemTiles.Count == 2) {
                        return listResult_ItemTiles;
                    }
                }
            }
        }
        return null;
    }


    

    //Tim 1 item tile co itemData giong itemTile hien tai
    private ItemTile Find_ItemTile(ItemTile itemTile) {
        for (int i = listSuggest_ItemTiles.Count-1; i >=0; i--) {
            if (listSuggest_ItemTiles[i].GetTouch_Avaiable())
            {
                if (listSuggest_ItemTiles[i].itemData.itemType == itemTile.itemData.itemType)
                {
                    return itemTile;
                }
            }
        }
        return null;
    }
    #endregion

    #region REVIVE

    public void Revive() {

        StartCoroutine(Revive_IEnumerator());
    }

    public IEnumerator Revive_IEnumerator() {
        yield return new WaitForSeconds(0.1f);
        SetUndoAll();
        yield return new WaitForSeconds(0.1f);
        SetShuffle();
    }
    #endregion


    #region TUT
    public List<ItemTile> listItemTile_Tutorials = new List<ItemTile>();

    public GameObject handGuild;

    public void ShowTut1_HandGuild_First()
    {
        Debug.Log("ShowTut1_HandGuild_StartShowTut1_HandGuild_Start");
        if (handGuild == null)
        {
            handGuild = Instantiate(Resources.Load("HandGuide"), gameObject.transform) as GameObject;
        }

        //float posX = listItemTile_Tutorials[0].transform.position.x;
        //Debug.Log("posX:" + posX);
        //float posY = listItemTile_Tutorials[0].transform.position.y;
        //Debug.Log("posY:" + posY);
        //Debug.LogError("AAAAAAAAA");
        handGuild.transform.localPosition = new Vector3(1.2f, 1.2f, 0);
    }
    public void ShowTut1_HandGuild()
    {
        Debug.Log("ShowTut1_HandGuildShowTut1_HandGuild");
        if (handGuild == null)
        {
            handGuild = Instantiate(Resources.Load("HandGuide"), gameObject.transform) as GameObject;
        }

        float posX = listItemTile_Tutorials[0].transform.position.x;
        Debug.Log("posX:"+ posX);
        float posY = listItemTile_Tutorials[0].transform.position.y;
        Debug.Log("posY:" + posY);
        Debug.LogError("AAAAAAAAA");
        handGuild.transform.position = new Vector3(posX, posY, 0);
    }

    public void SetNextTut1(ItemTile itemTile) {
        listItemTile_Tutorials.Remove(itemTile);

        if (listItemTile_Tutorials.Count > 0)
        {
            ShowTut1_HandGuild();
        }
        else {
            if (handGuild != null) Destroy(handGuild);

            GamePlayManager.instance.HideTut1();
            
            slotBG.GetComponent<SpriteRenderer>().sortingLayerName = "Default";
        }
    }
    #endregion
}
