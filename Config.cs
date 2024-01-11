using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Config
{
    public enum THEME_TYPE {
        fruits,
        test
    }
    public static bool isShowStarterPack = true;
    public static THEME_TYPE currTheme = THEME_TYPE.fruits;

    public enum ITEM_TYPE {
        ITEM_1 = 1,
        ITEM_2 = 2,
        ITEM_3 = 3,
        ITEM_4 = 4,
        ITEM_5 = 5,
        ITEM_6 = 6,
        ITEM_7 = 7,
        ITEM_8 = 8,
        ITEM_9 = 9,
        ITEM_10 = 10,
        ITEM_11 = 11,
        ITEM_12 = 12,
        ITEM_13 = 13,
        ITEM_14 = 14,
        ITEM_15 = 15,
        ITEM_16 = 16,
        ITEM_17 = 17,
        ITEM_18 = 18,
        ITEM_19 = 19,
        ITEM_20 = 20
    }

    public enum START_MOVE_TYPE {
        TOP,
        LEFT,
        RIGHT,
        BOTTOM
    }

    public enum ITEMTILE_STATE {
        START,
        START_TO_FLOOR,
        FLOOR,
        HOVER,
        MOVE_TO_SLOT,
        SLOT
    }

    public static List<START_MOVE_TYPE> listStartMoveType = new List<START_MOVE_TYPE>();

    public enum GAME_STATE
    {
        START,
        PLAYING,
        SHOP,
        PAUSE,
        END,
        WIN,
        LOSE,
        REWARD
    }

    public static GAME_STATE gameState = Config.GAME_STATE.START;

    public enum ITEMHELP_TYPE
    {
        UNDO,
        SUGGEST,
        SHUFFLE
    }


    public static void SetCount_ItemHelp(ITEMHELP_TYPE itemHelpType, int count) {
        PlayerPrefs.SetInt(itemHelpType.ToString(), count);
        PlayerPrefs.Save();
    }

    public static int GetCount_ItemHelp(ITEMHELP_TYPE itemHelpType) {
        if(itemHelpType == ITEMHELP_TYPE.UNDO)
            return PlayerPrefs.GetInt(itemHelpType.ToString(), 3);
        if (itemHelpType == ITEMHELP_TYPE.SHUFFLE)
            return PlayerPrefs.GetInt(itemHelpType.ToString(), 3);
        else
            return PlayerPrefs.GetInt(itemHelpType.ToString(), 3);
    }

    #region COIN
    public const string COIN = "coin";
    public static event Action<int> OnChangeCoin = delegate (int _coin) { };
    public static int currCoin;
    public static void SetCoin(int coinValue) {
        PlayerPrefs.SetInt(COIN, coinValue);
        PlayerPrefs.Save();
        currCoin = coinValue;
        OnChangeCoin(coinValue);
    }

    public static int GetCoin() {
        return PlayerPrefs.GetInt(COIN, 100);
    }
    #endregion

    #region SHOP
    public enum SHOPITEM
    {
        COIN,
        UNDO,
        SUGGEST,
        SHUFFLE
    }

    public static void BuySucces_ItemShop(ConfigItemShopData itemShopData) {
        if (itemShopData.shopItemType == SHOPITEM.UNDO) {
            SetCount_ItemHelp(ITEMHELP_TYPE.UNDO, GetCount_ItemHelp(ITEMHELP_TYPE.UNDO) + itemShopData.countItem);
        }
        else if (itemShopData.shopItemType == SHOPITEM.SHUFFLE)
        {
            SetCount_ItemHelp(ITEMHELP_TYPE.SHUFFLE, GetCount_ItemHelp(ITEMHELP_TYPE.SHUFFLE) + itemShopData.countItem);
        }
        else if (itemShopData.shopItemType == SHOPITEM.SUGGEST)
        {
            SetCount_ItemHelp(ITEMHELP_TYPE.SUGGEST, GetCount_ItemHelp(ITEMHELP_TYPE.SUGGEST) + itemShopData.countItem);
        }
        else if (itemShopData.shopItemType == SHOPITEM.COIN)
        {
            SetCoin(currCoin + itemShopData.countItem);
        }
    }


    public static SHOPITEM ConvertItemHelpTypeToShopItem(ITEMHELP_TYPE itemHelpType) {
        if (itemHelpType == ITEMHELP_TYPE.UNDO)
        {
            return SHOPITEM.UNDO;
        }
        else if (itemHelpType == ITEMHELP_TYPE.SUGGEST)
        {
            return SHOPITEM.SUGGEST;
        }
        else {
            return SHOPITEM.SHUFFLE;
        }
    }

    public static int GetCountItem_Pack_ItemType(SHOPITEM shopItemType, ConfigPackData configPackData) {
        for (int i = 0; i < configPackData.configItemShopDatas.Count; i++) {
            if (configPackData.configItemShopDatas[i].shopItemType == shopItemType) {
                return configPackData.configItemShopDatas[i].countItem;
            }
        }
        return 0;
    }
    #endregion


    public const string SOUND = "sound";
    public static bool isSound = true;
    public static void SetSound(bool _isSound) {
        isSound = _isSound;
        if (_isSound)
        {
            PlayerPrefs.SetInt(SOUND, 1);
        }
        else {
            PlayerPrefs.SetInt(SOUND, 0);
        }
        PlayerPrefs.Save();
    }

    public static void GetSound() {
        int soundInt = PlayerPrefs.GetInt(SOUND, 1);
        if (soundInt == 1)
        {
            isSound = true;
        }
        else {
            isSound = false;
        }
    }


    public const string MUSIC = "music";
    public static bool isMusic = true;
    public static void SetMusic(bool _isMusic)
    {
        isMusic = _isMusic;
        if (_isMusic)
        {
            PlayerPrefs.SetInt(MUSIC, 1);
        }
        else
        {
            PlayerPrefs.SetInt(MUSIC, 0);
        }
        PlayerPrefs.Save();
    }

    public static void GetMusic()
    {
        int musicInt = PlayerPrefs.GetInt(MUSIC, 1);
        if (musicInt == 1)
        {
            isMusic = true;
        }
        else
        {
            isMusic = false;
        }
    }


    #region SPIN_LASTTIME

    public static int SPIN_PRICE = 150;
    public static long GetTimeStamp()
    {

        return (long)(DateTime.UtcNow.Subtract(new DateTime(2020, 1, 1))).TotalSeconds;
    }

    public const int TIME_SPIN = 24 * 60 * 60;//1 * 60 * 60;
    public const string SPIN_LASTTIME = "spin_lastTime";
    public static event Action OnChangeSpin_LastTime = delegate () { };
    public static void SetSpin_LastTime() {
        long timeStamp = GetTimeStamp();
        PlayerPrefs.SetString(SPIN_LASTTIME, timeStamp + "");
        PlayerPrefs.Save();

        OnChangeSpin_LastTime();
    }

    public static long GetSpinLastTime() {
        string lastTime = PlayerPrefs.GetString(SPIN_LASTTIME, "0");
        return long.Parse(lastTime);
    }

    public static bool CheckSpinAvaiable()
    {
        long currTimeLastTime = Config.GetSpinLastTime();
        if (currTimeLastTime + Config.TIME_SPIN < Config.GetTimeStamp())
        {
            return true;
        }

        return false;
    }
    #endregion

    #region COUNTDOWNTIME
    public enum COUNTDOWN_TIME_TYPE
    {
        END
    }
    #endregion

    #region PIGGYBANK
    public const string PIGGYBANK = "piggyBank";
    public static int currPiggyBankCoin = 0;
    public static int ADD_PIGGYBANK_COIN = 5;
    public static int PIGGYBANK_COIN_MAX = 2000;
    public static int PIGGYBANK_COIN_MIN = 1000;
    public static int PIGGYBANK_COIN_xVALUE = 1;
    public static event Action OnChange_PiggyBank_Coin = delegate () { };
    public static void AddPiggyBank_Coin()
    {
        if (currPiggyBankCoin <= PIGGYBANK_COIN_MAX) {
            currPiggyBankCoin += ADD_PIGGYBANK_COIN;
            SetPiggyBank(currPiggyBankCoin);
        }
    }
    public static void SetPiggyBank(int piggyBankCoin) {

        currPiggyBankCoin = piggyBankCoin;
        PlayerPrefs.SetInt(PIGGYBANK, piggyBankCoin);
        PlayerPrefs.Save();

        OnChange_PiggyBank_Coin();
    }

    public static int GetPiggyBank() {
        return PlayerPrefs.GetInt(PIGGYBANK, 0);
    }


    #endregion


    #region STAR_CHEST
    public const string CHEST_STAR = "chest_countStar";
    public static int CHEST_STAR_MAX = 15;
    public static void SetChestCountStar(int _countStar) {
        if (_countStar >= CHEST_STAR_MAX) _countStar = CHEST_STAR_MAX;
        PlayerPrefs.SetInt(CHEST_STAR, _countStar);
        PlayerPrefs.Save();
    }

    public static int GetChestCountStar() {
        return PlayerPrefs.GetInt(CHEST_STAR, 0);
    }
    #endregion

    #region LEVEL_STAR
    public const string LEVEL_STAR = "levelStar";
    public static Dictionary<int, int> currDic_LevelStars = new Dictionary<int, int>();

    public static void SetLevelStar(int level, int star)
    {

        if (currDic_LevelStars.ContainsKey(level))
        {
            if (currDic_LevelStars[level] < star)
            {
                currDic_LevelStars[level] = star;
            }
        }
        else {
            currDic_LevelStars.Add(level, star);
        }

        List<string> listStrLevel = new List<string>();
        foreach (KeyValuePair<int, int> kvp in currDic_LevelStars) {
            listStrLevel.Add(kvp.Key + "_" + kvp.Value);
        }
        Debug.Log(JsonMapper.ToJson(listStrLevel));
        PlayerPrefs.SetString(LEVEL_STAR, JsonMapper.ToJson(listStrLevel));
        PlayerPrefs.Save();
    }

    public static void GetLevelStar() {
        currDic_LevelStars.Clear();
        if (PlayerPrefs.HasKey(LEVEL_STAR)) {
            string strlevelStar = PlayerPrefs.GetString(LEVEL_STAR);

            JsonData jsonData = JsonMapper.ToObject(strlevelStar);

            for (int i = 0; i < jsonData.Count; i++)
            {
                string str = jsonData[i].ToString();
                string[] strLevelStar = str.Split('_');
                currDic_LevelStars.Add(int.Parse(strLevelStar[0]), int.Parse(strLevelStar[1]));
            }
        }
    }
    public static int LevelStar(int level)
    {
        int stars = 0;
        if (!currDic_LevelStars.TryGetValue(level, out stars))
        {
            stars = 0;
        }
        return stars;
    }

    public static int GetCountStars() {
        int countStar = 0;
        foreach (KeyValuePair<int, int> kvp in currDic_LevelStars)
        {
            countStar += kvp.Value;
        }
        return countStar;
    }
    #endregion

    #region CURR_LEVEL
    public static bool isHackMode = false;
    public const int MAX_LEVEL = 1000;
    public static bool isSelectLevel = false;
    public static int currSelectLevel = 1;
    public const string CURR_LEVEL = "CURR_LEVEL";

    public static int currLevel = 1;

    public static void SetCurrLevel(int _currLevel) {
        if (_currLevel > currLevel)
        {
            currLevel = _currLevel;
            PlayerPrefs.SetInt(CURR_LEVEL, _currLevel);
            PlayerPrefs.Save();
        }
    }

    public static void GetCurrLevel() {
        currLevel = PlayerPrefs.GetInt(CURR_LEVEL, 1);
    }
    #endregion


    #region TUTORIAL
    public const string TUT_1 = "tut_1";
    public static void SetTut_1_Finished() {
        PlayerPrefs.SetInt(TUT_1, 1);
        PlayerPrefs.Save();
    }
    public static bool GetTut_1_Finished() {
        if (PlayerPrefs.GetInt(TUT_1, 0) == 1) {
            return true;
        }
        return false;
    }



    public static bool CheckTutorial_1() {
        if (GamePlayManager.instance.level == 1 && !GetTut_1_Finished()) {
            return true;
        }
        return false;
    }


    public static bool isShowTut2 = false;
    public const string TUT_2 = "tut_2";
    public static void SetTut_2_Finished()
    {
        PlayerPrefs.SetInt(TUT_2, 1);
        PlayerPrefs.Save();
    }
    public static bool GetTut_2_Finished()
    {
        if (PlayerPrefs.GetInt(TUT_2, 1) == 1)
        {
            return true;
        }
        return false;
    }


    public static bool CheckTutorial_2()
    {
        if (GamePlayManager.instance.level == 2 && !GetTut_2_Finished())
        {
            return true;
        }
        return false;
    }

    //TUt3
    public static bool isShowTut3 = false;
    public const string TUT_3 = "tut_3";
    public static void SetTut_3_Finished()
    {
        PlayerPrefs.SetInt(TUT_3, 1);
        PlayerPrefs.Save();
    }
    public static bool GetTut_3_Finished()
    {
        if (PlayerPrefs.GetInt(TUT_3, 0) == 1)
        {
            return true;
        }
        return false;
    }


    public static bool CheckTutorial_3()
    {
        if (GamePlayManager.instance.level == 3 && !GetTut_3_Finished())
        {
            return true;
        }
        return false;
    }


    //TUt4
    public static bool isShowTut4 = false;
    public const string TUT_4 = "tut_4";
    public static void SetTut_4_Finished()
    {
        PlayerPrefs.SetInt(TUT_4, 1);
        PlayerPrefs.Save();
    }
    public static bool GetTut_4_Finished()
    {
        if (PlayerPrefs.GetInt(TUT_4, 0) == 1)
        {
            return true;
        }
        return false;
    }


    public static bool CheckTutorial_4()
    {
        if (GamePlayManager.instance.level == 4 && !GetTut_4_Finished())
        {
            return true;
        }
        return false;
    }


    public enum TUT { 
        TUT_NEXTLEVEL_LEVEL1,
        TUT_TOUCHSPIN_LEVEL5
    }


    public static void SetTut_Finished(TUT tut)
    {
        PlayerPrefs.SetInt(tut.ToString(), 1);
        PlayerPrefs.Save();
    }

    public static bool GetTut_Finished(TUT tut)
    {
        if (PlayerPrefs.GetInt(tut.ToString(), 0) == 1)
        {
            return true;
        }
        return false;
    }
    #endregion



    #region ADS_INTERSTITIAL
    public const int interstitialAd_levelShowAd = 2;
    public const int interstitialAd_SHOW_WIN_INTERVAL = 2;
    public const int interstitialAd_SHOW_LOSE_INTERVAL = 2;
    public static int interstitialAd_countWin = 0;
    public static int interstitialAd_countLose = 0;
    public static int interstitialAd_countRestart = 0;
    public const int interstitialAd_SHOW_PAUSE_INTERVAL = 1;
    public static int interstitialAd_countPause = 0;
    #endregion

    #region IAP

    public enum IAP_ID
    {
        tileworld_starter_pack,
        piggy_bank,
        removead,
        removead_and_combo10,
        combo_10,
        undo_10,
        suggest_10,
        shuffle_10,
        coin_1000,
        special_offer,
        mega_bundle,
        master_bundle,
        coin_150,
        coin_500,
        tile_special_offer,
    }
    
    #region IAP
    public const string IAP = "iap_";

    public static void SetBuyIAP(IAP_ID idPack) {
        PlayerPrefs.SetInt(IAP + idPack.ToString(), 1);
        PlayerPrefs.Save();
    }

    public static bool GetBuyIAP(IAP_ID idPack) {
        int buyed = PlayerPrefs.GetInt(IAP + idPack.ToString(), 0);
        if (buyed == 1) return true;
        return false;
    }
    #endregion


#endregion

    public static bool CheckWideScreen() {
        if (Screen.width * 1f / Screen.height > 1242f / 2208f) {
            return true;
        }
        return false;
    }

#region RATE
    public const string RATE = "rate";
    public static void SetRate() {
        PlayerPrefs.SetInt(RATE, 1);
        PlayerPrefs.Save();
    }

    public static bool GetRate() {
        if (PlayerPrefs.GetInt(RATE, 0) == 1) return true;
        return false;
    }
#endregion

#region REMOVE_AD
    public const string REMOVE_AD = "remove_Ad";
    public static void SetRemoveAd()
    {

        PlayerPrefs.SetInt(REMOVE_AD, 1);
        PlayerPrefs.Save();
    }

    public static bool GetRemoveAd() {
        if (PlayerPrefs.GetInt(REMOVE_AD, 0) == 1) return true;
        return false;
    }
    #endregion

    #region AUTO
    public static float AUTO_TIME_TO_SUGGEST_SHUFFLE = 10f;
    #endregion

    public static int COIN_PRICE_ITEM = 100;


    public static int countShowFreeItem = 0;

    public static int INTERVAL_SHOW_FREEITEM_POPUP = 3;
    
    public static string FormatTime(int time)
    {
        int hour = time / 3600;
        int minus = (time - hour * 3600) / 60;
        int second = time - hour * 3600 - minus * 60;
        return String.Format("{0:00}:{1:00}:{2:00}", hour, minus, second);
    }


    #region DAILY_FREECOIN

    public const string DAILY_FREECOIN = "daily_freecoin";

    public static void SetDaily_FreeCoin()
    {
        PlayerPrefs.SetInt(DAILY_FREECOIN,Config.GetCurrDayTime());
        PlayerPrefs.Save();
    }

    public static int GetDaily_FreeCoin()
    {
        return PlayerPrefs.GetInt(DAILY_FREECOIN, 0);
    }

    public static bool CheckDaily_FreeCoin()
    {
        return Config.GetCurrDayTime() > GetDaily_FreeCoin();
    }

    #endregion

    #region DAILY_VIDEOCOIN
    
    public const string DAILY_VIDEOCOIN = "daily_videocoin";
    public const string DAILY_VIDEOCOIN_COUNT = "daily_videocoin_count";
    public const int DAILY_VIDEOCOIN_MAX = 10;

    public static void SetDaily_VideoCoin()
    {
        PlayerPrefs.SetInt(DAILY_VIDEOCOIN,Config.GetCurrDayTime());
        PlayerPrefs.Save();
    }

    public static int GetDaily_VideoCoin()
    {
        return PlayerPrefs.GetInt(DAILY_VIDEOCOIN, 0);
    }
    
    public static bool CheckDaily_VideoCoin()
    {
        return Config.GetCurrDayTime() > GetDaily_VideoCoin();
    }
    
    public static void SetDaily_VideoCoin_AddCount()
    {
        int currCount = GetDaily_VideoCoin_Count();
        if (currCount > 0)
        {
            SetDaily_VideoCoin_Count(currCount - 1);
        }
    }

    public static void SetDaily_VideoCoin_Count(int count)
    {
        PlayerPrefs.SetInt(DAILY_VIDEOCOIN_COUNT,count);
        PlayerPrefs.Save();
    }

    public static int GetDaily_VideoCoin_Count()
    {
        return PlayerPrefs.GetInt(DAILY_VIDEOCOIN_COUNT, DAILY_VIDEOCOIN_MAX);
    }
    
    
    #endregion
    
    
    public static int GetCurrDayTime() {
        int currDayTime = DateTime.Now.DayOfYear + DateTime.Now.Year * 10000;
        return currDayTime;
    }


    public const int PRICE_COIN_REVIVE = 100;
    
    #region DAILY_FREEREVIVE

    public const string DAILY_FREEREVIVE = "daily_freerevive";

    public static void SetDaily_FreeRevive()
    {
        PlayerPrefs.SetInt(DAILY_FREEREVIVE,Config.GetCurrDayTime());
        PlayerPrefs.Save();
    }

    public static int GetDaily_FreeRevive()
    {
        return PlayerPrefs.GetInt(DAILY_FREEREVIVE, 0);
    }

    public static bool CheckDaily_FreeRevive()
    {
        // return Config.GetCurrDayTime() > GetDaily_FreeRevive();
        return true;
    }

    #endregion
    
    
    // #region DAILY_FREESPIN
    //
    // public const string DAILY_FREEREVIVE = "daily_freerevive";
    //
    // public static void SetDaily_FreeRevive()
    // {
    //     PlayerPrefs.SetInt(DAILY_FREEREVIVE,Config.GetCurrDayTime());
    //     PlayerPrefs.Save();
    // }
    //
    // public static int GetDaily_FreeRevive()
    // {
    //     return PlayerPrefs.GetInt(DAILY_FREEREVIVE, 0);
    // }
    //
    // public static bool CheckDaily_FreeRevive()
    // {
    //     // return Config.GetCurrDayTime() > GetDaily_FreeRevive();
    //     return true;
    // }
    //
    // #endregion
    
    
    
}
