using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class GamePlayManager : MonoBehaviour
{
    public static GamePlayManager instance;
    [Header("Canvas")]
    public CanvasScaler canvasScaler;
    [Header("Select LEvel")]
    public int level = 1;
    public bool isTest = false;
    //public int maxlevel = 10;
    [Header("Button PAUSE")]
    public BBUIButton btnPause;
    [Header("Button UNDO")]
    public BBUIButton btnUndo;
    public Text txtUndoCount;
    public Text txtUndoAdd;
    public GameObject objUndoBG;

    [Header("Button SUGGEST")]
    public BBUIButton btnSuggest;
    public Text txtSuggestCount;
    public Text txtSuggestAdd;
    public GameObject objSuggestBG;

    [Header("Button SHUFFLE")]
    public BBUIButton btnShuffle;
    public Text txtShuffleCount;
    public Text txtShuffleAdd;
    public GameObject objShuffleBG;

    [Header("Button WatchVideo")]
    public BBUIButton btnWatchVideo;

    [Header("Button PigBank")]
    public BBUIButton btnPigBank;

    [Header("Text Level")]
    public Text txtLevel;

    [Header("BG Footer")]
    public GameObject bgFooter;

    [Header("Star Group")]
    public StarGroup starGroup;

    public bool isPlayTest = false;

    public GameObject lockGroup;
    private void Awake()
    {
        instance = this;
        //Input.multiTouchEnabled = false;
        //Config.SetCount_ItemHelp(Config.ITEMHELP_TYPE.UNDO, 10);
        //Config.SetCount_ItemHelp(Config.ITEMHELP_TYPE.SUGGEST, 10);
        //Config.SetCount_ItemHelp(Config.ITEMHELP_TYPE.SHUFFLE, 10);

        // if (Config.CheckWideScreen()) {
        //     canvasScaler.matchWidthOrHeight = 0.5f;
        // }
    }
    // Start is called before the first frame update
    void Start()
    {
      //  AdmobManager.instance.HideBannerAd();
        lockGroup.SetActive(true);
        isRevive = false;

        btnPause.OnPointerClickCallBack_Completed.AddListener(TouchPause);
        btnUndo.OnPointerClickCallBack_Completed.AddListener(TouchUndo);
        btnSuggest.OnPointerClickCallBack_Completed.AddListener(TouchSuggest);
        btnShuffle.OnPointerClickCallBack_Completed.AddListener(TouchShuffle);
        btnWatchVideo.OnPointerClickCallBack_Completed.AddListener(TouchWatchVideo);
        btnPigBank.OnPointerClickCallBack_Completed.AddListener(TouchPiggyBank);

        //AudioManager.instance.Play("Bmg");

        if (!isTest)
        {
            if (Config.isSelectLevel)
            {
                Config.isSelectLevel = false;
                level = Config.currSelectLevel;
            }
            else
            {
                level = PlayerPrefs.GetInt(Config.CURR_LEVEL);
                //level = Config.currLevel;
            }
        }


        if (level > Config.MAX_LEVEL) {
            level = Config.MAX_LEVEL;
        }
       
        if (level == 0) level = 1;

        txtLevel.text = $"Level: {level}";
        Config.gameState = Config.GAME_STATE.START;
        LoadLevelGame();
        InitViews();
        // Banner_Ad();
    }

    public void Banner_Ad() => Ad_Manager.instance.Show_Banner();

    // Update is called once per frame
    void Update()
    {
        if (Config.gameState == Config.GAME_STATE.PLAYING) {
            if (level > 4 && !isShowAutoSuggest_Shuffle) {
                timeAutoSuggest_Shuffle += Time.deltaTime;

                if (timeAutoSuggest_Shuffle >= Config.AUTO_TIME_TO_SUGGEST_SHUFFLE) {
                    ShowAutoSuggest_Shuffle();
                }
            }
        }
    }

    private GameObject levelGame;
    public void LoadLevelGame()
    {
        if (level == 0) level = 1;
        levelGame = Instantiate(Resources.Load("NewLevel/Level" + level)) as GameObject;
        // levelGame.transform.position = new Vector3(0f,-0.5f,0f);
        Debug.Log(slotBGTranform.position);
        levelGame.GetComponent<GameLevelManager>().InitSlotPosition(slotBGTranform.position);

        starGroup.InitStarGroup(GameLevelManager.instance.configLevelGame);

        StartCoroutine(LoadLevelGame_IEnumerator());
    }

    private IEnumerator LoadLevelGame_IEnumerator()
    {
        yield return new WaitForSeconds(0.02f);
        Debug.Log(slotBGTranform.position);
        levelGame.GetComponent<GameLevelManager>().InitSlotPosition(slotBGTranform.position);
    }


    public void InitViews() {
        btnPause.gameObject.SetActive(false);
        btnUndo.gameObject.SetActive(false);
        btnSuggest.gameObject.SetActive(false);
        btnShuffle.gameObject.SetActive(false);
        starGroup.gameObject.SetActive(false);
        btnWatchVideo.gameObject.SetActive(false);
        txtLevel.gameObject.SetActive(false);
        btnPigBank.gameObject.SetActive(false);
        bgFooter.gameObject.SetActive(false);

        

        //txtUndoCount.text = $"{Config.GetCount_ItemHelp(Config.ITEMHELP_TYPE.UNDO)}";
        objUndoBG.gameObject.SetActive(true);

        //txtSuggestCount.text = $"{Config.GetCount_ItemHelp(Config.ITEMHELP_TYPE.SUGGEST)}";
        objSuggestBG.gameObject.SetActive(true);

        //txtShuffleCount.text = $"{Config.GetCount_ItemHelp(Config.ITEMHELP_TYPE.SHUFFLE)}";
        objShuffleBG.gameObject.SetActive(true);


        if (level >= 2)
        {
            btnUndo.Interactable = true;
            btnUndo.GetComponent<ButtonItemLockManager>().ShowButtonItem_Lock(false);
        }
        else {
            btnUndo.Interactable = false;
            btnUndo.GetComponent<ButtonItemLockManager>().ShowButtonItem_Lock(true);
        }


        if (level >= 3)
        {
            btnSuggest.Interactable = true;
            btnSuggest.GetComponent<ButtonItemLockManager>().ShowButtonItem_Lock(false);
        }
        else
        {
            btnSuggest.Interactable = false;
            btnSuggest.GetComponent<ButtonItemLockManager>().ShowButtonItem_Lock(true);
        }

        if (level >= 4)
        {
            btnShuffle.Interactable = true;
            btnShuffle.GetComponent<ButtonItemLockManager>().ShowButtonItem_Lock(false);
        }
        else
        {
            btnShuffle.Interactable = false;
            btnShuffle.GetComponent<ButtonItemLockManager>().ShowButtonItem_Lock(true);
        }

        SetUpdate_CountItem();

        InitViews_ShowView();
    }

    public void InitViews_ShowView() {
        
        Sequence sequenceShowView = DOTween.Sequence();
        sequenceShowView.InsertCallback(0.2f, () =>
        {
            SoundManager.instance.PlaySound_ShowView();
            


            bgFooter.gameObject.SetActive(true);
            bgFooter.GetComponent<BBUIView>().ShowView();

            btnPigBank.gameObject.SetActive(true);
            btnPigBank.GetComponent<BBUIView>().ShowView();

            btnUndo.gameObject.SetActive(true);
            btnUndo.GetComponent<BBUIView>().ShowView();
        });

        sequenceShowView.InsertCallback(0.5f, () =>
        {
            txtLevel.gameObject.SetActive(true);
            txtLevel.GetComponent<BBUIView>().ShowView();
            starGroup.gameObject.SetActive(true);
            starGroup.GetComponent<BBUIView>().ShowView();

            btnSuggest.gameObject.SetActive(true);
            btnSuggest.GetComponent<BBUIView>().ShowView();
        });

        sequenceShowView.InsertCallback(0.7f, () =>
        {
            btnPause.gameObject.SetActive(true);
            btnPause.GetComponent<BBUIView>().ShowView();


            btnWatchVideo.gameObject.SetActive(true);
            btnWatchVideo.GetComponent<BBUIView>().ShowView();


            btnShuffle.gameObject.SetActive(true);
            btnShuffle.GetComponent<BBUIView>().ShowView();
        });

        sequenceShowView.InsertCallback(1.5f, () => {
            InitViews_ShowView_Finished();
        });
    }

    public void InitViews_ShowView_Finished() {
        
    }

    public void SetStartPlayingGame() {
        Debug.Log("SetStartPlayingGameSetStartPlayingGame");
        Config.gameState = Config.GAME_STATE.PLAYING;

        if (Config.CheckTutorial_1())
        {
            ShowTut1();
        }

        if (Config.CheckTutorial_3())
        {
            ShowTut3();
        }
        if (Config.CheckTutorial_4())
        {
            ShowTut4();
        }

        Debug.Log("SetStartPlayingGame:"+level);
        Debug.Log("SetStartPlayingGame:"+ Config.GetTut_Finished(Config.TUT.TUT_TOUCHSPIN_LEVEL5));
        if (level == 5 && !Config.GetTut_Finished(Config.TUT.TUT_TOUCHSPIN_LEVEL5) && Config.CheckSpinAvaiable())//
        {
            Config.SetTut_Finished(Config.TUT.TUT_TOUCHSPIN_LEVEL5);
            //ShowTut_TouchSpin();
            OpenSpinPopup();
        }
        
        lockGroup.SetActive(false);

        // if (level >= 6) {
        //     if (Config.GetCount_ItemHelp(Config.ITEMHELP_TYPE.SUGGEST) == 0 || Config.GetCount_ItemHelp(Config.ITEMHELP_TYPE.SHUFFLE) == 0 || Config.GetCount_ItemHelp(Config.ITEMHELP_TYPE.UNDO) == 0)
        //     {
        //         Config.countShowFreeItem++;
        //     }
        //         
        //     if (Config.countShowFreeItem % Config.INTERVAL_SHOW_FREEITEM_POPUP == 1)
        //     {
        //         if (Config.GetCount_ItemHelp(Config.ITEMHELP_TYPE.SUGGEST) == 0)
        //         {
        //             OpenFreeItemPopup(Config.ITEMHELP_TYPE.SUGGEST);
        //         }
        //         else if (Config.GetCount_ItemHelp(Config.ITEMHELP_TYPE.SHUFFLE) == 0)
        //         {
        //             OpenFreeItemPopup(Config.ITEMHELP_TYPE.SHUFFLE);
        //         }
        //         else if (Config.GetCount_ItemHelp(Config.ITEMHELP_TYPE.UNDO) == 0)
        //         {
        //             OpenFreeItemPopup(Config.ITEMHELP_TYPE.UNDO);
        //         }
        //         
        //     }
        // }
    }

    #region PAUSE
    public void TouchPause() {
        Debug.LogError("GAME PAUSE");
        if (Config.gameState == Config.GAME_STATE.PLAYING)
        {
            Config.gameState = Config.GAME_STATE.PAUSE;
            pausePopup.OpenPausePopup(level);

            HideTut_HandGuide();
        }
    }

    public void SetUnPause() {
        Config.gameState = Config.GAME_STATE.PLAYING;
    }

    [Header("PAUSE POPUP")]
    public PausePopup2 pausePopup;

    #endregion

    #region UNDO
    public void TouchUndo()
    {
        Debug.LogError("Touch Undo");
        if (GameLevelManager.instance.CheckUndoAvaiable() && Config.GetCount_ItemHelp(Config.ITEMHELP_TYPE.UNDO) > 0)
        {
            GameLevelManager.instance.SetUndo();
            Config.SetCount_ItemHelp(Config.ITEMHELP_TYPE.UNDO, Config.GetCount_ItemHelp(Config.ITEMHELP_TYPE.UNDO) - 1);
            //txtUndoCount.text = $"{Config.GetCount_ItemHelp(Config.ITEMHELP_TYPE.UNDO)}";
            SetUpdate_CountItem();
            objUndoBG.gameObject.SetActive(true);


            if (Config.CheckTutorial_2() && Config.isShowTut2) {
                HideTut2();
            }
        }
        else if (Config.GetCount_ItemHelp(Config.ITEMHELP_TYPE.UNDO) == 0) {
            if (Config.currCoin >= Config.COIN_PRICE_ITEM)
            {
                OpenBuyItemPopup(Config.ITEMHELP_TYPE.UNDO);
            }
            else {
                
                OpenBuyItemPopup(Config.ITEMHELP_TYPE.UNDO);
                // OpenFreeItemPopup(Config.ITEMHELP_TYPE.UNDO);
            }
            
        }

        HideTut_HandGuide();
    }
    #endregion

    #region SUGGEST
    public void TouchSuggest()
    {
        Debug.LogError("Touch Suggest");
        if (Config.GetCount_ItemHelp(Config.ITEMHELP_TYPE.SUGGEST) > 0)
        {
            GameLevelManager.instance.SetSuggest();

            if (Config.CheckTutorial_3() && Config.isShowTut3)
            {
                HideTut3();
            }
            
        }
        else {
            if (Config.currCoin >= Config.COIN_PRICE_ITEM)
            {
                OpenBuyItemPopup(Config.ITEMHELP_TYPE.SUGGEST);
            }
            else
            {
                OpenBuyItemPopup(Config.ITEMHELP_TYPE.SUGGEST);
                // OpenFreeItemPopup(Config.ITEMHELP_TYPE.SUGGEST);
            }
            
        }

        HideTut_HandGuide();

    }

    public void SetSuggestSuccess() {
        Config.SetCount_ItemHelp(Config.ITEMHELP_TYPE.SUGGEST, Config.GetCount_ItemHelp(Config.ITEMHELP_TYPE.SUGGEST) - 1);
        //txtSuggestCount.text = $"{Config.GetCount_ItemHelp(Config.ITEMHELP_TYPE.SUGGEST)}";
        SetUpdate_CountItem();
        objSuggestBG.gameObject.SetActive(true);
    }
    #endregion

    #region SHUFFLE
    public void TouchShuffle()
    {
        Debug.LogError("Touch Shuffle");
        if (Config.GetCount_ItemHelp(Config.ITEMHELP_TYPE.SHUFFLE) > 0)
        {
            GameLevelManager.instance.SetShuffle();
            Config.SetCount_ItemHelp(Config.ITEMHELP_TYPE.SHUFFLE, Config.GetCount_ItemHelp(Config.ITEMHELP_TYPE.SHUFFLE) - 1);
            //txtShuffleCount.text = $"{Config.GetCount_ItemHelp(Config.ITEMHELP_TYPE.SHUFFLE)}";
            SetUpdate_CountItem();
            objShuffleBG.gameObject.SetActive(true);

            if (Config.CheckTutorial_4() && Config.isShowTut4)
            {
                HideTut4();
            }
        }
        else
        {
            
            if (Config.currCoin >= Config.COIN_PRICE_ITEM)
            {
                OpenBuyItemPopup(Config.ITEMHELP_TYPE.SHUFFLE);
            }
            else
            {
                OpenBuyItemPopup(Config.ITEMHELP_TYPE.SHUFFLE);
                // OpenFreeItemPopup(Config.ITEMHELP_TYPE.SHUFFLE);
            }
        }
        HideTut_HandGuide();
    }
    #endregion


    #region WIN
    public WinPopup2 winPopup;
    public void SetGameWin() {
        if (Config.gameState != Config.GAME_STATE.WIN)
        {
            SetFinishedGame();
            Config.gameState = Config.GAME_STATE.WIN;
            
            winPopup.ShowWinPopup(level,starGroup.GetCurrStar(), GameLevelManager.instance.configLevelGame.listRewards_CoinValue[starGroup.GetCurrStar()]);

            Config.SetCurrLevel(level + 1);
            Config.currSelectLevel = Config.currLevel;

            if (level >= 15 && level % 5 == 0 && !Config.GetRate()) {
                OpenRatePopup();
            }
        }
    }


    public void SetNextGame() {
        SceneManager.LoadScene("Play");
    }

    #endregion


    private void SetFinishedGame() {
        btnPause.Interactable = false;
        btnPigBank.Interactable = false;
        btnWatchVideo.Interactable = false;
        btnShuffle.Interactable = false;
        btnUndo.Interactable = false;
        btnSuggest.Interactable = false;
    }


    #region LOSE
    public LosePopup2 losePopup;
    public void SetGameLose()
    {
        if (Config.gameState != Config.GAME_STATE.LOSE)
        {
            SetFinishedGame();
            Config.gameState = Config.GAME_STATE.LOSE;
            losePopup.ShowLosePopup(level, isRevive);
        }
    }

    public void SetReplayGame() {
        SceneManager.LoadScene("Play");
    }
    #endregion


    #region STAR_GROUP
    public void SetAddScore() {
        starGroup.AddScore();
    }
    #endregion

    #region FREE_ITEM
    [Header("Free ITem POPUP")]
    public FreeItemPopup freeItemPopup;

    public void OpenFreeItemPopup(Config.ITEMHELP_TYPE itemHelpType) {
        Config.gameState = Config.GAME_STATE.SHOP;
        freeItemPopup.OpenFreeItemPopup(itemHelpType);
    }
    public void SetFreeItem_Success() {
        Config.countShowFreeItem = 0;
        Debug.Log("SetFreeItem_SuccessSetFreeItem_Success");
        SetUpdate_CountItem();
    }
    #endregion


    #region BUY_ITEM
    public BuyItemPopup buyItemPopup;
    public void OpenBuyItemPopup(Config.ITEMHELP_TYPE itemHelpType)
    {
        Config.gameState = Config.GAME_STATE.SHOP;
        buyItemPopup.OpenBuyItemPopup(itemHelpType);
    }
    #endregion
    #region SPIN
    [Header("SPIN POPUP")]
    public SpinPopup spinPopup;

    public void TouchWatchVideo() {
        if (Config.gameState == Config.GAME_STATE.PLAYING)
        {
            OpenSpinPopup();

            HideTut_HandGuide();
        }
    }

    public void OpenSpinPopup()
    {
        Config.gameState = Config.GAME_STATE.SHOP;
        spinPopup.OpenSpinPopup();
    }


    #endregion

    public void test_button()
    {
       Config.SetCount_ItemHelp(Config.ITEMHELP_TYPE.UNDO, Config.GetCount_ItemHelp(Config.ITEMHELP_TYPE.UNDO) + 20);
       SetUpdate_CountItem();
    }

    public void SetUpdate_CountItem()
    {
        Debug.Log("SetFreeItem_SuccessSetFreeItem_Success");
        txtUndoCount.text = $"{Config.GetCount_ItemHelp(Config.ITEMHELP_TYPE.UNDO)}";
        txtUndoCount.gameObject.SetActive(true);
        txtUndoAdd.gameObject.SetActive(false);
        if (Config.GetCount_ItemHelp(Config.ITEMHELP_TYPE.UNDO) == 0)
        {
            txtUndoAdd.gameObject.SetActive(true);
            txtUndoCount.gameObject.SetActive(false);
            //btnUndo.GetComponent<ButtonItemLockManager>().ShowAddIcon();
            txtUndoCount.text = $"+";
        }
        else {
            if (level >= 2)
            {
                btnUndo.GetComponent<ButtonItemLockManager>().ShowButtonItem_Lock(false);
            }
            else {
                btnUndo.GetComponent<ButtonItemLockManager>().ShowButtonItem_Lock(true);
            }
        }
        txtSuggestCount.text = $"{Config.GetCount_ItemHelp(Config.ITEMHELP_TYPE.SUGGEST)}";
        txtSuggestCount.gameObject.SetActive(true);
        txtSuggestAdd.gameObject.SetActive(false);
        if (Config.GetCount_ItemHelp(Config.ITEMHELP_TYPE.SUGGEST) == 0)
        {
            txtSuggestAdd.gameObject.SetActive(true);
            txtSuggestCount.gameObject.SetActive(false);
            //btnSuggest.GetComponent<ButtonItemLockManager>().ShowAddIcon();
            txtSuggestCount.text = $"+";
        }
        else
        {
            if (level >= 3)
            {
                btnSuggest.GetComponent<ButtonItemLockManager>().ShowButtonItem_Lock(false);
            }
            else {
                btnSuggest.GetComponent<ButtonItemLockManager>().ShowButtonItem_Lock(true);
            }
        }
        txtShuffleCount.text = $"{Config.GetCount_ItemHelp(Config.ITEMHELP_TYPE.SHUFFLE)}";
        txtShuffleCount.gameObject.SetActive(true);
        txtShuffleAdd.gameObject.SetActive(false);
        if (Config.GetCount_ItemHelp(Config.ITEMHELP_TYPE.SHUFFLE) == 0)
        {
            txtShuffleAdd.gameObject.SetActive(true);
            txtShuffleCount.gameObject.SetActive(false);
            //btnShuffle.GetComponent<ButtonItemLockManager>().ShowAddIcon();
            txtShuffleCount.text = $"+";
        }
        else
        {
            if (level >= 4)
            {
                btnShuffle.GetComponent<ButtonItemLockManager>().ShowButtonItem_Lock(false);
            }
            else {
                btnShuffle.GetComponent<ButtonItemLockManager>().ShowButtonItem_Lock(true);
            }
        }


        if (Config.GetCount_ItemHelp(Config.ITEMHELP_TYPE.SUGGEST) != 0 && Config.GetCount_ItemHelp(Config.ITEMHELP_TYPE.SHUFFLE) != 0 && Config.GetCount_ItemHelp(Config.ITEMHELP_TYPE.UNDO) != 0)
        {
            Config.countShowFreeItem = 0;
        }

    }

    public void CloseShopSucces() {
        Config.gameState = Config.GAME_STATE.PLAYING;
    }


    #region CHESTPOPUP
    [Header("CHEST POPUP")]
    public ChestPopup chestPopup;
    public void OpenChestPopup(bool isFullStar) {
        chestPopup.OpenChestPopup(isFullStar);
        if (WinPopup.instance.isActiveAndEnabled) {
            WinPopup.instance.SetDisableNativeAd();
        }
    }

    public void CloseChestPopup() {
        if (WinPopup.instance.isActiveAndEnabled)
        {
            WinPopup.instance.SetEnableNativeAd();
        }
    }
    #endregion

    #region REWARDPOPUP
    [Header("REWARD POPUP")]
    public RewardPopup rewardPopup;

    public void OpenRewardPopup(List<ConfigItemShopData> _listDatas, bool _isShowCollectx2 = true)
    {
        Config.gameState = Config.GAME_STATE.WIN;
        Debug.Log("OpenRewardPopup:"+_listDatas.Count);
        rewardPopup.OpenRewardPopup(_listDatas, _isShowCollectx2);
    }

    public void SetRewardPpopup_Finished()
    {
        Config.gameState = Config.GAME_STATE.PLAYING;
    }

    #endregion


    #region PIGGYBANK_COIN
    [Header("PIGGYBANK POPUP")]
    public PiggyBankPopup piggyBankPopup;
    public void TouchPiggyBank() {
        if (Config.gameState == Config.GAME_STATE.PLAYING)
        {
            OpenPiggyBankPopup();

            HideTut_HandGuide();
        }
    }
    public void OpenPiggyBankPopup()
    {
        Config.gameState = Config.GAME_STATE.SHOP;
        piggyBankPopup.OpenPiggyBank();
    }


    #endregion


    #region REVIVE
    bool isRevive = false;
    public void SetRevive_Success() {
        isRevive = true;
        starGroup.Revive_InitStarGroup();

        Config.gameState = Config.GAME_STATE.PLAYING;


        btnPause.Interactable = true;
        btnPigBank.Interactable = true;
        btnWatchVideo.Interactable = true;
        btnShuffle.Interactable = true;
        btnUndo.Interactable = true;
        btnSuggest.Interactable = true;
    }
    #endregion



    #region TUTORIAL
    [Header("Tutorial")]
   
    public GameObject tut1Obj;

    bool isFisrtShowTut1 = true;
    public void ShowTut1() {
        tut1Obj.gameObject.SetActive(true);

        //if (isFisrtShowTut1)
        //{
        //    isFisrtShowTut1 = false;
        //    GameLevelManager.instance.ShowTut1_HandGuild_First();
        //}
        //else {
        //    GameLevelManager.instance.ShowTut1_HandGuild();
        //}
        GameLevelManager.instance.ShowTut1_HandGuild();
    }


    public void HideTut1() {
        tut1Obj.gameObject.SetActive(false);
        Config.SetTut_1_Finished();
    }


    public GameObject tut2Obj;
    private GameObject handGuild;
    public void ShowTut2_HandGuild()
    {
        if (handGuild == null)
        {
            handGuild = Instantiate(Resources.Load("HandGuide2"), gameObject.transform) as GameObject;
        }

        float posX = btnUndo.transform.position.x;
        float posY = btnUndo.transform.position.y;
        handGuild.transform.position = new Vector3(posX, posY, 0);
    }

    public void ShowTut2()
    {
        Config.isShowTut2 = true;
        btnUndo.gameObject.GetComponent<RectTransform>().SetAsLastSibling();
        tut2Obj.gameObject.SetActive(true);

        ShowTut2_HandGuild();
    }


    public void HideTut2()
    {
        if (handGuild != null) Destroy(handGuild);
        tut2Obj.gameObject.SetActive(false);
        btnUndo.gameObject.GetComponent<RectTransform>().SetSiblingIndex(1);
        Config.isShowTut2 = false;


        Config.SetTut_2_Finished();
    }

    //Tut3
    public GameObject tut3Obj;
    public void ShowTut3_HandGuild()
    {
        if (handGuild == null)
        {
            handGuild = Instantiate(Resources.Load("HandGuide2"), gameObject.transform) as GameObject;
        }

        float posX = btnSuggest.transform.position.x;
        float posY = btnSuggest.transform.position.y;
        handGuild.transform.position = new Vector3(posX, posY, 0);
    }

    public void ShowTut3()
    {
        Config.isShowTut3 = true;
        btnSuggest.gameObject.GetComponent<RectTransform>().SetAsLastSibling();
        tut3Obj.gameObject.SetActive(true);

        ShowTut3_HandGuild();
    }


    public void HideTut3()
    {
        HideTut_HandGuide();
        tut3Obj.gameObject.SetActive(false);
        btnSuggest.gameObject.GetComponent<RectTransform>().SetSiblingIndex(2);
        Config.isShowTut3 = false;


        Config.SetTut_3_Finished();
    }




    //Tut4
    public GameObject tut4Obj;
    public void ShowTut4_HandGuild()
    {
        if (handGuild == null)
        {
            handGuild = Instantiate(Resources.Load("HandGuide2"), gameObject.transform) as GameObject;
        }

        float posX = btnShuffle.transform.position.x;
        float posY = btnShuffle.transform.position.y;
        handGuild.transform.position = new Vector3(posX, posY, 0);
    }

    
    public void ShowTut4()
    {
        Config.isShowTut4 = true;
        btnShuffle.gameObject.GetComponent<RectTransform>().SetAsLastSibling();
        tut4Obj.gameObject.SetActive(true);

        ShowTut4_HandGuild();
    }

    
    public void HideTut4()
    {
        HideTut_HandGuide();
        tut4Obj.gameObject.SetActive(false);
        btnShuffle.gameObject.GetComponent<RectTransform>().SetSiblingIndex(3);
        Config.isShowTut4 = false;


        Config.SetTut_4_Finished();
    }

    public void HideTut_HandGuide()
    {
        if (handGuild != null) Destroy(handGuild);
        ResetTimeAutoSuggest();
    }



    public void ShowTut_NextLevel(Vector3 pos) {
        if (!Config.GetTut_Finished(Config.TUT.TUT_NEXTLEVEL_LEVEL1))
        {
            Config.SetTut_Finished(Config.TUT.TUT_NEXTLEVEL_LEVEL1);
            if (handGuild == null)
            {
                handGuild = Instantiate(Resources.Load("HandGuide"), gameObject.transform) as GameObject;
                handGuild.transform.position = pos;
            }
        }
    }


    public void ShowTut_TouchSpin()
    {
        if (handGuild == null)
        {
            handGuild = Instantiate(Resources.Load("HandGuide"), gameObject.transform) as GameObject;
            handGuild.transform.position = btnWatchVideo.transform.position;
        }
    }
    #endregion


    #region RATEPOPUP
    public RatePopup ratePopup;

    public void OpenRatePopup() {
        StartCoroutine(OpenRatePopup_IEnumerator());
    }

    public IEnumerator OpenRatePopup_IEnumerator()
    {
        yield return new WaitForSeconds(0.5f);
        ratePopup.OpenRatePopup();
    }
    #endregion


    #region SLOT BG
    public Transform slotBGTranform;

    public void SetSlotBGPos() {
        Debug.Log(slotBGTranform.position);
    }
    #endregion


    #region AUTO SHUFFLE SUGGEST
    private float timeAutoSuggest_Shuffle = 0;
    private bool isShowAutoSuggest_Shuffle = false;


    public void ResetTimeAutoSuggest() {
        timeAutoSuggest_Shuffle = 0;
        isShowAutoSuggest_Shuffle = false;
    }

    public void ShowAutoSuggest_Shuffle() {
        isShowAutoSuggest_Shuffle = true;   
        timeAutoSuggest_Shuffle = 0;
        if (SpinPopup.instance != null && SpinPopup.instance.isActiveAndEnabled)
        {
            return;
        }
        if (PiggyBankPopup.instance != null && PiggyBankPopup.instance.isActiveAndEnabled)
        {
            return;
        }
        
        if (FreeItemPopup.instance != null && FreeItemPopup.instance.isActiveAndEnabled)
        {
            return;
        }
        
        if (ShopPopup2.instance != null && ShopPopup2.instance.isActiveAndEnabled)
        {
            return;
        }

        if (GameLevelManager.instance.CheckSuggestAvaiable() && Config.GetCount_ItemHelp(Config.ITEMHELP_TYPE.SUGGEST) > 0) {
            ShowTut3_HandGuild();
            return;
        }

        if (Config.GetCount_ItemHelp(Config.ITEMHELP_TYPE.SHUFFLE) > 0)
        {
            ShowTut4_HandGuild();
            return;
        }

        ShowTut3_HandGuild();
    }
    #endregion



    #region SHOP

    [Header("SHOP")]
    public ShopPopup2 shopPopup;

    public void OpenShopPopup()
    {
        //shopPopup.OpenPopup();
    }

    #endregion
}
